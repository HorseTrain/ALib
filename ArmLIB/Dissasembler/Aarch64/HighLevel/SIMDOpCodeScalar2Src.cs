using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class SIMDOpCodeScalar2Src : OpCodeSIMD, IOpCodeRn, IOpCodeRm, OpCodeVectorSize
    {
        public OpCodeSize Size  { get; set; }
        public int Rn           { get; set; }
        public int Rm           { get; set; }

        public static SIMDOpCodeScalar2Src CreateF(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeScalar2Src(lowLevelAOpCode, Address, Name, SIMDInstructionMode.Float);

        public SIMDOpCodeScalar2Src(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name, SIMDInstructionMode Mode) : base(lowLevelAOpCode, Address, Name)
        {
            Rn = lowLevelAOpCode.Rn;
            Rm = lowLevelAOpCode.Rm;

            switch (Mode)
            {
                case SIMDInstructionMode.Float:
                    {
                        Size = (OpCodeSize)(lowLevelAOpCode.ptype + 2);

                        DecodingHelpers.EnsureSize(Size);
                    }
                    break;
                default: throw new Exception();
            }
        }

        public override string ToString() => $"{Name} {LoggerTools.GetRegister(Size, Rd, false, true)}, {LoggerTools.GetRegister(Size, Rn, false, true)}, {LoggerTools.GetRegister(Size, Rm, false, true)}";
    }
}
