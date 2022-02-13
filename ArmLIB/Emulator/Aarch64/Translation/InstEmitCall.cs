using ArmLIB.Emulator.Aarch64.Fallbacks;
using Compiler.Intermediate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64.Translation
{
    public static unsafe partial class InstEmit64
    {
        public static IOperand Call(ArmEmitContext ctx, void* Pointer, params IOperand[] Arguments)
        {
            if (ctx.CustomOffsets.ContainsKey("functiontable"))
            {
                if (!FunctionTable.IsAFallback(Pointer))
                {
                    throw new NotImplementedException();
                }

                int Index = FunctionTable.GetFallbackIndex(Pointer);

                Index <<= 3;

                IOperand FunctionPointerPointer = ctx.Add(ctx.FunctionTable, Const(Index));

                IOperand FunctionPointer = Load(ctx, FunctionPointerPointer, IntSize.Int64);

                return Call(ctx, FunctionPointer, Arguments);
            }   
            else
            {
                return Call(ctx, Const((ulong)Pointer), Arguments);
            }
        }

        static IOperand Call(ArmEmitContext ctx, IOperand Address, params IOperand[] Arguments)
        {
            IOperand Out = ctx.Local();

            List<IOperand> ArgTemp = new List<IOperand>() { Address };

            ArgTemp.AddRange(Arguments);

            ctx.ir.Emit(InstructionType.Normal, (int)Instruction.Call, new IOperand[] { Out }, ArgTemp.ToArray());

            return Out;
        }
    }
}
