using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Intermediate.Extensions.X86
{
    public class Xmm : IExtentionOperand
    {
        public int Reg  { get; set; }

        public override string ToString() => $"(Xmm: {Reg})";

        public static Xmm Create(int Reg) => new Xmm() { Reg = Reg };
    }
}
