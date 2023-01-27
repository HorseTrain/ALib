using ArmLIB.Dissasembler.Aarch64.HighLevel;
using ArmLIB.Emulator.Aarch64.Fallbacks;
using AlibCompiler.Intermediate;
using AlibCompiler.Intermediate.Extensions.X86;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64.Translation
{
    public static unsafe partial class InstEmit64
    {
        public static void Fcmp(ArmEmitContext ctx) => EmitFloatingPointCompare(ctx, false);
        public static void Fccmp(ArmEmitContext ctx) => EmitFloatingPointCompare(ctx, true);

        public static void Fcsel(ArmEmitContext ctx)
        {
            SIMDOpCodeFloatConditionalSelect opCode = ctx.CurrentInstruction as SIMDOpCodeFloatConditionalSelect;

            if (ctx.EnableX86Extentions)
            {
                IOperand n = Elm(ctx, ctx.GetVector(opCode.Rn, true), 3, 0);
                IOperand m = Elm(ctx, ctx.GetVector(opCode.Rm, true), 3, 0);

                IOperand Holds = ConditionHolds(ctx, opCode.cond);

                IOperand d = ctx.ConditionalSelect(Holds, n, m);

                Xmm Result = ctx.LocalVector();

                Elm(ctx, Result, d, (int)opCode.Size, 0);

                ctx.SetVector(opCode.Rd, Result);
            }
            else
            {
                throw new Exception();
            }
        }

        public static void EmitFloatingPointCompare(ArmEmitContext ctx, bool IsCcmp)
        {
            SIMDOpCodeFloatCompare opCode = ctx.CurrentInstruction as SIMDOpCodeFloatCompare;

            if (ctx.EnableX86Extentions)
            {
                IOperand n = Elm(ctx, ctx.GetVector(opCode.Rn, true), 3, 0);
                IOperand m = opCode.WithZero ? Const(0) : Elm(ctx, ctx.GetVector(opCode.Rm, true), 3, 0);

                ConstOperand End = ctx.CreateLabel();

                if (IsCcmp)
                {
                    ConstOperand NormalFCMP = ctx.CreateLabel();

                    IOperand Holds = ConditionHolds(ctx, opCode.cond);

                    ctx.JumpIf(NormalFCMP, Holds);

                    SetNZCV(ctx, Const(opCode.nzcv));

                    ctx.Jump(End);

                    ctx.MarkLabel(NormalFCMP);
                }

                IOperand nzcv = Call(ctx, (delegate*<ulong, ulong, OpCodeSize, int>)&FallbackFloat.FCompare, n, m, Const((int)opCode.Size));

                SetNZCV(ctx, nzcv);

                if (IsCcmp)
                {
                    ctx.MarkLabel(End);
                }
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
