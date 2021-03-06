using DCpu.Decoders;
using DCpu.IntermediateRepresentation;
using DCpu.State;
using DCpu.Translation;
using System;
using System.Diagnostics;

using static DCpu.Instructions.InstEmitHelper;
using static DCpu.IntermediateRepresentation.OperandHelper;

namespace DCpu.Instructions
{
    using Func1I = Func<Operand, Operand>;
    using Func2I = Func<Operand, Operand, Operand>;
    using Func3I = Func<Operand, Operand, Operand, Operand>;

    static class InstEmitSimdHelper
    {
#region "X86 SSE Intrinsics"
        public static readonly Intrinsic[] X86PaddInstruction = new Intrinsic[]
        {
            Intrinsic.X86Paddb,
            Intrinsic.X86Paddw,
            Intrinsic.X86Paddd,
            Intrinsic.X86Paddq
        };

        public static readonly Intrinsic[] X86PcmpeqInstruction = new Intrinsic[]
        {
            Intrinsic.X86Pcmpeqb,
            Intrinsic.X86Pcmpeqw,
            Intrinsic.X86Pcmpeqd,
            Intrinsic.X86Pcmpeqq
        };

        public static readonly Intrinsic[] X86PcmpgtInstruction = new Intrinsic[]
        {
            Intrinsic.X86Pcmpgtb,
            Intrinsic.X86Pcmpgtw,
            Intrinsic.X86Pcmpgtd,
            Intrinsic.X86Pcmpgtq
        };

        public static readonly Intrinsic[] X86PmaxsInstruction = new Intrinsic[]
        {
            Intrinsic.X86Pmaxsb,
            Intrinsic.X86Pmaxsw,
            Intrinsic.X86Pmaxsd
        };

        public static readonly Intrinsic[] X86PmaxuInstruction = new Intrinsic[]
        {
            Intrinsic.X86Pmaxub,
            Intrinsic.X86Pmaxuw,
            Intrinsic.X86Pmaxud
        };

        public static readonly Intrinsic[] X86PminsInstruction = new Intrinsic[]
        {
            Intrinsic.X86Pminsb,
            Intrinsic.X86Pminsw,
            Intrinsic.X86Pminsd
        };

        public static readonly Intrinsic[] X86PminuInstruction = new Intrinsic[]
        {
            Intrinsic.X86Pminub,
            Intrinsic.X86Pminuw,
            Intrinsic.X86Pminud
        };

        public static readonly Intrinsic[] X86PmovsxInstruction = new Intrinsic[]
        {
            Intrinsic.X86Pmovsxbw,
            Intrinsic.X86Pmovsxwd,
            Intrinsic.X86Pmovsxdq
        };

        public static readonly Intrinsic[] X86PmovzxInstruction = new Intrinsic[]
        {
            Intrinsic.X86Pmovzxbw,
            Intrinsic.X86Pmovzxwd,
            Intrinsic.X86Pmovzxdq
        };

        public static readonly Intrinsic[] X86PsllInstruction = new Intrinsic[]
        {
            0,
            Intrinsic.X86Psllw,
            Intrinsic.X86Pslld,
            Intrinsic.X86Psllq
        };

        public static readonly Intrinsic[] X86PsraInstruction = new Intrinsic[]
        {
            0,
            Intrinsic.X86Psraw,
            Intrinsic.X86Psrad
        };

        public static readonly Intrinsic[] X86PsrlInstruction = new Intrinsic[]
        {
            0,
            Intrinsic.X86Psrlw,
            Intrinsic.X86Psrld,
            Intrinsic.X86Psrlq
        };

        public static readonly Intrinsic[] X86PsubInstruction = new Intrinsic[]
        {
            Intrinsic.X86Psubb,
            Intrinsic.X86Psubw,
            Intrinsic.X86Psubd,
            Intrinsic.X86Psubq
        };

        public static readonly Intrinsic[] X86PunpckhInstruction = new Intrinsic[]
        {
            Intrinsic.X86Punpckhbw,
            Intrinsic.X86Punpckhwd,
            Intrinsic.X86Punpckhdq,
            Intrinsic.X86Punpckhqdq
        };

        public static readonly Intrinsic[] X86PunpcklInstruction = new Intrinsic[]
        {
            Intrinsic.X86Punpcklbw,
            Intrinsic.X86Punpcklwd,
            Intrinsic.X86Punpckldq,
            Intrinsic.X86Punpcklqdq
        };
#endregion

        public static int GetImmShl(OpCodeSimdShImm op)
        {
            return op.Imm - (8 << op.Size);
        }

        public static int GetImmShr(OpCodeSimdShImm op)
        {
            return (8 << (op.Size + 1)) - op.Imm;
        }

        public static Operand X86GetScalar(ArmEmitterContext context, float value)
        {
            return X86GetScalar(context, BitConverter.SingleToInt32Bits(value));
        }

        public static Operand X86GetScalar(ArmEmitterContext context, double value)
        {
            return X86GetScalar(context, BitConverter.DoubleToInt64Bits(value));
        }

        public static Operand X86GetScalar(ArmEmitterContext context, int value)
        {
            return context.VectorCreateScalar(Const(value));
        }

        public static Operand X86GetScalar(ArmEmitterContext context, long value)
        {
            return context.VectorCreateScalar(Const(value));
        }

        public static Operand X86GetAllElements(ArmEmitterContext context, float value)
        {
            return X86GetAllElements(context, BitConverter.SingleToInt32Bits(value));
        }

        public static Operand X86GetAllElements(ArmEmitterContext context, double value)
        {
            return X86GetAllElements(context, BitConverter.DoubleToInt64Bits(value));
        }

        public static Operand X86GetAllElements(ArmEmitterContext context, int value)
        {
            Operand vector = context.VectorCreateScalar(Const(value));

            vector = context.AddIntrinsic(Intrinsic.X86Shufps, vector, vector, Const(0));

            return vector;
        }

        public static Operand X86GetAllElements(ArmEmitterContext context, long value)
        {
            Operand vector = context.VectorCreateScalar(Const(value));

            vector = context.AddIntrinsic(Intrinsic.X86Movlhps, vector, vector);

            return vector;
        }

        public static int X86GetRoundControl(FPRoundingMode roundMode)
        {
            switch (roundMode)
            {
                case FPRoundingMode.ToNearest:            return 8 | 0;
                case FPRoundingMode.TowardsPlusInfinity:  return 8 | 2;
                case FPRoundingMode.TowardsMinusInfinity: return 8 | 1;
                case FPRoundingMode.TowardsZero:          return 8 | 3;
            }

            throw new ArgumentException($"Invalid rounding mode \"{roundMode}\".");
        }

        public static void EmitScalarUnaryOpF(ArmEmitterContext context, Intrinsic inst32, Intrinsic inst64)
        {
            OpCodeSimd op = (OpCodeSimd)context.CurrOp;

            Operand n = GetVec(op.Rn);

            Intrinsic inst = (op.Size & 1) != 0 ? inst64 : inst32;

            Operand res = context.AddIntrinsic(inst, n);

            if ((op.Size & 1) != 0)
            {
                res = context.VectorZeroUpper64(res);
            }
            else
            {
                res = context.VectorZeroUpper96(res);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitScalarBinaryOpF(ArmEmitterContext context, Intrinsic inst32, Intrinsic inst64)
        {
            OpCodeSimdReg op = (OpCodeSimdReg)context.CurrOp;

            Operand n = GetVec(op.Rn);
            Operand m = GetVec(op.Rm);

            Intrinsic inst = (op.Size & 1) != 0 ? inst64 : inst32;

            Operand res = context.AddIntrinsic(inst, n, m);

            if ((op.Size & 1) != 0)
            {
                res = context.VectorZeroUpper64(res);
            }
            else
            {
                res = context.VectorZeroUpper96(res);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorUnaryOpF(ArmEmitterContext context, Intrinsic inst32, Intrinsic inst64)
        {
            OpCodeSimd op = (OpCodeSimd)context.CurrOp;

            Operand n = GetVec(op.Rn);

            Intrinsic inst = (op.Size & 1) != 0 ? inst64 : inst32;

            Operand res = context.AddIntrinsic(inst, n);

            if (op.RegisterSize == RegisterSize.Simd64)
            {
                res = context.VectorZeroUpper64(res);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorBinaryOpF(ArmEmitterContext context, Intrinsic inst32, Intrinsic inst64)
        {
            OpCodeSimdReg op = (OpCodeSimdReg)context.CurrOp;

            Operand n = GetVec(op.Rn);
            Operand m = GetVec(op.Rm);

            Intrinsic inst = (op.Size & 1) != 0 ? inst64 : inst32;

            Operand res = context.AddIntrinsic(inst, n, m);

            if (op.RegisterSize == RegisterSize.Simd64)
            {
                res = context.VectorZeroUpper64(res);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static Operand EmitUnaryMathCall(ArmEmitterContext context, _F32_F32 f32, _F64_F64 f64, Operand n)
        {
            IOpCodeSimd op = (IOpCodeSimd)context.CurrOp;

            return (op.Size & 1) == 0 ? context.Call(f32, n) : context.Call(f64, n);
        }

        public static Operand EmitRoundMathCall(ArmEmitterContext context, MidpointRounding roundMode, Operand n)
        {
            IOpCodeSimd op = (IOpCodeSimd)context.CurrOp;

            Delegate dlg;

            if ((op.Size & 1) == 0)
            {
                dlg = new _F32_F32_MidpointRounding(MathF.Round);
            }
            else /* if ((op.Size & 1) == 1) */
            {
                dlg = new _F64_F64_MidpointRounding(Math.Round);
            }

            return context.Call(dlg, n, Const((int)roundMode));
        }

        public static Operand EmitSoftFloatCall(
            ArmEmitterContext context,
            _F32_F32 f32,
            _F64_F64 f64,
            params Operand[] callArgs)
        {
            IOpCodeSimd op = (IOpCodeSimd)context.CurrOp;

            Delegate dlg = (op.Size & 1) == 0 ? (Delegate)f32 : (Delegate)f64;

            return context.Call(dlg, callArgs);
        }

        public static Operand EmitSoftFloatCall(
            ArmEmitterContext context,
            _F32_F32_F32 f32,
            _F64_F64_F64 f64,
            params Operand[] callArgs)
        {
            IOpCodeSimd op = (IOpCodeSimd)context.CurrOp;

            Delegate dlg = (op.Size & 1) == 0 ? (Delegate)f32 : (Delegate)f64;

            return context.Call(dlg, callArgs);
        }

        public static Operand EmitSoftFloatCall(
            ArmEmitterContext context,
            _F32_F32_F32_F32 f32,
            _F64_F64_F64_F64 f64,
            params Operand[] callArgs)
        {
            IOpCodeSimd op = (IOpCodeSimd)context.CurrOp;

            Delegate dlg = (op.Size & 1) == 0 ? (Delegate)f32 : (Delegate)f64;

            return context.Call(dlg, callArgs);
        }

        public static void EmitScalarBinaryOpByElemF(ArmEmitterContext context, Func2I emit)
        {
            OpCodeSimdRegElemF op = (OpCodeSimdRegElemF)context.CurrOp;

            OperandType type = (op.Size & 1) != 0 ? OperandType.FP64 : OperandType.FP32;

            Operand n = context.VectorExtract(type, GetVec(op.Rn), 0);
            Operand m = context.VectorExtract(type, GetVec(op.Rm), op.Index);

            context.Copy(GetVec(op.Rd), context.VectorInsert(context.VectorZero(), emit(n, m), 0));
        }

        public static void EmitScalarTernaryOpByElemF(ArmEmitterContext context, Func3I emit)
        {
            OpCodeSimdRegElemF op = (OpCodeSimdRegElemF)context.CurrOp;

            OperandType type = (op.Size & 1) != 0 ? OperandType.FP64 : OperandType.FP32;

            Operand d = context.VectorExtract(type, GetVec(op.Rd), 0);
            Operand n = context.VectorExtract(type, GetVec(op.Rn), 0);
            Operand m = context.VectorExtract(type, GetVec(op.Rm), op.Index);

            context.Copy(GetVec(op.Rd), context.VectorInsert(context.VectorZero(), emit(d, n, m), 0));
        }

        public static void EmitScalarUnaryOpSx(ArmEmitterContext context, Func1I emit)
        {
            OpCodeSimd op = (OpCodeSimd)context.CurrOp;

            Operand n = EmitVectorExtractSx(context, op.Rn, 0, op.Size);

            Operand d = EmitVectorInsert(context, context.VectorZero(), emit(n), 0, op.Size);

            context.Copy(GetVec(op.Rd), d);
        }

        public static void EmitScalarBinaryOpSx(ArmEmitterContext context, Func2I emit)
        {
            OpCodeSimdReg op = (OpCodeSimdReg)context.CurrOp;

            Operand n = EmitVectorExtractSx(context, op.Rn, 0, op.Size);
            Operand m = EmitVectorExtractSx(context, op.Rm, 0, op.Size);

            Operand d = EmitVectorInsert(context, context.VectorZero(), emit(n, m), 0, op.Size);

            context.Copy(GetVec(op.Rd), d);
        }

        public static void EmitScalarUnaryOpZx(ArmEmitterContext context, Func1I emit)
        {
            OpCodeSimd op = (OpCodeSimd)context.CurrOp;

            Operand n = EmitVectorExtractZx(context, op.Rn, 0, op.Size);

            Operand d = EmitVectorInsert(context, context.VectorZero(), emit(n), 0, op.Size);

            context.Copy(GetVec(op.Rd), d);
        }

        public static void EmitScalarBinaryOpZx(ArmEmitterContext context, Func2I emit)
        {
            OpCodeSimdReg op = (OpCodeSimdReg)context.CurrOp;

            Operand n = EmitVectorExtractZx(context, op.Rn, 0, op.Size);
            Operand m = EmitVectorExtractZx(context, op.Rm, 0, op.Size);

            Operand d = EmitVectorInsert(context, context.VectorZero(), emit(n, m), 0, op.Size);

            context.Copy(GetVec(op.Rd), d);
        }

        public static void EmitScalarTernaryOpZx(ArmEmitterContext context, Func3I emit)
        {
            OpCodeSimdReg op = (OpCodeSimdReg)context.CurrOp;

            Operand d = EmitVectorExtractZx(context, op.Rd, 0, op.Size);
            Operand n = EmitVectorExtractZx(context, op.Rn, 0, op.Size);
            Operand m = EmitVectorExtractZx(context, op.Rm, 0, op.Size);

            d = EmitVectorInsert(context, context.VectorZero(), emit(d, n, m), 0, op.Size);

            context.Copy(GetVec(op.Rd), d);
        }

        public static void EmitScalarUnaryOpF(ArmEmitterContext context, Func1I emit)
        {
            OpCodeSimd op = (OpCodeSimd)context.CurrOp;

            OperandType type = (op.Size & 1) != 0 ? OperandType.FP64 : OperandType.FP32;

            Operand n = context.VectorExtract(type, GetVec(op.Rn), 0);

            context.Copy(GetVec(op.Rd), context.VectorInsert(context.VectorZero(), emit(n), 0));
        }

        public static void EmitScalarBinaryOpF(ArmEmitterContext context, Func2I emit)
        {
            OpCodeSimdReg op = (OpCodeSimdReg)context.CurrOp;

            OperandType type = (op.Size & 1) != 0 ? OperandType.FP64 : OperandType.FP32;

            Operand n = context.VectorExtract(type, GetVec(op.Rn), 0);
            Operand m = context.VectorExtract(type, GetVec(op.Rm), 0);

            context.Copy(GetVec(op.Rd), context.VectorInsert(context.VectorZero(), emit(n, m), 0));
        }

        public static void EmitScalarTernaryRaOpF(ArmEmitterContext context, Func3I emit)
        {
            OpCodeSimdReg op = (OpCodeSimdReg)context.CurrOp;

            OperandType type = (op.Size & 1) != 0 ? OperandType.FP64 : OperandType.FP32;

            Operand a = context.VectorExtract(type, GetVec(op.Ra), 0);
            Operand n = context.VectorExtract(type, GetVec(op.Rn), 0);
            Operand m = context.VectorExtract(type, GetVec(op.Rm), 0);

            context.Copy(GetVec(op.Rd), context.VectorInsert(context.VectorZero(), emit(a, n, m), 0));
        }

        public static void EmitVectorUnaryOpF(ArmEmitterContext context, Func1I emit)
        {
            OpCodeSimd op = (OpCodeSimd)context.CurrOp;

            Operand res = context.VectorZero();

            int sizeF = op.Size & 1;

            OperandType type = sizeF != 0 ? OperandType.FP64 : OperandType.FP32;

            int elems = op.GetBytesCount() >> sizeF + 2;

            for (int index = 0; index < elems; index++)
            {
                Operand ne = context.VectorExtract(type, GetVec(op.Rn), index);

                res = context.VectorInsert(res, emit(ne), index);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorBinaryOpF(ArmEmitterContext context, Func2I emit)
        {
            OpCodeSimdReg op = (OpCodeSimdReg)context.CurrOp;

            Operand res = context.VectorZero();

            int sizeF = op.Size & 1;

            OperandType type = sizeF != 0 ? OperandType.FP64 : OperandType.FP32;

            int elems = op.GetBytesCount() >> sizeF + 2;

            for (int index = 0; index < elems; index++)
            {
                Operand ne = context.VectorExtract(type, GetVec(op.Rn), index);
                Operand me = context.VectorExtract(type, GetVec(op.Rm), index);

                res = context.VectorInsert(res, emit(ne, me), index);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorTernaryOpF(ArmEmitterContext context, Func3I emit)
        {
            OpCodeSimdReg op = (OpCodeSimdReg)context.CurrOp;

            Operand res = context.VectorZero();

            int sizeF = op.Size & 1;

            OperandType type = sizeF != 0 ? OperandType.FP64 : OperandType.FP32;

            int elems = op.GetBytesCount() >> sizeF + 2;

            for (int index = 0; index < elems; index++)
            {
                Operand de = context.VectorExtract(type, GetVec(op.Rd), index);
                Operand ne = context.VectorExtract(type, GetVec(op.Rn), index);
                Operand me = context.VectorExtract(type, GetVec(op.Rm), index);

                res = context.VectorInsert(res, emit(de, ne, me), index);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorBinaryOpByElemF(ArmEmitterContext context, Func2I emit)
        {
            OpCodeSimdRegElemF op = (OpCodeSimdRegElemF)context.CurrOp;

            Operand res = context.VectorZero();

            int sizeF = op.Size & 1;

            OperandType type = sizeF != 0 ? OperandType.FP64 : OperandType.FP32;

            int elems = op.GetBytesCount() >> sizeF + 2;

            for (int index = 0; index < elems; index++)
            {
                Operand ne = context.VectorExtract(type, GetVec(op.Rn), index);
                Operand me = context.VectorExtract(type, GetVec(op.Rm), op.Index);

                res = context.VectorInsert(res, emit(ne, me), index);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorTernaryOpByElemF(ArmEmitterContext context, Func3I emit)
        {
            OpCodeSimdRegElemF op = (OpCodeSimdRegElemF)context.CurrOp;

            Operand res = context.VectorZero();

            int sizeF = op.Size & 1;

            OperandType type = sizeF != 0 ? OperandType.FP64 : OperandType.FP32;

            int elems = op.GetBytesCount() >> sizeF + 2;

            for (int index = 0; index < elems; index++)
            {
                Operand de = context.VectorExtract(type, GetVec(op.Rd), index);
                Operand ne = context.VectorExtract(type, GetVec(op.Rn), index);
                Operand me = context.VectorExtract(type, GetVec(op.Rm), op.Index);

                res = context.VectorInsert(res, emit(de, ne, me), index);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorUnaryOpSx(ArmEmitterContext context, Func1I emit)
        {
            OpCodeSimd op = (OpCodeSimd)context.CurrOp;

            Operand res = context.VectorZero();

            int elems = op.GetBytesCount() >> op.Size;

            for (int index = 0; index < elems; index++)
            {
                Operand ne = EmitVectorExtractSx(context, op.Rn, index, op.Size);

                res = EmitVectorInsert(context, res, emit(ne), index, op.Size);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorBinaryOpSx(ArmEmitterContext context, Func2I emit)
        {
            OpCodeSimdReg op = (OpCodeSimdReg)context.CurrOp;

            Operand res = context.VectorZero();

            int elems = op.GetBytesCount() >> op.Size;

            for (int index = 0; index < elems; index++)
            {
                Operand ne = EmitVectorExtractSx(context, op.Rn, index, op.Size);
                Operand me = EmitVectorExtractSx(context, op.Rm, index, op.Size);

                res = EmitVectorInsert(context, res, emit(ne, me), index, op.Size);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorTernaryOpSx(ArmEmitterContext context, Func3I emit)
        {
            OpCodeSimdReg op = (OpCodeSimdReg)context.CurrOp;

            Operand res = context.VectorZero();

            int elems = op.GetBytesCount() >> op.Size;

            for (int index = 0; index < elems; index++)
            {
                Operand de = EmitVectorExtractSx(context, op.Rd, index, op.Size);
                Operand ne = EmitVectorExtractSx(context, op.Rn, index, op.Size);
                Operand me = EmitVectorExtractSx(context, op.Rm, index, op.Size);

                res = EmitVectorInsert(context, res, emit(de, ne, me), index, op.Size);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorUnaryOpZx(ArmEmitterContext context, Func1I emit)
        {
            OpCodeSimd op = (OpCodeSimd)context.CurrOp;

            Operand res = context.VectorZero();

            int elems = op.GetBytesCount() >> op.Size;

            for (int index = 0; index < elems; index++)
            {
                Operand ne = EmitVectorExtractZx(context, op.Rn, index, op.Size);

                res = EmitVectorInsert(context, res, emit(ne), index, op.Size);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorBinaryOpZx(ArmEmitterContext context, Func2I emit)
        {
            OpCodeSimdReg op = (OpCodeSimdReg)context.CurrOp;

            Operand res = context.VectorZero();

            int elems = op.GetBytesCount() >> op.Size;

            for (int index = 0; index < elems; index++)
            {
                Operand ne = EmitVectorExtractZx(context, op.Rn, index, op.Size);
                Operand me = EmitVectorExtractZx(context, op.Rm, index, op.Size);

                res = EmitVectorInsert(context, res, emit(ne, me), index, op.Size);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorTernaryOpZx(ArmEmitterContext context, Func3I emit)
        {
            OpCodeSimdReg op = (OpCodeSimdReg)context.CurrOp;

            Operand res = context.VectorZero();

            int elems = op.GetBytesCount() >> op.Size;

            for (int index = 0; index < elems; index++)
            {
                Operand de = EmitVectorExtractZx(context, op.Rd, index, op.Size);
                Operand ne = EmitVectorExtractZx(context, op.Rn, index, op.Size);
                Operand me = EmitVectorExtractZx(context, op.Rm, index, op.Size);

                res = EmitVectorInsert(context, res, emit(de, ne, me), index, op.Size);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorBinaryOpByElemSx(ArmEmitterContext context, Func2I emit)
        {
            OpCodeSimdRegElem op = (OpCodeSimdRegElem)context.CurrOp;

            Operand res = context.VectorZero();

            Operand me = EmitVectorExtractSx(context, op.Rm, op.Index, op.Size);

            int elems = op.GetBytesCount() >> op.Size;

            for (int index = 0; index < elems; index++)
            {
                Operand ne = EmitVectorExtractSx(context, op.Rn, index, op.Size);

                res = EmitVectorInsert(context, res, emit(ne, me), index, op.Size);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorBinaryOpByElemZx(ArmEmitterContext context, Func2I emit)
        {
            OpCodeSimdRegElem op = (OpCodeSimdRegElem)context.CurrOp;

            Operand res = context.VectorZero();

            Operand me = EmitVectorExtractZx(context, op.Rm, op.Index, op.Size);

            int elems = op.GetBytesCount() >> op.Size;

            for (int index = 0; index < elems; index++)
            {
                Operand ne = EmitVectorExtractZx(context, op.Rn, index, op.Size);

                res = EmitVectorInsert(context, res, emit(ne, me), index, op.Size);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorTernaryOpByElemZx(ArmEmitterContext context, Func3I emit)
        {
            OpCodeSimdRegElem op = (OpCodeSimdRegElem)context.CurrOp;

            Operand res = context.VectorZero();

            Operand me = EmitVectorExtractZx(context, op.Rm, op.Index, op.Size);

            int elems = op.GetBytesCount() >> op.Size;

            for (int index = 0; index < elems; index++)
            {
                Operand de = EmitVectorExtractZx(context, op.Rd, index, op.Size);
                Operand ne = EmitVectorExtractZx(context, op.Rn, index, op.Size);

                res = EmitVectorInsert(context, res, emit(de, ne, me), index, op.Size);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorImmUnaryOp(ArmEmitterContext context, Func1I emit)
        {
            OpCodeSimdImm op = (OpCodeSimdImm)context.CurrOp;

            Operand imm = Const(op.Immediate);

            Operand res = context.VectorZero();

            int elems = op.GetBytesCount() >> op.Size;

            for (int index = 0; index < elems; index++)
            {
                res = EmitVectorInsert(context, res, emit(imm), index, op.Size);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorImmBinaryOp(ArmEmitterContext context, Func2I emit)
        {
            OpCodeSimdImm op = (OpCodeSimdImm)context.CurrOp;

            Operand imm = Const(op.Immediate);

            Operand res = context.VectorZero();

            int elems = op.GetBytesCount() >> op.Size;

            for (int index = 0; index < elems; index++)
            {
                Operand de = EmitVectorExtractZx(context, op.Rd, index, op.Size);

                res = EmitVectorInsert(context, res, emit(de, imm), index, op.Size);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorWidenRmBinaryOpSx(ArmEmitterContext context, Func2I emit)
        {
            EmitVectorWidenRmBinaryOp(context, emit, signed: true);
        }

        public static void EmitVectorWidenRmBinaryOpZx(ArmEmitterContext context, Func2I emit)
        {
            EmitVectorWidenRmBinaryOp(context, emit, signed: false);
        }

        private static void EmitVectorWidenRmBinaryOp(ArmEmitterContext context, Func2I emit, bool signed)
        {
            OpCodeSimdReg op = (OpCodeSimdReg)context.CurrOp;

            Operand res = context.VectorZero();

            int elems = 8 >> op.Size;

            int part = op.RegisterSize == RegisterSize.Simd128 ? elems : 0;

            for (int index = 0; index < elems; index++)
            {
                Operand ne = EmitVectorExtract(context, op.Rn,        index, op.Size + 1, signed);
                Operand me = EmitVectorExtract(context, op.Rm, part + index, op.Size,     signed);

                res = EmitVectorInsert(context, res, emit(ne, me), index, op.Size + 1);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorWidenRnRmBinaryOpSx(ArmEmitterContext context, Func2I emit)
        {
            EmitVectorWidenRnRmBinaryOp(context, emit, signed: true);
        }

        public static void EmitVectorWidenRnRmBinaryOpZx(ArmEmitterContext context, Func2I emit)
        {
            EmitVectorWidenRnRmBinaryOp(context, emit, signed: false);
        }

        private static void EmitVectorWidenRnRmBinaryOp(ArmEmitterContext context, Func2I emit, bool signed)
        {
            OpCodeSimdReg op = (OpCodeSimdReg)context.CurrOp;

            Operand res = context.VectorZero();

            int elems = 8 >> op.Size;

            int part = op.RegisterSize == RegisterSize.Simd128 ? elems : 0;

            for (int index = 0; index < elems; index++)
            {
                Operand ne = EmitVectorExtract(context, op.Rn, part + index, op.Size, signed);
                Operand me = EmitVectorExtract(context, op.Rm, part + index, op.Size, signed);

                res = EmitVectorInsert(context, res, emit(ne, me), index, op.Size + 1);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorWidenRnRmTernaryOpSx(ArmEmitterContext context, Func3I emit)
        {
            EmitVectorWidenRnRmTernaryOp(context, emit, signed: true);
        }

        public static void EmitVectorWidenRnRmTernaryOpZx(ArmEmitterContext context, Func3I emit)
        {
            EmitVectorWidenRnRmTernaryOp(context, emit, signed: false);
        }

        private static void EmitVectorWidenRnRmTernaryOp(ArmEmitterContext context, Func3I emit, bool signed)
        {
            OpCodeSimdReg op = (OpCodeSimdReg)context.CurrOp;

            Operand res = context.VectorZero();

            int elems = 8 >> op.Size;

            int part = op.RegisterSize == RegisterSize.Simd128 ? elems : 0;

            for (int index = 0; index < elems; index++)
            {
                Operand de = EmitVectorExtract(context, op.Rd,        index, op.Size + 1, signed);
                Operand ne = EmitVectorExtract(context, op.Rn, part + index, op.Size,     signed);
                Operand me = EmitVectorExtract(context, op.Rm, part + index, op.Size,     signed);

                res = EmitVectorInsert(context, res, emit(de, ne, me), index, op.Size + 1);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorWidenBinaryOpByElemSx(ArmEmitterContext context, Func2I emit)
        {
            EmitVectorWidenBinaryOpByElem(context, emit, signed: true);
        }

        public static void EmitVectorWidenBinaryOpByElemZx(ArmEmitterContext context, Func2I emit)
        {
            EmitVectorWidenBinaryOpByElem(context, emit, signed: false);
        }

        private static void EmitVectorWidenBinaryOpByElem(ArmEmitterContext context, Func2I emit, bool signed)
        {
            OpCodeSimdRegElem op = (OpCodeSimdRegElem)context.CurrOp;

            Operand res = context.VectorZero();

            Operand me = EmitVectorExtract(context, op.Rm, op.Index, op.Size, signed);;

            int elems = 8 >> op.Size;

            int part = op.RegisterSize == RegisterSize.Simd128 ? elems : 0;

            for (int index = 0; index < elems; index++)
            {
                Operand ne = EmitVectorExtract(context, op.Rn, part + index, op.Size, signed);

                res = EmitVectorInsert(context, res, emit(ne, me), index, op.Size + 1);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorWidenTernaryOpByElemSx(ArmEmitterContext context, Func3I emit)
        {
            EmitVectorWidenTernaryOpByElem(context, emit, signed: true);
        }

        public static void EmitVectorWidenTernaryOpByElemZx(ArmEmitterContext context, Func3I emit)
        {
            EmitVectorWidenTernaryOpByElem(context, emit, signed: false);
        }

        private static void EmitVectorWidenTernaryOpByElem(ArmEmitterContext context, Func3I emit, bool signed)
        {
            OpCodeSimdRegElem op = (OpCodeSimdRegElem)context.CurrOp;

            Operand res = context.VectorZero();

            Operand me = EmitVectorExtract(context, op.Rm, op.Index, op.Size, signed);;

            int elems = 8 >> op.Size;

            int part = op.RegisterSize == RegisterSize.Simd128 ? elems : 0;

            for (int index = 0; index < elems; index++)
            {
                Operand de = EmitVectorExtract(context, op.Rd,        index, op.Size + 1, signed);
                Operand ne = EmitVectorExtract(context, op.Rn, part + index, op.Size,     signed);

                res = EmitVectorInsert(context, res, emit(de, ne, me), index, op.Size + 1);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorPairwiseOpSx(ArmEmitterContext context, Func2I emit)
        {
            EmitVectorPairwiseOp(context, emit, signed: true);
        }

        public static void EmitVectorPairwiseOpZx(ArmEmitterContext context, Func2I emit)
        {
            EmitVectorPairwiseOp(context, emit, signed: false);
        }

        private static void EmitVectorPairwiseOp(ArmEmitterContext context, Func2I emit, bool signed)
        {
            OpCodeSimdReg op = (OpCodeSimdReg)context.CurrOp;

            Operand res = context.VectorZero();

            int pairs = op.GetPairsCount() >> op.Size;

            for (int index = 0; index < pairs; index++)
            {
                int pairIndex = index << 1;

                Operand n0 = EmitVectorExtract(context, op.Rn, pairIndex,     op.Size, signed);
                Operand n1 = EmitVectorExtract(context, op.Rn, pairIndex + 1, op.Size, signed);

                Operand m0 = EmitVectorExtract(context, op.Rm, pairIndex,     op.Size, signed);
                Operand m1 = EmitVectorExtract(context, op.Rm, pairIndex + 1, op.Size, signed);

                res = EmitVectorInsert(context, res, emit(n0, n1),         index, op.Size);
                res = EmitVectorInsert(context, res, emit(m0, m1), pairs + index, op.Size);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorAcrossVectorOpSx(ArmEmitterContext context, Func2I emit)
        {
            EmitVectorAcrossVectorOp(context, emit, signed: true, isLong: false);
        }

        public static void EmitVectorAcrossVectorOpZx(ArmEmitterContext context, Func2I emit)
        {
            EmitVectorAcrossVectorOp(context, emit, signed: false, isLong: false);
        }

        public static void EmitVectorLongAcrossVectorOpSx(ArmEmitterContext context, Func2I emit)
        {
            EmitVectorAcrossVectorOp(context, emit, signed: true, isLong: true);
        }

        public static void EmitVectorLongAcrossVectorOpZx(ArmEmitterContext context, Func2I emit)
        {
            EmitVectorAcrossVectorOp(context, emit, signed: false, isLong: true);
        }

        private static void EmitVectorAcrossVectorOp(
            ArmEmitterContext context,
            Func2I emit,
            bool signed,
            bool isLong)
        {
            OpCodeSimd op = (OpCodeSimd)context.CurrOp;

            int elems = op.GetBytesCount() >> op.Size;

            Operand res = EmitVectorExtract(context, op.Rn, 0, op.Size, signed);

            for (int index = 1; index < elems; index++)
            {
                Operand n = EmitVectorExtract(context, op.Rn, index, op.Size, signed);

                res = emit(res, n);
            }

            int size = isLong ? op.Size + 1 : op.Size;

            Operand d = EmitVectorInsert(context, context.VectorZero(), res, 0, size);

            context.Copy(GetVec(op.Rd), d);
        }

        public static void EmitVectorPairwiseOpF(ArmEmitterContext context, Func2I emit)
        {
            OpCodeSimdReg op = (OpCodeSimdReg)context.CurrOp;

            Operand res = context.VectorZero();

            int sizeF = op.Size & 1;

            OperandType type = sizeF != 0 ? OperandType.FP64 : OperandType.FP32;

            int pairs = op.GetPairsCount() >> sizeF + 2;

            for (int index = 0; index < pairs; index++)
            {
                int pairIndex = index << 1;

                Operand n0 = context.VectorExtract(type, GetVec(op.Rn), pairIndex);
                Operand n1 = context.VectorExtract(type, GetVec(op.Rn), pairIndex + 1);

                Operand m0 = context.VectorExtract(type, GetVec(op.Rm), pairIndex);
                Operand m1 = context.VectorExtract(type, GetVec(op.Rm), pairIndex + 1);

                res = context.VectorInsert(res, emit(n0, n1),         index);
                res = context.VectorInsert(res, emit(m0, m1), pairs + index);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitVectorPairwiseOpF(ArmEmitterContext context, Intrinsic inst32, Intrinsic inst64)
        {
            OpCodeSimdReg op = (OpCodeSimdReg)context.CurrOp;

            Operand n = GetVec(op.Rn);
            Operand m = GetVec(op.Rm);

            int sizeF = op.Size & 1;

            if (sizeF == 0)
            {
                if (op.RegisterSize == RegisterSize.Simd64)
                {
                    Operand unpck = context.AddIntrinsic(Intrinsic.X86Unpcklps, n, m);

                    Operand zero = context.VectorZero();

                    Operand part0 = context.AddIntrinsic(Intrinsic.X86Movlhps, unpck, zero);
                    Operand part1 = context.AddIntrinsic(Intrinsic.X86Movhlps, zero, unpck);

                    context.Copy(GetVec(op.Rd), context.AddIntrinsic(inst32, part0, part1));
                }
                else /* if (op.RegisterSize == RegisterSize.Simd128) */
                {
                    const int sm0 = 2 << 6 | 0 << 4 | 2 << 2 | 0 << 0;
                    const int sm1 = 3 << 6 | 1 << 4 | 3 << 2 | 1 << 0;

                    Operand part0 = context.AddIntrinsic(Intrinsic.X86Shufps, n, m, Const(sm0));
                    Operand part1 = context.AddIntrinsic(Intrinsic.X86Shufps, n, m, Const(sm1));

                    context.Copy(GetVec(op.Rd), context.AddIntrinsic(inst32, part0, part1));
                }
            }
            else /* if (sizeF == 1) */
            {
                Operand part0 = context.AddIntrinsic(Intrinsic.X86Unpcklpd, n, m);
                Operand part1 = context.AddIntrinsic(Intrinsic.X86Unpckhpd, n, m);

                context.Copy(GetVec(op.Rd), context.AddIntrinsic(inst64, part0, part1));
            }
        }


        [Flags]
        public enum SaturatingFlags
        {
            Scalar = 1 << 0,
            Signed = 1 << 1,

            Add = 1 << 2,
            Sub = 1 << 3,

            Accumulate = 1 << 4,

            ScalarSx = Scalar | Signed,
            ScalarZx = Scalar,

            VectorSx = Signed,
            VectorZx = 0
        }

        public static void EmitScalarSaturatingUnaryOpSx(ArmEmitterContext context, Func1I emit)
        {
            EmitSaturatingUnaryOpSx(context, emit, SaturatingFlags.ScalarSx);
        }

        public static void EmitVectorSaturatingUnaryOpSx(ArmEmitterContext context, Func1I emit)
        {
            EmitSaturatingUnaryOpSx(context, emit, SaturatingFlags.VectorSx);
        }

        private static void EmitSaturatingUnaryOpSx(ArmEmitterContext context, Func1I emit, SaturatingFlags flags)
        {
            OpCodeSimd op = (OpCodeSimd)context.CurrOp;

            Operand res = context.VectorZero();

            bool scalar = (flags & SaturatingFlags.Scalar) != 0;

            int elems = !scalar ? op.GetBytesCount() >> op.Size : 1;

            for (int index = 0; index < elems; index++)
            {
                Operand ne = EmitVectorExtractSx(context, op.Rn, index, op.Size);
                Operand de;

                if (op.Size <= 2)
                {
                    de = EmitSatQ(context, emit(ne), op.Size, signedSrc: true, signedDst: true);
                }
                else /* if (op.Size == 3) */
                {
                    de = EmitUnarySignedSatQAbsOrNeg(context, emit(ne));
                }

                res = EmitVectorInsert(context, res, de, index, op.Size);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        public static void EmitScalarSaturatingBinaryOpSx(ArmEmitterContext context, SaturatingFlags flags)
        {
            EmitSaturatingBinaryOp(context, null, SaturatingFlags.ScalarSx | flags);
        }

        public static void EmitScalarSaturatingBinaryOpZx(ArmEmitterContext context, SaturatingFlags flags)
        {
            EmitSaturatingBinaryOp(context, null, SaturatingFlags.ScalarZx | flags);
        }

        public static void EmitVectorSaturatingBinaryOpSx(ArmEmitterContext context, SaturatingFlags flags)
        {
            EmitSaturatingBinaryOp(context, null, SaturatingFlags.VectorSx | flags);
        }

        public static void EmitVectorSaturatingBinaryOpZx(ArmEmitterContext context, SaturatingFlags flags)
        {
            EmitSaturatingBinaryOp(context, null, SaturatingFlags.VectorZx | flags);
        }

        public static void EmitSaturatingBinaryOp(ArmEmitterContext context, Func2I emit, SaturatingFlags flags)
        {
            OpCodeSimd op = (OpCodeSimd)context.CurrOp;

            Operand res = context.VectorZero();

            bool scalar = (flags & SaturatingFlags.Scalar) != 0;
            bool signed = (flags & SaturatingFlags.Signed) != 0;

            bool add = (flags & SaturatingFlags.Add) != 0;
            bool sub = (flags & SaturatingFlags.Sub) != 0;

            bool accumulate = (flags & SaturatingFlags.Accumulate) != 0;

            int elems = !scalar ? op.GetBytesCount() >> op.Size : 1;

            if (add || sub)
            {
                OpCodeSimdReg opReg = (OpCodeSimdReg)op;

                for (int index = 0; index < elems; index++)
                {
                    Operand de;
                    Operand ne = EmitVectorExtract(context, opReg.Rn, index, op.Size, signed);
                    Operand me = EmitVectorExtract(context, opReg.Rm, index, op.Size, signed);

                    if (op.Size <= 2)
                    {
                        Operand temp = add ? context.Add     (ne, me)
                                           : context.Subtract(ne, me);

                        de = EmitSatQ(context, temp, op.Size, signedSrc: true, signedDst: signed);
                    }
                    else if (add) /* if (op.Size == 3) */
                    {
                        de = EmitBinarySatQAdd(context, ne, me, signed);
                    }
                    else /* if (sub) */
                    {
                        de = EmitBinarySatQSub(context, ne, me, signed);
                    }

                    res = EmitVectorInsert(context, res, de, index, op.Size);
                }
            }
            else if (accumulate)
            {
                for (int index = 0; index < elems; index++)
                {
                    Operand de;
                    Operand ne = EmitVectorExtract(context, op.Rn, index, op.Size, !signed);
                    Operand me = EmitVectorExtract(context, op.Rd, index, op.Size,  signed);

                    if (op.Size <= 2)
                    {
                        Operand temp = context.Add(ne, me);

                        de = EmitSatQ(context, temp, op.Size, signedSrc: true, signedDst: signed);
                    }
                    else /* if (op.Size == 3) */
                    {
                        de = EmitBinarySatQAccumulate(context, ne, me, signed);
                    }

                    res = EmitVectorInsert(context, res, de, index, op.Size);
                }
            }
            else
            {
                OpCodeSimdReg opReg = (OpCodeSimdReg)op;

                for (int index = 0; index < elems; index++)
                {
                    Operand ne = EmitVectorExtract(context, opReg.Rn, index, op.Size, signed);
                    Operand me = EmitVectorExtract(context, opReg.Rm, index, op.Size, signed);

                    Operand de = EmitSatQ(context, emit(ne, me), op.Size, true, signed);

                    res = EmitVectorInsert(context, res, de, index, op.Size);
                }
            }

            context.Copy(GetVec(op.Rd), res);
        }

        [Flags]
        public enum SaturatingNarrowFlags
        {
            Scalar    = 1 << 0,
            SignedSrc = 1 << 1,
            SignedDst = 1 << 2,

            ScalarSxSx = Scalar | SignedSrc | SignedDst,
            ScalarSxZx = Scalar | SignedSrc,
            ScalarZxZx = Scalar,

            VectorSxSx = SignedSrc | SignedDst,
            VectorSxZx = SignedSrc,
            VectorZxZx = 0
        }

        public static void EmitSaturatingNarrowOp(ArmEmitterContext context, SaturatingNarrowFlags flags)
        {
            OpCodeSimd op = (OpCodeSimd)context.CurrOp;

            bool scalar    = (flags & SaturatingNarrowFlags.Scalar)    != 0;
            bool signedSrc = (flags & SaturatingNarrowFlags.SignedSrc) != 0;
            bool signedDst = (flags & SaturatingNarrowFlags.SignedDst) != 0;

            int elems = !scalar ? 8 >> op.Size : 1;

            int part = !scalar && (op.RegisterSize == RegisterSize.Simd128) ? elems : 0;

            Operand res = part == 0 ? context.VectorZero() : context.Copy(GetVec(op.Rd));

            for (int index = 0; index < elems; index++)
            {
                Operand ne = EmitVectorExtract(context, op.Rn, index, op.Size + 1, signedSrc);

                Operand temp = EmitSatQ(context, ne, op.Size, signedSrc, signedDst);

                res = EmitVectorInsert(context, res, temp, part + index, op.Size);
            }

            context.Copy(GetVec(op.Rd), res);
        }

        // TSrc (16bit, 32bit, 64bit; signed, unsigned) > TDst (8bit, 16bit, 32bit; signed, unsigned).
        public static Operand EmitSatQ(ArmEmitterContext context, Operand op, int sizeDst, bool signedSrc, bool signedDst)
        {
            if ((uint)sizeDst > 2u)
            {
                throw new ArgumentOutOfRangeException(nameof(sizeDst));
            }

            Delegate dlg;

            if (signedSrc)
            {
                dlg = signedDst
                    ? (Delegate)new _S64_S64_S32(SoftFallback.SignedSrcSignedDstSatQ)
                    : (Delegate)new _U64_S64_S32(SoftFallback.SignedSrcUnsignedDstSatQ);
            }
            else
            {
                dlg = signedDst
                    ? (Delegate)new _S64_U64_S32(SoftFallback.UnsignedSrcSignedDstSatQ)
                    : (Delegate)new _U64_U64_S32(SoftFallback.UnsignedSrcUnsignedDstSatQ);
            }

            return context.Call(dlg, op, Const(sizeDst));
        }

        // TSrc (64bit) == TDst (64bit); signed.
        public static Operand EmitUnarySignedSatQAbsOrNeg(ArmEmitterContext context, Operand op)
        {
            Debug.Assert(((OpCodeSimd)context.CurrOp).Size == 3, "Invalid element size.");

            return context.Call(new _S64_S64(SoftFallback.UnarySignedSatQAbsOrNeg), op);
        }

        // TSrcs (64bit) == TDst (64bit); signed, unsigned.
        public static Operand EmitBinarySatQAdd(ArmEmitterContext context, Operand op1, Operand op2, bool signed)
        {
            Debug.Assert(((OpCodeSimd)context.CurrOp).Size == 3, "Invalid element size.");

            Delegate dlg = signed
                ? (Delegate)new _S64_S64_S64(SoftFallback.BinarySignedSatQAdd)
                : (Delegate)new _U64_U64_U64(SoftFallback.BinaryUnsignedSatQAdd);

            return context.Call(dlg, op1, op2);
        }

        // TSrcs (64bit) == TDst (64bit); signed, unsigned.
        public static Operand EmitBinarySatQSub(ArmEmitterContext context, Operand op1, Operand op2, bool signed)
        {
            Debug.Assert(((OpCodeSimd)context.CurrOp).Size == 3, "Invalid element size.");

            Delegate dlg = signed
                ? (Delegate)new _S64_S64_S64(SoftFallback.BinarySignedSatQSub)
                : (Delegate)new _U64_U64_U64(SoftFallback.BinaryUnsignedSatQSub);

            return context.Call(dlg, op1, op2);
        }

        // TSrcs (64bit) == TDst (64bit); signed, unsigned.
        public static Operand EmitBinarySatQAccumulate(ArmEmitterContext context, Operand op1, Operand op2, bool signed)
        {
            Debug.Assert(((OpCodeSimd)context.CurrOp).Size == 3, "Invalid element size.");

            Delegate dlg = signed
                ? (Delegate)new _S64_U64_S64(SoftFallback.BinarySignedSatQAcc)
                : (Delegate)new _U64_S64_U64(SoftFallback.BinaryUnsignedSatQAcc);

            return context.Call(dlg, op1, op2);
        }

        public static Operand EmitVectorExtractSx(ArmEmitterContext context, int reg, int index, int size)
        {
            return EmitVectorExtract(context, reg, index, size, true);
        }

        public static Operand EmitVectorExtractZx(ArmEmitterContext context, int reg, int index, int size)
        {
            return EmitVectorExtract(context, reg, index, size, false);
        }

        public static Operand EmitVectorExtract(ArmEmitterContext context, int reg, int index, int size, bool signed)
        {
            ThrowIfInvalid(index, size);

            Operand res = null;

            switch (size)
            {
                case 0:
                    res = context.VectorExtract8(GetVec(reg), index);
                    break;

                case 1:
                    res = context.VectorExtract16(GetVec(reg), index);
                    break;

                case 2:
                    res = context.VectorExtract(OperandType.I32, GetVec(reg), index);
                    break;

                case 3:
                    res = context.VectorExtract(OperandType.I64, GetVec(reg), index);
                    break;
            }

            if (signed)
            {
                switch (size)
                {
                    case 0: res = context.SignExtend8 (OperandType.I64, res); break;
                    case 1: res = context.SignExtend16(OperandType.I64, res); break;
                    case 2: res = context.SignExtend32(OperandType.I64, res); break;
                }
            }
            else
            {
                switch (size)
                {
                    case 0: res = context.ZeroExtend8 (OperandType.I64, res); break;
                    case 1: res = context.ZeroExtend16(OperandType.I64, res); break;
                    case 2: res = context.ZeroExtend32(OperandType.I64, res); break;
                }
            }

            return res;
        }

        public static Operand EmitVectorInsert(ArmEmitterContext context, Operand vector, Operand value, int index, int size)
        {
            ThrowIfInvalid(index, size);

            if (size < 3)
            {
                value = context.ConvertI64ToI32(value);
            }

            switch (size)
            {
                case 0: vector = context.VectorInsert8 (vector, value, index); break;
                case 1: vector = context.VectorInsert16(vector, value, index); break;
                case 2: vector = context.VectorInsert  (vector, value, index); break;
                case 3: vector = context.VectorInsert  (vector, value, index); break;
            }

            return vector;
        }

        private static void ThrowIfInvalid(int index, int size)
        {
            if ((uint)size > 3u)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            if ((uint)index >= 16u >> size)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}
