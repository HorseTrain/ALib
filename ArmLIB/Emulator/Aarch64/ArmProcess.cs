using ArmLIB.Dissasembler.Aarch64.HighLevel;
using ArmLIB.Emulator.Aarch64.Translation;
using ArmLIB.Emulator.CodeGenerators;
using AlibCompiler.Backend.X86;
using AlibCompiler.Intermediate;
using AlibCompiler.Tools.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64
{
    public unsafe class ArmProcess 
    {
        public IGuestMemoryModel GuestMemory                            { get; set; }
        public IHostMemoryManager HostMemory                            { get; set; }
        ConcurrentDictionary<ulong, TranslatedFunction> GuestFunctions  { get; set; }

        ConcurrentDictionary<ulong, List<TranslatedFunction>> GuestFunctionPages    { get; set; }

        public bool TestDisam                                           { get; set; }

        public FastLookupTable FastLookupTable                          { get; set; }

        public SvcCall svcFallBack                                      { get; set; }

        public bool UseFlt                                              => FastLookupTable != null;   

        public ArmProcess(IGuestMemoryModel GuestMemory, IHostMemoryManager HostMemory)
        {
            this.GuestMemory = GuestMemory;
            this.HostMemory = HostMemory;

            GuestFunctions = new ConcurrentDictionary<ulong, TranslatedFunction>();
        }

        public void InitFastLookupTable(ulong Start, ulong FltSize)
        {
            if (UseFlt)
                return;

            FastLookupTable = new FastLookupTable(Start, FltSize);
        }

        public void SubmitFastFunction(ulong VirtualAddress, void* NativeAddress)
        {
            if (!UseFlt || !FastLookupTable.InRange(VirtualAddress))
                return;

            ulong Offset = (ulong)NativeAddress - (uint)HostMemory.Base;

            if (Offset >= uint.MaxValue) //This should never happen?? 
                return;

            FastLookupTable.SubmitFunction(VirtualAddress, (uint)Offset);
        }

        public static void ValidateInstructionAddress(ulong Address) => Debug.Assert((Address & ~0b11UL) == Address);

        AOpCode ReadInstruction(ulong Address)
        {
            ValidateInstructionAddress(Address);

            int RawInstruction = *(int*)GuestMemory.TranslateAddress(Address);

            return MiddleMan.GetAOpCode((long)Address, RawInstruction);
        }

        ArmBasicBlock GetBasicBlock(ulong Address)
        {
            ArmBasicBlock Out = new ArmBasicBlock();

            Out.VirtualAddress = (long)Address;

            while (true)
            {
                AOpCode CurrentInstruction = ReadInstruction(Address);

                Out.Instructions.Add(CurrentInstruction);

                if (CurrentInstruction is IOpCodeBranch)
                    break;

                Address += 4;
            }

            if (TestDisam)
                Out.Validate();

            return Out;
        }

        ArmBasicBlock[] GetBasicBlocks(ulong Base, bool Multiple)
        {
            List<ArmBasicBlock> Out = new List<ArmBasicBlock>();

            HashSet<ulong> Already = new HashSet<ulong>();

            void GetBlock(ulong Address)
            {
                if (Already.Contains(Address))
                    return;

                Already.Add(Address);

                ArmBasicBlock block = GetBasicBlock(Address);

                Out.Add(block);

                if (Multiple)
                {
                    if (block.Last is IOpCodeConditionalBranch opcCB)
                    {
                        GetBlock((ulong)opcCB.Imm);
                        GetBlock((ulong)opcCB.NextAddress);
                    }
                    else if (block.Last is OpCodeUnconditionalBranchImm opcBimm)
                    {
                        GetBlock((ulong)opcBimm.Imm);
                    }
                }
            }

            GetBlock(Base);

            return Out.ToArray();
        }

        TranslatedFunction TranslateFunction(ulong Address, bool Multple, Dictionary<string, int> ContextOffsets = null, bool OptimizeSSA = false)
        {
            ArmBasicBlock[] Blocks = GetBasicBlocks(Address, Multple);

            if (ContextOffsets == null)
            {
                ContextOffsets = new Dictionary<string, int>() {

                    {"X", ArmContext.GetOffsetOf("X") },
                    {"Q", ArmContext.GetOffsetOf("Q") },

                    {"n", ArmContext.GetOffsetOf("n") },
                    {"z", ArmContext.GetOffsetOf("z") },
                    {"c", ArmContext.GetOffsetOf("c") },
                    {"v", ArmContext.GetOffsetOf("v") },

                    {"locals", ArmContext.GetOffsetOf("LocalBuffer") },

                };
            }

            ArmEmitContext emitContext = new ArmEmitContext(Blocks, this, ContextOffsets, OptimizeSSA);

            OperationBlock CurrentIR = emitContext.ir;

            int LocalStart = ContextOffsets["locals"];

            if (OptimizeSSA)
            {
                CurrentIR = SsaCodeOptimizer.ClampLocals(CurrentIR, LocalStart);

                CurrentIR = SsaCodeOptimizer.OptimizeDeadRegisters(CurrentIR, LocalStart);
            }

            //Console.WriteLine(CurrentIR);

            CompiledFunction nativeFunction = X86Assembler.CompileOperations<CompiledFunction>(CurrentIR, HostMemory, Address,out ulong FunctionAddress, out Allocation allocation);

            TranslatedFunction Out = new TranslatedFunction(HostMemory, FunctionAddress, nativeFunction, allocation);

            SubmitFunctionToPage(Address, Out);

            return Out;
        }

        void SubmitFunctionToPage(ulong Entry,TranslatedFunction function)
        {
            ulong PageIndex = Entry & ~4095UL;

            if (!GuestFunctionPages.ContainsKey(PageIndex))
            {
                GuestFunctionPages.TryAdd(PageIndex, new List<TranslatedFunction>());
            }

            GuestFunctionPages[PageIndex].Add(function);
        }

        public TranslatedFunction GetOrTranslateFunction(ulong Address, bool MultipleBlocks = true, Dictionary<string, int> ContextOffsets = null, bool OptimizeSSA = false)
        {
            TranslatedFunction Out;

            if (GuestFunctions.TryGetValue(Address, out Out))
            {
                return Out;
            }

            Out = TranslateFunction(Address, MultipleBlocks, ContextOffsets, OptimizeSSA);

            GuestFunctions.TryAdd(Address, Out);

            if (UseFlt)
            {
                ulong JitOffset = Out.NativeAddress - HostMemory.Base;

                if (JitOffset < uint.MaxValue)
                {
                    FastLookupTable.SubmitFunction(Address, (uint)JitOffset);
                }
            }

            return Out;
        }
    }
}
