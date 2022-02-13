using ArmLIB.Dissasembler.Aarch64.LowLevel;
using ArmLIB.Emulator.Aarch64.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class AOpCode
    {
        public InstEmit Emit            { get; set; }
        public Mnemonic Name            { get; set; }

        public LowLevelAOpCode RawData  { get; set; }

        public long Address         { get; set; }
        public int RawInstruction   { get; set; }

        public AOpCode(long Address, int RawInstruction, Mnemonic Name)
        {
            this.Address = Address;
            this.RawInstruction = RawInstruction;
            this.Name = Name;
        }

        public override string ToString()
        {
            return " " + MiddleMan.GetName(RawInstruction);
        }
    }
}
