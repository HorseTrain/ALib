using Compiler.Backend.X86;
using Compiler.Tools.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator
{
    public unsafe class TranslatedFunction
    {
        public IHostMemoryManager HostMemoryManager { get; set; }
        public ulong NativeAddress                  { get; set; }
        public CompiledFunction NativeFunction      { get; set; }

        public TranslatedFunction(IHostMemoryManager HostMemoryManager, ulong NativeAddress, CompiledFunction NativeFunction )
        {
            this.HostMemoryManager = HostMemoryManager;
            this.NativeAddress = NativeAddress;
            this.NativeFunction = NativeFunction;
        }

        public ulong Execute(void* context) => NativeFunction(context);
    }
}
