namespace DCpu.Decoders
{
    interface IOpCodeSimd : IOpCode
    {
        int Size { get; }
    }
}