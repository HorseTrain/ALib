using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeCompareAndBranchImm : AOpCode, IOpCodeConditionalBranch, IOpCodeRt, IOpCodeALUSize
    {
        public OpCodeSize Size      { get; set; }
        public int Rt               { get; set; }

        public long Imm             { get; set; }
        public long NextAddress     { get; set; }

        public static OpCodeCompareAndBranchImm Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeCompareAndBranchImm(lowLevelAOpCode, Address, Name);

        public OpCodeCompareAndBranchImm(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(Address, lowLevelAOpCode.RawInstruction, Name)
        {
            NextAddress = Address + 4;

            Rt = lowLevelAOpCode.Rt;

            Imm = DecodingHelpers.SignExtend(lowLevelAOpCode.imm19, 19) << 2;

            if (!DecodingOptions.RawBranchImm)
                Imm += Address;

            Size = DecodingHelpers.GetIntALUSize(lowLevelAOpCode.sf);
        }

        public override string ToString() => $"{Name} {LoggerTools.GetRegister(Size, Rt)}, {LoggerTools.GetImm(Imm)}";
    }
}
