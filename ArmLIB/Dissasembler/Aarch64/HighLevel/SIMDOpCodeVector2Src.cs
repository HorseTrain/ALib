using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class SIMDOpCodeVector2Src : OpCodeSIMD, IOpCodeRn, IOpCodeRm, OpCodeVectorSize
    {
        public OpCodeSize Size  { get; set; }
        public int Rn           { get; set; }
        public int Rm           { get; set; }
        public bool Half        { get; set; }

        public static SIMDOpCodeVector2Src CreateL(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeVector2Src(lowLevelAOpCode, Address, Name, SIMDInstructionMode.Logical);
        public static SIMDOpCodeVector2Src CreateIS(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeVector2Src(lowLevelAOpCode, Address, Name, SIMDInstructionMode.IntWithSize);
        public static SIMDOpCodeVector2Src CreateF(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeVector2Src(lowLevelAOpCode, Address, Name, SIMDInstructionMode.Float);

        public SIMDOpCodeVector2Src(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name, SIMDInstructionMode Mode) : base(lowLevelAOpCode, Address, Name)
        {
            Rn = lowLevelAOpCode.Rn;
            Rm = lowLevelAOpCode.Rm;

            Half = lowLevelAOpCode.Q == 0;

            switch (Mode)
            {
                case SIMDInstructionMode.Logical: Size = OpCodeSize.b; break;
                case SIMDInstructionMode.Float: Size = (OpCodeSize)(((lowLevelAOpCode.RawInstruction >> 22) & 1) + 2); break;
                case SIMDInstructionMode.IntWithSize: Size = (OpCodeSize)lowLevelAOpCode.size; break;
                default: throw new Exception();
            }
        }

        public override string ToString()
        {
            return $"{Name} {LoggerTools.GetIteratedVector(Rd, Half, Size)}, {LoggerTools.GetIteratedVector(Rn, Half, Size)}, {LoggerTools.GetIteratedVector(Rm, Half, Size)}";
        }
    }
}
