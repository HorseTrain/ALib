using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class SIMDOpCodeVectorMemoryMultiple : OpCodeMemory, IOpCodeMemWback
    {
        public bool Replicate   { get; set; }
        public int Index        { get; set; }

        public WbackMode Mode   { get; set; }
        public long Imm         { get; set; }

        public bool Half        { get; set; }

        public static SIMDOpCodeVectorMemoryMultiple Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeVectorMemoryMultiple(lowLevelAOpCode, Address, Name);

        public int rpt          { get; set; }
        public int selem        { get; set; }

        SIMDOpCodeVectorMemoryMultiple(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(lowLevelAOpCode, Address, Name)
        {
            IsVector = true;

            (int scale, int index, bool replicate) = DecodingHelpers.GetLoadMultipleElement(lowLevelAOpCode);

            Mode = WbackMode.None;

            Size = (OpCodeSize)scale;
            Replicate = replicate;
            Index = index;

            Half = lowLevelAOpCode.Q == 0;

            switch (lowLevelAOpCode.opcode)
            {
                case 0b0000: rpt = 1; selem = 4; break;   // LD/ST4 (4 registers)
                case 0b0010: rpt = 4; selem = 1; break;   // LD/ST1 (4 registers)
                case 0b0100: rpt = 1; selem = 3; break;   // LD/ST3 (3 registers)
                case 0b0110: rpt = 3; selem = 1; break;   // LD/ST1 (3 registers)
                case 0b0111: rpt = 1; selem = 1; break;   // LD/ST1 (1 register)
                case 0b1000: rpt = 1; selem = 2; break;   // LD/ST2 (2 registers)
                case 0b1010: rpt = 2; selem = 1; break;   // LD/ST1 (2 registers)
            }
        }

        public override string ToString()
        {
            if (Replicate)
            {
                return $"{Name}" + " {"+ $"{LoggerTools.GetIteratedVector(Rt, Half, Size)}" +"}" + $", [{LoggerTools.GetRegister(OpCodeSize.x, Rn, true)}]";
            }
            else
            {
                return $"{Name}" + " {" + $"v{Rt}.{Size}" + "}" + $"[{Index}], [{LoggerTools.GetRegister(OpCodeSize.x, Rn, true)}]";
            }
        }
    }
}
