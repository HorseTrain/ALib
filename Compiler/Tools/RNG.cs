using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlibCompiler.Tools
{
    public static class RNG
    {
        static Random R = new Random(0);

        public static ulong Rand => (ulong)R.Next() | ((ulong)R.Next() << 32);

        public static ulong RandR(int Top) => Rand % (ulong)Top;
        public static ulong RandR(int Start,int Top) => (ulong)Start + (Rand % (ulong)(Top - Start));
    }
}
