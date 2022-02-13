using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeConditionalBranchImm : AOpCode, IOpCodeConditionalBranch, IOpCodeCond
    {
        public long Imm             { get; set; }
        public long NextAddress     { get; set; }

        public Cond cond            { get; set; }

        public static OpCodeConditionalBranchImm Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeConditionalBranchImm(lowLevelAOpCode, Address, Name);

        public OpCodeConditionalBranchImm(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(Address, lowLevelAOpCode.RawInstruction, Name)
        {
            NextAddress = Address + 4;

            cond = (Cond)lowLevelAOpCode.cond;

            Imm = DecodingHelpers.SignExtend(lowLevelAOpCode.imm19, 19) << 2;

            if (!DecodingOptions.RawBranchImm)
                Imm += Address;
        }

        public override string ToString() => $"b.{cond.ToString().ToLower()} {LoggerTools.GetImm(Imm)}";
    }
}
