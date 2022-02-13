using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeMemoryReg : OpCodeMemory, IOpCodeExtendedM
    {
        public int Rm           { get; set; }
        public Extend Option    { get; set; }
        public int Shift        { get; set; }

        public static OpCodeMemoryReg Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeMemoryReg(lowLevelAOpCode, Address, Name);

        public OpCodeMemoryReg(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(lowLevelAOpCode, Address, Name)
        {
            Rm = lowLevelAOpCode.Rm;
            Option = (Extend)lowLevelAOpCode.option;

            if (lowLevelAOpCode.V == 1)
            {
                IsVector = true;

                Size = (OpCodeSize)(((lowLevelAOpCode.opc & 0b10) << 1) | lowLevelAOpCode.size);
            }
            else
            {
                Size = (OpCodeSize)lowLevelAOpCode.size;

                SignExtendLoad = (lowLevelAOpCode.opc & 0b10) != 0;
                SignExtend32 = (lowLevelAOpCode.opc & 0b01) == 1;
            }

            RawSize = (int)Size;

            if (Size != OpCodeSize.b && lowLevelAOpCode.S == 1)
                Shift = (int)Size;
        }

        string GetPointer()
        {
            string PossibleShift = Shift == 0 ? "" : $", lsl {Shift}";
            string PossibleExtend = Shift == 0 ? $", {Option}" : $", {Option} #{Shift}";

            string m = $"{LoggerTools.GetRegister((Option == Extend.SXTX || Option == Extend.UXTX) ? OpCodeSize.x : OpCodeSize.w, Rm, false)}{((int)Option == 0b11 ? $"{PossibleShift}" : $"{PossibleExtend}")}";

            return $"[{LoggerTools.GetRegister(OpCodeSize.x, Rn, true)}, {m}]";
        }

        public override string ToString()
        {
            if (IsVector)
            {
                return $"{Name} {LoggerTools.GetRegister(Size, Rt, false, IsVector)}, {GetPointer()}";
            }
            else
            {
                if (SignExtendLoad)
                {
                    return $"{Name} {LoggerTools.GetRegister(SignExtend32 ? OpCodeSize.w : OpCodeSize.x, Rt)}, {GetPointer()}";
                }
                else
                {
                    return $"{Name} {LoggerTools.GetRegister(Size == OpCodeSize.d ? OpCodeSize.x : OpCodeSize.w, Rt)}, {GetPointer()}";
                }
            }
        }
    }
}
