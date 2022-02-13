using System;

namespace DCpu.State
{
    [Flags]
    public enum FPSR
    {
        Ufc = 1 << 3,
        Qc  = 1 << 27
    }
}
