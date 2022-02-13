using ArmLIB.Dissasembler.Aarch64.LowLevel;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeMemoryExclusive : OpCodeMemory, IOpCodeRs
    {
        public int Rs           { get; set; }
        bool ExclusiveStore     { get; set; }

        public static OpCodeMemoryExclusive Create(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new OpCodeMemoryExclusive(lowLevelAOpCode, Address, Name);

        public OpCodeMemoryExclusive(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) : base(lowLevelAOpCode, Address, Name)
        {
            Rs = lowLevelAOpCode.Rs;

            ExclusiveStore = lowLevelAOpCode.L == 0 && (Name.ToString().Contains("x"));

            Size = (OpCodeSize)lowLevelAOpCode.size;

            RawSize = (int)Size;
        }

        public override string ToString()
        {
            OpCodeSize rSize = Size == OpCodeSize.d ? OpCodeSize.x : OpCodeSize.w;

            if (ExclusiveStore)
            {
                return $"{Name} {LoggerTools.GetRegister(OpCodeSize.w, Rs)}, {LoggerTools.GetRegister(rSize, Rt)}, [{LoggerTools.GetRegister(OpCodeSize.x, Rn)}]";
            }
            else
            {
                return $"{Name} {LoggerTools.GetRegister(rSize, Rt)}, [{LoggerTools.GetRegister(OpCodeSize.x, Rn)}]";
            }
        }
    }
}
