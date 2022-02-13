using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public static class DecodingHelpers
    {
        public static OpCodeSize GetIntALUSize(int sf) => sf == 1 ? OpCodeSize.x : OpCodeSize.w;

        public static long SignExtend(long Source, int Size)
        {
            Source <<= 64 - Size;
            Source >>= 64 - Size;

            return Source;
        }

        public static void EnsureSize(OpCodeSize size)
        {
            Debug.Assert(size == OpCodeSize.s || size == OpCodeSize.d);
        }

        public static long GetFloatImm(int imm8, OpCodeSize FloatSize)
        {
            long Imm = 0;

            int a = (imm8 >> 7) & 1;
            int b = (imm8 >> 6) & 1;
            int c = (imm8 >> 5) & 1;
            int d = (imm8 >> 4) & 1;
            int e = (imm8 >> 3) & 1;
            int f = (imm8 >> 2) & 1;
            int g = (imm8 >> 1) & 1;
            int h = (imm8 >> 0) & 1;

            if (FloatSize == OpCodeSize.s)
            {
                Imm |= ((long)h) << 19;
                Imm |= ((long)g) << 20;
                Imm |= ((long)f) << 21;
                Imm |= ((long)e) << 22;
                Imm |= ((long)d) << 23;
                Imm |= ((long)c) << 24;
                Imm |= ((long)b) << 25;
                Imm |= ((long)b) << 26;
                Imm |= ((long)b) << 27;
                Imm |= ((long)b) << 28;
                Imm |= ((long)b) << 29;
                Imm |= (((long)~b) & 1) << 30;
                Imm |= ((long)a) << 31;

                float ff = Aarch64DebugAssembler.Convert<float, uint>((uint)(ulong)Imm);

                Debug.Assert(!float.IsNaN(ff));
            }
            else if (FloatSize == OpCodeSize.d)
            {
                Imm |= ((long)h) << 48;
                Imm |= ((long)g) << 49;
                Imm |= ((long)f) << 50;
                Imm |= ((long)e) << 51;
                Imm |= ((long)d) << 52;
                Imm |= ((long)c) << 53;
                Imm |= ((long)b) << 54;
                Imm |= ((long)b) << 55;
                Imm |= ((long)b) << 56;
                Imm |= ((long)b) << 57;
                Imm |= ((long)b) << 58;
                Imm |= ((long)b) << 59;
                Imm |= ((long)b) << 60;
                Imm |= ((long)b) << 61;
                Imm |= (((long)~b) & 1) << 62;
                Imm |= ((long)a) << 63;
            }
            else
            {
                throw new NotImplementedException();
            }

            return Imm;
        }

        public static long ExpandImm8(int imm8)
        {
            long Out = 0;

            for (int i = 0; i < 8; ++i)
            {
                if (((imm8 >> i) & 1) == 1)
                {
                    Out |= 255L << (i * 8);
                }
            }

            return Out;
        }

        public static void GetElement(ref int Element, ref OpCodeSize Size, LowLevelAOpCode lowLevelAOpCode, SIMDInstructionMode Mode)
        {
            switch (Mode)
            {
                case SIMDInstructionMode.Float:
                    {
                        int sz = ((lowLevelAOpCode.RawInstruction >> 22) & 1);

                        Size = (OpCodeSize)(sz + 2);

                        switch ((sz << 1) | lowLevelAOpCode.L)
                        {
                            //0x
                            case 0b00:
                            case 0b01:
                                {
                                    Element = (lowLevelAOpCode.H << 1) | lowLevelAOpCode.L;
                                }
                                break;

                            case 0b10:
                                {
                                    Element = lowLevelAOpCode.H;
                                }
                                break;
                            default: throw new Exception();
                        }

                        EnsureSize(Size);
                    }
                    break;
                default: throw new Exception();
            }
        }

        public static (int,int, bool) GetLoadMultipleElement(LowLevelAOpCode llOpCode)
        {
            int init_scale = llOpCode.opcode >> 1;

            int scale = init_scale;
            int index = 0;
            bool replicate = false;

            switch (scale)
            {
                case 0:

                    index = (llOpCode.Q << 3) | (llOpCode.S << 2) | llOpCode.size;

                    break;

                case 1:

                    index = (llOpCode.Q << 2) | (llOpCode.S << 1) | ((llOpCode.size >> 1) & 1);

                    break;

                case 2:

                    if ((llOpCode.size & 1) == 0)
                    {
                        index = (llOpCode.Q << 1) | llOpCode.S;
                    }
                    else
                    {
                        index = llOpCode.Q;
                        scale = 3;
                    }

                    break;

                case 3:

                    scale = llOpCode.size;
                    replicate = true;
                    
                    break;
            }

            return (scale,index, replicate);
        }

        public static int LowestBitSet(int imm)
        {
            for (int i = 0; i < 32; ++i)
            {
                if (((imm >> i) & 1) == 1)
                    return i;
            }

            throw new Exception();
        }

        public static int GetBits(int data, int top, int bottom)
        {
            int size = top - bottom;

            return (data >> bottom) & ((1 << size) - 1);
        }
    }

    static class BitUtils
    {
        private static readonly sbyte[] HbsNibbleLut;

        static BitUtils()
        {
            HbsNibbleLut = new sbyte[] { -1, 0, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3 };
        }

        public static int CountBits(int value)
        {
            int count = 0;

            while (value != 0)
            {
                value &= ~(value & -value);

                count++;
            }

            return count;
        }

        public static long FillWithOnes(int bits)
        {
            return bits == 64 ? -1L : (1L << bits) - 1;
        }

        static int CountLeadingZeros(ulong Source, int Size)
        {
            int Out = 0;

            for (int i = (int)Size - 1; i >= 0; i--)
            {
                if (((Source >> i) & 1) == 0)
                {
                    Out++;
                }
                else
                {
                    break;
                }
            }

            return Out;
        }

        public static int HighestBitSet(int value)
        {
            return 31 - CountLeadingZeros((uint)value, 32);
        }

        public static int HighestBitSetNibble(int value)
        {
            return HbsNibbleLut[value];
        }

        public static long Replicate(long bits, int size)
        {
            long output = 0;

            for (int bit = 0; bit < 64; bit += size)
            {
                output |= bits << bit;
            }

            return output;
        }

        public static int RotateRight(int bits, int shift, int size)
        {
            return (int)RotateRight((uint)bits, shift, size);
        }

        public static uint RotateRight(uint bits, int shift, int size)
        {
            return (bits >> shift) | (bits << (size - shift));
        }

        public static long RotateRight(long bits, int shift, int size)
        {
            return (long)RotateRight((ulong)bits, shift, size);
        }

        public static ulong RotateRight(ulong bits, int shift, int size)
        {
            return (bits >> shift) | (bits << (size - shift));
        }
    }

    //From Ryujinx
    static class DecoderHelper
    {
        static DecoderHelper()
        {
            Imm8ToFP32Table = BuildImm8ToFP32Table();
            Imm8ToFP64Table = BuildImm8ToFP64Table();
        }

        public static readonly uint[] Imm8ToFP32Table;
        public static readonly ulong[] Imm8ToFP64Table;

        private static uint[] BuildImm8ToFP32Table()
        {
            uint[] tbl = new uint[256];

            for (int idx = 0; idx < 256; idx++)
            {
                tbl[idx] = ExpandImm8ToFP32((uint)idx);
            }

            return tbl;
        }

        private static ulong[] BuildImm8ToFP64Table()
        {
            ulong[] tbl = new ulong[256];

            for (int idx = 0; idx < 256; idx++)
            {
                tbl[idx] = ExpandImm8ToFP64((ulong)idx);
            }

            return tbl;
        }

        // abcdefgh -> aBbbbbbc defgh000 00000000 00000000 (B = ~b)
        private static uint ExpandImm8ToFP32(uint imm)
        {
            uint MoveBit(uint bits, int from, int to)
            {
                return ((bits >> from) & 1U) << to;
            }

            return MoveBit(imm, 7, 31) | MoveBit(~imm, 6, 30) |
                   MoveBit(imm, 6, 29) | MoveBit(imm, 6, 28) |
                   MoveBit(imm, 6, 27) | MoveBit(imm, 6, 26) |
                   MoveBit(imm, 6, 25) | MoveBit(imm, 5, 24) |
                   MoveBit(imm, 4, 23) | MoveBit(imm, 3, 22) |
                   MoveBit(imm, 2, 21) | MoveBit(imm, 1, 20) |
                   MoveBit(imm, 0, 19);
        }

        // abcdefgh -> aBbbbbbb bbcdefgh 00000000 00000000 00000000 00000000 00000000 00000000 (B = ~b)
        private static ulong ExpandImm8ToFP64(ulong imm)
        {
            ulong MoveBit(ulong bits, int from, int to)
            {
                return ((bits >> from) & 1UL) << to;
            }

            return MoveBit(imm, 7, 63) | MoveBit(~imm, 6, 62) |
                   MoveBit(imm, 6, 61) | MoveBit(imm, 6, 60) |
                   MoveBit(imm, 6, 59) | MoveBit(imm, 6, 58) |
                   MoveBit(imm, 6, 57) | MoveBit(imm, 6, 56) |
                   MoveBit(imm, 6, 55) | MoveBit(imm, 6, 54) |
                   MoveBit(imm, 5, 53) | MoveBit(imm, 4, 52) |
                   MoveBit(imm, 3, 51) | MoveBit(imm, 2, 50) |
                   MoveBit(imm, 1, 49) | MoveBit(imm, 0, 48);
        }

        public struct BitMask
        {
            public long WMask;
            public long TMask;
            public int Pos;
            public int Shift;
            public bool IsUndefined;

            public static BitMask Invalid => new BitMask { IsUndefined = true };
        }

        public static BitMask DecodeBitMask(int opCode, bool immediate)
        {
            int immS = (opCode >> 10) & 0x3f;
            int immR = (opCode >> 16) & 0x3f;

            int n = (opCode >> 22) & 1;
            int sf = (opCode >> 31) & 1;

            int length = BitUtils.HighestBitSet((~immS & 0x3f) | (n << 6));

            if (length < 1 || (sf == 0 && n != 0))
            {
                throw new Exception();

                return BitMask.Invalid;
            }

            int size = 1 << length;

            int levels = size - 1;

            int s = immS & levels;
            int r = immR & levels;

            if (immediate && s == levels)
            {
                throw new Exception();

                return BitMask.Invalid;
            }

            long wMask = BitUtils.FillWithOnes(s + 1);
            long tMask = BitUtils.FillWithOnes(((s - r) & levels) + 1);

            if (r > 0)
            {
                wMask = BitUtils.RotateRight(wMask, r, size);
                wMask &= BitUtils.FillWithOnes(size);
            }

            return new BitMask()
            {
                WMask = BitUtils.Replicate(wMask, size),
                TMask = BitUtils.Replicate(tMask, size),

                Pos = immS,
                Shift = immR
            };
        }

        public static long DecodeImm24_2(int opCode)
        {
            return ((long)opCode << 40) >> 38;
        }

        public static long DecodeImm26_2(int opCode)
        {
            return ((long)opCode << 38) >> 36;
        }

        public static long DecodeImmS19_2(int opCode)
        {
            return (((long)opCode << 40) >> 43) & ~3;
        }

        public static long DecodeImmS14_2(int opCode)
        {
            return (((long)opCode << 45) >> 48) & ~3;
        }

        public static bool VectorArgumentsInvalid(bool q, params int[] args)
        {
            if (q)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if ((args[i] & 1) == 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
