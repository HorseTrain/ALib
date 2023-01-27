using ArmLIB.Dissasembler.Aarch64.HighLevel;
using AlibCompiler.Intermediate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64.Translation
{
    public static partial class InstEmit64
    {
        public static IOperand Const(long Imm) => ConstOperand.Create(Imm);
        public static IOperand Const(ulong Imm) => ConstOperand.Create(Imm);

        public static IOperand GetN(ArmEmitContext ctx)
        {
            IOpCodeRn opCodeRN = ctx.CurrentInstruction as IOpCodeRn;

            bool isSP = (ctx.CurrentInstruction is IOpCodeRnIsSP nsp) && nsp.RnIsSP;

            return ctx.GetX(opCodeRN.Rn, isSP);
        }

        public static IOperand GetA(ArmEmitContext ctx) => ctx.GetX((ctx.CurrentInstruction as IOpCodeRa).Ra);

        public static IOperand GetM(ArmEmitContext ctx)
        {
            AOpCode opCode = ctx.CurrentInstruction;

            switch (opCode)
            {
                case OpCodeALUImm op: return Const(op.Imm);
                case OpCodeALUShiftedReg op:
                    {
                        IOperand m = ctx.GetX(op.Rm);

                        m = LogicalShift(ctx, m, Const(op.Shift), op.Type);

                        return m;
                    }

                case IOpCodeExtendedM op:
                    {
                        IOperand m = ctx.GetX(op.Rm);

                        m = Extend(ctx, m, (IntType)op.Option);

                        m = ctx.LogicalShiftLeft(m,Const(op.Shift));

                        return m;
                    };

                case OpCodeConditionalSelect op: return ctx.GetX(op.Rm);
                case OpCodeALU3Src op: return ctx.GetX(op.Rm);
                case OpCodeALU2Src op: return ctx.GetX(op.Rm);
                case OpCodeExtract op: return ctx.GetX(op.Rm);
                default: throw new Exception();
            }
        }

        public static void SetD(ArmEmitContext ctx, IOperand Data)
        {
            IOpCodeRd opCodeRd = ctx.CurrentInstruction as IOpCodeRd;

            bool isSP = (ctx.CurrentInstruction is IOpCodeRdIsSP dsp) && dsp.RdIsSP;

            ctx.SetX(opCodeRd.Rd, Data, isSP);
        }

        public static Cond GetCond(ArmEmitContext ctx) => (ctx.CurrentInstruction as IOpCodeCond).cond;
    }
}
