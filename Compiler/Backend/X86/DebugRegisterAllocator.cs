using Compiler.Intermediate;
using Compiler.Intermediate.Extensions.X86;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Backend.X86
{
    public class DebugRegisterAllocator
    {
        public ControlFlowGraph SourceCFG                           { get; set; }
        public Dictionary<ControlFlowNode, ConstOperand> NewLabels  { get; set; }

        public OperationBlock AllocatedCode             { get; set; }

        public DebugRegisterAllocator(ControlFlowGraph SourceCFG)
        {
            this.SourceCFG = SourceCFG;
        }

        Operation CurrentOperation;

        public void EmitNode(ControlFlowNode Source)
        {
            AllocatedCode.MarkLabel(NewLabels[Source]);

            for (int i = 0; i < Source.Length; ++i)
            {
                FreeGP = 3;
                FreeXmm = 0;

                Operation operation = Source.GetOperation(i);

                this.CurrentOperation = operation;

                if (operation.IsInstruction(Instruction.Return) || operation.IsInstruction(Instruction.HardJump))
                {
                    AllocatedCode.Emit(InstructionType.Normal, (int)operation.Instruction, new IOperand[0], operation.Sources);
                }
                else if (operation.IsInstruction(Instruction.JumpIf))
                {
                    List<IOperand> Sources = AllocateRegisters(operation.Sources, true);

                    AllocatedCode.Emit(InstructionType.Normal, (int)Instruction.JumpIf, new IOperand[0], new IOperand[] {NewLabels[Source.Branching], Sources[1] });
                    AllocatedCode.Emit(InstructionType.Normal, (int)Instruction.JumpIf, new IOperand[0], new IOperand[] { NewLabels[Source.Leading], ConstOperand.Create(1) });
                }
                else if (operation.IsInstruction(Instruction.Call))
                {
                    //Do not modify.
                    AllocatedCode.Emit(InstructionType.Normal, (int)Instruction.Call, operation.Destinations, operation.Sources);
                }
                else
                {
                    Des = new Dictionary<int, int>();
                    XmmDes = new Dictionary<int, int>();

                    List<IOperand> Sources = AllocateRegisters(operation.Sources, true);
                    List<IOperand> Destinations = AllocateRegisters(operation.Destinations, false);

                    AllocatedCode.Emit(operation.Type, operation.Instruction, Destinations.ToArray(), Sources.ToArray());

                    foreach (var des in Des)
                    {
                        EmitAllocateGp(des.Key, des.Value, false);
                    }

                    foreach (var des in XmmDes)
                    {
                        EmitAllocateXmm(des.Key, des.Value, false);
                    }

                }
            }

            //Console.WriteLine(AllocatedCode);
        }

        int FreeGP  { get; set; }
        int FreeXmm { get; set; }

        Dictionary<int, int> Des    { get; set; }
        Dictionary<int, int> XmmDes { get; set; }

        void EmitAllocateGp(int Host, int Guest, bool IsLoad)
        {
            AllocatedCode.Emit(InstructionType.Normal, (int)Instruction.AllocateRegister, new IOperand[] { }, new IOperand[] { IntReg.Create(IntSize.Int64, Host), ConstOperand.Create(Guest), ConstOperand.Create(IsLoad) });
        }

        void EmitAllocateXmm(int Host, int Guest, bool IsLoad)
        {
            AllocatedCode.Emit(InstructionType.Normal, (int)Instruction.AllocateRegister, new IOperand[] { }, new IOperand[] { Xmm.Create(Host), ConstOperand.Create(Guest), ConstOperand.Create(IsLoad) });
        }

        List<IOperand> AllocateRegisters(IOperand[] Operands, bool IsSource)
        {
            List<IOperand> Out = new List<IOperand>();

            foreach (IOperand Source in Operands)
            {
                switch (Source)
                {
                    case ConstOperand op:

                        /*
                        IntReg reg = IntReg.Create(IntSize.Int64, FreeGP);

                        AllocatedCode.Emit(InstructionType.Normal, (int)Instruction.Copy, new IOperand[] { reg }, new IOperand[] { new ConstOperand(op.Data, IntSize.Int64) });

                        Out.Add(IntReg.Create(op.Size, FreeGP));

                        FreeGP++;
                        */

                        Out.Add(op);

                        break;
                    case IntReg op:
                        {
                            bool AllIsNeeded = CurrentOperation.Type == InstructionType.X86;

                            if (IsSource)
                            {
                                EmitAllocateGp(FreeGP, op.Reg, true);
                            }
                            
                            if (!IsSource || AllIsNeeded)
                            {
                                Des.Add(FreeGP, op.Reg);
                            }

                            Out.Add(IntReg.Create(op.Size, FreeGP));

                            FreeGP++;
                        } break;

                    case Xmm op:
                        {
                            X86Instruction instruction = (X86Instruction)CurrentOperation.Instruction;

                            bool AllNeeded = (instruction == X86Instruction.Pinsrb || instruction == X86Instruction.Pinsrw || instruction == X86Instruction.Pinsrd || instruction == X86Instruction.Pinsrq);

                            if (IsSource || AllNeeded)
                            {
                                EmitAllocateXmm(FreeXmm, op.Reg, true);
                            }

                            if (!IsSource || AllNeeded)
                            {
                                XmmDes.Add(FreeXmm, op.Reg);
                            }

                            Out.Add(Xmm.Create(FreeXmm));

                            FreeXmm++;
                        }
                        break;

                    default: throw new Exception();
                }
            }

            return Out;
        }

        public ControlFlowGraph Allocate()
        {
            NewLabels = new Dictionary<ControlFlowNode, ConstOperand>();
            AllocatedCode = new OperationBlock();

            foreach (var node in SourceCFG.Nodes)
            {
                NewLabels.Add(node.Value, AllocatedCode.CreateLabel());
            }

            foreach (var node in SourceCFG.Nodes)
            {
                EmitNode(node.Value);
            }

            return new ControlFlowGraph(AllocatedCode);
        }
    }
}
