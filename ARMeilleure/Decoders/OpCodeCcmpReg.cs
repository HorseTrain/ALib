namespace DCpu.Decoders
{
    class OpCodeCcmpReg : OpCodeCcmp, IOpCodeAluRs
    {
        public int Rm => RmImm;

        public int Shift => 0;

        public ShiftType ShiftType => ShiftType.Lsl;

        public OpCodeCcmpReg(InstDescriptor inst, ulong address, int opCode) : base(inst, address, opCode) { }
    }
}