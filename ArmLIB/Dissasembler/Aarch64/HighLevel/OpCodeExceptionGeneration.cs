using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeExceptionGeneration : AOpCode, IOpCodeImm
    {
        public long Imm     { get; set; }

        public static OpCodeExceptionGeneration Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeExceptionGeneration(lowLevelAOpCode, Address, Name);

        public OpCodeExceptionGeneration(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(Address, lowLevelAOpCode.RawInstruction, Name)
        {
            Imm = lowLevelAOpCode.imm16;
        }

        public override string ToString() => $"{Name} {LoggerTools.GetImm(Imm)}";
    }
}
