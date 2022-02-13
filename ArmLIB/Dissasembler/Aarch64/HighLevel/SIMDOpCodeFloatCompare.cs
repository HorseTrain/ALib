using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class SIMDOpCodeFloatCompare : AOpCode, IOpCodeSimd, IOpCodeRn, IOpCodeRm, IOpCodeCond
    {
        public OpCodeSize Size  { get; set; }
        public int Rn           { get; set; }
        public int Rm           { get; set; }

        public bool IsConditional   { get; set; }
        public Cond cond            { get; set; }
        public int nzcv             { get; set; }

        public bool WithZero    { get; set; }

        public static SIMDOpCodeFloatCompare CreateNC(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeFloatCompare(lowLevelAOpCode, Address, Name, false);
        public static SIMDOpCodeFloatCompare CreateC(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeFloatCompare(lowLevelAOpCode, Address, Name, true);

        public SIMDOpCodeFloatCompare(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name, bool IsConditional) : base(Address, lowLevelAOpCode.RawInstruction, Name)
        {
            Rn = lowLevelAOpCode.Rn;
            Rm = lowLevelAOpCode.Rm;

            Size = (OpCodeSize)(lowLevelAOpCode.ptype + 2);

            DecodingHelpers.EnsureSize(Size);

            if (IsConditional)
            {
                cond = (Cond)lowLevelAOpCode.cond;
                nzcv = lowLevelAOpCode.nzcv;
            }
            else
            {
                WithZero = lowLevelAOpCode.opcode2 == 0b01000 || lowLevelAOpCode.opcode2 == 0b11000;
            }

            this.IsConditional = IsConditional;
        }

        public override string ToString() => $"{Name} {LoggerTools.GetRegister(Size, Rn, false, true)}, {(WithZero ? LoggerTools.GetImmF(0) : LoggerTools.GetRegister(Size, Rm, false, true))}{(IsConditional ? $", {LoggerTools.GetImm(nzcv)}, {cond.ToString().ToLower()}" : "")}";
    }
}
