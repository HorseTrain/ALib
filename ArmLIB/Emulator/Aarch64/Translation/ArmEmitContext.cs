using ArmLIB.Dissasembler.Aarch64.HighLevel;
using ArmLIB.Emulator.CodeGenerators;
using AlibCompiler.Intermediate;
using AlibCompiler.Intermediate.Extensions.X86;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64.Translation
{
    public unsafe class ArmEmitContext : SsaCodeGenerator
    {
        public HostArc Host                         { get; set; }
        public AOpCode CurrentInstruction           { get; set; }
        Dictionary<long, ConstOperand> BlockLabels  { get; set; }
        public ArmProcess process                      { get; set; }

        public bool EmulateAtomics                  { get; set; }
        public bool EnableSimd                      { get; set; }

        public bool EnableX86Extentions                 { get; set; } //sse1, sse2, avx

        public Dictionary<string, int> CustomOffsets    { get; set; }
        //Values Needed
        //ulong X[32]
        //ulong Q[64]
        //ulong n, z, c, v;
        //exitreason
        //return
        //locals

        int GetRegOffset(string Name) => CustomOffsets[Name];

        public bool OptimizeSSA                     { get; set; }
        public bool AllowInlineBranching            { get; set; }

        public ArmEmitContext(ArmBasicBlock[] BasicBlocks, ArmProcess process, Dictionary<string, int> ContextOffsets, bool OptimizeSSA) : base()
        {
            //context data
            Host = HostArc.X86;
            this.process = process;
            CustomOffsets = ContextOffsets;

            //optimizations
            this.OptimizeSSA = OptimizeSSA;
            AllowInlineBranching = true;
            EmulateAtomics = false;
            EnableX86Extentions = true;
            EnableSimd = true;

            //compile
            SetLocalStart(GetRegOffset("locals"));

            BlockLabels = new Dictionary<long, ConstOperand>();

            foreach (ArmBasicBlock block in BasicBlocks)
            {
                BlockLabels.Add(block.VirtualAddress, CreateLabel());
            }

            foreach (ArmBasicBlock block in BasicBlocks)
            {
                EmitBasicBlock(block);
            }
        }

        public static HashSet<Mnemonic> inss = new HashSet<Mnemonic>();

        void EmitBasicBlock(ArmBasicBlock block)
        {
            MarkLabel(BlockLabels[block.VirtualAddress]);

            foreach (AOpCode aOpCode in block.Instructions)
            {
                if (!OptimizeSSA)
                    ResetLocal();

                CurrentInstruction = aOpCode;

                CurrentEmitSize = OperandType.Int64;

                if (aOpCode is IOpCodeALUSize sized)
                {
                    if (sized.Size == OpCodeSize.w)
                        CurrentEmitSize = OperandType.Int32;
                }

                if (aOpCode.Emit == null || (IsSimd(aOpCode) && !EnableSimd))
                {
                    Console.WriteLine($"undefined emit: {aOpCode}"); 

                    //throw new Exception();

                    CurrentEmitSize = OperandType.Int64;

                    EmitUndefined();
                }
                else
                {
                    aOpCode.Emit(this);
                }
            }
        }

        bool IsSimd(AOpCode opCode)
        {
            return opCode is IOpCodeSimd || ((opCode is OpCodeMemory opm) && opm.IsVector);
        }

        public bool IsAValidBasicBlock(long Address)            => BlockLabels.ContainsKey(Address);
        public ConstOperand GetBlockLabel(long Address)         => BlockLabels[Address];

        IntReg CreateIntReg(int Index) => IntReg.Create(CurrentEmitSize, Index);

        public IntReg GetRegRaw(int Index, bool AllowRaw)
        {
            if (AllowRaw)
                return CreateIntReg(Index);

            IntReg Out = Local();

            ir.Copy(Out, CreateIntReg(Index));

            return Out;
        }

        public void SetRegRaw(int Index, IOperand Data)
        {
            ir.Copy(CreateIntReg(Index), ZeroExtend(Data, CurrentEmitSize));
        }

        public IOperand GetX(int Index, bool IsSP = false)
        {
            Debug.Assert(Index >= 0 && Index <= 31);

            if ((Index == 31 && IsSP) || (Index != 31))
                return GetRegRaw(GetRegOffset("X") + Index, true);

            return ConstOperand.Create(0);
        }

        public void SetX(int Index, IOperand Data, bool IsSP)
        {
            Debug.Assert(Index >= 0 && Index <= 31);

            if ((Index == 31 && IsSP) || (Index != 31))
                SetRegRaw(GetRegOffset("X") + Index, Data);
        }

        public IOperand GetRegRaw(string Name) => GetRegRaw(GetRegOffset(Name), true);
        public void SetRegRaw(string Name, IOperand Value) => SetRegRaw(GetRegOffset(Name), Value);

        public void ReturnWithValue(IOperand Data, ExitReason ReasonToExit)
        {
            if (ReasonToExit == ExitReason.Normal)
                Return(Data);

            Return(LogicalOr(Data, InstEmit64.Const((ulong)ReasonToExit << 48)));
        }

        public IOperand ZeroExtend(IOperand Source, OperandType Size)
        {
            if (Source is ConstOperand c)
                return ConstOperand.Create(c.Data, Size);

            return IntReg.Create(Size, (Source as IntReg).Reg);
        }

        public const string MemoryAddressString = "memoryaddress";
        public const string FunctionTableString = "functiontable";
        public const string FastLookupTableString = "fastlookuptable";
        public const string JitCacheBaseString = "jitcachebase";
        public const string ExclusiveAddressString = "exclusiveaddress";
        public const string ExclusiveValueString = "exclusivevalue";

        public IOperand MemoryAddress => CustomOffsets.ContainsKey(MemoryAddressString) ? GetRegRaw(MemoryAddressString) : InstEmit64.Const((ulong)process.GuestMemory.Base);
        public IOperand FunctionTable => CustomOffsets.ContainsKey(FunctionTableString) ? GetRegRaw(FunctionTableString) : InstEmit64.Const(Fallbacks.FunctionTable.GlobalFunctionTable);
        public IOperand FastLookupTable => CustomOffsets.ContainsKey(FastLookupTableString) ? GetRegRaw(FastLookupTableString) : InstEmit64.Const((ulong)process.FastLookupTable.BufferPTR);
        public IOperand JitCacheBase => CustomOffsets.ContainsKey(JitCacheBaseString) ? GetRegRaw(JitCacheBaseString) : InstEmit64.Const(process.HostMemory.Base);

        public void EmitUndefined()
        {
            ReturnWithValue(InstEmit64.Const(CurrentInstruction.Address), ExitReason.UndefinedInstruction);
        }

        public void MarkBranched()
        {

        }

        int GetContextVectorIndex(int Index) => GetRegOffset("Q") + (Index * 2);

        public Xmm CopyVector(Xmm Source)
        {
            Xmm Out = LocalVector(false);

            EmitX86(X86Instruction.Movaps, Out, Source);

            return Out;
        }

        public Xmm GetVector(int Index, bool GetRawVector = false)
        {
            if (GetRawVector)
            {
                return Xmm.Create(GetContextVectorIndex(Index));
            }

            Xmm Out = LocalVector(false);

            EmitX86(X86Instruction.Movaps, Out, Xmm.Create(GetContextVectorIndex(Index)));

            return Out;
        }

        public void SetVector(int Index, Xmm Vector)
        {
            EmitX86(X86Instruction.Movaps, Xmm.Create(GetContextVectorIndex(Index)), Vector);
        }
    }
}
