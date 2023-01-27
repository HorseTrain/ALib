using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AlibCompiler.Tools.Memory
{
    public unsafe class WindowsHeapManager : IDisposable, IHostMemoryManager
    {
        object Lock = new object();
        
        public bool FastMode                { get; private set; }
        ulong FastTop                       { get; set; }

        public ulong Base                   { get; set; }
        public ulong Size                   { get; set; }
        List<Allocation> Allocations        { get; set; }

        public WindowsHeapManager(ulong AllocationSize, bool FastMode = false)
        {
            Size = AllocationSize;

            Base = (ulong)WindowsAlloc.VirtualAlloc(null, AllocationSize, 0x1000, 4);

            int dummy;
            WindowsAlloc.VirtualProtect((void*)Base, AllocationSize, 0x40, &dummy);

            Allocations = new List<Allocation>();

            this.FastMode = FastMode;
        }

        Allocation CreateAllocation(ulong Place, ulong Size, int Index = -1)
        {
            if (Place + Size > this.Size)
            {
                throw new OutOfMemoryException("Unable To Allocate Memory");
            }

            Allocation Out = new Allocation(this, Place, Size);

            if (Index == -1)
            {
                Allocations.Add(Out);
            }
            else
            {
                Allocations.Insert(Index, Out);
            }

            return Out;
        }

        public Allocation Allocate(ulong Size)
        {           
            lock (Lock)
            {
                if (FastMode)
                {
                    Allocation Out = CreateAllocation(FastTop, Size);

                    FastTop += Size;

                    return Out;
                }

                if (Size == 0)
                    throw new Exception();

                if (Allocations.Count == 0)
                {
                    return CreateAllocation(0, Size);
                }

                List<(ulong, ulong)> AllocationRanges = new List<(ulong, ulong)>();

                ulong Last = 0;

                foreach (Allocation allocation in Allocations)
                {
                    ulong Start = allocation.VirtualAddress;
                    ulong End = Start + allocation.Size;

                    if (Start >= Last)
                    {
                        Last = Start;
                    }
                    else
                    {
                        throw new Exception();
                    }

                    AllocationRanges.Add((Start, End));
                }

                for (int i = 0; i < AllocationRanges.Count; ++i)
                {
                    (ulong s0, ulong e0) = AllocationRanges[i];

                    if (i + 1 < AllocationRanges.Count)
                    {
                        (ulong s1, ulong e1) = AllocationRanges[i + 1];

                        if (e0 + Size < s1)
                        {
                            return CreateAllocation(e0 + 1, Size, i + 1);
                        }
                    }
                    else
                    {
                        return CreateAllocation(e0 + 1, Size);
                    }
                }

                throw new Exception();
            }
        }

        public void Free(Allocation allocation)
        {
            if (FastMode)
                throw new Exception();

            lock (Lock)
            {
                Allocations.Remove(allocation);
            }
        }

        public void Dispose()
        {
            lock (Lock)
            {
                WindowsAlloc.VirtualFree((void*)Base, Size, 0x1000);
            }
        }
    }
}
