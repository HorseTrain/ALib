using DCpu.Translation;

namespace DCpu.CodeGen.RegisterAllocators
{
    interface IRegisterAllocator
    {
        AllocationResult RunPass(
            ControlFlowGraph cfg,
            StackAllocator stackAlloc,
            RegisterMasks regMasks);
    }
}