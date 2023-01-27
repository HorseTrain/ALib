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
        public static void Dup_Gp(ArmEmitContext ctx)
        {
            SIMDOpCodeDUP opCode = ctx.CurrentInstruction as SIMDOpCodeDUP;

            IOperand n = GetN(ctx);

            if (ctx.EnableX86Extentions)
            {
                Xmm Result = ctx.LocalVector();

                int Size = (int)opCode.Size;

                n = ctx.ZeroExtend(n, Size != 3 ? OperandType.Int32 : OperandType.Int64);

                int ElementEnd = GetElementCount(Size, opCode.Half);

                for (int i = 0; i < ElementEnd; ++i)
                {
                    Elm(ctx, Result, n, Size, i);
                }

                ctx.SetVector(opCode.Rd, Result);
            }
            else
            {
                throw new Exception();
            }
        }

        public static void Dup_Element(ArmEmitContext ctx)
        {
            SIMDOpCodeInsertElement opCode = ctx.CurrentInstruction as SIMDOpCodeInsertElement;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = ctx.GetVector(opCode.Rn, true);

                IOperand ExtractedData = Elm(ctx, n, (int)opCode.Size, opCode.Index);

                Xmm Result = ctx.LocalVector(true);

                Elm(ctx, Result, ExtractedData, (int)opCode.Size, 0);

                ctx.SetVector(opCode.Rd, Result);
            }
            else
            {
                throw new Exception();
            }
        }

        public static void Dup_Vector_Element(ArmEmitContext ctx)
        {
            SIMDOpCodeInsertElement opCode = ctx.CurrentInstruction as SIMDOpCodeInsertElement;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = ctx.GetVector(opCode.Rn, true);

                Xmm Result = ctx.LocalVector(true);

                IOperand ExtractedData = Elm(ctx, n, (int)opCode.Size, opCode.Index);

                int Size = (int)opCode.Size;

                int iterations = GetElementCount(Size, opCode.Half);

                for (int i = 0; i < iterations; ++i)
                {
                    Elm(ctx, Result, ExtractedData, Size, i);
                }

                ctx.SetVector(opCode.Rd, Result);
            }
            else
            {
                throw new Exception();
            }
        }

        public static void Umov_Element(ArmEmitContext ctx)
        {
            SIMDOpCodeInsertElement opCode = ctx.CurrentInstruction as SIMDOpCodeInsertElement;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = ctx.GetVector(opCode.Rn, true);

                IOperand ExtractedData = ctx.ZeroExtend(Elm(ctx, n, (int)opCode.Size, opCode.Index), OperandType.Int64);

                SetD(ctx, ExtractedData);
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
