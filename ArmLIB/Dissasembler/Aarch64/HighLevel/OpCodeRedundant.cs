using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeRedundant : AOpCode
    {
        public static OpCodeRedundant Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeRedundant(lowLevelAOpCode, Address, Name);

        public OpCodeRedundant(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(Address, lowLevelAOpCode.RawInstruction, Name)
        {
            //NO ENCODING NEEDED
        }

        public override string ToString()
        {
            return MiddleMan.GetName(RawInstruction);
        }
    }
}
