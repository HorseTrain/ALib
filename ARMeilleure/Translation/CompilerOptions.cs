using System;

namespace DCpu.Translation
{
    [Flags]
    enum CompilerOptions
    {
        None     = 0,
        SsaForm  = 1 << 0,
        Optimize = 1 << 1,
        Lsra     = 1 << 2,

        MediumCq = SsaForm | Optimize,
        HighCq   = SsaForm | Optimize | Lsra
    }
}