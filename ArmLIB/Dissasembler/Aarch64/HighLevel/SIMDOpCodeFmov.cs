using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    class SIMDOpCodeFmov : OpCodeSIMD, IOpCodeRn
    {
        public OpCodeSize Size  { get; set; }
        public int Rn           { get; set; }

        public bool ToVector    { get; set; }

        public static SIMDOpCodeFmov Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeFmov(lowLevelAOpCode, Address, Name);

        public SIMDOpCodeFmov(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(lowLevelAOpCode, Address, Name)
        {
            Rn = lowLevelAOpCode.Rn;

            Size = (OpCodeSize)(lowLevelAOpCode.ptype + 2);

            DecodingHelpers.EnsureSize(Size);

            ToVector = lowLevelAOpCode.opcode == 0b111;
        }

        public override string ToString()
        {
            OpCodeSize GpSize = Size == OpCodeSize.d ? OpCodeSize.x : OpCodeSize.w;

            if (ToVector)
            {
                return $"{Name} {LoggerTools.GetRegister(Size, Rd, false, true)}, {LoggerTools.GetRegister(GpSize, Rn)}";
            }
            else
            {
                return $"{Name} {LoggerTools.GetRegister(GpSize, Rd)}, {LoggerTools.GetRegister(Size, Rn, false, true)}";
            }
        }
    }
}
