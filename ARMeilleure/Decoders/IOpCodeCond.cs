namespace DCpu.Decoders
{
    interface IOpCodeCond : IOpCode
    {
        Condition Cond { get; }
    }
}