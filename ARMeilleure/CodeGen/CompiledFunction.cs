using DCpu.CodeGen.Unwinding;

namespace DCpu.CodeGen
{
    struct CompiledFunction
    {
        public byte[] Code { get; }

        public UnwindInfo UnwindInfo { get; }

        public CompiledFunction(byte[] code, UnwindInfo unwindInfo)
        {
            Code       = code;
            UnwindInfo = unwindInfo;
        }
    }
}