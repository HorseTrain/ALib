using System;

namespace DCpu.State
{
    [Flags]
    public enum PState
    {
        TFlag = 5,
        EFlag = 9,

        VFlag = 28,
        CFlag = 29,
        ZFlag = 30,
        NFlag = 31
    }
}
