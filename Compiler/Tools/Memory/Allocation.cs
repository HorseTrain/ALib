using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlibCompiler.Tools.Memory
{
    public unsafe class Allocation
    {
        public IHostMemoryManager Manager { get; private set; }
        public ulong VirtualAddress { get; private set; }
        public ulong RealAddress { get; private set; }
        public ulong Size { get; private set; }

        public Allocation(IHostMemoryManager Manager, ulong VirtualAddress, ulong Size)
        {
            this.Manager = Manager;
            this.VirtualAddress = VirtualAddress;
            this.Size = Size;

            RealAddress = Manager.Base + VirtualAddress;
        }

        public void Free() => Manager.Free(this);

        public ref T DataRef<T>(int Index) where T : unmanaged
        {
            return ref ((T*)RealAddress)[Index];
        }

        public void Write(void* Source, ulong Des,ulong Size)
        {
            WindowsAlloc.CopyMemory((void*)(RealAddress + Des), Source, Size);
        }
    }
}
