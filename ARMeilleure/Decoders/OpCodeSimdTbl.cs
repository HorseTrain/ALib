namespace DCpu.Decoders
{
    class OpCodeSimdTbl : OpCodeSimdReg
    {
        public OpCodeSimdTbl(InstDescriptor inst, ulong address, int opCode) : base(inst, address, opCode)
        {
            Size = ((opCode >> 13) & 3) + 1;
        }
    }
}