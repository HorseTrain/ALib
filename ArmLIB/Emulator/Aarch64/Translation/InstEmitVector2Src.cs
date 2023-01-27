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
    enum WorkWithResult
    {
        Nothing,
        Add,
        Subtract,
    }

    public static unsafe partial class InstEmit64
    {
        static X86Instruction[] AddVector = new X86Instruction[]
        {
            X86Instruction.Vpaddb,
            X86Instruction.Vpaddw,
            X86Instruction.Vpaddd,
            X86Instruction.Vpaddq
        };

        static X86Instruction[] SubVector = new X86Instruction[]
        {
            X86Instruction.Vpsubb,
            X86Instruction.Vpsubw,
            X86Instruction.Vpsubd,
            X86Instruction.Vpsubq
        };

        public static void Add_Vector(ArmEmitContext ctx) => EmitVectorSameWithSize(ctx, AddVector);
        public static void And_Vector(ArmEmitContext ctx) => EmitVectorLogicalSame(ctx, X86Instruction.Vandps, false);
        public static void Bic_Vector(ArmEmitContext ctx) => EmitVectorLogicalSame(ctx, X86Instruction.Vandps, true);
        public static void Cmeq(ArmEmitContext ctx) => EmitCM(ctx, Instruction.CompareEqual);
        public static void Cmhi(ArmEmitContext ctx) => EmitCM(ctx, Instruction.CompareGreater);
        public static void Eor_Vector(ArmEmitContext ctx) => EmitVectorLogicalSame(ctx, X86Instruction.Vxorps, false);
        public static void Fadd_Scalar(ArmEmitContext ctx) => EmitVectorScalar2Src(ctx, X86Instruction.Vaddps, X86Instruction.Vaddpd);
        public static void Fadd_Vector(ArmEmitContext ctx) => EmitVectorVector2Src(ctx, X86Instruction.Vaddps, X86Instruction.Vaddpd);
        public static void Fdiv_Scalar(ArmEmitContext ctx) => EmitVectorScalar2Src(ctx, X86Instruction.Vdivps, X86Instruction.Vdivpd);
        public static void Fdiv_Vector(ArmEmitContext ctx) => EmitVectorVector2Src(ctx, X86Instruction.Vdivps, X86Instruction.Vdivpd);
        public static void Fmax_Scalar(ArmEmitContext ctx) => EmitVectorScalar2Src(ctx, X86Instruction.Vmaxps, X86Instruction.Vmaxpd);
        public static void Fmax_Vector(ArmEmitContext ctx) => EmitVectorVector2Src(ctx, X86Instruction.Vmaxps, X86Instruction.Vmaxpd);
        public static void Fmin_Scalar(ArmEmitContext ctx) => EmitVectorScalar2Src(ctx, X86Instruction.Vminps, X86Instruction.Vminpd);
        public static void Fmin_Vector(ArmEmitContext ctx) => EmitVectorVector2Src(ctx, X86Instruction.Vminps, X86Instruction.Vminpd);
        public static void Fmla_Scalar_Element(ArmEmitContext ctx) => EmitVectorScalar2SrcElement(ctx, X86Instruction.Vmulps, X86Instruction.Vmulpd, WorkWithResult.Add);
        public static void Fmla_Vector(ArmEmitContext ctx) => EmitVectorVector2Src(ctx, X86Instruction.Vmulps, X86Instruction.Vmulpd, WorkWithResult.Add);
        public static void Fmla_Vector_Element(ArmEmitContext ctx) => EmitVectorVector2SrcElement(ctx, X86Instruction.Vmulps, X86Instruction.Vmulpd, WorkWithResult.Add);
        public static void Fmls_Scalar_Element(ArmEmitContext ctx) => EmitVectorScalar2SrcElement(ctx, X86Instruction.Vmulps, X86Instruction.Vmulpd, WorkWithResult.Subtract);
        public static void Fmls_Vector(ArmEmitContext ctx) => EmitVectorVector2Src(ctx, X86Instruction.Vmulps, X86Instruction.Vmulpd, WorkWithResult.Subtract);
        public static void Fmls_Vector_Element(ArmEmitContext ctx) => EmitVectorVector2SrcElement(ctx, X86Instruction.Vmulps, X86Instruction.Vmulpd, WorkWithResult.Subtract);
        public static void Fmul_Scalar(ArmEmitContext ctx) => EmitVectorScalar2Src(ctx, X86Instruction.Vmulps, X86Instruction.Vmulpd);
        public static void Fmul_Scalar_Element(ArmEmitContext ctx) => EmitVectorScalar2SrcElement(ctx, X86Instruction.Vmulps, X86Instruction.Vmulpd);
        public static void Fmul_Vector(ArmEmitContext ctx) => EmitVectorVector2Src(ctx, X86Instruction.Vmulps, X86Instruction.Vmulpd);
        public static void Fmul_Vector_Element(ArmEmitContext ctx) => EmitVectorVector2SrcElement(ctx, X86Instruction.Vmulps, X86Instruction.Vmulpd);
        public static void Fnmul_Scalar(ArmEmitContext ctx) => EmitVectorScalar2Src(ctx, X86Instruction.Vmulps, X86Instruction.Vmulpd, true);
        public static void Fsub_Scalar(ArmEmitContext ctx) => EmitVectorScalar2Src(ctx, X86Instruction.Vsubps, X86Instruction.Vsubpd);
        public static void Fsub_Vector(ArmEmitContext ctx) => EmitVectorVector2Src(ctx, X86Instruction.Vsubps, X86Instruction.Vsubpd);
        public static void Orn_Vector(ArmEmitContext ctx) => EmitVectorLogicalSame(ctx, X86Instruction.Vorps, true);
        public static void Orr_Vector(ArmEmitContext ctx) => EmitVectorLogicalSame(ctx, X86Instruction.Vorps, false);
        public static void Sub_Vector(ArmEmitContext ctx) => EmitVectorSameWithSize(ctx, SubVector);
        public static void Faddp_Vector(ArmEmitContext ctx) => EmitVectorPairwise(ctx, X86Instruction.Vaddps, X86Instruction.Vaddpd);
        public static void Zip1(ArmEmitContext ctx) => EmitZip(ctx, 0);
        public static void Zip2(ArmEmitContext ctx) => EmitZip(ctx, 1);

        static List<Xmm> ConcactVectors(ArmEmitContext ctx, bool Half,Xmm n, Xmm m)
        {
            if (Half)
            {
                Xmm Temp = ctx.LocalVector(false);

                Elm(ctx, Temp, Elm(ctx, n, 3, 0), 3, 0);
                Elm(ctx, Temp, Elm(ctx, m, 3, 0), 3, 1);

                return new List<Xmm>() { Temp };
            }
            else
            {
                return new List<Xmm>() { n, m };
            }
        }

        public static Xmm X86GetVectorN(ArmEmitContext ctx)
        {
            int rn = (ctx.CurrentInstruction as IOpCodeRn).Rn;

            return ctx.GetVector(rn, true);
        }

        public static Xmm X86GetVectorD(ArmEmitContext ctx, bool IsReference)
        {
            int rd = (ctx.CurrentInstruction as IOpCodeRd).Rd;

            return ctx.GetVector(rd, !IsReference);
        }

        public static Xmm X86GetVectorM(ArmEmitContext ctx, bool NegateAll)
        {
            Xmm Result;

            switch (ctx.CurrentInstruction)
            {
                case SIMDOpCodeScalar2Src op: Result = ctx.GetVector(op.Rm, true); break;
                case SIMDOpCodeVector2Src op: Result = ctx.GetVector(op.Rm, true); break;
                case SIMDOpCodeScalar2SrcElement op:
                    {
                        IOperand WorkingValue = Elm(ctx, ctx.GetVector(op.Rm, false), (int)op.Size, op.Element);

                        Result = ctx.LocalVector(false);

                        Elm(ctx, Result, WorkingValue, (int)op.Size, 0);

                    }; break;

                case SIMDOpCodeVector2SrcElement op:
                    {
                        IOperand WorkingValue = Elm(ctx, ctx.GetVector(op.Rm, false), (int)op.Size, op.Element);

                        Result = ctx.LocalVector(false);

                        int ElementCount = GetElementCount((int)op.Size, op.Half);

                        for (int i = 0; i < ElementCount; ++i)
                        {
                            Elm(ctx, Result, WorkingValue, (int)op.Size, i);
                        }

                    } break;
                default: throw new Exception();
            }

            if (NegateAll)
            {
                OpCodeSize size = (ctx.CurrentInstruction as OpCodeVectorSize).Size;

                Xmm Negator = ctx.LocalVector(false);

                int Iterations = GetElementCount((int)size, false);

                ulong MaskImm = 1UL << ((8 << ((int)size)) - 1);

                for (int i = 0; i < Iterations; ++i)
                {
                    Elm(ctx, Negator, Const(MaskImm), (int)size, i);
                }

                Result = ctx.CopyVector(Result);

                ctx.EmitX86(X86Instruction.Vxorps, Result, Result, Negator);
            }

            return Result;
        }

        static void ModifyResult(ArmEmitContext ctx, Xmm Result, WorkWithResult Working)
        {
            if (Working == WorkWithResult.Nothing)
                return;

            OpCodeSize size = (ctx.CurrentInstruction as OpCodeVectorSize).Size;

            int rd = (ctx.CurrentInstruction as IOpCodeRd).Rd;

            Xmm d = ctx.GetVector(rd, true);

            switch (Working)
            {
                case WorkWithResult.Add: ctx.EmitX86(size == OpCodeSize.d ? X86Instruction.Vaddpd : X86Instruction.Vaddps, Result, d, Result); break;
                case WorkWithResult.Subtract: ctx.EmitX86(size == OpCodeSize.d ? X86Instruction.Vsubpd : X86Instruction.Vsubps, Result, d, Result); break;
                default: throw new Exception();
            }
        }

        static Xmm X86InvertVector(ArmEmitContext ctx, Xmm Source)
        {
            Xmm Out = ctx.LocalVector();

            for (int i = 0; i < 2; ++i)
            {
                IOperand Element = Elm(ctx, Source, 3, i);

                Element = ctx.Not(Element);

                Elm(ctx, Out, Element, 3, i);
            }

            return Out;
        }

        static void X86ClearVectorTopIfNeeded(ArmEmitContext ctx, Xmm Source, bool Half)
        {
            if (Half)
                Elm(ctx, Source, Const(0), 3, 1);
        }

        static void X86ScalariseVector(ArmEmitContext ctx, Xmm Source, bool IsSingle)
        {
            X86ClearVectorTopIfNeeded(ctx, Source, true);

            if (IsSingle)
            {
                Elm(ctx, Source, Const(0), 2, 1);
            }
        }

        static void EmitVectorLogicalSame(ArmEmitContext ctx, X86Instruction Instruction, bool Invert)
        {
            SIMDOpCodeVector2Src opCode = ctx.CurrentInstruction as SIMDOpCodeVector2Src;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = ctx.GetVector(opCode.Rn, true);
                Xmm m = ctx.GetVector(opCode.Rm, true);

                if (Invert)
                    m = X86InvertVector(ctx, m);

                Xmm d = ctx.LocalVector(false);

                ctx.EmitX86(Instruction, d, n, m);

                X86ClearVectorTopIfNeeded(ctx, d, opCode.Half);

                ctx.SetVector(opCode.Rd, d);
            }
            else
            {
                throw new Exception();
            }
        }

        public static void Frsqrts_Vector(ArmEmitContext ctx)
        {
            SIMDOpCodeVector2Src opCode = ctx.CurrentInstruction as SIMDOpCodeVector2Src;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = ctx.GetVector(opCode.Rn, true);
                Xmm m = ctx.GetVector(opCode.Rm, true);

                X86Instruction MulIns = opCode.Size == OpCodeSize.d ? X86Instruction.Vmulpd : X86Instruction.Vmulps;
                X86Instruction SubIns = opCode.Size == OpCodeSize.d ? X86Instruction.Vsubpd : X86Instruction.Vsubps;

                int ImmElementCount = GetElementCount((int)opCode.Size, opCode.Half);

                Xmm half = ctx.LocalVector(false);
                Xmm three = ctx.LocalVector(false);


                for (int i= 0; i < ImmElementCount; ++i)
                {
                    Elm(ctx, half, Const(opCode.Size == OpCodeSize.s ? Aarch64DebugAssembler.Convert<uint, float>(0.5f) : Aarch64DebugAssembler.Convert<ulong, double>(0.5)), (int)opCode.Size, i);
                    Elm(ctx, three, Const(opCode.Size == OpCodeSize.s ? Aarch64DebugAssembler.Convert<uint, float>(3.0f) : Aarch64DebugAssembler.Convert<ulong, double>(3.0)), (int)opCode.Size, i);
                }

                Xmm WorkingResult = ctx.LocalVector(false);

                ctx.EmitX86(MulIns, WorkingResult, n, m);
                ctx.EmitX86(SubIns, WorkingResult, three, WorkingResult);
                ctx.EmitX86(MulIns, WorkingResult, half, WorkingResult);

                X86ClearVectorTopIfNeeded(ctx, WorkingResult, opCode.Half);

                ctx.SetVector(opCode.Rd, WorkingResult);
            }
            else
            {
                throw new Exception();
            }
        }

        public static void Bsl(ArmEmitContext ctx)
        {
            SIMDOpCodeVector2Src opCode = ctx.CurrentInstruction as SIMDOpCodeVector2Src;

            if (ctx.EnableX86Extentions)
            {
                Xmm m = ctx.GetVector(opCode.Rm, true);
                Xmm d = ctx.GetVector(opCode.Rd, true);
                Xmm n = ctx.GetVector(opCode.Rn, true);

                Xmm t0 = ctx.LocalVector();
                Xmm t1 = ctx.LocalVector();
                Xmm result = ctx.LocalVector();

                ctx.EmitX86(X86Instruction.Vxorps, t0, m, n);
                ctx.EmitX86(X86Instruction.Vandps, t1, t0, d);
                ctx.EmitX86(X86Instruction.Vxorps, result, t1, m);

                X86ClearVectorTopIfNeeded(ctx, result, opCode.Half);

                ctx.SetVector(opCode.Rd, result);
            }
            else
            {
                throw new Exception();
            }
        }

        static void EmitVectorSameWithSize(ArmEmitContext ctx, X86Instruction[] InstructionSources)
        {
            SIMDOpCodeVector2Src opCode = ctx.CurrentInstruction as SIMDOpCodeVector2Src;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = ctx.GetVector(opCode.Rn, true);
                Xmm m = ctx.GetVector(opCode.Rm, true);

                Xmm d = ctx.LocalVector(false);

                ctx.EmitX86(InstructionSources[(int)opCode.Size], d, n, m);

                X86ClearVectorTopIfNeeded(ctx, d, opCode.Half);

                ctx.SetVector(opCode.Rd, d);
            }
            else
            {
                throw new Exception();
            }
        }

        //Result is scalar.
        static void EmitVectorScalar2Src(ArmEmitContext ctx, X86Instruction SingleInstruction, X86Instruction DoubleInstruction, bool NegateM = false)
        {
            SIMDOpCodeScalar2Src opCode = ctx.CurrentInstruction as SIMDOpCodeScalar2Src;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = X86GetVectorN(ctx);
                Xmm m = X86GetVectorM(ctx, NegateM);

                Xmm result = X86GetVectorD(ctx, false);

                ctx.EmitX86(opCode.Size == OpCodeSize.d ? DoubleInstruction : SingleInstruction, result, n, m);

                X86ScalariseVector(ctx, result, opCode.Size == OpCodeSize.s);
            }
            else
            {
                throw new Exception();
            }
        }

        static void EmitVectorScalar2SrcElement(ArmEmitContext ctx, X86Instruction SingleInstruction, X86Instruction DoubleInstruction, WorkWithResult WorkWithResult = WorkWithResult.Nothing)
        {
            SIMDOpCodeScalar2SrcElement opCode = ctx.CurrentInstruction as SIMDOpCodeScalar2SrcElement;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = X86GetVectorN(ctx);
                Xmm m = X86GetVectorM(ctx, false);

                Xmm result = ctx.LocalVector(false);

                ctx.EmitX86(opCode.Size == OpCodeSize.d ? DoubleInstruction : SingleInstruction, result, n, m);

                ModifyResult(ctx, result, WorkWithResult);

                X86ScalariseVector(ctx, result, opCode.Size == OpCodeSize.s);

                ctx.SetVector(opCode.Rd, result);
            }
            else
            {
                throw new Exception();
            }
        }

        //Result Vector
        static void EmitVectorVector2Src(ArmEmitContext ctx, X86Instruction SingleInstruction, X86Instruction DoubleInstruction, WorkWithResult WorkWithResult = WorkWithResult.Nothing)
        {
            SIMDOpCodeVector2Src opCode = ctx.CurrentInstruction as SIMDOpCodeVector2Src;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = X86GetVectorN(ctx);
                Xmm m = X86GetVectorM(ctx, false);

                Xmm result = ctx.LocalVector(false);

                ctx.EmitX86(opCode.Size == OpCodeSize.d ? DoubleInstruction : SingleInstruction, result, n, m);

                ModifyResult(ctx, result, WorkWithResult);

                X86ClearVectorTopIfNeeded(ctx, result, opCode.Half);

                ctx.SetVector(opCode.Rd, result);
            }
            else
            {
                throw new Exception();
            }
        }

        static void EmitVectorVector2SrcElement(ArmEmitContext ctx, X86Instruction SingleInstruction, X86Instruction DoubleInstruction, WorkWithResult WorkWithResult = WorkWithResult.Nothing)
        {
            SIMDOpCodeVector2SrcElement opCode = ctx.CurrentInstruction as SIMDOpCodeVector2SrcElement;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = X86GetVectorN(ctx);
                Xmm m = X86GetVectorM(ctx, false);

                Xmm result = ctx.LocalVector(false);

                ctx.EmitX86(opCode.Size == OpCodeSize.d ? DoubleInstruction : SingleInstruction, result, n, m);

                ModifyResult(ctx, result, WorkWithResult);

                X86ClearVectorTopIfNeeded(ctx, result, opCode.Half);

                ctx.SetVector(opCode.Rd, result);
            }
            else
            {
                throw new Exception();
            }
        }

        static void EmitVectorPairwise(ArmEmitContext ctx, X86Instruction SingleInstruction, X86Instruction DoubleInstruction)
        {
            SIMDOpCodeVector2Src opCode = ctx.CurrentInstruction as SIMDOpCodeVector2Src;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = X86GetVectorN(ctx);
                Xmm m = X86GetVectorM(ctx, false);

                int Size = (int)opCode.Size;

                int ElementCount = GetElementCount(Size, opCode.Half);

                Xmm Result = ctx.LocalVector(false);

                List<Xmm> Concact = ConcactVectors(ctx, opCode.Half, n, m);

                for (int i = 0; i < ElementCount; ++i)
                {
                    Xmm elm0 = X86CreateXmmScalar(ctx, X86VectorExtract(ctx, Concact, Size, 2 * i), opCode.Size);
                    Xmm elm1 = X86CreateXmmScalar(ctx, X86VectorExtract(ctx, Concact, Size, (2 * i) + 1), opCode.Size);

                    Xmm resv = ctx.LocalVector(false);

                    ctx.EmitX86(opCode.Size == OpCodeSize.s ? SingleInstruction : DoubleInstruction, resv, elm0, elm1);

                    IOperand elmRes = Elm(ctx, resv, Size, 0);

                    Elm(ctx, Result, elmRes, Size, i);
                }

                X86ClearVectorTopIfNeeded(ctx, Result, opCode.Half);

                ctx.SetVector(opCode.Rd, Result);
            }
            else
            {
                throw new Exception();
            }
        }

        static void EmitZip(ArmEmitContext ctx, int part)
        {
            SIMDOpCodeVector2Src opCode = ctx.CurrentInstruction as SIMDOpCodeVector2Src;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = X86GetVectorN(ctx);
                Xmm m = X86GetVectorM(ctx, false);

                int Size = (int)opCode.Size;

                int elements = GetElementCount(Size, opCode.Half);
                int pairs = elements / 2;

                int _base = part * pairs;

                Xmm result = ctx.LocalVector(false);

                for (int p = 0; p < pairs; ++p)
                {
                    Elm(ctx, result, Elm(ctx, n, Size, _base + p), Size, 2 * p);
                    Elm(ctx, result, Elm(ctx, m, Size, _base + p), Size, (2 * p) + 1);
                }

                X86ClearVectorTopIfNeeded(ctx, result, opCode.Half);

                ctx.SetVector(opCode.Rd, result);
            }
            else
            {
                throw new Exception();
            }
        }

        static void EmitCM(ArmEmitContext ctx, Instruction CompareInstruction)
        {
            SIMDOpCodeVector2Src opCode = ctx.CurrentInstruction as SIMDOpCodeVector2Src;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = ctx.GetVector(opCode.Rn, true);
                Xmm m = ctx.GetVector(opCode.Rm, true);

                Xmm result = ctx.LocalVector();

                int Size = (int)opCode.Size;

                int Iterations = GetElementCount(Size, opCode.Half);

                for (int i = 0; i < Iterations; ++i)
                {
                    IOperand n_Extract = ctx.ZeroExtend(Elm(ctx, n, Size, i), OperandType.Int64);
                    IOperand m_Extract = ctx.ZeroExtend(Elm(ctx, m, Size, i), OperandType.Int64);

                    IOperandReg Result = ctx.Local();

                    ctx.ir.Emit(CompareInstruction, Result, n_Extract, m_Extract);

                    Elm(ctx, result, ctx.Subtract(Const(0), Result), Size, i);
                }

                ctx.SetVector(opCode.Rd, result);
            }
            else
            {
                throw new Exception();
            }
        }

        public static void Ext(ArmEmitContext ctx)
        {
            SIMDOpCodeVectorExtract opCode = ctx.CurrentInstruction as SIMDOpCodeVectorExtract;

            if (ctx.EnableX86Extentions)
            {
                Xmm n = ctx.GetVector(opCode.Rn);
                Xmm m = ctx.GetVector(opCode.Rm);

                List<Xmm> Concact = ConcactVectors(ctx, opCode.Half, n, m);

                Xmm result = ctx.LocalVector(true);

                int iterations = GetElementCount(0, opCode.Half);

                for (int i = 0; i < iterations; ++i)
                {
                    int offset = (int)opCode.Imm + i;

                    Elm(ctx, result, X86VectorExtract(ctx, Concact, 0, offset), 0, i);
                }

                ctx.SetVector(opCode.Rd, result);
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
