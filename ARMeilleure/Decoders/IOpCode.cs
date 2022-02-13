using DCpu.IntermediateRepresentation;

namespace DCpu.Decoders
{
    interface IOpCode
    {
        ulong Address { get; }

        InstDescriptor Instruction { get; }

        RegisterSize RegisterSize { get; }

        int GetBitsCount();

        OperandType GetOperandType();
    }
}