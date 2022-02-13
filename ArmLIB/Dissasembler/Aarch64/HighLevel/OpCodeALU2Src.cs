using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeALU2Src : OpCodeALU, IOpCodeRm
    {
        public int Rm   { get; set; }

        public static OpCodeALU2Src Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeALU2Src(lowLevelAOpCode, Address, Name);

        public OpCodeALU2Src(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(lowLevelAOpCode, Address, Name)
        {
            Rm = lowLevelAOpCode.Rm;
        }

        public override string ToString() => $"{Name} {LoggerTools.GetRegister(Size, Rd, RdIsSP)}, {LoggerTools.GetRegister(Size, Rn, RnIsSP)}, {LoggerTools.GetRegister(Size, Rm)}";
    }
}
