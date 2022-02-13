using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64
{
    public unsafe class FastLookupTable : IDisposable
    {
        public ulong Base                   { get; set; }
        public ulong Size                   { get; set; }

        public uint[] Buffer                { get; set; }
        GCHandle FLTHandle                  { get; set; }

        public uint* BufferPTR
        {
            get
            {
                fixed (uint* temp = Buffer)
                    return temp;
            }
        }

        public FastLookupTable(ulong Base, ulong Size)
        {
            this.Base = Base;
            this.Size = Size;

            Buffer = new uint[Size >> 2];

            for (int i= 0; i < Buffer.Length; ++i)
            {
                Buffer[i] = uint.MaxValue;
            }

            FLTHandle = GCHandle.Alloc(Buffer, GCHandleType.Pinned);
        }

        public void Dispose()
        {
            FLTHandle.Free();
        }

        public bool InRange(ulong Address)
        {
            return (Address >= Base) && (Address <= Base + Size);
        }

        public void SubmitFunction(ulong Address, uint Offset)
        {
            if (!InRange(Address))
                return;

            Process.ValidateInstructionAddress(Address);

            Address -= Base;

            Address >>= 2;

            Buffer[Address] = Offset;
        }

        public uint RequestAddress(ulong Address)
        {
            if (!InRange(Address))
                return uint.MaxValue;

            Process.ValidateInstructionAddress(Address);

            Address -= Base;

            Address >>= 2;

            return Buffer[Address];
        }
    }
}
