namespace DCpu.Decoders
{
    interface IOpCodeAluImm : IOpCodeAlu
    {
        long Immediate { get; }
    }
}