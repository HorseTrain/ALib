using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeALUShiftedReg : OpCodeALU, IOpCodeRm
    {
        public int Rm           { get; set; }
        public ShiftType Type   { get; set; }
        public int Shift        { get; set; }

        public static OpCodeALUShiftedReg Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeALUShiftedReg(lowLevelAOpCode, Address, Name);

        public OpCodeALUShiftedReg(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(lowLevelAOpCode, Address, Name)
        {
            Rm = lowLevelAOpCode.Rm;

            Type = (ShiftType)lowLevelAOpCode.shift;
            Shift = lowLevelAOpCode.imm6;
        }

        public override string ToString() => $"{Name} {LoggerTools.GetRegister(Size, Rd)}, {LoggerTools.GetRegister(Size, Rn)}, {LoggerTools.GetRegister(Size, Rm)}, {Type.ToString().ToLower()} {LoggerTools.GetImm(Shift)}";
    }
}
