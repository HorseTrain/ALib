using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public interface IOpCodeMemorySignExtend
    {
        public bool SignExtendLoad      { get; set; }
        public bool SignExtend32        { get; set; }
    }
}
