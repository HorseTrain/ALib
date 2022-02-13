using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeSys : AOpCode, IOpCodeRt
    {
        public int Rt   { get; set; }

        public static OpCodeSys Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeSys(lowLevelAOpCode, Address, Name);

        public OpCodeSys(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(Address, lowLevelAOpCode.RawInstruction, Name)
        {
            //TODO:

            Rt = lowLevelAOpCode.Rt;
        }

        public override string ToString()
        {
            return MiddleMan.GetName(RawInstruction);
        }
    }
}
