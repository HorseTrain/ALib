using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeMemoryPair : OpCodeMemory, IOpCodeMemWback, IOpCodeRt2
    {
        public long Imm         { get; set; }
        public WbackMode Mode   { get; set; }
        public int Rt2          { get; set; }

        public static OpCodeMemoryPair Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeMemoryPair(lowLevelAOpCode, Address, Name);

        OpCodeMemoryPair(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(lowLevelAOpCode, Address, Name)
        {
            Rt2 = lowLevelAOpCode.Rt2;

            Imm = DecodingHelpers.SignExtend(lowLevelAOpCode.imm7, 7);

            int scale;

            switch ((lowLevelAOpCode.RawInstruction >> 23) & 0b11)
            {
                case 0b10: Mode = WbackMode.None; break;
                case 0b01: Mode = WbackMode.Post; break;
                case 0b11: Mode = WbackMode.Pre; break;
                default: throw new Exception();
            }

            if (lowLevelAOpCode.V == 1)
            {
                IsVector = true;

                scale = 2 + lowLevelAOpCode.opc;

                switch (scale)
                {
                    case 2: Size = OpCodeSize.s; break;
                    case 3: Size = OpCodeSize.d; break;
                    case 4: Size = OpCodeSize.q; break;
                    default: throw new Exception();
                }
            }
            else
            {
                if (lowLevelAOpCode.LowLevelName == LowLevelNames.LDPSW)
                {
                    SignExtend32 = false;
                    SignExtendLoad = true;
                }

                scale = 2 + ((lowLevelAOpCode.opc & 0b10) >> 1);

                switch (scale)
                {
                    case 3: Size = OpCodeSize.x; break;
                    case 2: Size = OpCodeSize.w; break;
                    default: throw new Exception();
                }
            }

            RawSize = scale;

            Imm <<= scale;
        }

        string GetMemoryOperand()
        {
            if (Mode == WbackMode.None)
                return $"[{LoggerTools.GetRegister(OpCodeSize.x, Rn, RnIsSP)}, {LoggerTools.GetImm(Imm)}]";

            if (Mode == WbackMode.Pre)
                return $"[{LoggerTools.GetRegister(OpCodeSize.x, Rn, RnIsSP)}, {LoggerTools.GetImm(Imm)}]!";

            return $"[{LoggerTools.GetRegister(OpCodeSize.x, Rn, RnIsSP)}], {LoggerTools.GetImm(Imm)}";
        }

        public override string ToString()
        {
            if (SignExtendLoad)
            {
                return $"{Name} {LoggerTools.GetRegister(OpCodeSize.x, Rt, false, IsVector)}, {LoggerTools.GetRegister(OpCodeSize.x, Rt2)}, {GetMemoryOperand()}";
            }
            else
            {
                return $"{Name} {LoggerTools.GetRegister(Size, Rt, false, IsVector)}, {LoggerTools.GetRegister(Size, Rt2)}, {GetMemoryOperand()}";
            }
        }
    }
}
