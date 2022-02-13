using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeConditionalCompare : AOpCode, IOpCodeALUSize, IOpCodeRn, IOpCodeCond
    {
        public OpCodeSize Size  { get; set; }
        public int Rn           { get; set; }
        public int Imm_Rm       { get; set; }
        public bool IsReg       { get; set; }

        public Cond cond        { get; set; }
        public int nzcv         { get; set; }

        public static OpCodeConditionalCompare Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeConditionalCompare(lowLevelAOpCode, Address, Name);

        public OpCodeConditionalCompare(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(Address, lowLevelAOpCode.RawInstruction, Name)
        {
            Size = DecodingHelpers.GetIntALUSize(lowLevelAOpCode.sf);

            Rn = lowLevelAOpCode.Rn;

            cond = (Cond)lowLevelAOpCode.cond;
            nzcv = lowLevelAOpCode.nzcv;

            IsReg = lowLevelAOpCode.ClassName == LowLevelClassNames.Conditional_compare_register;

            Imm_Rm = IsReg ? lowLevelAOpCode.Rm : lowLevelAOpCode.imm5;
        }

        public override string ToString()
        {
            if (IsReg)
            {
                return $"{Name} {LoggerTools.GetRegister(Size, Rn)}, {LoggerTools.GetRegister(Size, Imm_Rm )}, {LoggerTools.GetImm(nzcv)}, {cond.ToString().ToLower()}";
            }
            else
            {
                return $"{Name} {LoggerTools.GetRegister(Size, Rn)}, {LoggerTools.GetImm(Imm_Rm)}, {LoggerTools.GetImm(nzcv)}, {cond.ToString().ToLower()}";
            }
        }
    }
}
