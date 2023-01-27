using ArmLIB.Dissasembler.Aarch64.HighLevel;
using AlibCompiler.Intermediate;
using AlibCompiler.Intermediate.Extensions.X86;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Mono.Unix.Native;

namespace ArmLIB.Emulator.CodeGenerators
{
    public delegate void IfBlock();

    public class SsaCodeGenerator
    {
        public OperationBlock ir { get; set; }

        public OperandType CurrentEmitSize { get; set; }
        protected int LocalStart { get; set; }
        int LocalCurrent { get; set; }

        public SsaCodeGenerator()
        {
            ir = new OperationBlock();
        }

        public void SetLocalStart(int offset)
        {
            LocalStart = offset;
            LocalCurrent = offset;
        }

        public void ResetLocal()
        {
            LocalCurrent = LocalStart;
        }

        public void SetOperandGenerationSize(OperandType Size) => CurrentEmitSize = Size;

        const int LocalSize = 2;

        HashSet<int> ReservedGPLocals = new HashSet<int>();
        HashSet<int> ReservedXMMLocals = new HashSet<int>();

        public IntReg Local()
        {
            while (true)
            {
                LocalCurrent += LocalSize;

                if (!ReservedXMMLocals.Contains(LocalCurrent))
                {
                    break;
                }
            }

            ReservedGPLocals.Add(LocalCurrent);

            return IntReg.Create(CurrentEmitSize, LocalCurrent);
        }

        public Xmm LocalVector(bool Zero = true)
        {
            while (true)
            {
                LocalCurrent += LocalSize;

                if (!ReservedGPLocals.Contains(LocalCurrent))
                {
                    break;
                }
            }

            ReservedXMMLocals.Add(LocalCurrent);

            Xmm Out = Xmm.Create(LocalCurrent);

            if (Zero)
                EmitX86(X86Instruction.Vxorps, Out, Out, Out);

            return Out;
        }

        public IntReg EmitOperationSSA(Instruction instruction, params IOperand[] Arguments)
        {
            IntReg Out = Local();

            OperandType Size = Out.Size;

            for (int i = 0; i < Arguments.Length; ++i)
            {
                (Arguments[i] as IOperandSize).Size = Size;
            }

            ir.Emit(InstructionType.Normal, (int)instruction, new IOperand[] { Out }, Arguments);

            return Out;
        }

        IOperand EmitSignExtend(IOperand Source, int SourceSize)
        {
            IntReg Out = Local();

            if (Source is ConstOperand c)
            {
                Source = ConstOperand.Create(c.Data, (OperandType)SourceSize);
            }
            else if (Source is IntReg i)
            {
                Source = IntReg.Create((OperandType)SourceSize, i.Reg);
            }

            ir.Emit(InstructionType.Normal, (int)Instruction.SignExtend, new IOperand[] { Out }, new IOperand[] { Source });

            return Out;
        }

        public IOperand Add(IOperand Source0, IOperand Source1) => EmitOperationSSA(Instruction.Add, Source0, Source1);
        public IOperand Copy(IOperand Source0) => EmitOperationSSA(Instruction.Copy, Source0);
        public IOperand Call(params IOperand[] Arguments) => EmitOperationSSA(Instruction.Call, Arguments);
        public IOperand CompareEqual(IOperand Source0, IOperand Source1) => EmitOperationSSA(Instruction.CompareEqual, Source0, Source1);
        public IOperand CompareGreater(IOperand Source0, IOperand Source1) => EmitOperationSSA(Instruction.CompareGreater, Source0, Source1);
        public IOperand CompareGreaterOrEqual(IOperand Source0, IOperand Source1) => EmitOperationSSA(Instruction.CompareGreaterOrEqual, Source0, Source1);
        public IOperand CompareLess(IOperand Source0, IOperand Source1) => EmitOperationSSA(Instruction.CompareLess, Source0, Source1);
        public IOperand CompareLessSigned(IOperand Source0, IOperand Source1) => EmitOperationSSA(Instruction.CompareLessSigned, Source0, Source1);
        public IOperand ConditionalSelect(IOperand Condition, IOperand Yes, IOperand No) => EmitOperationSSA(Instruction.ConditionalSelect, Condition, Yes, No);
        public IOperand Divide(IOperand Source0, IOperand Source1) => EmitOperationSSA(Instruction.Divide, Source0, Source1);
        public IOperand DivideSigned(IOperand Source0, IOperand Source1) => EmitOperationSSA(Instruction.DivideSigned, Source0, Source1);
        public IOperand LogicalAnd(IOperand Source0, IOperand Source1) => EmitOperationSSA(Instruction.LogicalAnd, Source0, Source1);
        public IOperand LogicalDoubleShiftRight(IOperand Source0, IOperand Source1, IOperand Shift) => EmitOperationSSA(Instruction.LogicalDoubleShiftRight, Source0, Source1, Shift);
        public IOperand LogicalExclusiveOr(IOperand Source0, IOperand Source1) => EmitOperationSSA(Instruction.LogicalExclusiveOr, Source0, Source1);
        public IOperand LogicalOr(IOperand Source0, IOperand Source1)
        {
            if ((Source0 is ConstOperand co0) && (co0.Data == 0))
                return Source1;

            if ((Source1 is ConstOperand co1) && (co1.Data == 0))
                return Source0;

            return EmitOperationSSA(Instruction.LogicalOr, Source0, Source1);
        }
        public IOperand LogicalRotateRight(IOperand Source0, IOperand Source1) => EmitOperationSSA(Instruction.LogicalRotateRight, Source0, Source1);
        public IOperand LogicalShiftLeft(IOperand Source0, IOperand Source1) => EmitOperationSSA(Instruction.LogicalShiftLeft, Source0, Source1);
        public IOperand LogicalShiftRight(IOperand Source0, IOperand Source1) => EmitOperationSSA(Instruction.LogicalShiftRight, Source0, Source1);
        public IOperand LogicalShiftRightSigned(IOperand Source0, IOperand Source1) => EmitOperationSSA(Instruction.LogicalShiftRightSigned, Source0, Source1);
        public IOperand Multiply(IOperand Source0, IOperand Source1) => EmitOperationSSA(Instruction.Multiply, Source0, Source1);
        public IOperand MultiplyHi(IOperand Source0, IOperand Source1) => EmitOperationSSA(Instruction.MultiplyHi, Source0, Source1);
        public IOperand MultiplyHiSigned(IOperand Source0, IOperand Source1) => EmitOperationSSA(Instruction.MultiplyHiSigned, Source0, Source1);
        public IOperand Not(IOperand Source0) => EmitOperationSSA(Instruction.Not, Source0);
        public IOperand SignExtend16(IOperand Source0) => EmitSignExtend(Source0, 1);
        public IOperand SignExtend32(IOperand Source0) => EmitSignExtend(Source0, 2);
        public IOperand SignExtend8(IOperand Source0) => EmitSignExtend(Source0, 0);
        public IOperand Subtract(IOperand Source0, IOperand Source1) => EmitOperationSSA(Instruction.Subtract, Source0, Source1);

        public void Jump(ConstOperand Label) => JumpIf(Label, ConstOperand.Create(1));
        public void JumpIf(ConstOperand Label, IOperand Condition) => ir.EmitAllSources(Instruction.JumpIf, Label, Condition);
        public void Nop() => ir.EmitAllSources(Instruction.Nop);
        public void Return(IOperand Value) => ir.Emit(InstructionType.Normal, (int)Instruction.Return, new IOperand[0],new IOperand[]{Value});
        public void Store(IOperand Address, IOperand Data) => ir.EmitAllSources(Instruction.Store, Address, Data);

        public ConstOperand CreateLabel() => ConstOperand.Create(0);
        public void MarkLabel(ConstOperand Source) => Source.Data = (ulong)ir.RawOperations.Count;

        public void EmitX86(X86Instruction instruction, IOperand Destination, params IOperand[] Sources) => ir.Emit(InstructionType.X86, (int)instruction, new IOperand[] { Destination}, Sources);
        public void EmitX86AS(X86Instruction instruction, params IOperand[] Sources) => ir.Emit(InstructionType.X86, (int)instruction, new IOperand[0], Sources);

        public void If(IOperand Condition, IfBlock Yes, IfBlock No = null)
        {
            ConstOperand End = CreateLabel();
            ConstOperand ConditionPassed = CreateLabel();

            JumpIf(End, ConditionPassed);

            if (No != null)
            {
                No();
            }

            Jump(End);

            MarkLabel(ConditionPassed);

            Yes();

            MarkLabel(End);
        }
    }
}
