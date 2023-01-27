using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlibCompiler.Intermediate
{
    public enum Instruction
    {
        Add,
        Call,
        CompareEqual,
        CompareGreaterOrEqual,
        CompareGreater,
        CompareLessSigned,
        CompareLess,
        ConditionalSelect,
        Divide,
        DivideSigned,
        Load,
        LogicalAnd,
        LogicalExclusiveOr,
        LogicalOr,
        LogicalRotateRight,
        LogicalShiftLeft,
        LogicalShiftRight,
        LogicalShiftRightSigned,
        Multiply,
        MultiplyHi,
        MultiplyHiSigned,
        LogicalDoubleShiftRight,
        Not,
        SignExtend,
        Subtract,
        Copy,
        CompareAndSwap,
        GetContext,

        Store,
        Nop,

        JumpIf,
        Return,
        HardJump,

        AllocateRegister,
    }
}
