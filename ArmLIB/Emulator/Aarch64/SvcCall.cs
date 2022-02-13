using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64
{
    public delegate void SvcCall(ulong Context,ulong Address,int Svc);
}
