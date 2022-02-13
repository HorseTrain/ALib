namespace DCpu.Decoders
{
    class OpCodeSimdMemLit : OpCode, IOpCodeSimd, IOpCodeLit
    {
        public int  Rt        { get; private set; }
        public long Immediate { get; private set; }
        public int  Size      { get; private set; }
        public bool Signed   => false;
        public bool Prefetch => false;

        public OpCodeSimdMemLit(InstDescriptor inst, ulong address, int opCode) : base(inst, address, opCode)
        {
            int opc = (opCode >> 30) & 3;

            if (opc == 3)
            {
                Instruction = InstDescriptor.Undefined;

                return;
            }

            Rt = opCode & 0x1f;

            Immediate = (long)address + DecoderHelper.DecodeImmS19_2(opCode);

            Size = opc + 2;
        }
    }
}