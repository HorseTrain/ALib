using DCpu.IntermediateRepresentation;

namespace DCpu.CodeGen.Unwinding
{
    struct UnwindPushEntry
    {
        public int Index { get; }

        public RegisterType Type { get; }

        public int StreamEndOffset { get; }

        public UnwindPushEntry(int index, RegisterType type, int streamEndOffset)
        {
            Index           = index;
            Type            = type;
            StreamEndOffset = streamEndOffset;
        }
    }
}