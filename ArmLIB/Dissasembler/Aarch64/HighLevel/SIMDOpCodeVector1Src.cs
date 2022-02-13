using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class SIMDOpCodeVector1Src : OpCodeSIMD, IOpCodeRn
    {
        public int Rn           { get; set; }
        public OpCodeSize Size  { get; set; }
        public bool Half        { get; set; }

        public bool WithZero    { get; set; }

        SIMDInstructionMode mode    { get; set; }

        public static SIMDOpCodeVector1Src CreateFSZ(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeVector1Src(lowLevelAOpCode, Address, Name, SIMDInstructionMode.FloatSZ);
        public static SIMDOpCodeVector1Src CreateF(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeVector1Src(lowLevelAOpCode, Address, Name, SIMDInstructionMode.Float);
        public static SIMDOpCodeVector1Src CreateL(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeVector1Src(lowLevelAOpCode, Address, Name, SIMDInstructionMode.Logical);
        public static SIMDOpCodeVector1Src CreateFZ(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeVector1Src(lowLevelAOpCode, Address, Name, SIMDInstructionMode.FloatWithZero);
        public static SIMDOpCodeVector1Src CreateIS(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeVector1Src(lowLevelAOpCode, Address, Name, SIMDInstructionMode.IntWithSize);
        public static SIMDOpCodeVector1Src CreateVDS(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeVector1Src(lowLevelAOpCode, Address, Name, SIMDInstructionMode.VectorDoubleSize);

        public SIMDOpCodeVector1Src(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name, SIMDInstructionMode Mode) : base(lowLevelAOpCode, Address, Name)
        {
            Rn = lowLevelAOpCode.Rn;

            Half = lowLevelAOpCode.Q == 0;

            if (Mode == SIMDInstructionMode.FloatWithZero)
                WithZero = true;

            this.mode = Mode;

            switch (Mode)
            {
                case SIMDInstructionMode.IntWithSize: Size = (OpCodeSize)lowLevelAOpCode.size; break;

                case SIMDInstructionMode.Float:
                case SIMDInstructionMode.FloatWithZero:
                    {
                        Size = (OpCodeSize)(lowLevelAOpCode.ptype + 2);

                        DecodingHelpers.EnsureSize(Size);
                    }; break;

                case SIMDInstructionMode.FloatSZ:
                    {
                        Size = (OpCodeSize)(lowLevelAOpCode.size + 2);

                        DecodingHelpers.EnsureSize(Size);
                    }; break;

                case SIMDInstructionMode.VectorDoubleSize:
                    {
                        Size = (OpCodeSize)(lowLevelAOpCode.size);
                    }; break;

                case SIMDInstructionMode.Logical:
                    {
                        Size = OpCodeSize.b;
                    } break;
                default: throw new Exception();
            }
        }

        public override string ToString()
        {
            if (mode == SIMDInstructionMode.VectorDoubleSize)
            {
                return $"{Name} {Size + 1}{Rd}, {LoggerTools.GetIteratedVector(Rn, Half, Size)}";
            }

            return $"{Name} {LoggerTools.GetIteratedVector(Rd, Half, Size)}, {LoggerTools.GetIteratedVector(Rn, Half, Size)}{(WithZero ? $", {LoggerTools.GetImmF(0)}" : "")}";
        }
    }
}
