using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public static class LoggerTools
    {
        public static string GetRegister(OpCodeSize Size, int Reg, bool IsSP = false, bool IsVector = false)
        {
            if (IsSP && Reg == 31 && (Size == OpCodeSize.x || Size == OpCodeSize.w))
            {
                if (Size == OpCodeSize.x)
                    return "sp";

                return "wsp";
            }

            if (Reg == 31 && (Size == OpCodeSize.x || Size == OpCodeSize.w))
            {
                if (Size == OpCodeSize.x)
                    return "xzr";

                return "wzr";
            }

            return $"{Size}{Reg}";
        }

        public static string GetImm(long imm) => $"#0x{imm:x}";
        public static string GetImmF(double imm) => $"#{imm:f16}";

        public static int GetIterations(bool Half, OpCodeSize Size)
        {
            int hShift = Half ? 1 : 0;

            switch (Size)
            {
                case OpCodeSize.b: return 16 >> hShift;
                case OpCodeSize.h: return 8 >> hShift;
                case OpCodeSize.s: return 4 >> hShift;
                case OpCodeSize.d: return 2 >> hShift;
                default: throw new Exception();
            }
        }

        public static string GetIteratedVector(int Vec, bool Half, OpCodeSize Size)
        {
            if (Size == OpCodeSize.d && Half)
                return $"d{Vec}";

            return $"v{Vec}.{GetIterations(Half, Size)}{Size}";
        }
    }
}
