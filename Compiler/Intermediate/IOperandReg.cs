using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Intermediate
{
    public interface IOperandReg : IOperand
    {
        int Reg  { get; set; }
    }
}
