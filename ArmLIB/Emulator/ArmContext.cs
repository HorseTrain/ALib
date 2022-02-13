using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator
{
    public unsafe struct ArmContext
    {
        public fixed ulong X[32];
        public fixed ulong Q[32 * 2];

        public ulong n, z, c, v;

        public ulong ExclusiveValue;
        public ulong ExclusiveAddress;

        public fixed ulong LocalBuffer[10000];

        public static int GetOffsetOf(string Name) => ((int)Marshal.OffsetOf<ArmContext>(Name) >> 3);
    }
}
