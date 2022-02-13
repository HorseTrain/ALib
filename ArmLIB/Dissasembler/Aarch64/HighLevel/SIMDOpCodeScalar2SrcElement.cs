using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class SIMDOpCodeScalar2SrcElement : OpCodeSIMD, IOpCodeRn, IOpCodeRm, OpCodeVectorSize
    {
        public int Rn           { get; set; }
        public int Rm           { get; set; }

        public int Element;
        OpCodeSize _size;

        public OpCodeSize Size { get => _size; set { Size = value; } }

        public static SIMDOpCodeScalar2SrcElement CreateF(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeScalar2SrcElement(lowLevelAOpCode, Address, Name, SIMDInstructionMode.Float);

        public SIMDOpCodeScalar2SrcElement(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name, SIMDInstructionMode Mode) : base(lowLevelAOpCode, Address, Name)
        {
            Rn = lowLevelAOpCode.Rn;
            Rm = lowLevelAOpCode.Rm | (lowLevelAOpCode.M << 4); //why tf is this done?

            DecodingHelpers.GetElement(ref Element, ref _size, lowLevelAOpCode, Mode);
        }

        public override string ToString() => $"{Name} {LoggerTools.GetRegister(_size, Rd, false, true)}, {LoggerTools.GetRegister(_size, Rn, false, true)}, v{Rm}.{_size}[{Element}]";
    }
}
