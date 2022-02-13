namespace DCpu.Decoders
{
    interface IOpCodeAluRs : IOpCodeAlu
    {
        int Shift { get; }
        int Rm    { get; }

        ShiftType ShiftType { get; }
    }
}