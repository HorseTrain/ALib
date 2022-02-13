using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeConditionalSelect : AOpCode, IOpCodeALUSize,IOpCodeRd, IOpCodeRn, IOpCodeRm, IOpCodeCond
    {
        public OpCodeSize Size  { get; set; }
        public int Rd           { get; set; }
        public int Rn           { get; set; }
        public int Rm           { get; set; }
        public Cond cond        { get; set; }

        public static OpCodeConditionalSelect Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeConditionalSelect(lowLevelAOpCode, Address, Name);

        public OpCodeConditionalSelect(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(Address, lowLevelAOpCode.RawInstruction, Name)
        {
            Rd = lowLevelAOpCode.Rd;
            Rn = lowLevelAOpCode.Rn;
            Rm = lowLevelAOpCode.Rm;

            cond = (Cond)lowLevelAOpCode.cond;

            Size = DecodingHelpers.GetIntALUSize(lowLevelAOpCode.sf);
        }

        public override string ToString() => $"{Name} {LoggerTools.GetRegister(Size, Rd)}, {LoggerTools.GetRegister(Size, Rn)}, {LoggerTools.GetRegister(Size, Rm)}, {cond.ToString().ToLower()}";
    }
}
