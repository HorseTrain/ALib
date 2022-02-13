using Compiler.Intermediate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator.Aarch64.Translation
{
    public static partial class InstEmit64
    {
        public static IOperand CompareLessThanZero(ArmEmitContext ctx, IOperand Source)
        {
            return ctx.CompareLessSigned(Source, Const( 0));
        }

        public static (IOperand, IOperand) GetNegZeroFlags(ArmEmitContext ctx, IOperand d)
        {
            IOperand neg = CompareLessThanZero(ctx, d);
            IOperand zero = IsZero(ctx, d);

            return (neg, zero);
        }

        public static (IOperand, IOperand, IOperand, IOperand) GetAddsFlags(ArmEmitContext ctx, IOperand d, IOperand n, IOperand m)
        {
            (IOperand neg, IOperand zero) = GetNegZeroFlags(ctx, d);

            IOperand carry = ctx.CompareLess(d, n);
            IOperand overflow = CompareLessThanZero(ctx, ctx.LogicalAnd(ctx.LogicalExclusiveOr(d, n), ctx.Not(ctx.LogicalExclusiveOr(n, m))));

            return (neg, zero, carry, overflow);
        }

        public static (IOperand, IOperand, IOperand, IOperand) GetSubsFlags(ArmEmitContext ctx, IOperand d, IOperand n, IOperand m)
        {
            (IOperand neg, IOperand zero) = GetNegZeroFlags(ctx, d);

            IOperand carry = ctx.CompareGreaterOrEqual(n,m);
            IOperand overflow = CompareLessThanZero(ctx, ctx.LogicalAnd(ctx.LogicalExclusiveOr(d, n), ctx.LogicalExclusiveOr(n,m)));

            return (neg, zero, carry, overflow);
        }

        public static void SetNZCV(ArmEmitContext ctx, IOperand n, IOperand z, IOperand c, IOperand v)
        {
            ctx.SetRegRaw("n", n);
            ctx.SetRegRaw("z", z);
            ctx.SetRegRaw("c", c);
            ctx.SetRegRaw("v", v);
        }

        public static void SetNZCV(ArmEmitContext ctx, IOperand nzcv)
        {
            SetNZCV(ctx,
                ctx.LogicalAnd(ctx.LogicalShiftRight(nzcv, Const(3)), Const(1)),
                ctx.LogicalAnd(ctx.LogicalShiftRight(nzcv, Const(2)), Const(1)),
                ctx.LogicalAnd(ctx.LogicalShiftRight(nzcv, Const(1)), Const(1)),
                ctx.LogicalAnd(ctx.LogicalShiftRight(nzcv, Const(0)), Const(1))
                );
        }

        public static (IOperand, IOperand, IOperand, IOperand) GetNZCV(ArmEmitContext ctx) => 
            (
            ctx.GetRegRaw("n"), 
            ctx.GetRegRaw("z"), 
            ctx.GetRegRaw("c"), 
            ctx.GetRegRaw("v"));
    }
}
