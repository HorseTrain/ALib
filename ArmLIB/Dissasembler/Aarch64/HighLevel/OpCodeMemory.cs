using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class OpCodeMemory : AOpCode, IOpCodeRt, IOpCodeRn, IOpCodeRnIsSP, IOpCodeMemorySignExtend
    {
        public int Rn           { get; set; }
        public bool RnIsSP      { get; set; }

        public int Rt           { get; set; }

        public int RawSize      { get; set; }
        public OpCodeSize Size  { get; set; }

        public bool SignExtendLoad  { get; set; }
        public bool SignExtend32    { get; set; }

        public bool IsVector        { get; set; }

        protected OpCodeMemory(LowLevelAOpCode Source, long Address, Mnemonic Name) : base(Address, Source.RawInstruction, Name)
        {
            RnIsSP = true;
            Rn = Source.Rn;

            Rt = Source.Rt;
        }

        public static int GetSizeNum(OpCodeSize Size)
        {
            if (Size == OpCodeSize.x)
                return 3;

            if (Size == OpCodeSize.w)
                return 2;

            return (int)Size;
        }
    }
}
