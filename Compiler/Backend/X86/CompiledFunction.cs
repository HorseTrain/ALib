using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Backend.X86
{
    public unsafe delegate ulong CompiledFunction(void* Context);
}
