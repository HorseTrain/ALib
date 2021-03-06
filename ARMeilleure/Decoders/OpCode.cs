using DCpu.IntermediateRepresentation;
using System;

namespace DCpu.Decoders
{
    class OpCode : IOpCode
    {
        public ulong Address   { get; private set; }
        public int   RawOpCode { get; private set; }

        public int OpCodeSizeInBytes { get; protected set; } = 4;

        public InstDescriptor Instruction { get; protected set; }

        public RegisterSize RegisterSize { get; protected set; }

        public OpCode(InstDescriptor inst, ulong address, int opCode)
        {
            Address   = address;
            RawOpCode = opCode;

            Instruction = inst;

            RegisterSize = RegisterSize.Int64;
        }

        public int GetPairsCount() => GetBitsCount() / 16;
        public int GetBytesCount() => GetBitsCount() / 8;

        public int GetBitsCount()
        {
            switch (RegisterSize)
            {
                case RegisterSize.Int32:   return 32;
                case RegisterSize.Int64:   return 64;
                case RegisterSize.Simd64:  return 64;
                case RegisterSize.Simd128: return 128;
            }

            throw new InvalidOperationException();
        }

        public OperandType GetOperandType()
        {
            return RegisterSize == RegisterSize.Int32 ? OperandType.I32 : OperandType.I64;
        }
    }
}