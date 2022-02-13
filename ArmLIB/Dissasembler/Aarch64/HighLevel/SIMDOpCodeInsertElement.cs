using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class SIMDOpCodeInsertElement : OpCodeSIMD, IOpCodeRn
    {
        public OpCodeSize Size          { get; set; }
        public int Rn                   { get; set; }

        public bool Half                { get; set; }

        public int Index                { get; set; }

        public SIMDInstructionMode Mode { get; set; }

        public static SIMDOpCodeInsertElement CreateT(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeInsertElement(lowLevelAOpCode, Address, Name, SIMDInstructionMode.MoveToScalar);
        public static SIMDOpCodeInsertElement CreateU(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeInsertElement(lowLevelAOpCode, Address, Name, SIMDInstructionMode.NULL);
        public static SIMDOpCodeInsertElement CreateF(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeInsertElement(lowLevelAOpCode, Address, Name, SIMDInstructionMode.NULL);
        public static SIMDOpCodeInsertElement CreateV(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeInsertElement(lowLevelAOpCode, Address, Name, SIMDInstructionMode.MoveToVector);

        SIMDOpCodeInsertElement(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name, SIMDInstructionMode Mode) : base(lowLevelAOpCode, Address, Name)
        {
            this.Mode = Mode;

            Rn = lowLevelAOpCode.Rn;

            Size = (OpCodeSize)DecodingHelpers.LowestBitSet(lowLevelAOpCode.imm5);

            Index = lowLevelAOpCode.imm5 >> ((int)Size + 1);

            if (Size == OpCodeSize.d)
                Index &= 1;

            Half = lowLevelAOpCode.Q == 0;
        }

        public override string ToString()
        {
            if (Mode == SIMDInstructionMode.MoveToVector)
                return $"{Name} {LoggerTools.GetIteratedVector(Rd, Half, Size)} ,v{Rn}.{Size}[{Index}]";

            if (Mode == SIMDInstructionMode.MoveToScalar)
                return $"{Name} v{Rd}.{Size}[{Index}], {(Size == OpCodeSize.d ? OpCodeSize.x : OpCodeSize.w)}{Rn}";

            OpCodeSize LogSize = Size;

            if (Name == Mnemonic.umov)
            {
                LogSize = OpCodeSize.w;

                if (Size == OpCodeSize.d)
                    LogSize = OpCodeSize.x;
            }

            return $"{Name} {LoggerTools.GetRegister(LogSize, Rd, false)}, v{Rn}.{Size}[{Index}]";
        }
    }
}
