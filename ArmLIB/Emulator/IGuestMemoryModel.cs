using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator
{
    public unsafe interface IGuestMemoryModel
    {
        public void* Base   { get; set; }

        public void* TranslateAddress(ulong Address);
    }
}
