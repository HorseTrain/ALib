using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.LowLevel
{
    public delegate LowLevelAOpCode LowLevelInstructionCreate(int RawInstruction, LowLevelNames Name);
}
