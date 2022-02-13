using ArmLIB.Dissasembler.Aarch64.HighLevel;
using ArmLIB.Emulator.Aarch64.Fallbacks;
using Compiler.Intermediate;
using Compiler.Intermediate.Extensions.X86;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64.Translation
{
    public static unsafe partial class InstEmit64
    {
        public static void Scvtf_Scalar(ArmEmitContext ctx) => EmitConvertToFloat(ctx, (delegate*<long, OpCodeSize, OpCodeSize, ulong>)&FallbackFloat.ConvertToFloatSigned);
        public static void Ucvtf_Scalar(ArmEmitContext ctx) => EmitConvertToFloat(ctx, (delegate*<ulong, OpCodeSize, OpCodeSize, ulong>)&FallbackFloat.ConvertToFloatUnsigned);
        public static void Scvtf_Vector(ArmEmitContext ctx) => EmitConvertToFloatVector(ctx, (delegate*<long, OpCodeSize, OpCodeSize, ulong>)&FallbackFloat.ConvertToFloatSigned);
        public static void Ucvtf_Vector(ArmEmitContext ctx) => EmitConvertToFloatVector(ctx, (delegate*<ulong, OpCodeSize, OpCodeSize, ulong>)&FallbackFloat.ConvertToFloatUnsigned);

        public static void Fcvtas_Scalar(ArmEmitContext ctx) => EmitConvertToInt(ctx, RoundingMode.AwayFromZero, (delegate*<ulong, OpCodeSize, OpCodeSize, RoundingMode, ulong>)&FallbackFloat.ConvertToSignedInt);
        public static void Fcvtau_Scalar(ArmEmitContext ctx) => EmitConvertToInt(ctx, RoundingMode.AwayFromZero, (delegate*<ulong, OpCodeSize, OpCodeSize, RoundingMode, ulong>)&FallbackFloat.ConvertToUnsignedInt);
        public static void Fcvtms_Scalar(ArmEmitContext ctx) => EmitConvertToInt(ctx, RoundingMode.TowardNegInf, (delegate*<ulong, OpCodeSize, OpCodeSize, RoundingMode, ulong>)&FallbackFloat.ConvertToSignedInt);
        public static void Fcvtmu_Scalar(ArmEmitContext ctx) => EmitConvertToInt(ctx, RoundingMode.TowardNegInf, (delegate*<ulong, OpCodeSize, OpCodeSize, RoundingMode, ulong>)&FallbackFloat.ConvertToUnsignedInt);
        public static void Fcvtps_Scalar(ArmEmitContext ctx) => EmitConvertToInt(ctx, RoundingMode.TowardPosInf, (delegate*<ulong, OpCodeSize, OpCodeSize, RoundingMode, ulong>)&FallbackFloat.ConvertToSignedInt);
        public static void Fcvtpu_Scalar(ArmEmitContext ctx) => EmitConvertToInt(ctx, RoundingMode.TowardPosInf, (delegate*<ulong, OpCodeSize, OpCodeSize, RoundingMode, ulong>)&FallbackFloat.ConvertToUnsignedInt);
        public static void Fcvtzs_Scalar(ArmEmitContext ctx) => EmitConvertToInt(ctx, RoundingMode.TowardZero, (delegate*<ulong, OpCodeSize, OpCodeSize, RoundingMode, ulong>)&FallbackFloat.ConvertToSignedInt);
        public static void Fcvtzu_Scalar(ArmEmitContext ctx) => EmitConvertToInt(ctx, RoundingMode.TowardZero, (delegate*<ulong, OpCodeSize, OpCodeSize, RoundingMode, ulong>)&FallbackFloat.ConvertToUnsignedInt);

        public static void Fcvt(ArmEmitContext ctx)
        {
            SIMDOpCodeScalar1Src opCode = ctx.CurrentInstruction as SIMDOpCodeScalar1Src;

            if (ctx.EnableX86Extentions)
            {
                OpCodeSize SourceSize = opCode.Size;

                Xmm Working = ctx.GetVector(opCode.Rn);

                if (SourceSize == OpCodeSize.s)
                {
                    ctx.EmitX86(X86Instruction.Cvtss2sd, Working, Working);
                }
                else
                {
                    ctx.EmitX86(X86Instruction.Cvtsd2ss, Working, Working);
                }

                X86ScalariseVector(ctx, Working, SourceSize == OpCodeSize.d);

                ctx.SetVector(opCode.Rd, Working);
            }
            else
            {
                throw new Exception();
            }
        }

        public static void Fmov_Scalar_Gp(ArmEmitContext ctx)
        {
            SIMDOpCodeFmov opCode = ctx.CurrentInstruction as SIMDOpCodeFmov;

            if (ctx.EnableX86Extentions)
            {
                if (opCode.ToVector)
                {
                    IOperand n = GetN(ctx);

                    if (opCode.Size == OpCodeSize.s)
                        n = ctx.LogicalAnd(n, Const(uint.MaxValue));

                    Xmm Result = ctx.LocalVector();

                    X86VectorInsert(ctx, Result, n, 3, 0);

                    ctx.SetVector(opCode.Rd, Result);
                }
                else
                {
                    Xmm n = ctx.GetVector(opCode.Rn);

                    IOperand Result = X86VectorExtract(ctx, n, 3, 0);

                    if (opCode.Size == OpCodeSize.s)
                        Result = ctx.LogicalAnd(Result, Const(uint.MaxValue));

                    SetD(ctx, Result);
                }
            }
            else
            {
                throw new Exception();
            }
        }

        public static void Ins_General(ArmEmitContext ctx)
        {
            SIMDOpCodeInsertElement opCode = ctx.CurrentInstruction as SIMDOpCodeInsertElement;

            if (ctx.EnableX86Extentions)
            {
                Debug.Assert(opCode.Mode == SIMDInstructionMode.MoveToScalar);

                IOperand n = GetN(ctx);

                Xmm CurrentD = ctx.GetVector(opCode.Rd);

                X86VectorInsert(ctx, CurrentD, n, (int)opCode.Size, opCode.Index);

                ctx.SetVector(opCode.Rd, CurrentD);
            }
            else
            {
                throw new Exception();
            }
        }

        public static void Ins_Element(ArmEmitContext ctx)
        {
            SIDMOpCodeInsertVectorElement opCode = ctx.CurrentInstruction as SIDMOpCodeInsertVectorElement;

            if (ctx.EnableX86Extentions)
            {
                Xmm d = ctx.GetVector(opCode.Rd);
                Xmm n = ctx.GetVector(opCode.Rn);

                X86VectorInsert(ctx, d, X86VectorExtract(ctx, n, (int)opCode.Size, opCode.SourceIndex), (int)opCode.Size, opCode.DestinationIndex);

                ctx.SetVector(opCode.Rd, d);
            }
            else
            {
                throw new Exception();
            }
        }

        //mark
        static unsafe void EmitConvertToFloat(ArmEmitContext ctx, void* Fallback)
        {
            SIMDOpCodeIntegerFloatConversionScalar opCode = ctx.CurrentInstruction as SIMDOpCodeIntegerFloatConversionScalar;

            if (ctx.EnableX86Extentions)
            {
                //TODO: optimize with avx

                IOperand n;

                switch (opCode.Mode)
                {
                    case SIMDInstructionMode.IntToFloat: n = GetN(ctx); break;
                    case SIMDInstructionMode.IntToFloatVector: n = X86VectorExtract(ctx, ctx.GetVector(opCode.Rn, true), 3, 0); break;
                    default: throw new Exception();
                }

                IOperand ConvertedValue = Call(ctx, Fallback, n, Const((int)opCode.DesSize), Const((int)opCode.SourceSize));

                Xmm Result = ctx.LocalVector();

                X86VectorInsert(ctx, Result, ConvertedValue, 3, 0);

                ctx.SetVector(opCode.Rd, Result);
            }
            else
            {
                throw new Exception();
            }
        }

        static unsafe void EmitConvertToFloatVector(ArmEmitContext ctx, void* Fallback)
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
                    IOperand WorkingWith = X86VectorExtract(ctx, n, size, i);

                    WorkingWith = Call(ctx, Fallback, WorkingWith, Const(size), Const(size));

                    X86VectorInsert(ctx, result, WorkingWith, size, i);
                }

                ctx.SetVector(opCode.Rd, result);
            }
            else
            {
                throw new Exception();
            }
        }

        static unsafe void EmitConvertToInt(ArmEmitContext ctx, RoundingMode rounding,void* Fallback)
        {
            SIMDOpCodeIntegerFloatConversionScalar opCode = ctx.CurrentInstruction as SIMDOpCodeIntegerFloatConversionScalar;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = ctx.GetVector(opCode.Rn, opCode.Mode != SIMDInstructionMode.FloatToFixed);

                if (opCode.Mode == SIMDInstructionMode.FloatToFixed)
                {
                    Xmm Multiplier = X86CreateXmmScalar(ctx,opCode.SourceSize,Math.Pow(2.0d, opCode.FixedShift));

                    ctx.EmitX86(opCode.SourceSize == OpCodeSize.s ? X86Instruction.Vmulps : X86Instruction.Vmulpd, n, n, Multiplier);
                }

                IOperand ConvertedValue = Call(ctx, Fallback, X86VectorExtract(ctx, n, 3, 0), Const((int)opCode.DesSize), Const((int)opCode.SourceSize), Const((int)rounding));

                SetD(ctx, ConvertedValue);
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
