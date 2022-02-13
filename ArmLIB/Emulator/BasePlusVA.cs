using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator
{
    public unsafe class BasePlusVA : IGuestMemoryModel
    {
        public void* Base           { get; set; }

        public void* TranslateAddress(ulong Address)
        {
            return (byte*)Base + Address;
        }

        public BasePlusVA(void* Base)
        {
            this.Base = Base;
        }
    }
}
