using DCpu.Memory;
using DCpu.State;
using DCpu.Translation;
using ArmLIB.Dissasembler.Aarch64.HighLevel;
using ArmLIB.Dissasembler.Aarch64.LowLevel;
using ArmLIB.Emulator;
using ArmLIB.Emulator.Aarch64;
using AlibCompiler.Tools.Memory;
using Gee.External.Capstone.Arm64;
using KeystoneNET;
using System;
using System.IO;
using AlibCompiler.Backend;
using System.Collections.Generic;

namespace ArmLIB
{
    public unsafe class Testing
    {
        static Random r = new Random();

        static int rand() => r.Next();
        static int rand(int range) => r.Next(range);

        static string Fill(string Source, int size)
        {
            while (Source.Length <= size)
                Source += " ";

            return Source;
        }

        public static void Main(string[] Args)
        {
            Aarch64DebugAssembler assembler = new Aarch64DebugAssembler();

            void GenerateFCVT(string name)
            {
                assembler.Emit($"{name} x{rand(31)}, d{rand(32)}, {rand(64) | 1}");
                assembler.Emit($"{name} x{rand(31)}, s{rand(32)}, {rand(64) | 1}");
                assembler.Emit($"{name} w{rand(31)}, d{rand(32)}, {rand(32) | 1}");
                assembler.Emit($"{name} w{rand(31)}, s{rand(32)}, {rand(32) | 1}");
            }


            void GenerateFCVTnf(string name)
            {
                assembler.Emit($"{name} x{rand(31)}, d{rand(32)}");
                assembler.Emit($"{name} x{rand(31)}, s{rand(32)}");
                assembler.Emit($"{name} w{rand(31)}, d{rand(32)}");
                assembler.Emit($"{name} w{rand(31)}, s{rand(32)}");
            }

            void EmitCvtf(string name)
            {
                assembler.Emit($"{name} d{rand(32)}, x{rand(32)}");
                assembler.Emit($"{name} d{rand(32)}, w{rand(32)}");
                assembler.Emit($"{name} s{rand(32)}, x{rand(32)}");
                assembler.Emit($"{name} s{rand(32)}, w{rand(32)}");
            }

            void EmitScalarIns(string name)
            {
                assembler.Emit($"{name} s{rand(32)}, s{rand(32)}, s{rand(32)}");
                assembler.Emit($"{name} d{rand(32)}, d{rand(32)}, d{rand(32)}");
            }

            void EmitVectorIns(string name, bool As = false)
            {
                if (As)
                {
                    assembler.Emit($"{name} v{rand(32)}.16b, v{rand(32)}.16b, v{rand(32)}.16b");
                    assembler.Emit($"{name} v{rand(32)}.8b, v{rand(32)}.8b, v{rand(32)}.8b");

                    assembler.Emit($"{name} v{rand(32)}.8h, v{rand(32)}.8h, v{rand(32)}.8h");
                    assembler.Emit($"{name} v{rand(32)}.4h, v{rand(32)}.4h, v{rand(32)}.4h");
                }

                assembler.Emit($"{name} v{rand(32)}.2s, v{rand(32)}.2s, v{rand(32)}.2s");
                assembler.Emit($"{name} v{rand(32)}.4s, v{rand(32)}.4s, v{rand(32)}.4s");
                assembler.Emit($"{name} v{rand(32)}.2d, v{rand(32)}.2d, v{rand(32)}.2d");
            }

            void EmitVectorIns1(string name, bool As = false)
            {
                if (As)
                {
                    assembler.Emit($"{name} v{rand(32)}.16b, v{rand(32)}.16b");
                    assembler.Emit($"{name} v{rand(32)}.8b, v{rand(32)}.8b");

                    assembler.Emit($"{name} v{rand(32)}.8h, v{rand(32)}.8h");
                    assembler.Emit($"{name} v{rand(32)}.4h, v{rand(32)}.4h");
                }

                assembler.Emit($"{name} v{rand(32)}.2s, v{rand(32)}.2s");
                assembler.Emit($"{name} v{rand(32)}.4s, v{rand(32)}.4s");
                assembler.Emit($"{name} v{rand(32)}.2d, v{rand(32)}.2d");
            }

            void EmitVectorIns8(string name)
            {
                assembler.Emit($"{name} v{rand(32)}.16b, v{rand(32)}.16b, v{rand(32)}.16b");
                assembler.Emit($"{name} v{rand(32)}.8b, v{rand(32)}.8b, v{rand(32)}.8b");
            }

            void EmitVectorIns8_1src(string name)
            {
                assembler.Emit($"{name} v{rand(32)}.16b, v{rand(32)}.16b");
                assembler.Emit($"{name} v{rand(32)}.8b, v{rand(32)}.8b");
            }

            void EmitFScalar(string name)
            {
                assembler.Emit($"{name} s{rand(32)}, s{rand(32)}, s{rand(32)}");
                assembler.Emit($"{name} d{rand(32)}, d{rand(32)}, d{rand(32)}");
            }

            void EmitFScalarElement(string name)
            {
                assembler.Emit($"{name} s{rand(32)}, s{rand(32)}, v{rand(32)}.s[{rand(4)}]");
                assembler.Emit($"{name} d{rand(32)}, d{rand(32)}, v{rand(32)}.d[{rand(2)}]");
            }

            int size = 100;

            if (true)
            {
                for (int i = 0; i < size; ++i)
                {
                    //

                    /*
                    LowLevelInstructionTable.EmitWith(assembler, LowLevelClassNames.Add_subtract_immediate);
                    LowLevelInstructionTable.EmitWith(assembler, LowLevelClassNames.Conditional_select);
                    LowLevelInstructionTable.EmitWith(assembler, LowLevelClassNames.Add_subtract_shifted_register);
                    LowLevelInstructionTable.EmitWith(assembler, LowLevelClassNames.Conditional_select);
                    LowLevelInstructionTable.EmitWith(assembler, LowLevelClassNames.Conditional_compare_register);
                    LowLevelInstructionTable.EmitWith(assembler, LowLevelClassNames.Conditional_select);
                    */

                    assembler.Emit($"mov x0, {(rand(200) * 16) & (ushort.MaxValue)}");
                    assembler.Emit($"ldr q{rand(32)}, [x0]");

                    assembler.Emit($"mov x0, {(rand(200) * 16) & (ushort.MaxValue)}");
                    assembler.Emit($"str q{rand(32)}, [x0]");

                    int b = rand(16);

                    /*
                    assembler.Emit("tbl v" + rand(32) + ".8b, {v"+b+".16b}, v"+rand(32)+".8b");
                    assembler.Emit("tbl v" + rand(32) + ".8b, {v"+b+ ".16b, v" + (b+ 1)+ ".16b}, v" + rand(32) + ".8b");
                    assembler.Emit("tbl v" + rand(32) + ".8b, {v" + b + ".16b, v" + (b + 1) + ".16b, v" + (b + 2) + ".16b }, v" + rand(32) + ".8b");

                    assembler.Emit("tbl v" + rand(32) + ".16b, {v" + b + ".16b}, v" + rand(32) + ".16b");
                    assembler.Emit("tbl v" + rand(32) + ".16b, {v" + b + ".16b, v" + (b + 1) + ".16b}, v" + rand(32) + ".16b");
                    assembler.Emit("tbl v" + rand(32) + ".16b, {v" + b + ".16b, v" + (b + 1) + ".16b, v" + (b + 2) + ".16b }, v" + rand(32) + ".16b");
                    */

                }
            }
            else
            {
                //assembler.Emit($"mov x0, {(rand() * 16) & (ushort.MaxValue)}");
                assembler.Emit("madd x3, x2, x1, x0");
            }

            assembler.Emit("br x0");

            assembler.emit_22(0, 1);

            for (int i = 0; i < 100000; ++i)
            {
                assembler.AddIns((uint)Aarch64DebugAssembler.Convert<uint, float>(3.01f));
            }

            int[] AlibMemory = assembler.Instructions.ToArray();
            int[] RyuMemory = assembler.Instructions.ToArray();

            ExecutionContext rExecution = new ExecutionContext();
            ArmContext aExecution = new ArmContext();

            rExecution.GetPstateFlag(PState.NFlag);

            for (int i = 1; i < 32; ++i)
            {
                ulong r = (ulong)rand() | ((ulong)rand() << 32);

                rExecution.SetX(i, r);
                aExecution.X[i] = r;
            }

            fixed (int* ryj = RyuMemory)
            {
                MemoryManager mm = new MemoryManager((IntPtr)ryj);



                mm.Map(0, 0, 1L * 1024 * 1024 * 1024);

                Translator t = new Translator(mm);

                t.ExecuteSingle(rExecution, 0);
            }

            Console.WriteLine("TEST");

            fixed (int* amem = AlibMemory)
            {
                BasePlusVA bpa = new BasePlusVA(amem);

                ArmProcess p = new ArmProcess(bpa, new WindowsHeapManager(100UL * 1024UL * 1024UL));

                p.TestDisam = true;

                p.GetOrTranslateFunction(0, false, null, true).Execute(&aExecution);
            }

            for (int i = 0; i < 32; ++i)
            {
                string a = $"{aExecution.X[i]:x16}";
                string r = $"{rExecution.GetX(i):x16}";

                Console.WriteLine($"{i:d3}: {a} {r}: {r == a}");
            }

            for (int i = 0; i < 32; ++i)
            {
                string a = $"{aExecution.Q[i * 2]:x16} {aExecution.Q[(i * 2) + 1]:x16}";
                string r = $"{rExecution.GetV(i).GetInt64(0):x16} {rExecution.GetV(i).GetInt64(1):x16}";

                Console.WriteLine($"{i:d3}: {a} {r}: {r == a}");
            }

            for (int i = 0; i < AlibMemory.Length; ++i)
            {
                if (AlibMemory[i] != RyuMemory[i])
                {
                    throw new Exception();
                }
            }
        }

        public static void Test(string Source0, string Source1)
        {
            StreamReader s0 = new StreamReader(Source0);
            StreamReader s1 = new StreamReader(Source1);

            int line = 0;

            while (true)
            {
                line++;

                string l0 = s0.ReadLine();
                string l1 = s1.ReadLine();

                if (l0 != l1)
                {
                    Console.WriteLine(line);

                    throw new Exception();
                }
            }
        }
    }
}
