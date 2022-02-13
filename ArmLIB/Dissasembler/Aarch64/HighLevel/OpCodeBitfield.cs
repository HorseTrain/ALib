using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeBitfield : OpCodeALU
    {
        public int Imms     { get; set; }
        public int Immr     { get; set; }

        public static OpCodeBitfield Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeBitfield(lowLevelAOpCode, Address, Name);

        public OpCodeBitfield(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(lowLevelAOpCode,Address, Name)
        {
            Imms = lowLevelAOpCode.imms;
            Immr = lowLevelAOpCode.immr;
        }

        public override string ToString() => $"{Name} {LoggerTools.GetRegister(Size, Rd, RdIsSP)}, {LoggerTools.GetRegister(Size, Rn, RnIsSP)}, {LoggerTools.GetImm(Immr)}, {LoggerTools.GetImm(Imms)} ";
    }
}
