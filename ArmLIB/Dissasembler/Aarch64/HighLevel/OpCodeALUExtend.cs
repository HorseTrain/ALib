using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeALUExtend : OpCodeALU, IOpCodeExtendedM
    {
        public int Rm           { get; set; }
        public Extend Option    { get; set; }
        public int Shift        { get; set; }

        public static OpCodeALUExtend Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeALUExtend(lowLevelAOpCode, Address, Name);

        OpCodeALUExtend(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(lowLevelAOpCode, Address, Name)
        {
            Rm = lowLevelAOpCode.Rm;

            switch (lowLevelAOpCode.ClassName)
            {
                case LowLevelClassNames.Add_subtract_extended_register:
                    {
                        RnIsSP = true;
                        RdIsSP = lowLevelAOpCode.S == 0;

                        Option = (Extend)lowLevelAOpCode.option;
                        Shift = lowLevelAOpCode.imm3;
                    }
                    break;
                default: throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            bool Is64 = Size == OpCodeSize.x && (Option == Extend.SXTX || Option == Extend.UXTX);

            return $"{Name} {LoggerTools.GetRegister(Size, Rd, RdIsSP)}, {LoggerTools.GetRegister(Size, Rn, RnIsSP)}, {LoggerTools.GetRegister(Is64 ? OpCodeSize.x : OpCodeSize.w, Rm)}, {Option.ToString().ToLower()} {LoggerTools.GetImm(Shift)}";
        }
    }
}
