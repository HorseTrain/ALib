using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeUnconditionalBranchImm : AOpCode, IOpCodeBranch, IOpCodeImm
    {
        public long Imm     { get; set; }

        public static OpCodeUnconditionalBranchImm Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeUnconditionalBranchImm(lowLevelAOpCode, Address, Name);

        public OpCodeUnconditionalBranchImm(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base (Address, lowLevelAOpCode.RawInstruction, Name)
        {
            Imm = DecodingHelpers.SignExtend(lowLevelAOpCode.imm26, 26) << 2;

            if (!DecodingOptions.RawBranchImm)
                Imm += Address;
        }

        public override string ToString() => $"{Name} {LoggerTools.GetImm(Imm)}";
    }
}
