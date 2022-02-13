using ArmLIB.Dissasembler.Aarch64.HighLevel;
using ArmLIB.Emulator.Aarch64.Fallbacks;
using Compiler.Intermediate;
using Compiler.Intermediate.Extensions.X86;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64.Translation
{
    public static unsafe partial class InstEmit64
    {
        public static void Tbl(ArmEmitContext ctx)
        {
            SIMDOpCodeVectorTableLookup opCode = ctx.CurrentInstruction as SIMDOpCodeVectorTableLookup;

            Xmm ContextStart = ctx.GetVector(0, true);

            IOperand ContextStartPointer = ctx.Add(ctx.EmitOperationSSA(Instruction.GetContext), Const(ContextStart.Reg * 8));

            Call(ctx, (delegate*<Vector128<byte>*, int, int, int, void>)&FallbackVector.Tbl, ContextStartPointer, Const(opCode.Rd), Const(opCode.Rn), Const(opCode.Rm | (opCode.Len << 5)));

            X86ClearVectorTopIfNeeded(ctx, ctx.GetVector(opCode.Rd, true), opCode.Half);
        }
    }
}
