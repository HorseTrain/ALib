using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    //ins rd, rn, imm
    public class OpCodeALUImm : OpCodeALU, IOpCodeImm
    {
        public long Imm     { get; set; }

        bool ImmZeroShift;

        public static AOpCode Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeALUImm(lowLevelAOpCode, Address, Name);

        OpCodeALUImm(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base (lowLevelAOpCode, Address, Name)
        {
            switch (lowLevelAOpCode.ClassName)
            {
                case LowLevelClassNames.Add_subtract_immediate:
                    {
                        Imm = lowLevelAOpCode.imm12 << (lowLevelAOpCode.sh * 12);

                        RnIsSP = true;
                        RdIsSP = lowLevelAOpCode.S == 0;

                        ImmZeroShift = Imm == 0 && lowLevelAOpCode.sh == 1;
                    }
                    break;

                case LowLevelClassNames.Logical_immediate:
                    {
                        Imm = DecoderHelper.DecodeBitMask(lowLevelAOpCode.RawInstruction, true).WMask;

                        RdIsSP = true;

                        if (Name == Mnemonic.ands)
                            RdIsSP = false;

                        if (Size == OpCodeSize.w)
                            Imm &= uint.MaxValue;

                        break;
                    }
                default: throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            if (ImmZeroShift)
            {
                return $"{Name} {LoggerTools.GetRegister(Size, Rd, RdIsSP)}, {LoggerTools.GetRegister(Size, Rn, RnIsSP)}, {LoggerTools.GetImm(Imm)}, lsl 12";
            }

            return $"{Name} {LoggerTools.GetRegister(Size, Rd, RdIsSP)}, {LoggerTools.GetRegister(Size, Rn, RnIsSP)}, {LoggerTools.GetImm(Imm)}";
        }
    }
}
