namespace DCpu.Decoders
{
    class OpCodeBReg : OpCode
    {
        public int Rn { get; private set; }

        public OpCodeBReg(InstDescriptor inst, ulong address, int opCode) : base(inst, address, opCode)
        {
            int op4 = (opCode >>  0) & 0x1f;
            int op2 = (opCode >> 16) & 0x1f;

            if (op2 != 0b11111 || op4 != 0b00000)
            {
                Instruction = InstDescriptor.Undefined;

                return;
            }

            Rn = (opCode >> 5) & 0x1f;
        }
    }
}