using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Tools
{
    public static class Disam
    {
        public static string GetSource(byte[] Buffer)
        {
            StringBuilder Out = new StringBuilder();

            var dat = new SharpDisasm.Disassembler(Buffer, SharpDisasm.ArchitectureMode.x86_64).Disassemble();

            foreach (var ins in dat)
            {
                Out.AppendLine($"{ins.Offset:x3}: {ins.ToString()}");
            }

            return Out.ToString();
        }
    }
}
