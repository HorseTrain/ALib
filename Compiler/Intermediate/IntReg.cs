using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlibCompiler.Intermediate
{
    public class IntReg : IOperandReg, IOperandSize
    {
        public OperandType Size     { get; set; }
        public int Reg          { get; set; }

        IntReg(OperandType Size, int Reg)
        {
            this.Size = Size;
            this.Reg = Reg;

            Debug.Assert(Reg >= 0);
        }

        public static IntReg Create(OperandType Size, int Reg) => new IntReg(Size, Reg);
        public static IntReg Create(OperandType Size, long Reg) => new IntReg(Size, (int)Reg);
        public static IntReg Create(OperandType Size, ulong Reg) => new IntReg(Size, (int)Reg);

        public override string ToString() => $"({Size}: {Reg})";
    }
}
