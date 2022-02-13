using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodePcRelAddressing : AOpCode ,IOpCodeRd, IOpCodeImm
    {
        public int Rd   { get; set; }
        public long Imm { get; set; }

        public static OpCodePcRelAddressing Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodePcRelAddressing(lowLevelAOpCode, Address, Name);

        public OpCodePcRelAddressing(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(Address, lowLevelAOpCode.RawInstruction, Name)
        {
            Rd = lowLevelAOpCode.Rd;

            Imm = (((long)lowLevelAOpCode.RawInstruction << 40) >> 43) & ~3;
            Imm |= ((long)lowLevelAOpCode.RawInstruction >> 29) & 3;

            if (lowLevelAOpCode.LowLevelName == LowLevelNames.ADRP)
                Imm <<= 12;
        }

        public override string ToString() => $"{Name} {LoggerTools.GetRegister(OpCodeSize.x, Rd)}, {LoggerTools.GetImm(Imm)}";
    }
}
