using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeALU : AOpCode, IOpCodeALUSize, IOpCodeRn, IOpCodeRnIsSP, IOpCodeRd, IOpCodeRdIsSP
    {
        public OpCodeSize Size  { get; set; }

        public int Rd           { get; set; }
        public bool RdIsSP      { get; set; }

        public int Rn           { get; set; }
        public bool RnIsSP      { get; set; }

        protected OpCodeALU(LowLevelAOpCode Source,long Address, Mnemonic Name) : base (Address, Source.RawInstruction, Name)
        {
            Rd = Source.Rd;
            Rn = Source.Rn;

            Size = DecodingHelpers.GetIntALUSize(Source.sf);
        }
    }
}
