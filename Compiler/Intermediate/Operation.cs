using AlibCompiler.Intermediate.Extensions.X86;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlibCompiler.Intermediate
{
    public class Operation
    {
        const int TypeBit = 24;

        int _instruction            { get; set; }

        public int Instruction      => _instruction & ~(255 << TypeBit);
        public InstructionType Type => (InstructionType)(_instruction >> TypeBit);

        public Operation(InstructionType Type, int Instruction)
        {
            _instruction = ((int)Type << TypeBit) | Instruction;
        }

        public Operation(Instruction instruction) : this (InstructionType.Normal, (int)instruction)
        {
            Destinations = new IOperand[0];
            Sources = new IOperand[0];
        }

        public Operation(X86Instruction instruction) : this(InstructionType.X86, (int)instruction)
        {
            Destinations = new IOperand[0];
            Sources = new IOperand[0];
        }

        public IOperand[] Destinations  { get; set; }
        public IOperand[] Sources       { get; set; }

        public bool IsInstruction(Instruction Instruction) => Type == InstructionType.Normal && this.Instruction == (int)Instruction;
        public bool IsInstruction(X86Instruction Instruction) => Type == InstructionType.X86 && this.Instruction == (int)Instruction;

        public void Validate()
        {
            foreach (IOperand des in Destinations)
            {
                Debug.Assert(!(des is ConstOperand));
            }
        }

        string InstructionString()
        {
            switch (Type)
            {
                case InstructionType.Normal: return ((Instruction)Instruction).ToString();
                case InstructionType.X86: return ((X86Instruction)Instruction).ToString();
                default: throw new Exception();
            }
        }

        public override string ToString()
        {
            StringBuilder Out = new StringBuilder(InstructionString() + " ");

            foreach (IOperand des in Destinations)
            {
                Out.Append(des.ToString() + " ");
            }

            Out.Append(": ");

            foreach (IOperand source in Sources)
            {
                Out.Append(source.ToString() + " ");
            }

            return Out.ToString();
        }
    }
}
