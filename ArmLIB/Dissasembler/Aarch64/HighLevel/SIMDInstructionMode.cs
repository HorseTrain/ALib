using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public enum SIMDInstructionMode
    {
        Float,
        Logical,
        FloatDifferentSize,
        FloatWithZero,
        IntWithSize,
        FloatSZ,
        IntToFloat,
        IntToFloatVector,
        FloatToInt,
        FloatToFixed,
        VectorDoubleSize,
        MoveToScalar,
        MoveToVector,
        NULL,
        ExtractVector,
        ShiftVector
    }
}
