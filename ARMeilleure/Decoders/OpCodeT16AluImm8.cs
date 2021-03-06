namespace DCpu.Decoders
{
    class OpCodeT16AluImm8 : OpCodeT16, IOpCode32Alu
    {
        private int _rdn;

        public int Rd => _rdn;
        public int Rn => _rdn;

        public bool SetFlags => false;

        public int Immediate { get; private set; }

        public OpCodeT16AluImm8(InstDescriptor inst, ulong address, int opCode) : base(inst, address, opCode)
        {
            Immediate = (opCode >> 0) & 0xff;
            _rdn      = (opCode >> 8) & 0x7;
        }
    }
}