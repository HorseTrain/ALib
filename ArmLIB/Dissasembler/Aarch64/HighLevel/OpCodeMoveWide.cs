using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeMoveWide : AOpCode, IOpCodeALUSize,IOpCodeRd, IOpCodeImm
    {
        public OpCodeSize Size  { get; set; }
        public int Rd           { get; set; }
        public long Imm         { get; set; }
        public int Shift        { get; set; }

        public static OpCodeMoveWide Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeMoveWide(lowLevelAOpCode, Address, Name);

        public OpCodeMoveWide(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(Address, lowLevelAOpCode.RawInstruction, Name)
        {
            Rd = lowLevelAOpCode.Rd;
            Imm = lowLevelAOpCode.imm16;
            Shift = lowLevelAOpCode.hw * 16;

            Size = DecodingHelpers.GetIntALUSize(lowLevelAOpCode.sf);
        }

        public override string ToString() => $"{Name} {LoggerTools.GetRegister(Size, Rd)}, {LoggerTools.GetImm(Imm)}, lsl {LoggerTools.GetImm(Shift)}";
    }
}
