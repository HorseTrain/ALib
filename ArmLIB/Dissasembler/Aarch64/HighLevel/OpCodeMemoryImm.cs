using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeMemoryImm : OpCodeMemory, IOpCodeMemWback
    {
        public long Imm         { get; set; }
        public WbackMode Mode   { get; set; }

        public static OpCodeMemoryImm Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeMemoryImm(lowLevelAOpCode, Address, Name);

        public OpCodeMemoryImm(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(lowLevelAOpCode, Address, Name)
        {
            if (lowLevelAOpCode.V == 1)
            {
                IsVector = true;

                int Scale = ((lowLevelAOpCode.opc & 0b010) << 1) | lowLevelAOpCode.size;

                Size = (OpCodeSize)Scale;

                RawSize = Scale;
            }
            else
            {
                RawSize = lowLevelAOpCode.size; 

                Size = (OpCodeSize)lowLevelAOpCode.size;

                if (Size == OpCodeSize.s || Size == OpCodeSize.d)
                    Size += 3;

                SignExtendLoad = (lowLevelAOpCode.opc & 0b10) != 0;
                SignExtend32 = (lowLevelAOpCode.opc & 0b01) == 1;
            }

            switch (lowLevelAOpCode.ClassName)
            {
                case LowLevelClassNames.Load_store_register_unsigned_immediate:
                    {
                        Imm = lowLevelAOpCode.imm12;

                        Imm <<= GetSizeNum(Size);

                        Mode = WbackMode.None;
                    }; break;

                case LowLevelClassNames.Load_store_register_immediate_post_indexed:
                case LowLevelClassNames.Load_store_register_immediate_pre_indexed:
                case LowLevelClassNames.Load_store_register_unscaled_immediate:
                    {
                        Imm = DecodingHelpers.SignExtend(lowLevelAOpCode.imm9, 9);

                        //Mode = WbackMode.None;

                        switch ((RawInstruction >> 10) & 0b11)
                        {
                            case 0b01: Mode = WbackMode.Post; break;
                            case 0b11: Mode = WbackMode.Pre; break;
                            case 0b00: Mode = WbackMode.None; break;
                            default: throw new Exception();
                        }
                    } break;
                default: throw new NotImplementedException();
            }
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
            OpCodeSize LogSize =  !IsVector ? (Size == OpCodeSize.x ? OpCodeSize.x : OpCodeSize.w) : Size;

            if (SignExtendLoad)
            {
                return $"{Name} {LoggerTools.GetRegister(SignExtend32 ? OpCodeSize.w : OpCodeSize.x, Rt, false, IsVector)}, {GetMemoryOperand()}";
            }
            else
            {
                return $"{Name} {LoggerTools.GetRegister(LogSize, Rt, false, IsVector)}, {GetMemoryOperand()}";
            }
        }
    }
}
