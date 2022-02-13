using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class SIMDOpCodeVector2SrcElement : OpCodeSIMD, IOpCodeRn, IOpCodeRm, OpCodeVectorSize
    {
        public int Rn           { get; set; }
        public int Rm           { get; set; }
        public bool Half        { get; set; }

        public OpCodeSize _size;
        public int Element;

        public OpCodeSize Size { get => _size; set { Size = value; } }

        public static SIMDOpCodeVector2SrcElement CreateF(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeVector2SrcElement(lowLevelAOpCode, Address, Name, SIMDInstructionMode.Float);

        public SIMDOpCodeVector2SrcElement(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name, SIMDInstructionMode Mode) : base(lowLevelAOpCode, Address, Name)
        {
            Rn = lowLevelAOpCode.Rn;
            Rm = lowLevelAOpCode.Rm | (lowLevelAOpCode.M << 4); //why tf is this done?

            Half = lowLevelAOpCode.Q == 0;

            DecodingHelpers.GetElement(ref Element, ref _size, lowLevelAOpCode, Mode);
        }

        public override string ToString() => $"{Name} {LoggerTools.GetIteratedVector(Rd, Half, Size)}, {LoggerTools.GetIteratedVector(Rn, Half, Size)}, v{Rm}.{Size}[{Element}]";
    }
}
