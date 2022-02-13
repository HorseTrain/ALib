using DCpu.Decoders;
using DCpu.IntermediateRepresentation;
using DCpu.State;
using DCpu.Translation;

using static DCpu.Instructions.InstEmitAluHelper;
using static DCpu.Instructions.InstEmitFlowHelper;
using static DCpu.Instructions.InstEmitHelper;
using static DCpu.IntermediateRepresentation.OperandHelper;

namespace DCpu.Instructions
{
    static partial class InstEmit
    {
        public static void Ccmn(ArmEmitterContext context) => EmitCcmp(context, isNegated: true);
        public static void Ccmp(ArmEmitterContext context) => EmitCcmp(context, isNegated: false);

        private static void EmitCcmp(ArmEmitterContext context, bool isNegated)
        {
            OpCodeCcmp op = (OpCodeCcmp)context.CurrOp;

            Operand lblTrue = Label();
            Operand lblEnd  = Label();

            EmitCondBranch(context, lblTrue, op.Cond);

            SetFlag(context, PState.VFlag, Const((op.Nzcv >> 0) & 1));
            SetFlag(context, PState.CFlag, Const((op.Nzcv >> 1) & 1));
            SetFlag(context, PState.ZFlag, Const((op.Nzcv >> 2) & 1));
            SetFlag(context, PState.NFlag, Const((op.Nzcv >> 3) & 1));

            context.Branch(lblEnd);

            context.MarkLabel(lblTrue);

            Operand n = GetAluN(context);
            Operand m = GetAluM(context);

            if (isNegated)
            {
                Operand d = context.Add(n, m);

                EmitNZFlagsCheck(context, d);

                EmitAddsCCheck(context, n, d);
                EmitAddsVCheck(context, n, m, d);
            }
            else
            {
                Operand d = context.Subtract(n, m);

                EmitNZFlagsCheck(context, d);

                EmitSubsCCheck(context, n, m);
                EmitSubsVCheck(context, n, m, d);
            }

            context.MarkLabel(lblEnd);
        }
    }
}