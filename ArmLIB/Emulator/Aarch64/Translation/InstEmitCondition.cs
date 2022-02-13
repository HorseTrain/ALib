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
        public static void Ccmn(ArmEmitContext ctx) => EmitConditionalCompare(ctx, true);
        public static void Ccmp(ArmEmitContext ctx) => EmitConditionalCompare(ctx, false);
        public static void Csel(ArmEmitContext ctx) => EmitConditionalSelect(ctx, false, false);
        public static void Csinc(ArmEmitContext ctx) => EmitConditionalSelect(ctx, true, false);
        public static void Csinv(ArmEmitContext ctx) => EmitConditionalSelect(ctx, false, true);
        public static void Csneg(ArmEmitContext ctx) => EmitConditionalSelect(ctx, true, true);

        public static IOperand IsOne(ArmEmitContext ctx, IOperand Source) => ctx.CompareEqual(Source, ConstOperand.Create(1));
        public static IOperand IsZero(ArmEmitContext ctx, IOperand Source) => ctx.CompareEqual(Source, ConstOperand.Create(0));
        public static IOperand InvertBool(ArmEmitContext ctx, IOperand Source) => ctx.LogicalExclusiveOr(Source, Const(1));
        public static IOperand ConditionHolds(ArmEmitContext ctx, Cond cond)
        {
            (IOperand n,
             IOperand z,
             IOperand c,
             IOperand v) = GetNZCV(ctx);

            IOperand Result = Const(0);

            switch ((Cond)((int)cond & ~1))
            {
                case Cond.EQ: Result = IsOne(ctx, z); break;
                case Cond.CS: Result = IsOne(ctx, c); break;
                case Cond.MI: Result = IsOne(ctx, n); break;
                case Cond.VS: Result = IsOne(ctx, v); break;
                case Cond.HI: Result = ctx.LogicalAnd(IsOne(ctx, c), IsZero(ctx, z)); break;
                case Cond.GE: Result = ctx.CompareEqual(n, v); break;
                case Cond.GT: Result = ctx.LogicalAnd(ctx.CompareEqual(n, v), IsZero(ctx, z)); break;
                case Cond.AL: Result = Const(1); break;
            }

            if ((((int)cond) & 1) == 1 && ((int)cond) != 0b1111)
            {
                Result = InvertBool(ctx, Result);
            }

            return Result;
        }

        public static void EmitConditionalSelect(ArmEmitContext ctx, bool Incrament, bool Invert)
        {
            IOperand n = GetN(ctx);
            IOperand m = GetM(ctx);

            Cond cond = GetCond(ctx);

            if (Invert)
                m = ctx.Not(m);

            if (Incrament)
                m = ctx.Add(m, Const(1));

            IOperand Holds = ConditionHolds(ctx, cond);

            IOperand d = ctx.ConditionalSelect(Holds, n, m);

            SetD(ctx, d);
        }

        public static void EmitConditionalCompare(ArmEmitContext ctx, bool IsAdd)
        {
            OpCodeConditionalCompare opCode = (OpCodeConditionalCompare)ctx.CurrentInstruction;

            IOperand n = GetN(ctx);
            IOperand m = opCode.IsReg ? ctx.GetX(opCode.Imm_Rm) : Const(opCode.Imm_Rm);

            ConstOperand End = ctx.CreateLabel();
            ConstOperand YesConditionHolds = ctx.CreateLabel();

            IOperand Holds = ConditionHolds(ctx, opCode.cond);

            ctx.JumpIf(YesConditionHolds, Holds);

            SetNZCV(ctx, Const((opCode.nzcv >> 3) & 1), Const((opCode.nzcv >> 2) & 1), Const((opCode.nzcv >> 1) & 1), Const((opCode.nzcv >> 0) & 1));

            ctx.Jump(End);
            ctx.MarkLabel(YesConditionHolds);

            IOperand N, Z, C, V;

            if (IsAdd)
            {
                IOperand d = ctx.Add(n,m);

                (N, Z, C, V) = GetAddsFlags(ctx, d,n,m);
            }
            else
            {
                IOperand d = ctx.Subtract(n, m);

                (N, Z, C, V) = GetSubsFlags(ctx, d, n, m);
            }

            SetNZCV(ctx, N, Z, C, V);

            ctx.MarkLabel(End);
        }
    }
}
