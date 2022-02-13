using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class SIMDOpCodeVectorTableLookup : OpCodeSIMD, IOpCodeRn, IOpCodeRm
    {
        public int Rn       { get; set; }
        public int Rm       { get; set; }
        public int Len      { get; set; }

        public bool Half    { get; set; }

        public static SIMDOpCodeVectorTableLookup Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeVectorTableLookup(lowLevelAOpCode, Address, Name);

        public SIMDOpCodeVectorTableLookup(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(lowLevelAOpCode, Address, Name)
        {
            Rn = lowLevelAOpCode.Rn;
            Rm = lowLevelAOpCode.Rm;
            Len = lowLevelAOpCode.len;

            Half = lowLevelAOpCode.Q == 0;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
