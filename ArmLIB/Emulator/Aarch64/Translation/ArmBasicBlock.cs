using ArmLIB.Dissasembler.Aarch64.HighLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64.Translation
{
    public class ArmBasicBlock 
    {
        public long VirtualAddress          { get; set; }
        public List<AOpCode> Instructions   { get; set; }

        public AOpCode Last                 => Instructions.Last();
        public AOpCode First                => Instructions.First();

        public ArmBasicBlock()
        {
            Instructions = new List<AOpCode>();
        }

        public AOpCode GetOpCode(int Index) => Instructions[Index];

        public void Validate()
        {
#if !ANDROID
            List<int> RawInstructions = new List<int>();

            foreach (AOpCode opCode in Instructions)
            {
                RawInstructions.Add(opCode.RawInstruction);
            }

            Testing.TestDisam(RawInstructions.ToArray());
#endif
        }

        public override string ToString()
        {
            StringBuilder Out = new StringBuilder($"{LoggerTools.GetImm(VirtualAddress)}:\n");

            foreach (AOpCode aOpCode in Instructions)
            {
                Out.AppendLine($"\t{aOpCode}");
            }

            return Out.ToString();
        }
    }
}
