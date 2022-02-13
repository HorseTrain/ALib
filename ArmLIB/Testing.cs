using ArmLIB.Dissasembler.Aarch64.HighLevel;
using ArmLIB.Dissasembler.Aarch64.LowLevel;
using ArmLIB.Emulator;
using ArmLIB.Emulator.Aarch64;
using Compiler.Backend;
using Compiler.Intermediate;
using Compiler.Tools.Memory;
using Gee.External.Capstone.Arm64;
using KeystoneNET;
using System;
using System.Collections.Generic;
using System.IO;

namespace ArmLIB
{
    public class Testing
    {
        public static Keystone ks = new Keystone(KeystoneArchitecture.KS_ARCH_ARM64, KeystoneMode.KS_MODE_LITTLE_ENDIAN);

        public static string GetIns(Arm64Instruction instruction) => instruction.Mnemonic + " " + instruction.Operand;

        public static unsafe void TestDisam(int[] Data, bool Log = false)
        {
            lock (ks)
            {
                foreach (int ins in Data)
                {
                    fixed (byte* dat = ks.Assemble(MiddleMan.GetAOpCode(0, ins).ToString(), 0).Buffer)
                    {
                        if (dat == null)
                        {
                            Console.WriteLine("is       " + MiddleMan.GetAOpCode(0, ins).ToString());
                            Console.WriteLine("should  " + new AOpCode(0, ins, Mnemonic.undefined).ToString());

                            throw new Exception();
                        }

                        int tmp = *(int*)dat;

                        if (ins != tmp)
                        {
                            Console.WriteLine("is " + MiddleMan.GetAOpCode(0, ins).ToString());
                            Console.WriteLine("should " + new AOpCode(0, ins, Mnemonic.undefined).ToString());

                            Console.WriteLine(LowLevelInstructionTable.GetOpHex(ins) + " " + ins);
                            Console.WriteLine(LowLevelInstructionTable.GetOpHex(tmp) + " " + tmp);

                            throw new Exception();
                        }

                        if (Log)
                        {
                            Console.WriteLine(MiddleMan.GetAOpCode(0, ins).ToString());
                        }
                    }
                }
            }
        }
    }
}
