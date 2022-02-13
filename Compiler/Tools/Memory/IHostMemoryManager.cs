using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Tools.Memory
{
    public interface IHostMemoryManager
    {
        ulong Base { get; set; }
        ulong Size { get; set; }
        Allocation Allocate(ulong Size);
        void Free(Allocation allocation);
    }
}
