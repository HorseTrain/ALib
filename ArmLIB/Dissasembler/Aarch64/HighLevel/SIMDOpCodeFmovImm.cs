using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class SIMDOpCodeFmovImm : OpCodeSIMD, IOpCodeImm
    {
        public OpCodeSize Size  { get; set; }
        public long Imm         { get; set; }

        public static SIMDOpCodeFmovImm Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeFmovImm(lowLevelAOpCode, Address, Name);

        public SIMDOpCodeFmovImm(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(lowLevelAOpCode, Address, Name)
        {
            Size = (OpCodeSize)(lowLevelAOpCode.ptype + 2);

            DecodingHelpers.EnsureSize(Size);

            Imm = DecodingHelpers.GetFloatImm(lowLevelAOpCode.imm8, Size);
        }

        public override string ToString()
        {
            switch (Size)
            {
                case OpCodeSize.s: return $"{Name} {LoggerTools.GetRegister(Size, Rd)}, {LoggerTools.GetImmF(Aarch64DebugAssembler.Convert<float, int>((int)Imm))}";
                case OpCodeSize.d: return $"{Name} {LoggerTools.GetRegister(Size, Rd)}, {LoggerTools.GetImmF(Aarch64DebugAssembler.Convert<double, long>(Imm))}";
                default: throw new Exception();
            }
        }
    }
}
