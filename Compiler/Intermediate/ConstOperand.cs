using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Intermediate
{
    public class ConstOperand : IOperand, IOperandSize
    {
        public IntSize Size { get; set; } = IntSize.Undefined;
        public ulong Data   { get; set; }

        public override string ToString() => $"(C: {Data})";

        public ConstOperand(long Source)
        {
            Data = (ulong)Source;
        }

        public ConstOperand(int Source)
        {
            Data = (ulong)(long)Source;
        }

        public ConstOperand(ulong Source)
        {
            Data = Source;
        }

        public ConstOperand(ulong Source, IntSize Size)
        {
            Data = Source;
            this.Size = Size;
        }

        public static ConstOperand Create(long Source) => new ConstOperand(Source);
        public static ConstOperand Create(int Source) => new ConstOperand(Source);
        public static ConstOperand Create(ulong Source) => new ConstOperand(Source);
        public static ConstOperand Create(bool Data) => new ConstOperand(Data ? 1 : 0);
        public static ConstOperand Create(ulong Data, IntSize Size) => new ConstOperand(Data, Size);
    }
}
