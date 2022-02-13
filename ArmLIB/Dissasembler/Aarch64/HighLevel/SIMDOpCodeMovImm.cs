using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class SIMDOpCodeMovImm : OpCodeSIMD, IOpCodeImm
    {
        public OpCodeSize Size  { get; set; }
        public long Imm         { get; set; }
        public bool IsFloat     { get; set; }

        public bool Half        { get; set; }

        int rimm                { get; set; }
        int shift               { get; set; }

        bool DisData            { get; set; }

        public static SIMDOpCodeMovImm Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeMovImm(lowLevelAOpCode, Address, Name);

        private static long ShlOnes(long value, int shift)
        {
            if (shift != 0)
            {
                return value << shift | (long)(ulong.MaxValue >> (64 - shift));
            }
            else
            {
                return value;
            }
        }

        public SIMDOpCodeMovImm(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(lowLevelAOpCode, Address, Name)
        {
            int imm = (RawInstruction >> 5) & 0x1F;
            imm |= (((RawInstruction >> 16) & 0x7) << 5);

            Half = lowLevelAOpCode.Q == 0;

            int cMode = (RawInstruction >> 12) & 0xf;
            int modeLow = cMode & 1;
            int modeHigh = cMode >> 1;

            if (lowLevelAOpCode.op == 0 && (lowLevelAOpCode.cmode == 0b1110))
            {
                //8 bit

                Size = OpCodeSize.b;

                Imm = imm;
            }
            else if (lowLevelAOpCode.op == 0 && ((lowLevelAOpCode.cmode & 0b1101) == 0b1000))
            {
                //16 bit shifted imm

                Size = OpCodeSize.h;

                Imm <<= (modeHigh & 1) << 3;
            }
            else if (lowLevelAOpCode.op == 0 && ((lowLevelAOpCode.cmode & 0b1001) == 0))
            {
                //32 bit shifting imm

                shift = (modeHigh << 3);
                rimm = imm;

                Imm = imm << (modeHigh << 3);

                Size = OpCodeSize.s;

                DisData = true;
            }
            else if (lowLevelAOpCode.op == 0 && ((lowLevelAOpCode.cmode & 0b1110) == 0b1100))
            {
                //32 bit shifting ones.

                Size = OpCodeSize.s;

                Imm = ShlOnes(imm, 8 << modeLow);
            }
            else if (lowLevelAOpCode.cmode == 0b1110 && lowLevelAOpCode.op == 1)
            {
                //64 bit
                IsFloat = false;

                Size = OpCodeSize.d;

                Imm = DecodingHelpers.ExpandImm8(imm);
            }
            else
            {
                throw new Exception();
            }
        }

        public override string ToString()
        {
            if (IsFloat)
            {
                throw new Exception();
            }
            else
            {
                if (DisData)
                    return $"{Name} {LoggerTools.GetIteratedVector(Rd, Half, Size)}, {LoggerTools.GetImm(rimm)}, lsl {LoggerTools.GetImm(shift)}";

                return $"{Name} {LoggerTools.GetIteratedVector(Rd, Half, Size)}, {LoggerTools.GetImm(Imm)}";
            }
        }
    }
}
