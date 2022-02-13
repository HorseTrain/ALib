using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public enum Extend
    {
        UXTB = 0, //sign extend byte
        UXTH = 1, //sign extend short
        UXTW = 2, //sign extend int
        UXTX = 3, //nothing
        SXTB = 4, //zero extend byte
        SXTH = 5, //zero extend short
        SXTW = 6, //zero extend int
        SXTX = 7, //nothing
    }

    public enum IntType
    {
        UInt8 = 0,
        UInt16 = 1,
        UInt32 = 2,
        UInt64 = 3,
        Int8 = 4,
        Int16 = 5,
        Int32 = 6,
        Int64 = 7
    }
}
