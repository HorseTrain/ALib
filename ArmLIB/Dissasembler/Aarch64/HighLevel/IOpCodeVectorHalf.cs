using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public interface IOpCodeVectorHalf
    {
        public OpCodeSize Size  { get; set; }
        public bool Half        { get; set; }
    }
}
