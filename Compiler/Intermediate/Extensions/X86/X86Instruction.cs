using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlibCompiler.Intermediate.Extensions.X86
{
    public enum X86Instruction
    {
        //normal
        Bswap,
        Lzcnt,
        Popcnt,

        //intrinsic
        Cvtsd2ss,
        Cvtss2sd,
        Movaps,
        Pextrb,
        Pextrd,
        Pextrq,
        Pextrw,
        Pinsrb,
        Pinsrd,
        Pinsrq,
        Pinsrw,
        Sqrtpd,
        Sqrtps,
        Vaddpd,
        Vaddps,
        Vandps,
        Vdivpd,
        Vdivps,
        Vmaxpd,
        Vmaxps,
        Vminpd,
        Vminps,
        Vmovupd_load,
        Vmovupd_store,
        Vmulpd,
        Vmulps,
        Vorps,
        Vpaddb,
        Vpaddd,
        Vpaddq,
        Vpaddw,
        Vpsubb,
        Vpsubd,
        Vpsubq,
        Vpsubw,
        Vsubpd,
        Vsubps,
        Vxorps,
        Vroundps,
        Vroundpd,
        Vrsqrtps,

        //micro operations
        Add_GetFlags,
    }
}
