using ArmLIB.Dissasembler.Aarch64.HighLevel;
using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64.Translation
{
    public class SIMDOpCodeVectorExtract : OpCodeSIMD, IOpCodeRn, IOpCodeRm, IOpCodeImm
    {
        public int Rn       { get; set; }
        public int Rm       { get; set; }
        public long Imm     { get; set; }

        public bool Half    { get; set; }

        public static SIMDOpCodeVectorExtract CreateEXT(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeVectorExtract(lowLevelAOpCode, Address, Name, SIMDInstructionMode.ExtractVector);

        SIMDOpCodeVectorExtract(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name, SIMDInstructionMode Mode) : base(lowLevelAOpCode, Address, Name)
        {
            Rn = lowLevelAOpCode.Rn;
            Rm = lowLevelAOpCode.Rm;

            switch (Mode)
            {
                case SIMDInstructionMode.ExtractVector: Imm = lowLevelAOpCode.imm4; break;
                default: throw new Exception();
            }

            Half = lowLevelAOpCode.Q == 0;
        }

        public override string ToString()
        {
            return $"{Name} {LoggerTools.GetIteratedVector(Rd, Half, OpCodeSize.b)}, {LoggerTools.GetIteratedVector(Rn, Half, OpCodeSize.b)}, {LoggerTools.GetIteratedVector(Rm, Half, OpCodeSize.b)}, {LoggerTools.GetImm(Imm)}";
        }
    }
}
