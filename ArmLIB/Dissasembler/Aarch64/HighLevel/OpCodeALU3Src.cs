using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeALU3Src : OpCodeALU, IOpCodeRm, IOpCodeRa
    {
        public int Rm   { get; set; }
        public int Ra   { get; set; }

        public static OpCodeALU3Src Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeALU3Src(lowLevelAOpCode, Address, Name);

        public OpCodeALU3Src(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(lowLevelAOpCode, Address, Name)
        {
            Rm = lowLevelAOpCode.Rm;
            Ra = lowLevelAOpCode.Ra;
        }

        public override string ToString()
        {
            if (Name == Mnemonic.smaddl || Name == Mnemonic.smsubl || Name == Mnemonic.umaddl || Name == Mnemonic.umsubl)
            {
                return $"{Name} {LoggerTools.GetRegister(Size, Rd, RdIsSP)}, {LoggerTools.GetRegister(OpCodeSize.w, Rn, RnIsSP)}, {LoggerTools.GetRegister(OpCodeSize.w, Rm)}, {LoggerTools.GetRegister(OpCodeSize.x, Ra)}";
            }

            return $"{Name} {LoggerTools.GetRegister(Size, Rd, RdIsSP)}, {LoggerTools.GetRegister(Size, Rn, RnIsSP)}, {LoggerTools.GetRegister(Size, Rm)}, {LoggerTools.GetRegister(Size, Ra)}";
        }
    }
}
