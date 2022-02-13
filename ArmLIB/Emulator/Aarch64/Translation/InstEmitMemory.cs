using ArmLIB.Dissasembler.Aarch64.HighLevel;
using Compiler.Intermediate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64.Translation
{
    public static partial class InstEmit64
    {
        public static void Ldr(ArmEmitContext ctx) => EmitLoad(ctx);
        public static void Str(ArmEmitContext ctx) => EmitStore(ctx);
        public static void Ldp(ArmEmitContext ctx) => EmitLoadPair(ctx);
        public static void Stp(ArmEmitContext ctx) => EmitStorePair(ctx);
        public static void Ldx(ArmEmitContext ctx) => LoadEx(ctx, true);
        public static void Lda(ArmEmitContext ctx) => LoadEx(ctx, false);
        public static void Stx(ArmEmitContext ctx) => StoreEx(ctx, true);
        public static void Stl(ArmEmitContext ctx) => StoreEx(ctx, false);

        static long ExclusiveMask => ~((4L << 4) - 1);

        public static void Clrex(ArmEmitContext ctx)
        {
            if (ctx.EmulateAtomics)
            {
                ctx.SetRegRaw(ArmEmitContext.ExclusiveAddressString, Const(ulong.MaxValue));
            }
            else
            {
                ctx.EmitUndefined();
            }
        }

        static IOperand GetPointer(ArmEmitContext ctx)
        {
            int rn = (ctx.CurrentInstruction as OpCodeMemory).Rn;

            IOperand Base = GetN(ctx);

            switch (ctx.CurrentInstruction)
            {
                case IOpCodeMemWback op:
                    {
                        IOperand Offset = Const(op.Imm);

                        if (op.Mode == WbackMode.None)
                        {
                            return ctx.Add(Base, Offset);
                        }
                        else if (op.Mode == WbackMode.Post)
                        {
                            IOperand BaseToReturn = ctx.Copy(Base); //rn is overwritten to, but also returned.

                            ctx.SetX(rn, ctx.Add(Base, Offset), true);

                            return BaseToReturn;
                        }
                        else /* if (op.Mode == WbackMode.Pre) */
                        {
                            IOperand Result = ctx.Add(Base, Offset);

                            ctx.SetX(rn, Result, true);

                            return Result;
                        }
                    };

                case OpCodeMemoryReg:
                    {
                        return ctx.Add(Base, GetM(ctx));
                    }

                case OpCodeMemoryExclusive: return Base;
                default: throw new Exception();
            }
        }

        static unsafe IOperand TranslateAddress(ArmEmitContext ctx, IOperand VirtualAddress)
        {
            return ctx.Add(ctx.MemoryAddress, VirtualAddress);
        }

        public static IOperand Load(ArmEmitContext ctx, IOperand Address, IntSize Size)
        {
            IOperand Result = ctx.Local();

            ctx.ir.Emit(InstructionType.Normal, (int)Instruction.Load, new IOperand[] { ctx.ZeroExtend(Result, Size) }, new IOperand[] { Address });

            return Result;
        }

        static unsafe IOperand VirtualLoad(ArmEmitContext ctx, IOperand Address, IntSize Size)
        {
            Address = TranslateAddress(ctx, Address);

            return ctx.ZeroExtend(Load(ctx, Address, Size), IntSize.Int64);
        }

        static unsafe void VirtualStore(ArmEmitContext ctx, IOperand Address, IOperand Value, IntSize Size)
        {
            Address = TranslateAddress(ctx, Address);

            ctx.ir.Emit(InstructionType.Normal, (int)Instruction.Store, new IOperand[0], new IOperand[] { Address,ctx.ZeroExtend(Value, Size) });
        }

        static void EmitLoad(ArmEmitContext ctx)
        {
            OpCodeMemory opCode = ctx.CurrentInstruction as OpCodeMemory;

            IOperand Address = GetPointer(ctx);

            IOperand Value = VirtualLoad(ctx, Address, (IntSize)opCode.RawSize);

            if (opCode.SignExtendLoad)
            {
                switch ((int)opCode.Size)
                {
                    case 0: Value = ctx.SignExtend8(Value); break;
                    case 1: Value = ctx.SignExtend16(Value); break;
                    default: Value = ctx.SignExtend32(Value); break;
                }

                if (opCode.SignExtend32)
                {
                    Value = ctx.LogicalAnd(Value, Const(uint.MaxValue));
                }
            }

            ctx.SetX(opCode.Rt, Value, false);
        }

        static void EmitStore(ArmEmitContext ctx)
        {
            OpCodeMemory opCode = ctx.CurrentInstruction as OpCodeMemory;

            IOperand Address = GetPointer(ctx);

            IOperand Value = ctx.GetX(opCode.Rt);

            VirtualStore(ctx, Address, Value, (IntSize)opCode.RawSize);
        }

        static void EmitLoadPair(ArmEmitContext ctx)
        {
            OpCodeMemoryPair opCode = ctx.CurrentInstruction as OpCodeMemoryPair;

            IOperand Address = GetPointer(ctx);

            IOperand t1 = VirtualLoad(ctx, Address, (IntSize)opCode.RawSize);
            IOperand t2 = VirtualLoad(ctx, ctx.Add(Address, Const(1 << opCode.RawSize)), (IntSize)opCode.RawSize);

            if (opCode.SignExtendLoad)
            {
                t1 = ctx.SignExtend32(t1);
                t2 = ctx.SignExtend32(t2);
            }

            ctx.SetX(opCode.Rt, t1, false);
            ctx.SetX(opCode.Rt2, t2, false);
        }

        static void EmitStorePair(ArmEmitContext ctx)
        {
            OpCodeMemoryPair opCode = ctx.CurrentInstruction as OpCodeMemoryPair;

            IOperand Address = GetPointer(ctx);

            VirtualStore(ctx, Address, ctx.GetX(opCode.Rt), (IntSize)opCode.RawSize);
            VirtualStore(ctx, ctx.Add(Address, Const(1 << opCode.RawSize)), ctx.GetX(opCode.Rt2), (IntSize)opCode.RawSize);
        }

        static unsafe void LoadEx(ArmEmitContext ctx, bool IsExclusive)
        {
            OpCodeMemoryExclusive opCode = ctx.CurrentInstruction as OpCodeMemoryExclusive;

            if (ctx.EmulateAtomics)
            {
                IOperand Address = GetPointer(ctx);

                IOperand Value = VirtualLoad(ctx, Address, (IntSize)opCode.RawSize);

                if (IsExclusive)
                {
                    ctx.SetRegRaw(ArmEmitContext.ExclusiveAddressString, TranslateAddress(ctx,Address));
                    ctx.SetRegRaw(ArmEmitContext.ExclusiveValueString, Value);
                }

                ctx.SetX(opCode.Rt, Value, false);
            }
            else
            {
                ctx.EmitUndefined();
            }
        }

        static unsafe void StoreEx(ArmEmitContext ctx, bool IsExclusive)
        {
            OpCodeMemoryExclusive opCode = ctx.CurrentInstruction as OpCodeMemoryExclusive;

            if (ctx.EmulateAtomics)
            {
                if (IsExclusive)
                {
                    IOperand CasSuccess = ctx.Local();

                    IOperand Address = ctx.GetRegRaw(ArmEmitContext.ExclusiveAddressString);
                    IOperand Expecting = ctx.GetRegRaw(ArmEmitContext.ExclusiveValueString);

                    IOperand ToSwap = ctx.GetX(opCode.Rt);

                    ctx.ir.Emit(InstructionType.Normal, (int)Instruction.CompareAndSwap, new IOperand[] { CasSuccess }, new IOperand[] { Address, ctx.ZeroExtend(Expecting, (IntSize)opCode.RawSize), ctx.ZeroExtend(ToSwap, (IntSize)opCode.RawSize) });

                    ctx.SetX(opCode.Rs, ctx.ConditionalSelect(CasSuccess, Const(0), Const(1)), false);
                }
                else
                {
                    IOperand Address = GetPointer(ctx);

                    VirtualStore(ctx, Address, ctx.GetX(opCode.Rt), (IntSize)opCode.Size);
                }
            }
            else
            {
                ctx.EmitUndefined();
            }
        }
    }
}
