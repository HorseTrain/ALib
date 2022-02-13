using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public interface IOpCodeMemWback : IOpCodeImm
    {
        public WbackMode Mode   { get; set; }
    }
}
