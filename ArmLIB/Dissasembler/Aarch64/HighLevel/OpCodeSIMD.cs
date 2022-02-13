using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeSIMD : AOpCode, IOpCodeRd, IOpCodeSimd
    {
        public int Rd   { get; set; }

        protected OpCodeSIMD(LowLevelAOpCode Source, long Address, Mnemonic Name) : base(Address, Source.RawInstruction, Name)
        {
            Rd = Source.Rd;
        }
    }
}
