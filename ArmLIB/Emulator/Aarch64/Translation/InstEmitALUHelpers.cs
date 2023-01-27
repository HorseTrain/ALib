using ArmLIB.Dissasembler.Aarch64.HighLevel;
using AlibCompiler.Intermediate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64.Translation
{
    public static partial class InstEmit64
    {
        public static bool CheckIfConstZero(IOperand Source) => (Source is ConstOperand co && co.Data == 0);

        public static IOperand LogicalShift(ArmEmitContext ctx, IOperand Source, IOperand Shift, ShiftType Type)
        {
            if (CheckIfConstZero(Shift))
                return Source;

            switch (Type)
            {
                case ShiftType.LSL: return ctx.LogicalShiftLeft(Source, Shift);
                case ShiftType.LSR: return ctx.LogicalShiftRight(Source, Shift);
                case ShiftType.ASR: return ctx.LogicalShiftRightSigned(Source, Shift);
                case ShiftType.ROR: return ctx.LogicalRotateRight(Source, Shift);
                default: throw new Exception();
            }
        }

        public static IOperand Extend(ArmEmitContext ctx, IOperand Source, IntType Type)
        {
            switch (Type)
            {
                case IntType.Int8: return ctx.SignExtend8(Source);
                case IntType.Int16: return ctx.SignExtend16(Source);
                case IntType.Int32:
                    {
                        if (ctx.CurrentEmitSize == OperandType.Int32)
                            return Source;

                        return ctx.SignExtend32(Source);
                    }

                case IntType.UInt8: return ctx.LogicalAnd(Source, ConstOperand.Create((ulong)byte.MaxValue));
                case IntType.UInt16: return ctx.LogicalAnd(Source, ConstOperand.Create((ulong)ushort.MaxValue));
                case IntType.UInt32: return ctx.LogicalAnd(Source, ConstOperand.Create((ulong)uint.MaxValue));
            }

            return Source;
        }
    }
}
