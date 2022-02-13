using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class SIMDOpCodeDUP : OpCodeSIMD, IOpCodeRn
    {
        public OpCodeSize Size  { get; set; }
        public int Rn           { get; set; }
        public bool Half        { get; set; }

        public static SIMDOpCodeDUP Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeDUP(lowLevelAOpCode, Address, Name);

        public SIMDOpCodeDUP(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(lowLevelAOpCode, Address, Name)
        {
            Rn = lowLevelAOpCode.Rn;

            Size = (OpCodeSize)DecodingHelpers.LowestBitSet(lowLevelAOpCode.imm5);

            Half = lowLevelAOpCode.Q == 0;
        }

        public override string ToString() => $"{Name} {LoggerTools.GetIteratedVector(Rd, Half, Size)}, {LoggerTools.GetRegister(Size == OpCodeSize.d ? OpCodeSize.x : OpCodeSize.w,Rn)}";
    }
}
