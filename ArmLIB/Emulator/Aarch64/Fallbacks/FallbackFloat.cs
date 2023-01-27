using ArmLIB.Dissasembler.Aarch64.HighLevel;
using AlibCompiler.Intermediate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64.Fallbacks
{
    public enum RoundingMode
    {
        TowardZero,
        AwayFromZero,
        TowardPosInf,
        TowardNegInf
    }

    public static class FallbackFloat
    {
        static bool IsSingle(OpCodeSize size) => size == OpCodeSize.w || size == OpCodeSize.s;

        public static double Round(double src, RoundingMode mode)
        {
            switch (mode)
            {
                case RoundingMode.TowardPosInf: return Math.Ceiling(src);
                case RoundingMode.TowardNegInf: return Math.Floor(src);
                case RoundingMode.TowardZero: return Math.Round(src, MidpointRounding.ToZero);
                case RoundingMode.AwayFromZero: return Math.Round(src, MidpointRounding.AwayFromZero);
                default: throw new NotImplementedException();
            }
        }

        public static ulong RoundFB(ulong src, OpCodeSize size,RoundingMode mode)
        {
            double n = GetF(src, size);

            n = Round(n, mode);

            return GetU(n, size);
        }

        static double GetF(ulong src, OpCodeSize size)
        {
            if (IsSingle(size))
            {
                return Aarch64DebugAssembler.Convert<float, uint>((uint)src);
            }

            return Aarch64DebugAssembler.Convert<double, ulong>(src);
        }

        static ulong GetU(double src, OpCodeSize size)
        {
            if (IsSingle(size))
            {
                return Aarch64DebugAssembler.Convert<uint, float>((float)src);
            }

            return Aarch64DebugAssembler.Convert<ulong, double>(src);
        }

        public static ulong ConvertToFloatUnsigned(ulong iSource, OpCodeSize DestinationSize, OpCodeSize SourceSize)
        {
            if (IsSingle(SourceSize))
                iSource = (uint)iSource;

            return GetU(iSource, DestinationSize);
        }

        public static ulong ConvertToFloatSigned(long iSource, OpCodeSize DestinationSize, OpCodeSize SourceSize)
        {
            if (IsSingle(SourceSize))
                iSource = (int)iSource;

            return GetU(iSource, DestinationSize);
        }

        public static int FCompare(ulong N, ulong M, OpCodeSize Size)
        {
            double n = GetF(N, Size);
            double m = GetF(M, Size);

            if (double.IsNaN(n) || double.IsNaN(m))
            {
                return 0b0011;
            }
            else
            {
                if (n == m)
                {
                    return 0b0110;
                }
                else if (n < m)
                {
                    return 0b1000;
                }
                else
                {
                    return 0b0010;
                }
            }
        }

        public static ulong ConvertToSignedInt(ulong N, OpCodeSize to, OpCodeSize from, RoundingMode mode)
        {
            double n = GetF(N, from);

            if (double.IsNaN(n))
                return 0;

            n = Round(n, mode);

            long max = IsSingle(to) ? int.MaxValue : long.MaxValue;
            long min = IsSingle(to) ? int.MinValue : long.MinValue;

            long Out;

            if (n >= max)
            {
                Out = max;
            }
            else if (n <= min)
            {
                Out = min;
            }
            else
            {
                Out = (long)n;
            }

            if (IsSingle(to))
                return (uint)(int)Out;

            return (ulong)Out;
        }

        public static ulong ConvertToUnsignedInt(ulong _n, OpCodeSize to, OpCodeSize from, RoundingMode mode)
        {
            double n = GetF(_n, from);

            if (double.IsNaN(n))
                return 0;

            n = Round(n, mode);

            ulong max = IsSingle(to) ? uint.MaxValue : ulong.MaxValue;
            ulong min = 0;

            ulong Out;

            if (n >= max)
            {
                Out = max;
            }
            else if (n <= min)
            {
                Out = min;
            }
            else
            {
                Out = (ulong)n;
            }

            if (IsSingle(to))
                return (uint)Out;

            return Out;
        }
    }
}

