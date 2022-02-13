using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Intermediate
{
    public class ControlFlowNode
    {
        public OperationBlock Source        { get; set; }

        public int Start                    { get; set; }
        public int End                      { get; set; }

        public int Length => End - Start;

        public ControlFlowNode Branching    { get; set; }
        public ControlFlowNode Leading      { get; set; }

        public int LeadingCount             { get; set; }

        public ControlFlowNode(OperationBlock Source, int Start, int End)
        {
            this.Source = Source;
            this.Start = Start;
            this.End = End;
        }

        public Operation GetOperation(int Index) => Source.RawOperations[Start + Index];
        public Operation LastOperation => GetOperation(Length - 1);

        public override string ToString()
        {
            StringBuilder Out = new StringBuilder($"{Start}:\n");

            for (int i = 0; i < Length; ++i)
            {
                Out.AppendLine($"\t{GetOperation(i)}");
            }

            return Out.ToString();
        }
    }
}
