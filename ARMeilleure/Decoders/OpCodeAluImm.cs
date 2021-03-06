using System;

namespace DCpu.Decoders
{
    class OpCodeAluImm : OpCodeAlu, IOpCodeAluImm
    {
        public long Immediate { get; private set; }

        public OpCodeAluImm(InstDescriptor inst, ulong address, int opCode) : base(inst, address, opCode)
        {
            if (DataOp == DataOp.Arithmetic)
            {
                Immediate = (opCode >> 10) & 0xfff;

                int shift = (opCode >> 22) & 3;

                Immediate <<= shift * 12;
            }
            else if (DataOp == DataOp.Logical)
            {
                var bm = DecoderHelper.DecodeBitMask(opCode, true);

                if (bm.IsUndefined)
                {
                    Instruction = InstDescriptor.Undefined;

                    return;
                }

                Immediate = bm.WMask;
            }
            else
            {
                throw new ArgumentException(nameof(opCode));
            }
        }
    }
}