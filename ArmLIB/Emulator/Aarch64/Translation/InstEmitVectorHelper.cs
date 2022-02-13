using ArmLIB.Dissasembler.Aarch64.HighLevel;
using Compiler.Intermediate;
using Compiler.Intermediate.Extensions.X86;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64.Translation
{
    public static partial class InstEmit64
    {
        public static void X86VectorInsert(ArmEmitContext ctx,Xmm Result,IOperand Source, int Size, int Index)
        {
            if (Size == 3)
                Source = ctx.ZeroExtend(Source, IntSize.Int64);
            else
                Source = ctx.ZeroExtend(Source, IntSize.Int32);

            switch (Size)
            {
                case 0: ctx.EmitX86(X86Instruction.Pinsrb, Result, Source, Const(Index)); break;
                case 1: ctx.EmitX86(X86Instruction.Pinsrw, Result, Source, Const(Index)); break;
                case 2: ctx.EmitX86(X86Instruction.Pinsrd, Result, Source, Const(Index)); break;
                case 3: ctx.EmitX86(X86Instruction.Pinsrq, Result, Source, Const(Index)); break;
                default: throw new Exception();
            }
        }

        public static IOperand X86VectorExtract(ArmEmitContext ctx, Xmm Source, int Size, int Index)
        {
            IOperand Out = ctx.Local();

            if (Size == 3)
                Out = ctx.ZeroExtend(Out, IntSize.Int64);
            else
                Out = ctx.ZeroExtend(Out, IntSize.Int32);

            switch (Size)
            {
                case 0: ctx.EmitX86(X86Instruction.Pextrb, Out, Source, Const(Index)); break;
                case 1: ctx.EmitX86(X86Instruction.Pextrw, Out, Source, Const(Index)); break;
                case 2: ctx.EmitX86(X86Instruction.Pextrd, Out, Source, Const(Index)); break;
                case 3: ctx.EmitX86(X86Instruction.Pextrq, Out, Source, Const(Index)); break;
                default: throw new Exception();
            }

            return Out;
        }

        public static IOperand X86VectorExtract(ArmEmitContext ctx, List<Xmm> Source, int Size, int Index)
        {
            int MaxSize = 1 << (4 - Size);

            int XmmIndex = Index / MaxSize;
            int InnerIndex = Index & (MaxSize - 1);

            return X86VectorExtract(ctx, Source[XmmIndex], Size, InnerIndex);
        }

        public static Xmm X86CreateXmmScalar(ArmEmitContext ctx, IOperand Value, OpCodeSize Size)
        {
            Xmm Out = ctx.LocalVector();

            X86VectorInsert(ctx, Out, Value, (int)Size, 0);

            return Out;
        }

        public static Xmm X86CreateXmmScalar(ArmEmitContext ctx, OpCodeSize Size, double Source)
        {
            if (Size == OpCodeSize.s)
            {
                return X86CreateXmmScalar(ctx, Const(Aarch64DebugAssembler.Convert<uint, float>((float)Source)), Size);
            }

            return X86CreateXmmScalar(ctx, Const(Aarch64DebugAssembler.Convert<ulong, double>(Source)), Size);
        }

        public static int GetElementCount(int Size, bool Half)
        {
            int Out = 1 << (4 - Size);

            if (Half)
                Out >>= 1;

            return Out;
        }
    }
}
