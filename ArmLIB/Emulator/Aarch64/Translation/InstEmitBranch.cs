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
        public static void B_Uncond(ArmEmitContext ctx) => B_BL_Uncond(ctx, false);
        public static void BL_Uncond(ArmEmitContext ctx) => B_BL_Uncond(ctx, true);
        public static void Blr(ArmEmitContext ctx) => BranchRegister(ctx, true);
        public static void Br(ArmEmitContext ctx) => BranchRegister(ctx, false);
        public static void Cbnz(ArmEmitContext ctx) => CompareAndBranch(ctx, false);
        public static void Cbz(ArmEmitContext ctx) => CompareAndBranch(ctx, true);
        public static void Tbnz(ArmEmitContext ctx) => TestBitBranch(ctx, false);
        public static void Tbz(ArmEmitContext ctx) => TestBitBranch(ctx, true);
        public static void Ret(ArmEmitContext ctx) => BranchRegister(ctx, false);

        public static void B_Cond(ArmEmitContext ctx)
        {
            OpCodeConditionalBranchImm opCode = ctx.CurrentInstruction as OpCodeConditionalBranchImm;

            IOperand Holds = ConditionHolds(ctx, opCode.cond);

            EmitBranchWithConditionImm(ctx, Holds, opCode.Imm, opCode.NextAddress);
        }

        static void TestBitBranch(ArmEmitContext ctx, bool IsZero)
        {
            OpCodeTestBitAndBranch opCode = ctx.CurrentInstruction as OpCodeTestBitAndBranch;

            IOperand t = ctx.GetX(opCode.Rt);

            IOperand bit = ctx.LogicalShiftRight(t, Const(opCode.Bit));

            bit = ctx.LogicalAnd(bit, Const(1));

            if (IsZero)
            {
                bit = ctx.CompareEqual(bit, Const(0));
            }

            EmitBranchWithConditionImm(ctx, bit, opCode.Imm, opCode.NextAddress);
        }

        static void CompareAndBranch(ArmEmitContext ctx, bool IsZero)
        {
            OpCodeCompareAndBranchImm opCode = ctx.CurrentInstruction as OpCodeCompareAndBranchImm;

            IOperand t = ctx.GetX(opCode.Rt);

            IOperand YesBranch = ctx.CompareEqual(t, Const(0));

            if (!IsZero)
            {
                YesBranch = InvertBool(ctx, YesBranch);
            }

            EmitBranchWithConditionImm(ctx, YesBranch, opCode.Imm, opCode.NextAddress);
        }

        static void BranchRegister(ArmEmitContext ctx, bool SetLR)
        {
            IOperand NewAddress = GetN(ctx);

            if (SetLR)
            {
                ctx.SetX(30, Const(ctx.CurrentInstruction.Address + 4),false);
            }

            ctx.MarkBranched();

            BranchVariable(ctx,NewAddress);
        }

        static void B_BL_Uncond(ArmEmitContext ctx, bool SetLR)
        {
            OpCodeUnconditionalBranchImm opCode = ctx.CurrentInstruction as OpCodeUnconditionalBranchImm;

            if (SetLR)
            {
                ctx.SetX(30, Const(opCode.Address + 4), false);
            }

            EmitBranchImm(ctx,opCode.Imm);
        }

        static void EmitBranchImm(ArmEmitContext ctx, long NewAddress)
        {
            ctx.CurrentEmitSize = IntSize.Int64;

            ctx.MarkBranched();

            if (ctx.IsAValidBasicBlock(NewAddress) && ctx.AllowInlineBranching)
            {
                ctx.Jump(ctx.GetBlockLabel(NewAddress));
            }
            else
            {
                BranchVariable(ctx,Const(NewAddress));
            }
        }

        static void EmitBranchWithConditionImm(ArmEmitContext ctx, IOperand Condition,long BranchingAddress, long LeadingAddress)
        {
            ctx.CurrentEmitSize = IntSize.Int64;

            ctx.MarkBranched();

            Condition = ctx.ZeroExtend(Condition, ctx.CurrentEmitSize);

            if (ctx.IsAValidBasicBlock(BranchingAddress) && ctx.IsAValidBasicBlock(LeadingAddress) && ctx.AllowInlineBranching)
            {
                ctx.JumpIf(ctx.GetBlockLabel(BranchingAddress), Condition);
                ctx.Jump(ctx.GetBlockLabel(LeadingAddress));
            }
            else
            {
                BranchVariable(ctx, ctx.ConditionalSelect(Condition, Const(BranchingAddress), Const(LeadingAddress)));
            }
        }

        static unsafe void BranchVariable(ArmEmitContext ctx, IOperand Address)
        {
            ctx.CurrentEmitSize = IntSize.Int64;

            if (ctx.process.UseFlt)
            {
                FastLookupTable fastLookup = ctx.process.FastLookupTable; 

                ConstOperand End = ctx.CreateLabel();

                IOperand AddressAboveLowerBound = ctx.CompareGreaterOrEqual(Address, Const(fastLookup.Base));
                IOperand AddressBelowUpperBound = ctx.CompareLess(Address, Const(fastLookup.Base + fastLookup.Size));

                IOperand AddressInBound = ctx.LogicalAnd(AddressAboveLowerBound, AddressBelowUpperBound);

                ctx.JumpIf(End, InvertBool(ctx, AddressInBound));

                IOperand WorkingAddress = ctx.Subtract(Address, Const(fastLookup.Base));

                IOperand JitCacheOffset = Load(ctx, ctx.Add(ctx.FastLookupTable, WorkingAddress), IntSize.Int32);

                ctx.JumpIf(End, ctx.CompareEqual(JitCacheOffset, Const(uint.MaxValue)));

                IOperand JitCacheBase = ctx.JitCacheBase;

                IOperand GuestFunctionAddress = ctx.Add(JitCacheBase, JitCacheOffset);

                ctx.ir.Emit(InstructionType.Normal, (int)Instruction.HardJump, new IOperand[] { }, new IOperand[] { GuestFunctionAddress });

                ctx.MarkLabel(End);
            }

            ctx.ReturnWithValue(Address, ExitReason.Normal);
        }
    }
}
