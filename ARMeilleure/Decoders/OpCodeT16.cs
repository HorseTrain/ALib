namespace DCpu.Decoders
{
    class OpCodeT16 : OpCode32
    {
        public OpCodeT16(InstDescriptor inst, ulong address, int opCode) : base(inst, address, opCode)
        {
            Cond = Condition.Al;

            OpCodeSizeInBytes = 2;
        }
    }
}