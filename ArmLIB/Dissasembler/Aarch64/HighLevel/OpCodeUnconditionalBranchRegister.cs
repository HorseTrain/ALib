using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeUnconditionalBranchRegister : AOpCode, IOpCodeBranchRegister
    {
        public int Rn   { get; set; }

        public static OpCodeUnconditionalBranchRegister Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeUnconditionalBranchRegister(lowLevelAOpCode, Address, Name);

        public OpCodeUnconditionalBranchRegister(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(Address, lowLevelAOpCode.RawInstruction, Name)
        {
            Rn = lowLevelAOpCode.Rn;
        }

        public override string ToString() => $"{Name} {LoggerTools.GetRegister(OpCodeSize.x, Rn)}";
    }
}
