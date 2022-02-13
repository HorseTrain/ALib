using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class SIMDOpCodeScalar1Src : OpCodeSIMD, IOpCodeRn
    {
        public OpCodeSize Size  { get; set; }
        public int Rn           { get; set; }

        bool DifferentSize      { get; set; }

        public static SIMDOpCodeScalar1Src CreateF(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeScalar1Src(lowLevelAOpCode, Address, Name, SIMDInstructionMode.Float);
        public static SIMDOpCodeScalar1Src CreateFCVT(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeScalar1Src(lowLevelAOpCode, Address, Name, SIMDInstructionMode.FloatDifferentSize);

        public SIMDOpCodeScalar1Src(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name, SIMDInstructionMode Mode) : base(lowLevelAOpCode, Address, Name)
        {
            Rn = lowLevelAOpCode.Rn;

            if (Mode == SIMDInstructionMode.FloatDifferentSize)
                DifferentSize = true;

            switch (Mode)
            {
                case SIMDInstructionMode.Float:
                case SIMDInstructionMode.FloatDifferentSize:
                    {
                        Size = (OpCodeSize)(lowLevelAOpCode.ptype + 2);

                        DecodingHelpers.EnsureSize(Size);
                    } break;
                default: throw new Exception();
            }
        }

        public override string ToString()
        {
            if (DifferentSize)
            {
                return $"{Name} {LoggerTools.GetRegister(Size == OpCodeSize.d ? OpCodeSize.s : OpCodeSize.d, Rd)}, {LoggerTools.GetRegister(Size, Rn)}";
            }

            return $"{Name} {LoggerTools.GetRegister(Size, Rd)}, {LoggerTools.GetRegister(Size, Rn)}";
        }
    }
}
