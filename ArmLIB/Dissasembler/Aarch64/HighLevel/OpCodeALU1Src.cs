using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeALU1Src : OpCodeALU
    {
        public static OpCodeALU1Src Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeALU1Src(lowLevelAOpCode, Address, Name);

        OpCodeALU1Src(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(lowLevelAOpCode, Address, Name)
        {
            //lol
        }

        public override string ToString() => $"{Name} {LoggerTools.GetRegister(Size, Rd)}, {LoggerTools.GetRegister(Size, Rn)}";
    }
}
