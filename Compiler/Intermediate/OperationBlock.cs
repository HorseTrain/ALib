using Compiler.Intermediate.Extensions.X86;
using SharpDisasm;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Intermediate
{
    public class OperationBlock
    {
        public List<Operation> RawOperations { get; set; }

        public OperationBlock()
        {
            RawOperations = new List<Operation>();
        }

        public void Add(Operation operation)
        {
            RawOperations.Add(operation);
        }

        public void Emit(InstructionType Type, int Instruction, IOperand[] Destinations, IOperand[] Sources)
        {
            Add(new Operation(Type, Instruction) { Destinations = Destinations, Sources = Sources });
        }

        public void Emit(Instruction instruction, IOperandReg Destination, params IOperand[] Sources)
        {
            Emit(InstructionType.Normal, (int)instruction, new IOperand[] { Destination }, Sources);
        }

        public void EmitAllSources(Instruction instruction, params IOperand[] Sources)
        {
            Emit(InstructionType.Normal, (int)instruction, new IOperand[] { }, Sources);
        }

        public ConstOperand CreateLabel() => ConstOperand.Create(0);
        public void MarkLabel(ConstOperand Source) => Source.Data = (ulong)RawOperations.Count;

        public void Add(IOperandReg Destination, IOperand Source0, IOperand Source1) => Emit(Instruction.Add, Destination, Source0, Source1);
        public void Call(IOperandReg Destination, params IOperand[] Arguments) => Emit(Instruction.Call, Destination, Arguments);
        public void CompareEqual(IOperandReg Destination, IOperand Source0, IOperand Source1) => Emit(Instruction.CompareEqual, Destination, Source0, Source1);
        public void CompareGreaterOrEqual(IOperandReg Destination, IOperand Source0, IOperand Source1) => Emit(Instruction.CompareGreaterOrEqual, Destination, Source0, Source1);
        public void CompareLessSigned(IOperandReg Destination, IOperand Source0, IOperand Source1) => Emit(Instruction.CompareLessSigned, Destination, Source0, Source1);
        public void CompareLess(IOperandReg Destination, IOperand Source0, IOperand Source1) => Emit(Instruction.CompareLess, Destination, Source0, Source1);
        public void ConditionalSelect(IOperandReg Destination, IOperand Condition, IOperand Yes, IOperand No) => Emit(Instruction.ConditionalSelect, Destination, Condition, Yes, No);
        public void Divide(IOperandReg Destination, IOperand Source0, IOperand Source1) => Emit(Instruction.Divide, Destination, Source0, Source1);
        public void DivideSigned(IOperandReg Destination, IOperand Source0, IOperand Source1) => Emit(Instruction.DivideSigned, Destination, Source0, Source1);
        public void Load(IOperandReg Destination, IOperand Source0) => Emit(Instruction.Load, Destination, Source0);
        public void LogicalAnd(IOperandReg Destination, IOperand Source0, IOperand Source1) => Emit(Instruction.LogicalAnd, Destination, Source0, Source1);
        public void LogicalExclusiveOr(IOperandReg Destination, IOperand Source0, IOperand Source1) => Emit(Instruction.LogicalExclusiveOr, Destination, Source0, Source1);
        public void LogicalOr(IOperandReg Destination, IOperand Source0, IOperand Source1) => Emit(Instruction.LogicalOr, Destination, Source0, Source1);
        public void LogicalRotateRight(IOperandReg Destination, IOperand Source0, IOperand Source1) => Emit(Instruction.LogicalRotateRight, Destination, Source0, Source1);
        public void LogicalShiftRightSigned(IOperandReg Destination, IOperand Source0, IOperand Source1) => Emit(Instruction.LogicalShiftRightSigned, Destination, Source0, Source1);
        public void LogicalShiftLeft(IOperandReg Destination, IOperand Source0, IOperand Source1) => Emit(Instruction.LogicalShiftLeft, Destination, Source0, Source1);
        public void LogicalShiftRight(IOperandReg Destination, IOperand Source0, IOperand Source1) => Emit(Instruction.LogicalShiftRight, Destination, Source0, Source1);
        public void Multiply(IOperandReg Destination, IOperand Source0, IOperand Source1) => Emit(Instruction.Multiply, Destination, Source0, Source1);
        public void MultiplyHi(IOperandReg Destination, IOperand Source0, IOperand Source1) => Emit(Instruction.MultiplyHi, Destination, Source0, Source1);
        public void MultiplyHiSigned(IOperandReg Destination, IOperand Source0, IOperand Source1) => Emit(Instruction.MultiplyHiSigned, Destination, Source0, Source1);
        public void Not(IOperandReg Destination, IOperand Source0) => Emit(Instruction.Not, Destination, Source0);
        public void SignExtend(IOperandReg Destination, IOperand Source0, IOperand Source1) => Emit(Instruction.SignExtend, Destination, Source0, Source1);
        public void Subtract(IOperandReg Destination, IOperand Source0, IOperand Source1) => Emit(Instruction.Subtract, Destination, Source0, Source1);
        public void Copy(IOperandReg Destination, IOperand Source0) => Emit(Instruction.Copy, Destination, Source0);

        public void JumpIf(ConstOperand Label, IOperand Condition) => EmitAllSources(Instruction.Store, Label, Condition);
        public void Nop() => EmitAllSources(Instruction.Nop);
        public void Return() => EmitAllSources(Instruction.Return);
        public void Store(IOperand Address, IOperand Data) => EmitAllSources(Instruction.Store, Address, Data);

        public void EmitX86(X86Instruction instruction, IOperand[] Destinations,IOperand[] Sources) => Emit(InstructionType.X86, (int)instruction, Destinations,Sources);

        public override string ToString()
        {
            StringBuilder Out = new StringBuilder();

            int l = 0;

            foreach (Operation operation in RawOperations)
            {
                Out.AppendLine($"{l++:d3}: " + operation.ToString());
            }

            return Out.ToString();
        }
    }
}
