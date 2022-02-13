using ArmLIB.Dissasembler.Aarch64.HighLevel;
using ArmLIB.Emulator.Aarch64.Fallbacks;
using Compiler.Intermediate;
using Compiler.Intermediate.Extensions.X86;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64.Translation
{
    public static unsafe partial class InstEmit64
    {
        public static void Frintm(ArmEmitContext ctx) => EmitFrint(ctx, RoundingMode.TowardNegInf);

        public static void Frsqrte_Vector(ArmEmitContext ctx)
        {
            SIMDOpCodeVector1Src opCode = ctx.CurrentInstruction as SIMDOpCodeVector1Src;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = ctx.GetVector(opCode.Rn, true);

                if (opCode.Size == OpCodeSize.s)
                {
                    Xmm d = ctx.GetVector(opCode.Rd, true);

                    ctx.EmitX86(X86Instruction.Vrsqrtps, d, n);

                    X86ClearVectorTopIfNeeded(ctx, d, opCode.Half);
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                throw new Exception();
            }
        }

        public static void Movi(ArmEmitContext ctx)
        {
            SIMDOpCodeMovImm opCode = ctx.CurrentInstruction as SIMDOpCodeMovImm;

            int Iterations = GetElementCount((int)opCode.Size, opCode.Half);

            if (ctx.EnableX86Extentions)
            {
                Xmm Result = ctx.LocalVector();

                for (int i = 0; i < Iterations; ++i)
                {
                    X86VectorInsert(ctx, Result, Const(opCode.Imm), (int)opCode.Size, i);
                }

                ctx.SetVector(opCode.Rd, Result);
            }
            else
            {
                throw new Exception();
            }
        }

        public static void Fmov_Scalar_Imm(ArmEmitContext ctx)
        {
            SIMDOpCodeFmovImm opCode = ctx.CurrentInstruction as SIMDOpCodeFmovImm;

            if (ctx.EnableX86Extentions)
            {
                Xmm Result = ctx.LocalVector();

                X86VectorInsert(ctx, Result, Const(opCode.Imm), 3, 0);

                ctx.SetVector(opCode.Rd, Result);
            }
            else
            {
                throw new Exception();
            }
        }

        public static void Fneg_Scalar(ArmEmitContext ctx)
        {
            SIMDOpCodeScalar1Src opCode = ctx.CurrentInstruction as SIMDOpCodeScalar1Src;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = ctx.GetVector(opCode.Rn);

                Xmm Mask = X86CreateXmmScalar(ctx, Const(1UL << (opCode.Size == OpCodeSize.s ? 31 : 63)), opCode.Size);

                ctx.EmitX86(X86Instruction.Vxorps, n,n,Mask);

                X86ScalariseVector(ctx, n, opCode.Size == OpCodeSize.s);

                ctx.SetVector(opCode.Rd, n);
            }
            else
            {
                throw new Exception();
            }
        }

        public static void Fabs_Scalar(ArmEmitContext ctx)
        {
            SIMDOpCodeScalar1Src opCode = ctx.CurrentInstruction as SIMDOpCodeScalar1Src;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = ctx.GetVector(opCode.Rn);

                Xmm Mask = X86CreateXmmScalar(ctx, Const(opCode.Size == OpCodeSize.s ? (uint.MaxValue >> 1) : (ulong.MaxValue >> 1)), opCode.Size);

                ctx.EmitX86(X86Instruction.Vandps, n, n, Mask);

                X86ScalariseVector(ctx, n, opCode.Size == OpCodeSize.s);

                ctx.SetVector(opCode.Rd, n);
            }
            else
            {
                throw new Exception();
            }
        }

        public static void Fsqrt_Scalar(ArmEmitContext ctx)
        {
            SIMDOpCodeScalar1Src opCode = ctx.CurrentInstruction as SIMDOpCodeScalar1Src;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = ctx.GetVector(opCode.Rn);

                ctx.EmitX86(opCode.Size == OpCodeSize.s ? X86Instruction.Sqrtps : X86Instruction.Sqrtpd, n, n);

                X86ScalariseVector(ctx, n, opCode.Size == OpCodeSize.s);

                ctx.SetVector(opCode.Rd, n);
            }
            else
            {
                throw new Exception();
            }
        }

        public static void Cnt(ArmEmitContext ctx)
        {
            SIMDOpCodeVector1Src opCode = ctx.CurrentInstruction as SIMDOpCodeVector1Src;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = ctx.GetVector(opCode.Rn, true);

                Xmm result = ctx.LocalVector();

                int size = (int)opCode.Size;

                int iterations = GetElementCount(size, opCode.Half);

                for (int i = 0; i < iterations; ++i)
                {
                    IOperand Byte = X86VectorExtract(ctx, n, size, i);

                    ctx.EmitX86(X86Instruction.Popcnt, Byte, Byte);

                    X86VectorInsert(ctx,result, Byte, size, i);
                }

                ctx.SetVector(opCode.Rd, result);
            }
            else
            {
                throw new Exception();
            }
        }

        public static void Uaddlv(ArmEmitContext ctx)
        {
            SIMDOpCodeVector1Src opCode = ctx.CurrentInstruction as SIMDOpCodeVector1Src;

            if (ctx.EnableX86Extentions)
            {
                int SourceSize = (int)opCode.Size;
                int ResultSize = SourceSize + 1;

                Xmm n = ctx.GetVector(opCode.Rn, true);

                int Iterations = GetElementCount(SourceSize, opCode.Half);

                IOperand Result = Const(0);

                for (int i = 0; i < Iterations; ++i)
                {
                    IOperand Source = X86VectorExtract(ctx, n, SourceSize, i);

                    Source = ctx.ZeroExtend(Source, IntSize.Int64);

                    Result = ctx.Add(Result, Source);
                }

                Xmm result = ctx.LocalVector();

                X86VectorInsert(ctx, result, Result, ResultSize, 0);

                ctx.SetVector(opCode.Rd, result);
            }
            else
            {
                throw new Exception();
            }
        }

        static int GetRoundingControl(RoundingMode Mode)
        {
            switch (Mode)
            {
                case RoundingMode.TowardNegInf: return 8 | 1; 
                default: throw new Exception(); 
            }
        }

        public static void EmitFrint(ArmEmitContext ctx, RoundingMode Mode)
        {
            SIMDOpCodeScalar1Src opCode = ctx.CurrentInstruction as SIMDOpCodeScalar1Src;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = ctx.GetVector(opCode.Rn, true);

                Xmm Result = ctx.LocalVector(false);

                ctx.EmitX86(opCode.Size == OpCodeSize.s ? X86Instruction.Vroundps : X86Instruction.Vroundpd, Result, n, Const(GetRoundingControl(Mode)));

                X86ScalariseVector(ctx, Result, opCode.Size == OpCodeSize.s);

                ctx.SetVector(opCode.Rd, Result);
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
