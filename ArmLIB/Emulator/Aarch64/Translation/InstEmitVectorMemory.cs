using ArmLIB.Dissasembler.Aarch64.HighLevel;
using AlibCompiler.Intermediate;
using AlibCompiler.Intermediate.Extensions.X86;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64.Translation
{
    public static partial class InstEmit64
    {
        public static void Ldp_Vector(ArmEmitContext ctx) => EmitLoadVectorPair(ctx);
        public static void Ldr_Vector(ArmEmitContext ctx) => EmitLoadVector(ctx);
        public static void Stp_Vector(ArmEmitContext ctx) => EmitStoreVectorPair(ctx);
        public static void Str_Vector(ArmEmitContext ctx) => EmitStoreVector(ctx);
        public static void Ldm(ArmEmitContext ctx) => LoadMultiple(ctx);

        static Xmm X86VirtualVectorLoad(ArmEmitContext ctx,IOperand VirtualAddress, int Size)
        {
            Xmm Out;

            if (Size != 4)
            {
                Out = ctx.LocalVector();

                IOperand Value = VirtualLoad(ctx, VirtualAddress, (OperandType)Size);

                Elm(ctx, Out, Value, Size, 0);
            }
            else
            {
                Out = ctx.LocalVector(false);

                IOperand PhysicalAddress = TranslateAddress(ctx, VirtualAddress);

                ctx.EmitX86(X86Instruction.Vmovupd_load, Out, PhysicalAddress);
            }

            return Out;
        }

        static void X86VirtualVectorStore(ArmEmitContext ctx, IOperand VirtualAddress, Xmm Vector, int Size)
        {
            if (Size != 4)
            {
                IOperand ValueToStore = Elm(ctx, Vector, Size, 0);

                VirtualStore(ctx, VirtualAddress, ValueToStore, (OperandType)Size);
            }
            else
            {
                IOperand PhysicalAddress = TranslateAddress(ctx, VirtualAddress);

                ctx.EmitX86AS(X86Instruction.Vmovupd_store, PhysicalAddress, Vector);            
            }
        }

        static void EmitStoreVector(ArmEmitContext ctx)
        {
            OpCodeMemory opCode = ctx.CurrentInstruction as OpCodeMemory;

            IOperand Address = GetPointer(ctx);

            if (ctx.EnableX86Extentions)
            {
                Xmm Value = ctx.GetVector(opCode.Rt, true);

                X86VirtualVectorStore(ctx, Address, Value, opCode.RawSize);
            }
            else
            {
                throw new Exception();
            }
        }

        static void EmitStoreVectorPair(ArmEmitContext ctx)
        {
            OpCodeMemoryPair opCode = ctx.CurrentInstruction as OpCodeMemoryPair;

            IOperand Address = GetPointer(ctx);

            if (ctx.EnableX86Extentions)
            {
                Xmm t = ctx.GetVector(opCode.Rt, true);
                Xmm t2 = ctx.GetVector(opCode.Rt2, true);

                X86VirtualVectorStore(ctx, Address, t, opCode.RawSize);
                X86VirtualVectorStore(ctx, ctx.Add(Address, Const(1 << opCode.RawSize)), t2, opCode.RawSize);
            }
            else
            {
                throw new Exception();
            }
        }

        static void EmitLoadVectorPair(ArmEmitContext ctx)
        {
            OpCodeMemoryPair opCode = ctx.CurrentInstruction as OpCodeMemoryPair;

            IOperand Address = GetPointer(ctx);

            if (ctx.EnableX86Extentions)
            {
                ctx.SetVector(opCode.Rt, X86VirtualVectorLoad(ctx, Address, opCode.RawSize));
                ctx.SetVector(opCode.Rt2, X86VirtualVectorLoad(ctx, ctx.Add(Address, Const(1 << opCode.RawSize)), opCode.RawSize));
            }
            else
            {
                throw new Exception();
            }
        }

        static void EmitLoadVector(ArmEmitContext ctx)
        {
            OpCodeMemory opCode = ctx.CurrentInstruction as OpCodeMemory;

            IOperand Address = GetPointer(ctx);

            if (ctx.EnableX86Extentions)
            {
                Xmm Value = X86VirtualVectorLoad(ctx, Address, opCode.RawSize);

                ctx.SetVector(opCode.Rt, Value);
            }
            else
            {
                throw new Exception();
            }
        }

        static void LoadMultiple(ArmEmitContext ctx)
        {
            SIMDOpCodeVectorMemoryMultiple opCode = ctx.CurrentInstruction as SIMDOpCodeVectorMemoryMultiple;

            IOperand Address = GetPointer(ctx);

            if (ctx.EnableX86Extentions)
            {
                if (opCode.Replicate)
                {
                    int elements = GetElementCount((int)opCode.Size, opCode.Half);

                    for (int r = 0; r < opCode.rpt; ++r)
                    {
                        for (int e = 0; e < elements; ++e)
                        {
                            int tt = (opCode.Rt + r) % 32;

                            for (int s = 0; s < opCode.selem; ++s)
                            {
                                Xmm rval = ctx.GetVector(tt);

                                X86ClearVectorTopIfNeeded(ctx, rval, opCode.Half);

                                IOperand Value = VirtualLoad(ctx, Address, (OperandType)opCode.Size);

                                Elm(ctx, rval, Value, (int)opCode.Size, e);

                                ctx.SetVector(tt, rval);

                                Address = ctx.Add(Address, Const(1 << (int)opCode.Size));
                                tt = (tt + 1) % 32;
                            }
                        }
                    }
                }
                else
                {
                    Xmm t = ctx.GetVector(opCode.Rt, true);

                    IOperand Value = VirtualLoad(ctx, Address, (OperandType)opCode.Size);

                    Elm(ctx, t, Value, (int)opCode.Size, opCode.Index );
                }
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
