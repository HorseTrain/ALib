using ArmLIB.Dissasembler.Aarch64.HighLevel;
using AlibCompiler.Intermediate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64.Translation
{
    public static partial class InstEmit64
    {
        public static void Movz(ArmEmitContext ctx)
        {
            OpCodeMoveWide opCode = ctx.CurrentInstruction as OpCodeMoveWide;

            long Imm = opCode.Imm << opCode.Shift;

            SetD(ctx, Const(Imm));
        }

        public static void Movn(ArmEmitContext ctx)
        {
            OpCodeMoveWide opCode = ctx.CurrentInstruction as OpCodeMoveWide;

            long Imm = opCode.Imm << opCode.Shift;

            Imm = ~Imm;

            SetD(ctx, Const(Imm));
        }

        public static void Movk(ArmEmitContext ctx)
        {
            OpCodeMoveWide opCode = ctx.CurrentInstruction as OpCodeMoveWide;

            long Mask = (long)ushort.MaxValue << opCode.Shift;
            long Imm = opCode.Imm << opCode.Shift;

            Mask = ~Mask;

            IOperand d = ctx.GetX(opCode.Rd);

            d = ctx.LogicalAnd(d, Const(Mask));
            d = ctx.LogicalOr(d, Const(Imm));

            SetD(ctx, d);
        }
    }
}
