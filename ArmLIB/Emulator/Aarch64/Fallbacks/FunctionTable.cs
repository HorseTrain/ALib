using ArmLIB.Dissasembler.Aarch64.HighLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64.Fallbacks
{
    public static unsafe class FunctionTable
    {
        static ulong[] Fallbacks            { get; set; }

        public static ulong GlobalFunctionTable
        {
            get
            {
                fixed (ulong* fb = Fallbacks)
                {
                    return (ulong)fb;
                }
            }
        }

        static unsafe FunctionTable()
        {
            FunctionTemp = new HashSet<ulong>();

            Add((delegate*<ulong, ulong, OpCodeSize, int>)&FallbackFloat.FCompare);
            Add((delegate*<long, OpCodeSize, OpCodeSize, ulong>)&FallbackFloat.ConvertToFloatSigned);
            Add((delegate*<ulong, OpCodeSize, OpCodeSize, ulong>)&FallbackFloat.ConvertToFloatUnsigned);
            Add((delegate*<ulong, OpCodeSize, OpCodeSize, RoundingMode, ulong>)&FallbackFloat.ConvertToSignedInt);
            Add((delegate*<ulong, OpCodeSize, OpCodeSize, RoundingMode, ulong>)&FallbackFloat.ConvertToUnsignedInt);

            Fallbacks = FunctionTemp.ToArray();

            GCHandle.Alloc(Fallbacks, GCHandleType.Pinned);
        }

        static HashSet<ulong> FunctionTemp  { get; set; }

        static void Add(void* Function)
        {
            if (FunctionTemp.Contains((ulong)Function))
                throw new Exception();

            FunctionTemp.Add((ulong)Function);
        }

        public static bool IsAFallback(ulong Address) => FunctionTemp.Contains(Address);
        public static bool IsAFallback(void* Address) => IsAFallback((ulong)Address);

        public static int GetFallbackIndex(void* Address)
        {
            for (int i = 0; i < Fallbacks.Length; ++i)
            {
                if (Fallbacks[i] == (ulong)Address)
                    return i;
            }

            return -1;
        }
    }
}
