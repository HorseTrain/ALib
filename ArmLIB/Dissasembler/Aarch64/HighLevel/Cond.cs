using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public enum Cond
    {
        EQ, NE,
        CS, CC,
        MI, PL,
        VS, VC,
        HI, LS,
        GE, LT,
        GT, LE,
        AL, NV
    }
}
