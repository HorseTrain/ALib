using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Intermediate
{
    public class ControlFlowGraph 
    {
        public OperationBlock Operations                { get; private set; }
        public Dictionary<int, ControlFlowNode> Nodes   { get; private set; }
        public List<ControlFlowNode> OrderedNodes       { get; private set; }

        public ControlFlowGraph(OperationBlock Operations)
        {
            this.Operations = Operations;

            Nodes = new Dictionary<int, ControlFlowNode>();
            OrderedNodes = new List<ControlFlowNode>();

            CurrentQue = new List<int>();
            NewQue = new List<int>();
            OrderedNodes = new List<ControlFlowNode>();

            CurrentQue.Add(0);

            while (true)
            {
                foreach (int q in CurrentQue)
                    GetNode(q);

                CurrentQue = new List<int>();
                CurrentQue.AddRange(NewQue);

                NewQue = new List<int>();

                if (CurrentQue.Count == 0)
                    break;
            }

            foreach (var node in Nodes)
            {
                Operation operation = node.Value.GetOperation(node.Value.Length - 1);

                int Next = node.Value.Start + node.Value.Length;

                if (operation.IsInstruction(Instruction.JumpIf))
                {
                    node.Value.Branching = Nodes[(int)(operation.Sources[0] as ConstOperand).Data];
                }

                if (Nodes.ContainsKey(Next))
                node.Value.Leading = Nodes[Next];
            }

            foreach (var node in Nodes)
            {
                if (node.Value.Leading != null)
                    node.Value.Leading.LeadingCount++;

                if (node.Value.Branching != null)
                    node.Value.Branching.LeadingCount++;
            }
        }

        List<int> CurrentQue;
        List<int> NewQue;

        void GetNode(int Location)
        {
            if (Nodes.ContainsKey(Location))
                return;

            int End = Location;

            for (int i = Location; i < Operations.RawOperations.Count; ++i)
            {
                Operation operation = Operations.RawOperations[i];

                if (operation.IsInstruction(Instruction.JumpIf))
                {
                    ConstOperand Label = operation.Sources[0] as ConstOperand;

                    NewQue.Add((int)Label.Data);
                    NewQue.Add(i + 1);

                    End = i + 1;

                    break;
                }

                if (operation.IsInstruction(Instruction.Return) || operation.IsInstruction(Instruction.HardJump))
                {
                    NewQue.Add(i + 1);

                    End = i + 1;

                    break;
                }

                End = i + 1;
            }

            Nodes.Add(Location,new ControlFlowNode(Operations, Location, End ));
        }

        public override string ToString()
        {
            StringBuilder Out = new StringBuilder();

            foreach (var node in Nodes)
            {
                Out.AppendLine(node.Value.ToString());
            }

            return Out.ToString();
        }
    }
}
