using Compiler.Intermediate;
using Compiler.Intermediate.Extensions.X86;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmLIB.Emulator.CodeGenerators
{
    public class SsaCodeOptimizer
    {
        SsaCodeOptimizer()
        {
            //private 
        }

        static bool IsLocal(IOperand operand, int LocalStart)
        {
            return (operand is IOperandReg ir) && ir.Reg >= LocalStart;
        }

        static int GetReg(IOperand operand) => (operand as IntReg).Reg;

        public static OperationBlock ClampLocals(OperationBlock source, int LocalStart, bool Log = false)
        {
            //Simply reorder all of the locals.

            HashSet<int> Gps = new HashSet<int>();
            HashSet<int> Vectors = new HashSet<int>();

            bool IsLocal(int reg) => reg >= LocalStart;

            foreach (Operation operation in source.RawOperations)
            {
                void LoopThroughOperands(IOperand[] operands)
                {
                    foreach (IOperand operand in operands)
                    {
                        if (operand is IntReg ir)
                        {
                            if (IsLocal(ir.Reg))
                                Gps.Add(ir.Reg);
                        }
                        else if (operand is Xmm xmm)
                        {
                            if (IsLocal(xmm.Reg))
                                Vectors.Add(xmm.Reg);
                        }
                    }
                }

                LoopThroughOperands(operation.Destinations);
                LoopThroughOperands(operation.Sources);
            }

            int top = LocalStart;

            Dictionary<int, int> Remap(HashSet<int> Source, int Size)
            {
                Dictionary<int, int> Out = new Dictionary<int, int>();

                foreach (int reg in Source)
                {
                    Out.Add(reg, top);

                    top += Size;
                }

                return Out;
            }

            Dictionary<int, int> GpMap = Remap(Gps, 1);
            Dictionary<int, int> VecMap = Remap(Vectors, 2);

            OperationBlock Out = new OperationBlock();

            IOperand[] GetMappedOperands(IOperand[] Sources)
            {
                List<IOperand> Out = new List<IOperand>();

                foreach (IOperand source in Sources)
                {
                    if (source is IntReg ir)
                    {
                        if (GpMap.ContainsKey(ir.Reg))
                        {
                            Out.Add(IntReg.Create(ir.Size, GpMap[ir.Reg]));
                        }
                        else
                        {
                            Out.Add(IntReg.Create(ir.Size, ir.Reg));
                        }
                    }
                    else if (source is Xmm xmm)
                    {
                        if (VecMap.ContainsKey(xmm.Reg))
                        {
                            Out.Add(Xmm.Create(VecMap[xmm.Reg]));
                        }
                        else
                        {
                            Out.Add(Xmm.Create(xmm.Reg));
                        }
                    }
                    else if (source is ConstOperand)
                    {
                        Out.Add(source);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }

                return Out.ToArray();
            }

            foreach (Operation operation in source.RawOperations)
            {
                Out.Emit(operation.Type, operation.Instruction, GetMappedOperands(operation.Destinations), GetMappedOperands(operation.Sources));
            }

            //if (Log)
            //Console.WriteLine(top);

            return Out;
        }

        public static OperationBlock OptimizeDeadRegisters(OperationBlock ir, int LocalStart)
        {
            //Console.WriteLine(ir);

            ControlFlowGraph cfg = new ControlFlowGraph(ir);

            Dictionary<ControlFlowNode, ConstOperand> NewLabelMap = new Dictionary<ControlFlowNode, ConstOperand>();

            Dictionary<int, HashSet<ControlFlowNode>> VariableUses = new Dictionary<int, HashSet<ControlFlowNode>>();

            //Find all globals.
            HashSet<int> Globals = new HashSet<int>();

            OperationBlock Out = new OperationBlock();

            foreach (var node in cfg.Nodes)
            {
                NewLabelMap.Add(node.Value, Out.CreateLabel());

                ControlFlowNode currentNode = node.Value;

                void CheckLookupForGlobal(IOperand[] Operands)
                {
                    foreach (IOperand operand in Operands)
                    {
                        if (IsLocal(operand, LocalStart))
                        {
                            IOperandReg iReg = operand as IOperandReg;

                            if (!VariableUses.ContainsKey(iReg.Reg))
                                VariableUses.Add(iReg.Reg, new HashSet<ControlFlowNode>());

                            VariableUses[iReg.Reg].Add(currentNode);

                            if (VariableUses[iReg.Reg].Count != 1)
                                Globals.Add(iReg.Reg);
                        }
                    }
                }

                for (int i = 0; i < currentNode.Length; ++i)
                {
                    Operation CurrentInstruction = currentNode.GetOperation(i);

                    CheckLookupForGlobal(CurrentInstruction.Destinations);
                    CheckLookupForGlobal(CurrentInstruction.Sources);
                }
            }

            Dictionary<int, int> GlobalMap = new Dictionary<int, int>();

            int GlobalCurrent = LocalStart;

            foreach (int global in Globals)
            {
                GlobalMap.Add(global, GlobalCurrent);

                GlobalCurrent++;
            }

            bool IsGlobal(IOperand operand)
            {
                return (operand is IOperandReg ir) && Globals.Contains(ir.Reg);
            }

            ControlFlowNode[] SourceNodes = cfg.Nodes.Values.ToArray();

            for (int sn = 0; sn < SourceNodes.Length; ++sn)
            {
                ControlFlowNode currentNode = SourceNodes[sn];

                Dictionary<int, int> VariableBirthMap = new Dictionary<int, int>();
                Dictionary<int, int> VariableDeathMap = new Dictionary<int, int>();

                HashSet<int> AlreadyAdded = new HashSet<int>();

                for (int i = 0; i < currentNode.Length; ++i)
                {
                    Operation CurrentInstruction = currentNode.GetOperation(i);

                    //if (CurrentInstruction.Type != InstructionType.Normal)
                    //    continue;

                    foreach (IOperand destination in CurrentInstruction.Destinations)
                    {
                        if (IsLocal(destination, LocalStart))
                        {
                            int reg = (destination as IOperandReg).Reg;

                            if (Globals.Contains(reg))
                                continue;

                            if (AlreadyAdded.Contains(reg))
                                continue; //a lot of the time, native instructions break ssa. 

                            VariableBirthMap.Add(reg, i);
                            VariableDeathMap.Add(reg, i);

                            AlreadyAdded.Add(reg);
                        }
                    }

                    foreach (IOperand source in CurrentInstruction.Sources)
                    {
                        if (IsLocal(source, LocalStart))
                        {
                            int reg = (source as IOperandReg).Reg;

                            if (Globals.Contains(reg))
                                continue;

                            VariableDeathMap[reg] = i;
                        }
                    }
                }

                List<List<int>> Births = new List<List<int>>();
                List<List<int>> Deaths = new List<List<int>>();

                for (int i = 0; i < currentNode.Length; ++i)
                {
                    Births.Add(new List<int>());
                    Deaths.Add(new List<int>());
                }

                foreach (var birth in VariableBirthMap)
                {
                    Births[birth.Value].Add(birth.Key);
                }

                foreach (var death in VariableDeathMap)
                {
                    Deaths[death.Value].Add(death.Key);
                }

                Dictionary<int, int> SlotMap = new Dictionary<int, int>();

                HashSet<int> TakenSlots = new HashSet<int>();

                int RequestSlot()
                {
                    int Test = GlobalCurrent;

                    while (true)
                    {
                        if (!TakenSlots.Contains(Test))
                            return Test;

                        Test++;
                    }
                }

                void BirthAll(int i)
                {
                    foreach (int birth in Births[i])
                    {
                        int NewSlot = RequestSlot();

                        SlotMap.Add(birth, NewSlot);
                        TakenSlots.Add(NewSlot);
                    }
                }

                void KillAll(int i)
                {
                    foreach (int death in Deaths[i])
                    {
                        int OldSlot = SlotMap[death];

                        SlotMap.Remove(death);
                        TakenSlots.Remove(OldSlot);
                    }
                }

                List<IOperand> GetNewOperands(IOperand[] Source)
                {
                    List<IOperand> Out = new List<IOperand>();

                    foreach (IOperand source in Source)
                    {
                        if (source is IOperandReg)
                        {
                            if (source is IntReg ir)
                            {
                                if (IsGlobal(source))
                                {
                                    int NewReg = GlobalMap[ir.Reg];

                                    Out.Add(IntReg.Create(ir.Size, NewReg));
                                }
                                else if (IsLocal(source, LocalStart))
                                {
                                    int NewReg = SlotMap[ir.Reg];

                                    Out.Add(IntReg.Create(ir.Size, NewReg));
                                }
                                else //context variable.
                                {
                                    Out.Add(IntReg.Create(ir.Size, ir.Reg));
                                }
                            }
                            else if (source is Xmm xmm)
                            {
                                if (IsGlobal(source))
                                {
                                    int NewReg = int.MaxValue - (GlobalMap[xmm.Reg] * 2);

                                    Out.Add(Xmm.Create(NewReg));
                                }
                                else if (IsLocal(source, LocalStart))
                                {
                                    int NewReg = int.MaxValue - (SlotMap[xmm.Reg] * 2);

                                    Out.Add(Xmm.Create(NewReg));
                                }
                                else //context variable.
                                {
                                    Out.Add(Xmm.Create(xmm.Reg));
                                }
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                        else
                        {
                            Out.Add(source);
                        }
                    }

                    return Out;
                }

                Out.MarkLabel(NewLabelMap[currentNode]);

                for (int i = 0; i < currentNode.Length; ++i)
                {
                    Operation CurrentInstruction = currentNode.GetOperation(i);

                    BirthAll(i);

                    if (CurrentInstruction.IsInstruction(Compiler.Intermediate.Instruction.JumpIf))
                    {
                        List<IOperand> Arguments = GetNewOperands(CurrentInstruction.Sources);

                        Out.Emit(CurrentInstruction.Type, CurrentInstruction.Instruction, new IOperand[] {  },new IOperand[] { NewLabelMap[currentNode.Branching], Arguments[1] });

                        if (!((sn + 1 < SourceNodes.Length) && (SourceNodes[sn + 1] == currentNode.Leading)))
                        {
                            Out.Emit(CurrentInstruction.Type, CurrentInstruction.Instruction, new IOperand[] { }, new IOperand[] { NewLabelMap[currentNode.Leading], ConstOperand.Create(1) });
                        }
                    }
                    else
                    {
                        List<IOperand> newSources = GetNewOperands(CurrentInstruction.Sources);
                        List<IOperand> newDestinations = GetNewOperands(CurrentInstruction.Destinations);

                        Out.Emit(CurrentInstruction.Type, CurrentInstruction.Instruction, newDestinations.ToArray(), newSources.ToArray());
                    }

                    KillAll(i);
                }
            }

            Out = ClampLocals(Out, LocalStart, true);

            return Out;
        }
    }
}
