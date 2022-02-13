using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64.Fallbacks
{
    public static unsafe class FallbackVector
    {
        static byte GetElement(List<Vector128<byte>> Source, int Index)
        {
            int BigIndex = Index >> 4;
            int SmallIndex = Index & 15;

            return Source[BigIndex].GetElement(SmallIndex);
        }

        public static void Tbl(Vector128<byte>* V, int Rd, int Rn, int Len_Rm)
        {
            int Rm = Len_Rm & 0b11111;
            int Len = Len_Rm >> 5;

            int regs = Len + 1;

            Vector128<byte> indicies = V[Rm];
            List<Vector128<byte>> Table = new List<Vector128<byte>>();

            int n = Rn;

            for (int i = 0; i < regs; ++i)
            {
                Table.Add(V[n]);

                n = (n + 1) % 32;
            }

            Vector128<byte> result = new Vector128<byte>();

            for (int i = 0; i < 16; ++i)
            {
                int index = indicies.GetElement(i);

                if (index < 16 * regs)
                {
                    result = result.WithElement(i, GetElement(Table, index));
                }
            }

            V[Rd] = result;
        }
    }
}
