using ArmLIB.Dissasembler.Aarch64.HighLevel;
using AlibCompiler.Intermediate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64.Translation
{
    public static partial class InstEmit64
    {
        public static void DoNothing(ArmEmitContext ctx)
        {
            ctx.Nop();
        }

        public static unsafe void Svc(ArmEmitContext ctx)
        {
            OpCodeExceptionGeneration opCode = ctx.CurrentInstruction as OpCodeExceptionGeneration;

            if (ctx.process.svcFallBack != null)
            {
                Call(ctx, (void*)Marshal.GetFunctionPointerForDelegate(ctx.process.svcFallBack), ctx.EmitOperationSSA(Instruction.GetContext),Const(opCode.Address), Const(opCode.Imm));
            }
            else
            {
                //ctx.SetRegRaw("svc",Const(opCode.Imm));

                //ctx.ReturnWithValue(Const(ctx.CurrentInstruction.Address + 4), ExitReason.Svc);

                ctx.EmitUndefined();
            }
        }

        public static void Msr(ArmEmitContext ctx)
        {
            ctx.EmitUndefined();
        }

        public static void Mrs(ArmEmitContext ctx)
        {
            ctx.EmitUndefined();
        }
    }
}
