using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeTestBitAndBranch : AOpCode, IOpCodeConditionalBranch, IOpCodeRt, IOpCodeALUSize
    {
        public OpCodeSize Size      { get; set; }
        public int Rt               { get; set; }

        public long Imm             { get; set; }
        public long NextAddress     { get; set; }

        public int Bit              { get; set; }

        public static OpCodeTestBitAndBranch Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeTestBitAndBranch(lowLevelAOpCode, Address, Name);

        public OpCodeTestBitAndBranch(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(Address, lowLevelAOpCode.RawInstruction, Name)
        {
            NextAddress = Address + 4;

            Rt = lowLevelAOpCode.Rt;

            Imm = DecodingHelpers.SignExtend(lowLevelAOpCode.imm14, 14) << 2;

            if (!DecodingOptions.RawBranchImm)
                Imm += Address;

            Size = DecodingHelpers.GetIntALUSize(lowLevelAOpCode.b5);

            Bit = lowLevelAOpCode.b40;

            if (Size == OpCodeSize.x)
                Bit += 32;
        }

        public override string ToString() => $"{Name} {LoggerTools.GetRegister(Size, Rt)}, {LoggerTools.GetImm(Bit)}, {LoggerTools.GetImm(Imm)}";
    }
}
