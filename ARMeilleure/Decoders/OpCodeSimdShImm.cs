using DCpu.Common;

namespace DCpu.Decoders
{
    class OpCodeSimdShImm : OpCodeSimd
    {
        public int Imm { get; private set; }

        public OpCodeSimdShImm(InstDescriptor inst, ulong address, int opCode) : base(inst, address, opCode)
        {
            Imm = (opCode >> 16) & 0x7f;

            Size = BitUtils.HighestBitSetNibble(Imm >> 3);
        }
    }
}
