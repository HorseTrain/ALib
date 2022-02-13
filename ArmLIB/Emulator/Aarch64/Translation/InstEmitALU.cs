using ArmLIB.Dissasembler.Aarch64.HighLevel;
using Compiler.Intermediate;
using Compiler.Intermediate.Extensions.X86;
using System;
using System.Collections.Generic;

namespace ArmLIB.Emulator.Aarch64.Translation
{
    public static partial class InstEmit64
    {
        public static void Adc(ArmEmitContext ctx) => EmitAddSubtractWithCarry(ctx, true, false);
        public static void Adcs(ArmEmitContext ctx) => EmitAddSubtractWithCarry(ctx, true, true);
        public static void Add(ArmEmitContext ctx) => SetD(ctx, ctx.Add(GetN(ctx), GetM(ctx)));
        public static void And(ArmEmitContext ctx) => SetD(ctx, ctx.LogicalAnd(GetN(ctx), GetM(ctx)));
        public static void Asrv(ArmEmitContext ctx) => SetD(ctx, ctx.LogicalShiftRightSigned(GetN(ctx), ctx.LogicalAnd(GetM(ctx), Const((8 << (int)ctx.CurrentEmitSize) - 1))));
        public static void Bfm(ArmEmitContext ctx) => EmitBitfield(ctx, false, false);
        public static void Bic(ArmEmitContext ctx) => SetD(ctx, ctx.LogicalAnd(GetN(ctx), ctx.Not(GetM(ctx))));
        public static void Eon(ArmEmitContext ctx) => SetD(ctx, ctx.LogicalExclusiveOr(GetN(ctx), ctx.Not(GetM(ctx))));
        public static void Eor(ArmEmitContext ctx) => SetD(ctx, ctx.LogicalExclusiveOr(GetN(ctx), GetM(ctx)));
        public static void Lslv(ArmEmitContext ctx) => SetD(ctx, ctx.LogicalShiftLeft(GetN(ctx), ctx.LogicalAnd(GetM(ctx), Const((8 << (int)ctx.CurrentEmitSize) - 1))));
        public static void Lsrv(ArmEmitContext ctx) => SetD(ctx, ctx.LogicalShiftRight(GetN(ctx), ctx.LogicalAnd(GetM(ctx), Const((8 << (int)ctx.CurrentEmitSize) - 1))));
        public static void Madd(ArmEmitContext ctx) => SetD(ctx, ctx.Add(GetA(ctx), ctx.Multiply(GetN(ctx), GetM(ctx))));
        public static void Msub(ArmEmitContext ctx) => SetD(ctx, ctx.Subtract(GetA(ctx), ctx.Multiply(GetN(ctx), GetM(ctx))));
        public static void Orn(ArmEmitContext ctx) => SetD(ctx, ctx.LogicalOr(GetN(ctx), ctx.Not(GetM(ctx))));
        public static void Orr(ArmEmitContext ctx) => SetD(ctx, ctx.LogicalOr(GetN(ctx), GetM(ctx)));
        public static void Rorv(ArmEmitContext ctx) => SetD(ctx, ctx.LogicalRotateRight(GetN(ctx), ctx.LogicalAnd(GetM(ctx), Const((8 << (int)ctx.CurrentEmitSize) - 1))));
        public static void Sbc(ArmEmitContext ctx) => EmitAddSubtractWithCarry(ctx, false, false);
        public static void Sbcs(ArmEmitContext ctx) => EmitAddSubtractWithCarry(ctx, false, true);
        public static void Sdiv(ArmEmitContext ctx) => EmitDivide(ctx, true);
        public static void Smaddl(ArmEmitContext ctx) => SetD(ctx, ctx.Add(GetA(ctx), ctx.Multiply(ctx.SignExtend32(GetN(ctx)), ctx.SignExtend32(GetM(ctx)))));
        public static void Smsubl(ArmEmitContext ctx) => SetD(ctx, ctx.Subtract(GetA(ctx), ctx.Multiply(ctx.SignExtend32(GetN(ctx)), ctx.SignExtend32(GetM(ctx)))));
        public static void Smulh(ArmEmitContext ctx) => SetD(ctx, ctx.MultiplyHiSigned(GetN(ctx), GetM(ctx)));
        public static void Sub(ArmEmitContext ctx) => SetD(ctx, ctx.Subtract(GetN(ctx), GetM(ctx)));
        public static void Udiv(ArmEmitContext ctx) => EmitDivide(ctx, false);
        public static void Umaddl(ArmEmitContext ctx) => SetD(ctx, ctx.Add(GetA(ctx), ctx.Multiply(ctx.LogicalAnd(GetN(ctx), Const(uint.MaxValue)), ctx.LogicalAnd(GetM(ctx), Const(uint.MaxValue)))));
        public static void Umsubl(ArmEmitContext ctx) => SetD(ctx, ctx.Subtract(GetA(ctx), ctx.Multiply(ctx.LogicalAnd(GetN(ctx), Const(uint.MaxValue)), ctx.LogicalAnd(GetM(ctx), Const(uint.MaxValue)))));
        public static void Umulh(ArmEmitContext ctx) => SetD(ctx, ctx.MultiplyHi(GetN(ctx), GetM(ctx)));
        public static void Ubfm(ArmEmitContext ctx) => EmitBitfield(ctx, true, false);

        public static void Sbfm(ArmEmitContext ctx)
        {
            OpCodeBitfield opCode = ctx.CurrentInstruction as OpCodeBitfield;

            if (opCode.RawData.sf == 0 && opCode.RawData.N == 0 && opCode.Imms == 0b11111)
            {
                EmitAsrImm(ctx);
            }
            else if (opCode.RawData.sf == 1 && opCode.RawData.N == 1 && opCode.RawData.imms == 0b111111)
            {
                EmitAsrImm(ctx);
            }
            else
            {
                EmitBitfield(ctx, true, true);
            }
        }

        static void EmitAddSubtractWithCarry(ArmEmitContext ctx, bool IsAdd,bool SetFlags)
        {
            IOperand n = GetN(ctx);
            IOperand m = GetM(ctx);

            IOperand d = IsAdd ? ctx.Add(n, m) : ctx.Subtract(n,m);

            IOperand Carry = ctx.GetRegRaw("c");
            IOperand Borrow = !IsAdd ? ctx.LogicalExclusiveOr(Carry, Const(1)) : Const(0);

            d = IsAdd ? ctx.Add(d, Carry) : ctx.Subtract(d, Borrow);

            if (SetFlags)
            {
                (IOperand N, IOperand Z) = GetNegZeroFlags(ctx, d);

                IOperand C, V;

                if (IsAdd)
                {
                    C = ctx.LogicalOr(ctx.LogicalAnd(ctx.CompareEqual(d, n), Carry), ctx.CompareLess(d, n));
                    V = CompareLessThanZero(ctx, ctx.LogicalAnd(ctx.LogicalExclusiveOr(d, n), ctx.Not(ctx.LogicalExclusiveOr(n, m))));
                }
                else
                {
                    C = ctx.LogicalOr(ctx.LogicalAnd(ctx.CompareEqual(n,m), Carry), ctx.CompareGreater(n, m));
                    V = CompareLessThanZero(ctx, ctx.LogicalAnd(ctx.LogicalExclusiveOr(d, n), ctx.LogicalExclusiveOr(n, m)));
                }

                SetNZCV(ctx, N, Z, C, V);
            }

            SetD(ctx, d);
        }

        static IOperand X86ReverseBytes(ArmEmitContext ctx, IOperand Source, IntSize Size)
        {
            IOperand LocalWork = ctx.Copy(Source);

            ctx.ir.EmitX86(X86Instruction.Bswap, new IOperand[] { ctx.ZeroExtend(LocalWork, Size) }, new IOperand[0]);

            if (Size <= IntSize.Int16)
            {
                return ctx.LogicalAnd(LocalWork, Const((8 << (int)Size) - 1));
            }

            return LocalWork;
        }

        static IOperand Replicate(ArmEmitContext ctx, IOperand Source, IOperand Bit)
        {
            Source = ctx.LogicalShiftRight(Source, Bit);

            Source = ctx.LogicalAnd(Source, Const(1));

            return ctx.Subtract(Const(0), Source);
        }

        public static void Adds(ArmEmitContext ctx)
        {
            IOperand n = GetN(ctx);
            IOperand m = GetM(ctx);

            IOperand d = ctx.Add(n, m);

            (IOperand N, IOperand Z, IOperand C, IOperand V) = GetAddsFlags(ctx, d, n, m);

            SetNZCV(ctx, N, Z, C, V);

            SetD(ctx, d);

            return;
        }

        public static void Subs(ArmEmitContext ctx)
        {
            IOperand n = GetN(ctx);
            IOperand m = GetM(ctx);

            IOperand d = ctx.Subtract(n, m);

            (IOperand N, IOperand Z, IOperand C, IOperand V) = GetSubsFlags(ctx, d, n, m);

            SetNZCV(ctx, N, Z, C, V);

            SetD(ctx, d);
        }

        static void EmitX86AddWithFlags(ArmEmitContext ctx, IOperand n, IOperand m)
        {
            IOperand d = ctx.Local();

            IOperand is_neg = ctx.Local();
            IOperand is_zero = ctx.Local();
            IOperand is_carry = ctx.Local();
            IOperand is_overflow = ctx.Local();

            ctx.ir.EmitX86(X86Instruction.Add_GetFlags, new IOperand[] { d, is_neg, is_zero, is_carry, is_overflow }, new IOperand[] { n, m });

            SetNZCV(ctx, ctx.LogicalAnd(is_neg, Const(1)), ctx.LogicalAnd(is_zero, Const(1)), ctx.LogicalAnd(is_carry, Const(1)), ctx.LogicalAnd(is_overflow, Const(1)));

            SetD(ctx, d);
        }

        public static void Ands(ArmEmitContext ctx)
        {
            IOperand n = GetN(ctx);
            IOperand m = GetM(ctx);

            IOperand d = ctx.LogicalAnd(n, m);

            (IOperand N, IOperand Z) = GetNegZeroFlags(ctx, d);

            SetNZCV(ctx, N, Z, Const(0), Const(0));

            SetD(ctx, d);
        }

        public static void Bics(ArmEmitContext ctx)
        {
            IOperand n = GetN(ctx);
            IOperand m = GetM(ctx);

            m = ctx.Not(m);

            IOperand d = ctx.LogicalAnd(n, m);

            (IOperand N, IOperand Z) = GetNegZeroFlags(ctx, d);

            SetNZCV(ctx, N, Z, Const(0), Const(0));

            SetD(ctx, d);
        }

        public static void Extr(ArmEmitContext ctx)
        {
            OpCodeExtract opCode = ctx.CurrentInstruction as OpCodeExtract;

            IOperand n = GetN(ctx);
            IOperand m = GetM(ctx);

            IOperand d = ctx.LogicalDoubleShiftRight(n,m,Const(opCode.Imm));

            SetD(ctx, d);
        }

        static void EmitDivide(ArmEmitContext ctx, bool isSigned)
        {
            IOperand n = GetN(ctx);
            IOperand m = GetM(ctx);

            SetD(ctx, isSigned ? ctx.DivideSigned(n, m) : ctx.Divide(n, m));
        }

        static void EmitAsrImm(ArmEmitContext ctx)
        {
            OpCodeBitfield opCode = ctx.CurrentInstruction as OpCodeBitfield;

            IOperand n = GetN(ctx);
            IOperand m = Const(opCode.Immr);

            IOperand d = ctx.LogicalShiftRightSigned(n, m);

            SetD(ctx, d);
        }

        static void EmitLslImm(ArmEmitContext ctx)
        {
            OpCodeBitfield opCode = ctx.CurrentInstruction as OpCodeBitfield;

            var data = DecoderHelper.DecodeBitMask(opCode.RawInstruction, false);

            int CurrentSize = opCode.Size == OpCodeSize.w ? 32 : 64;

            IOperand n = GetN(ctx);
            IOperand m = Const(CurrentSize - data.Shift);

            IOperand d = ctx.LogicalShiftLeft(n, m);

            SetD(ctx, d);
        }

        static void EmitSbfx(ArmEmitContext ctx)
        {
            OpCodeBitfield opCode = ctx.CurrentInstruction as OpCodeBitfield;

            IOperand n = GetN(ctx);
        }

        public static void Adr(ArmEmitContext ctx)
        {
            OpCodePcRelAddressing opCode = ctx.CurrentInstruction as OpCodePcRelAddressing;

            SetD(ctx, Const(opCode.Address + opCode.Imm));
        }

        public static void Adrp(ArmEmitContext ctx)
        {
            OpCodePcRelAddressing opCode = ctx.CurrentInstruction as OpCodePcRelAddressing;

            SetD(ctx, Const((opCode.Address + opCode.Imm) & ~4095L));
        }

        public static void Rev(ArmEmitContext ctx)
        {
            IOperand n = GetN(ctx);

            if (ctx.Host == HostArc.X86)
            {
                SetD(ctx, X86ReverseBytes(ctx,n, ctx.CurrentEmitSize));
            }
            else
            {
                throw new Exception();
            }
        }

        public static void Rev32(ArmEmitContext ctx)
        {
            IOperand n = GetN(ctx);

            if (ctx.Host == HostArc.X86)
            {
                IOperand Bottom = X86ReverseBytes(ctx, n, IntSize.Int32);
                IOperand Top = X86ReverseBytes(ctx, ctx.LogicalShiftRight(n, Const(32)), IntSize.Int32);

                SetD(ctx, ctx.LogicalOr(Bottom, ctx.LogicalShiftLeft(Top, Const(32))));
            }
            else
            {
                throw new Exception();
            }
        }

        public static void Rev16(ArmEmitContext ctx)
        {
            IOperand n = GetN(ctx);

            IOperand Result = Const(0);

            for (int i = 0; i < (ctx.CurrentEmitSize == IntSize.Int32 ? 2 : 4); ++i)
            {
                IOperand Short = ctx.LogicalShiftRight(n, Const(i * 16));

                Short = ctx.LogicalAnd(Short, Const(ushort.MaxValue));

                IOperand Byte0 = ctx.LogicalAnd(Short, Const(255));
                IOperand Byte1 = ctx.LogicalAnd(ctx.LogicalShiftRight(Short, Const(8)), Const(255));

                IOperand ReversedShort = ctx.LogicalOr(ctx.LogicalShiftLeft(Byte0, Const(8)), Byte1);

                Result = ctx.LogicalOr(Result, ctx.LogicalShiftLeft(ReversedShort, Const(i * 16)));
            }

            SetD(ctx, Result);
        }

        public static void Rbit(ArmEmitContext ctx)
        {
            IOperand n = GetN(ctx);

            List<IOperand> Bits = new List<IOperand>();

            for (int i = 0; i < (8 << (int)ctx.CurrentEmitSize); ++i)
            {
                Bits.Add(ctx.LogicalAnd(ctx.LogicalShiftRight(n, Const(i)),Const(1)));
            }

            Bits.Reverse();

            IOperand Result = Const(0);

            for (int i = 0; i < Bits.Count; ++i)
            {
                Result = ctx.LogicalOr(Result, ctx.LogicalShiftLeft(Bits[i], Const(i)));
            }

            SetD(ctx, Result);
        }

        public static void Clz(ArmEmitContext ctx)
        {
            IOperand n = GetN(ctx);

            if (ctx.Host == HostArc.X86)
            {
                IOperand Result = ctx.Local();

                ctx.ir.EmitX86(X86Instruction.Lzcnt, new IOperand[] { Result }, new IOperand[] { n });

                SetD(ctx, Result);
            }
            else
            {
                throw new Exception();
            }
        }

        public static void Cls(ArmEmitContext ctx)
        {
            ctx.EmitUndefined();
        }

        public static void EmitBitfield(ArmEmitContext ctx, bool InZeros, bool Extend)
        {
            OpCodeBitfield opCode = (OpCodeBitfield)ctx.CurrentInstruction;

            var Masks = DecoderHelper.DecodeBitMask(opCode.RawInstruction, false);

            IOperand dst = InZeros ? Const(0) : ctx.GetX(opCode.Rd);
            IOperand src = GetN(ctx);

            IOperand bot = ctx.LogicalOr(ctx.LogicalAnd(dst, Const(~Masks.WMask)), ctx.LogicalAnd(ctx.LogicalRotateRight(src, Const(opCode.Immr)), Const(Masks.WMask) ));

            IOperand top = Extend ? Replicate(ctx, src, Const(opCode.Imms)) : dst;

            IOperand d = ctx.LogicalOr(ctx.LogicalAnd(top, Const(~Masks.TMask)), ctx.LogicalAnd(bot, Const(Masks.TMask)));

            SetD(ctx, d);
        }
    }
}
