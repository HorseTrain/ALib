using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class SIMDOpCodeFloatConditionalSelect : OpCodeSIMD, IOpCodeRn, IOpCodeRm, IOpCodeCond
    {
        public OpCodeSize Size  { get; set; }
        public int Rn           { get; set; }
        public int Rm           { get; set; }
        public Cond cond        { get; set; }

        public static SIMDOpCodeFloatConditionalSelect Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeFloatConditionalSelect(lowLevelAOpCode, Address, Name);

        public SIMDOpCodeFloatConditionalSelect(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(lowLevelAOpCode, Address, Name)
        {
            Rn = lowLevelAOpCode.Rn;
            Rm = lowLevelAOpCode.Rm;
            cond = (Cond)lowLevelAOpCode.cond;

            Size = (OpCodeSize)(lowLevelAOpCode.ptype + 2);

            DecodingHelpers.EnsureSize(Size);
        }

        public override string ToString() => $"{Name} {LoggerTools.GetRegister(Size, Rd, false, true)}, {LoggerTools.GetRegister(Size, Rn, false, true)}, {LoggerTools.GetRegister(Size, Rm, false, true)}, {cond.ToString().ToLower()}";
    }
}
