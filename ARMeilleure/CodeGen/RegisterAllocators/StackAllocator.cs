using DCpu.Common;
using DCpu.IntermediateRepresentation;
using System;

namespace DCpu.CodeGen.RegisterAllocators
{
    class StackAllocator
    {
        private int _offset;

        public int TotalSize => _offset;

        public int Allocate(OperandType type)
        {
            return Allocate(type.GetSizeInBytes());
        }

        public int Allocate(int sizeInBytes)
        {
            int offset = _offset;

            _offset += sizeInBytes;

            return offset;
        }
    }
}