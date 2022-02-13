using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class SIDMOpCodeInsertVectorElement : OpCodeSIMD, IOpCodeRn
    {
        public OpCodeSize Size  { get; set; }
        public int Rn           { get; set; }

        public int SourceIndex      { get; set; }
        public int DestinationIndex { get; set; }

        public static SIDMOpCodeInsertVectorElement Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIDMOpCodeInsertVectorElement(lowLevelAOpCode, Address, Name);

        public SIDMOpCodeInsertVectorElement(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(lowLevelAOpCode, Address, Name)
        {
            Rn = lowLevelAOpCode.Rn;

            Size = (OpCodeSize)DecodingHelpers.LowestBitSet(lowLevelAOpCode.imm5);

            DestinationIndex = lowLevelAOpCode.imm5 >> ((int)Size + 1);
            SourceIndex = lowLevelAOpCode.imm4 >> (int)Size;
        }

        public override string ToString() => $"{Name} v{Rd}.{Size}[{DestinationIndex}], v{Rn}.{Size}[{SourceIndex}]";
    }
}
