using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeExtract : OpCodeALU, IOpCodeRm, IOpCodeImm
    {
        public int Rm   { get; set; }
        public long Imm { get; set; }

        public static OpCodeExtract Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeExtract(lowLevelAOpCode, Address, Name);

        OpCodeExtract(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(lowLevelAOpCode, Address, Name)
        {
            Rm = lowLevelAOpCode.Rm;

            Imm = lowLevelAOpCode.imms;
        }

        public override string ToString()
        {
            return $"{Name} {LoggerTools.GetRegister(Size, Rd)}, {LoggerTools.GetRegister(Size, Rn)}, {LoggerTools.GetRegister(Size, Rm)}, {LoggerTools.GetImm(Imm)}";
        }
    }
}
