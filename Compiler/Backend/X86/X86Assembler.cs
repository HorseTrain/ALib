using Compiler.Intermediate;
using Compiler.Intermediate.Extensions.X86;
using Compiler.Tools;
using Compiler.Tools.Memory;
using Iced.Intel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using static Iced.Intel.AssemblerRegisters;

namespace Compiler.Backend.X86
{
    public class X86Assembler
    {
        public static AssemblerRegister64[] _64 = new AssemblerRegister64[]
        {
            rax,
            rcx,
            rdx,
            rbx,
            rbp,
            rsi,
            rdi,
            r8,
            r9,
            r10,
            r11,
            r12,
            r13,
            r14
        };

        static AssemblerRegister32[] _32 = new AssemblerRegister32[]
        {
            eax,
            ecx,
            edx,
            ebx,
            ebp,
            esi,
            edi,
            r8d,
            r9d,
            r10d,
            r11d,
            r12d,
            r13d,
            r14d
        };

        static AssemblerRegister16[] _16 = new AssemblerRegister16[]
        {
            ax,
            cx,
            dx,
            bx,
            bp,
            si,
            di,
            r8w,
            r9w,
            r10w,
            r11w,
            r12w,
            r13w,
            r14w
        };

        static AssemblerRegister8[] _8 = new AssemblerRegister8[]
        {
            al,
            cl,
            dl,
            bl,
            bpl,
            sil,
            dil,
            r8b,
            r9b,
            r10b,
            r11b,
            r12b,
            r13b,
            r14b
        };

        public static AssemblerRegisterXMM[] _xmm = new AssemblerRegisterXMM[]
        {
            xmm0,
            xmm1,
            xmm2,
            xmm3,
            xmm4,
            xmm5,
            xmm6,
            xmm7,
            xmm8,
            xmm9,
            xmm10,
            xmm11,
            xmm12,
            xmm13,
            xmm14,
            xmm15,
        };

        dynamic RAX => GetHostRegister(PreferedSize, IndexOf(rax));
        dynamic RCX => GetHostRegister(PreferedSize, IndexOf(rcx));
        dynamic RDX => GetHostRegister(PreferedSize, IndexOf(rdx));

        public static int IndexOf(AssemblerRegister64 reg) => _64.ToList().IndexOf(reg);

        dynamic GetPointerN(int Index)
        {
            if(Index >= 100000)
            {
                throw new Exception();
            }

            return __[r15 + (Index * 8)];
        }

        public static dynamic GetHostRegister(IntSize Size, int Reg)
        {
            switch (Size)
            {
                case IntSize.Int8: return _8[Reg];
                case IntSize.Int16: return _16[Reg];
                case IntSize.Int32: return _32[Reg];
                case IntSize.Int64: return _64[Reg];
                default: throw new Exception();
            }
        }

        IHostMemoryManager allocator { get; set; }
        ControlFlowGraph cfg { get; set; }
        Assembler c { get; set; }

        OperationBlock Source { get; set; }

        ControlFlowNode CurrentNode { get; set; }
        Operation CurrentOperation { get; set; }
        Dictionary<int, Label> Labels { get; set; }

        void EnsureSameIntRegSize(params IOperand[] Sources)
        {
            return;

            IntSize type = (Sources[0] as IntReg).Size;

            for (int i = 0; i < Sources.Length; ++i)
            {
                if (Sources[i] is IntReg ir && ir.Size != type)
                    throw new Exception();
            }
        }

        ulong GA;

        X86Assembler(ControlFlowGraph cfg, IHostMemoryManager allocator, ulong GuestAddress)
        {
            //Console.WriteLine(cfg.Operations);

            this.Source = cfg.Operations;
            this.GA = GuestAddress;

            this.cfg = new RegisterAllocator(cfg).Allocate();

            this.c = new Assembler(64);
            this.allocator = allocator;
        }

        public byte[] CompileFunction()
        {
            Labels = new Dictionary<int, Label>();

            foreach (ControlFlowNode node in cfg.Nodes.Values)
                Labels.Add(node.Start, c.CreateLabel());

            c.mov(r15, rcx);

            c.push(rdx);
            c.push(rbx);
            c.push(rbp);

            foreach (ControlFlowNode node in cfg.Nodes.Values)
            {
                Label l = Labels[node.Start];

                if (node.Length == 0)
                    continue;

                c.Label(ref l);

                CompileNode(node);
            }

            const ulong RIP = 0x1234_5678_1000_0000;
            var stream = new MemoryStream();
            c.Assemble(new StreamCodeWriter(stream), RIP);

            byte[] Temp = new byte[stream.Length];

            Buffer.BlockCopy(stream.GetBuffer(), 0, Temp, 0, Temp.Length);

            return Temp;
        }

        public static unsafe T CompileOperations<T>(OperationBlock block, IHostMemoryManager allocator, ulong GuestAddress,out ulong FunctionAddress) where T : Delegate
        {
            //Console.WriteLine(block);

            X86Assembler assembler = new X86Assembler(new ControlFlowGraph(block), allocator, GuestAddress);

            byte[] Buffer = assembler.CompileFunction();

            //Console.WriteLine(Disam.GetSource(Buffer));

            //File.WriteAllText($"ir/ir-{GuestAddress:x}.txt", assembler.cfg.Operations.ToString() + "\n" + assembler.Source.ToString() +"\n" + Disam.GetSource(Buffer));

            Allocation allocation = allocator.Allocate((ulong)Buffer.Length);

            FunctionAddress = allocation.RealAddress;

            fixed (byte* source = Buffer)
            {
                allocation.Write(source, 0, (ulong)Buffer.Length);
            }

            return Marshal.GetDelegateForFunctionPointer<T>((IntPtr)allocation.RealAddress);
        }

        void CompileNode(ControlFlowNode Node)
        {
            //c.nop();

            CurrentNode = Node;

            for (int i = 0; i < Node.Length; ++i)
            {
                CurrentOperation = Node.GetOperation(i);

                CompileOperation(CurrentOperation);
            }
        }

        public static bool RAProblemInstruction(Intermediate.Instruction ins)
        {
            switch (ins)
            {
                //ra problem instructions.
                case Intermediate.Instruction.Divide: return true;
                case Intermediate.Instruction.DivideSigned: return true;
                case Intermediate.Instruction.LogicalRotateRight: return true;
                case Intermediate.Instruction.LogicalShiftLeft: return true;
                case Intermediate.Instruction.LogicalShiftRight: return true;
                case Intermediate.Instruction.LogicalShiftRightSigned: return true;
                case Intermediate.Instruction.Multiply: return true;
                case Intermediate.Instruction.MultiplyHi: return true;
                case Intermediate.Instruction.MultiplyHiSigned: return true;
                case Intermediate.Instruction.CompareAndSwap: return true;
                case Intermediate.Instruction.LogicalDoubleShiftRight: return true;
                case Intermediate.Instruction.HardJump: return true;
            }

            return false;
        }
        public static bool RAProblemInstruction(X86Instruction ins)
        {
            return ins == X86Instruction.Pinsrb || ins == X86Instruction.Pinsrw || ins == X86Instruction.Pinsrd || ins == X86Instruction.Pinsrq || ins == X86Instruction.Bswap;
        }

        void CompileOperation(Operation operation)
        {
            switch (operation.Type)
            {
                case InstructionType.Normal:
                    {
                        switch ((Intermediate.Instruction)operation.Instruction)
                        {
                            //ra problem instructions.
                            case Intermediate.Instruction.Divide: EmitDivide(false); break;
                            case Intermediate.Instruction.DivideSigned: EmitDivide(true); break;
                            case Intermediate.Instruction.LogicalRotateRight: EmitShift(Intermediate.Instruction.LogicalRotateRight); break;
                            case Intermediate.Instruction.LogicalShiftLeft: EmitShift(Intermediate.Instruction.LogicalShiftLeft); break;
                            case Intermediate.Instruction.LogicalShiftRight: EmitShift(Intermediate.Instruction.LogicalShiftRight); break;
                            case Intermediate.Instruction.LogicalShiftRightSigned: EmitShift(Intermediate.Instruction.LogicalShiftRightSigned); break;
                            case Intermediate.Instruction.Multiply: EmitMultiply(false, false); break;
                            case Intermediate.Instruction.MultiplyHi: EmitMultiply(true, false); break;
                            case Intermediate.Instruction.MultiplyHiSigned: EmitMultiply(true, true); break;
                            case Intermediate.Instruction.CompareAndSwap: EmitCompareAndSwap(); break;

                            //normal instructions
                            case Intermediate.Instruction.Add: EmitAdd(); break;
                            case Intermediate.Instruction.CompareEqual: EmitCompare(Intermediate.Instruction.CompareEqual); break;
                            case Intermediate.Instruction.CompareGreaterOrEqual: EmitCompare(Intermediate.Instruction.CompareGreaterOrEqual); break;
                            case Intermediate.Instruction.CompareGreater: EmitCompare(Intermediate.Instruction.CompareGreater); break;
                            case Intermediate.Instruction.CompareLess: EmitCompare(Intermediate.Instruction.CompareLess); break;
                            case Intermediate.Instruction.CompareLessSigned: EmitCompare(Intermediate.Instruction.CompareLessSigned); break;
                            case Intermediate.Instruction.LogicalAnd: EmitD_N(Intermediate.Instruction.LogicalAnd); break;
                            case Intermediate.Instruction.LogicalExclusiveOr: EmitD_N(Intermediate.Instruction.LogicalExclusiveOr); break;
                            case Intermediate.Instruction.LogicalOr: EmitD_N(Intermediate.Instruction.LogicalOr); break;
                            case Intermediate.Instruction.Subtract: EmitD_N(Intermediate.Instruction.Subtract); break;
                            case Intermediate.Instruction.Copy: EmitCopy(); break;
                            case Intermediate.Instruction.Not: EmitNot(); break;
                            case Intermediate.Instruction.ConditionalSelect: EmitConditionalSelect(); break;
                            case Intermediate.Instruction.LogicalDoubleShiftRight: EmitDoubleShiftRight(); break;
                            case Intermediate.Instruction.SignExtend: EmitSignExtend(); break;
                            case Intermediate.Instruction.Nop: break;
                            case Intermediate.Instruction.Load: EmitLoad(); break;
                            case Intermediate.Instruction.Store: EmitStore(); break;
                            case Intermediate.Instruction.Call: EmitCall(); break;
                            case Intermediate.Instruction.GetContext: EmitGetContext(); break;

                            //control flow instructions
                            case Intermediate.Instruction.Return: EmitReturn(); break;
                            case Intermediate.Instruction.JumpIf: EmitJumpIf(); break;
                            case Intermediate.Instruction.HardJump: EmitHardJump(); break;

                                //misc
                            case Intermediate.Instruction.AllocateRegister: EmitAllocate(); break;

                            default: throw new Exception();
                        }

                    }; break;

                case InstructionType.X86:
                    {
                        switch ((X86Instruction)operation.Instruction)
                        {
                            case X86Instruction.Bswap: EmitBswap(); break;
                            case X86Instruction.Lzcnt: EmitLzcnt(); break;
                            case X86Instruction.Popcnt: EmitPopCnt(); break;

                            case X86Instruction.Add_GetFlags: EmitAddSubtract_GetFlags(); break;

                            case X86Instruction.Cvtsd2ss: EmitCvtsd2ss(); break;
                            case X86Instruction.Cvtss2sd: EmitCvtss2sd(); break;
                            case X86Instruction.Movaps: EmitMovaps(); break;
                            case X86Instruction.Pextrb: EmitPext(); break;
                            case X86Instruction.Pextrd: EmitPext(); break;
                            case X86Instruction.Pextrq: EmitPext(); break;
                            case X86Instruction.Pextrw: EmitPext(); break;
                            case X86Instruction.Pinsrb: EmitPinsr(); break;
                            case X86Instruction.Pinsrd: EmitPinsr(); break;
                            case X86Instruction.Pinsrq: EmitPinsr(); break;
                            case X86Instruction.Pinsrw: EmitPinsr(); break;
                            case X86Instruction.Sqrtpd: EmitSqrtpd(); break;
                            case X86Instruction.Sqrtps: EmitSqrtps(); break;
                            case X86Instruction.Vaddpd: EmitVector2Src(); break;
                            case X86Instruction.Vaddps: EmitVector2Src(); break;
                            case X86Instruction.Vandps: EmitVector2Src(); break;
                            case X86Instruction.Vdivpd: EmitVector2Src(); break;
                            case X86Instruction.Vdivps: EmitVector2Src(); break;
                            case X86Instruction.Vmaxpd: EmitVector2Src(); break;
                            case X86Instruction.Vmaxps: EmitVector2Src(); break;
                            case X86Instruction.Vminpd: EmitVector2Src(); break;
                            case X86Instruction.Vminps: EmitVector2Src(); break;
                            case X86Instruction.Vmovupd_load: EmitVmovupd(true); break;
                            case X86Instruction.Vmovupd_store: EmitVmovupd(false); break;
                            case X86Instruction.Vmulpd: EmitVector2Src(); break;
                            case X86Instruction.Vmulps: EmitVector2Src(); break;
                            case X86Instruction.Vorps: EmitVector2Src(); break;
                            case X86Instruction.Vpaddb: EmitVector2Src(); break;
                            case X86Instruction.Vpaddd: EmitVector2Src(); break;
                            case X86Instruction.Vpaddq: EmitVector2Src(); break;
                            case X86Instruction.Vpaddw: EmitVector2Src(); break;
                            case X86Instruction.Vpsubb: EmitVector2Src(); break;
                            case X86Instruction.Vpsubd: EmitVector2Src(); break;
                            case X86Instruction.Vpsubq: EmitVector2Src(); break;
                            case X86Instruction.Vpsubw: EmitVector2Src(); break;
                            case X86Instruction.Vsubpd: EmitVector2Src(); break;
                            case X86Instruction.Vsubps: EmitVector2Src(); break;
                            case X86Instruction.Vxorps: EmitVector2Src(); break;
                            case X86Instruction.Vroundpd: EmitRound(); break;
                            case X86Instruction.Vroundps: EmitRound(); break;
                            case X86Instruction.Vrsqrtps: EmitVrsqrtps(); break;
                            default: throw new Exception();
                        }

                    }; break;
                default: throw new Exception();
            }

            CleanScraps();
        }

        void CleanScraps()
        {
            ScrappedRegisters.Reverse();

            foreach (int scrap in ScrappedRegisters)
            {
                c.pop(_64[scrap]);
            }

            ScrappedRegisters = new List<int>();
        }

        bool TestRegisterInUse(Type type, int Reg)
        {
            foreach (IOperand operand in CurrentOperation.Destinations)
            {
                if (operand.GetType() == type)
                {
                    if ((operand as IOperandReg).Reg == Reg)
                        return true;
                }
            }

            foreach (IOperand operand in CurrentOperation.Sources)
            {
                if (operand.GetType() == type)
                {
                    if ((operand as IOperandReg).Reg == Reg)
                        return true;
                }
            }

            return false;
        }

        List<int> ScrappedRegisters = new List<int>();

        int GetScrappedRegister(Type type)
        {
            int top = (type == typeof(IntReg) ? _64.Length : _xmm.Length);

            for (int i = top - 1; i != -1; i--)
            {
                bool InUse = TestRegisterInUse(type, i);

                if (InUse)
                    continue;

                if (ScrappedRegisters.Contains(i))
                    continue;

                ScrappedRegisters.Add(i);

                c.push(_64[i]);

                return i;
            }

            throw new Exception();
        }

        IntSize PreferedSize;

        dynamic GetHostR(int IR, IOperand[] Sources, IntSize Size = (IntSize)(-1))
        {
            IOperand Source = Sources[IR];

            if (Size == (IntSize)(-1) && (Source is IOperandSize))
            {
                Size = (Source as IOperandSize).Size;
            }

            if (Size == IntSize.Undefined)
            {
                throw new Exception();
            }

            switch (Source)
            {
                case IntReg r: return GetHostRegister(Size, r.Reg);
                case ConstOperand cst:
                    {
                        int sReg = GetScrappedRegister(typeof(IntReg));

                        c.mov(_64[sReg], cst.Data);

                        return GetHostRegister(Size, sReg);
                    };

                case Xmm xmm: return _xmm[xmm.Reg];
                default: throw new Exception();
            }
        }

        void EmitAdd()
        {
            EnsureSameIntRegSize(CurrentOperation.Destinations[0], CurrentOperation.Sources[0], CurrentOperation.Sources[1]);

            dynamic dReg = GetHostR(0, CurrentOperation.Destinations);
            dynamic source0 = GetHostR(0, CurrentOperation.Sources);

            dynamic source1;

            if (CurrentOperation.Sources[1] is ConstOperand cop && cop.Data < int.MaxValue)
            {
                source1 = (int)cop.Data;
            }
            else
            {
                source1 = GetHostR(1, CurrentOperation.Sources);
            }

            if (dReg == source0)
            {
                c.add(dReg, source1);
            }
            else if (source1.GetType() == dReg.GetType() && dReg == source1)
            {
                c.add(dReg, source0);
            }
            else
            {
                c.lea(dReg, __[source0 + source1]);
            }
        }

        void EmitD_N(Intermediate.Instruction instruction)
        {
            PreferedSize = (CurrentOperation.Destinations[0] as IntReg).Size;

            EnsureSameIntRegSize(CurrentOperation.Destinations[0], CurrentOperation.Sources[0], CurrentOperation.Sources[1]);

            dynamic dReg = GetHostR(0, CurrentOperation.Destinations);
            dynamic source0 = GetHostR(0, CurrentOperation.Sources);

            dynamic source1;

            if (CurrentOperation.Sources[1] is ConstOperand cop && cop.Data < int.MaxValue)
            {
                source1 = (int)cop.Data;
            }
            else
            {
                source1 = GetHostR(1, CurrentOperation.Sources);
            }

            dynamic d;
            dynamic s;

            bool NeedsWorkers = false;

            if (dReg != source0)
            {
                d = dReg;

                c.mov(d, source0);
                s = source1;
            }
            else
            {
                NeedsWorkers = true;

                d = GetHostRegister(PreferedSize, GetScrappedRegister(typeof(IntReg)));

                c.mov(d, source0);
                s = source1;
            }

            switch (instruction)
            {
                case Intermediate.Instruction.Subtract: c.sub(d, s); break;
                case Intermediate.Instruction.LogicalAnd: c.and(d, s); break;
                case Intermediate.Instruction.LogicalExclusiveOr: c.xor(d, s); break;
                case Intermediate.Instruction.LogicalOr: c.or(d, s); break;
                default: throw new Exception();
            }

            if (NeedsWorkers)
                c.mov(dReg, d);
        }

        void EmitCopy()
        {
            PreferedSize = (CurrentOperation.Destinations[0] as IntReg).Size;

            EnsureSameIntRegSize(CurrentOperation.Destinations[0], CurrentOperation.Sources[0]);

            if (CurrentOperation.Sources[0] is ConstOperand cop)
            {
                c.mov(GetHostR(0, CurrentOperation.Destinations, IntSize.Int64), cop.Data);
            }
            else
            {
                dynamic dReg = GetHostR(0, CurrentOperation.Destinations);
                dynamic s0Reg = GetHostR(0, CurrentOperation.Sources);

                c.mov(dReg, s0Reg);
            }
        }

        void EmitNot()
        {
            PreferedSize = (CurrentOperation.Destinations[0] as IntReg).Size;

            EnsureSameIntRegSize(CurrentOperation.Destinations[0], CurrentOperation.Sources[0]);

            dynamic dReg = GetHostR(0, CurrentOperation.Destinations);
            dynamic s0Reg = GetHostR(0, CurrentOperation.Sources);

            c.mov(dReg, s0Reg);
            c.not(dReg);
        }

        void EmitConditionalSelect()
        {
            PreferedSize = (CurrentOperation.Destinations[0] as IntReg).Size;

            EnsureSameIntRegSize(CurrentOperation.Destinations[0], CurrentOperation.Sources[0], CurrentOperation.Sources[1], CurrentOperation.Sources[2]);

            dynamic dReg = GetHostR(0, CurrentOperation.Destinations);

            dynamic condition = GetHostR(0, CurrentOperation.Sources);
            dynamic yes = GetHostR(1, CurrentOperation.Sources);
            dynamic no = GetHostR(2, CurrentOperation.Sources);

            c.cmp(condition, 1);

            c.cmove(dReg, yes);
            c.cmovne(dReg, no);
        }

        void EmitSignExtend()
        {
            PreferedSize = (CurrentOperation.Sources[0] as IOperandSize).Size;

            dynamic dReg = GetHostR(0, CurrentOperation.Destinations);
            dynamic source0 = GetHostR(0, CurrentOperation.Sources);

            if (PreferedSize == IntSize.Int32)
            {
                c.movsxd(dReg, source0);
            }
            else
            {
                c.movsx(dReg, source0);
            }
        }

        void EmitLoad()
        {
            PreferedSize = (CurrentOperation.Sources[0] as IOperandSize).Size;

            IntSize DestinationSize = (CurrentOperation.Destinations[0] as IOperandSize).Size;

            dynamic destination64 = GetHostR(0, CurrentOperation.Destinations, IntSize.Int64);

            dynamic destination = GetHostR(0, CurrentOperation.Destinations);
            dynamic address = GetHostR(0, CurrentOperation.Sources);

            c.mov(destination, __[address]);

            if (DestinationSize == IntSize.Int16)
            {
                c.and(destination64, 65535);
            }
            else if (DestinationSize == IntSize.Int8)
            {
                c.and(destination64, 255);
            }
        }

        void EmitStore()
        {
            PreferedSize = (CurrentOperation.Sources[1] as IOperandSize).Size;

            dynamic address = GetHostR(0, CurrentOperation.Sources);
            dynamic value = GetHostR(1, CurrentOperation.Sources);

            c.mov(__[address], value);
        }
        
        void EmitCall()
        {
            //call des : function, args....

            IOperand[] Sources = CurrentOperation.Sources;

            dynamic GetSource(int index, IOperand[] _args)
            {
                if (_args[index] is ConstOperand co)
                {
                    return co.Data;
                }
                else if (_args[index] is IntReg ir)
                {
                    return GetPointerN(ir.Reg);
                }
                else
                {
                    throw new Exception();
                }
            }

            //save r15
            c.push(r15);
            c.sub(rsp,0x58);

            IOperand[] Args = Sources.ToList().GetRange(1, Sources.Length - 1).ToArray();

            for (int i = Args.Length - 1; i != -1; --i)
            {
                if (i == 0)
                    c.mov(rcx, GetSource(i, Args));
                else if (i == 1)
                    c.mov(rdx, GetSource(i, Args));
                else if (i == 2)
                    c.mov(r8, GetSource(i, Args));
                else if (i == 3)
                    c.mov(r9, GetSource(i, Args));
                else
                {
                    c.mov(r14, GetSource(i, Args));

                    c.push(r14);
                }
            }

            //load pointer
            c.mov(rax, GetSource(0, Sources));
            c.call(rax);

            c.add(rsp, 0x58);
            c.pop(r15);

            c.mov(GetPointerN((CurrentOperation.Destinations[0] as IntReg).Reg), rax);
        }

        void EmitGetContext()
        {
            dynamic result = GetHostR(0, CurrentOperation.Destinations, IntSize.Int64);

            c.mov(result, r15); 
        }

        void EmitCompareAndSwap()
        {
            PreferedSize = (CurrentOperation.Sources[1] as IOperandSize).Size;

            EnsureSameIntRegSize(CurrentOperation.Sources[1], CurrentOperation.Sources[2]);

            dynamic result64 = GetHostR(0, CurrentOperation.Destinations, IntSize.Int64);
            dynamic result8 = GetHostR(0, CurrentOperation.Destinations, IntSize.Int8);

            dynamic address = GetHostR(0, CurrentOperation.Sources);
            dynamic expecting = GetHostR(1, CurrentOperation.Sources);
            dynamic toswap = GetHostR(2, CurrentOperation.Sources);

            c.mov(RAX, expecting);
            c.cmpxchg(__[address], toswap);

            c.setz(result8);
            c.and(result64, 1);
        }

        void EmitMultiply(bool Hi, bool Signed)
        {
            //it is assumed rax and rdx are allocated out.

            PreferedSize = (CurrentOperation.Destinations[0] as IntReg).Size;

            EnsureSameIntRegSize(CurrentOperation.Destinations[0], CurrentOperation.Sources[0], CurrentOperation.Sources[1]);

            dynamic dReg = GetHostR(0, CurrentOperation.Destinations);
            dynamic s0Reg = GetHostR(0, CurrentOperation.Sources);
            dynamic s1Reg = GetHostR(1, CurrentOperation.Sources);

            //It is assumed that rax, and rdx are allocated out.

            c.mov(RDX, 0);

            c.mov(RAX, s0Reg);

            if (Signed)
            {
                c.imul(s1Reg);
            }
            else
            {
                c.mul(s1Reg);
            }

            if (Hi)
            {
                c.mov(dReg, RDX);
            }
            else
            {
                c.mov(dReg, RAX);
            }
        }

        void EmitDivide(bool Signed)
        {
            //it is assumed rax and rdx are allocated out.

            PreferedSize = (CurrentOperation.Destinations[0] as IntReg).Size;

            EnsureSameIntRegSize(CurrentOperation.Destinations[0], CurrentOperation.Sources[0], CurrentOperation.Sources[1]);

            dynamic dReg = GetHostR(0, CurrentOperation.Destinations);
            dynamic s0Reg = GetHostR(0, CurrentOperation.Sources);
            dynamic s1Reg = GetHostR(1, CurrentOperation.Sources);

            Label isZero = c.CreateLabel();
            Label end = c.CreateLabel();

            c.cmp(s1Reg, 0);

            c.je(isZero);

            c.je(end);

            c.mov(RAX, s0Reg);

            if (Signed)
            {
                if (PreferedSize == IntSize.Int32)
                {
                    c.cdq();
                }
                else
                {
                    c.cqo();
                }

                c.idiv(s1Reg);
            }
            else
            {
                c.mov(rdx, 0);

                c.div(s1Reg);
            }

            c.mov(dReg, RAX);

            c.jmp(end);
            c.Label(ref isZero);

            c.mov(dReg, 0);

            c.Label(ref end);
        }

        void EmitShift(Intermediate.Instruction instruction)
        {
            //it is assumed rcx and rax are allocated out.

            PreferedSize = (CurrentOperation.Destinations[0] as IntReg).Size;

            EnsureSameIntRegSize(CurrentOperation.Destinations[0], CurrentOperation.Sources[0], CurrentOperation.Sources[1]);

            dynamic dReg = GetHostR(0, CurrentOperation.Destinations);
            dynamic source0 = GetHostR(0, CurrentOperation.Sources);

            dynamic source1;

            dynamic worker = RAX;

            if (CurrentOperation.Sources[1] is ConstOperand cop)
            {
                source1 = (byte)(cop.Data & 255);

                c.mov(dReg, source0);
                worker = dReg;
            }
            else
            {
                source1 = GetHostR(1, CurrentOperation.Sources);

                c.mov(RCX, source1);

                source1 = cl;
            }

            c.mov(worker, source0);

            switch (instruction)
            {
                case Intermediate.Instruction.LogicalShiftLeft: c.shl(worker, source1); break;
                case Intermediate.Instruction.LogicalShiftRight: c.shr(worker, source1); break;
                case Intermediate.Instruction.LogicalShiftRightSigned: c.sar(worker, source1); break;
                case Intermediate.Instruction.LogicalRotateRight: c.ror(worker, source1); break;
                default: throw new Exception();
            }

            if (dReg != worker)
                c.mov(dReg, worker);
        }

        void EmitDoubleShiftRight()
        {
            PreferedSize = (CurrentOperation.Destinations[0] as IntReg).Size;

            EnsureSameIntRegSize(CurrentOperation.Destinations[0], CurrentOperation.Sources[0], CurrentOperation.Sources[1], CurrentOperation.Sources[2]);

            dynamic dReg = GetHostR(0, CurrentOperation.Destinations);
            dynamic s1Reg = GetHostR(0, CurrentOperation.Sources);
            dynamic s0Reg = GetHostR(1, CurrentOperation.Sources);
            dynamic shift = GetHostR(2, CurrentOperation.Sources);

            c.mov(RCX, shift);
            c.mov(RAX, s0Reg);

            c.shrd(RAX, s1Reg, cl);

            c.mov(dReg, RAX);
        }

        void EmitJumpIf()
        {
            if (CurrentOperation.Sources[1] is ConstOperand cop)
            {
                if (cop.Data == 1)
                {
                    c.jmp(Labels[CurrentNode.Branching.Start]);
                }
                else
                {
                    c.jmp(Labels[CurrentNode.Leading.Start]);
                }

                return;
            }

            dynamic Condition = GetHostR(1, CurrentOperation.Sources);

            c.cmp(Condition, 0);

            CleanScraps();

            c.je(Labels[CurrentNode.Leading.Start]);
            c.jmp(Labels[CurrentNode.Branching.Start]);
        }

        void EmitHardJump()
        {
            c.pop(rbp);
            c.pop(rbx);
            c.pop(rdx);

            if (CurrentOperation.Sources[0] is ConstOperand cop)
            {
                c.mov(rax, cop.Data);
            }
            else if (CurrentOperation.Sources[0] is IntReg ireg)
            {
                c.mov(rax, GetPointerN(ireg.Reg));
            }

            c.mov(rcx, r15);

            c.jmp(rax);
        }

        void EmitCompare(Intermediate.Instruction instruction)
        {
            PreferedSize = (CurrentOperation.Destinations[0] as IntReg).Size;

            EnsureSameIntRegSize(CurrentOperation.Destinations[0], CurrentOperation.Sources[0], CurrentOperation.Sources[1]);

            dynamic dReg = GetHostR(0, CurrentOperation.Destinations);
            dynamic source0 = GetHostR(0, CurrentOperation.Sources);

            dynamic source1;

            if (CurrentOperation.Sources[1] is ConstOperand cop && cop.Data < int.MaxValue)
            {
                source1 = (int)cop.Data;
            }
            else
            {
                source1 = GetHostR(1, CurrentOperation.Sources);
            }

            c.cmp(source0, source1);

            AssemblerRegister8 dReg8 = _8[(CurrentOperation.Destinations[0] as IntReg).Reg];

            switch (instruction)
            {
                case Intermediate.Instruction.CompareEqual: c.sete(dReg8); break;
                case Intermediate.Instruction.CompareGreaterOrEqual: c.setae(dReg8); break;
                case Intermediate.Instruction.CompareLessSigned: c.setl(dReg8); break;
                case Intermediate.Instruction.CompareLess: c.setb(dReg8); break;
                case Intermediate.Instruction.CompareGreater: c.seta(dReg8); break;
                default: throw new Exception();
            }

            c.and(dReg, 1);
        }

        void EmitReturn()
        {
            c.pop(rbp);
            c.pop(rbx);
            c.pop(rdx);

            if (CurrentOperation.Sources[0] is ConstOperand cop)
            {
                c.mov(rax, cop.Data);
            }
            else if (CurrentOperation.Sources[0] is IntReg ireg)
            {
                c.mov(rax, GetPointerN(ireg.Reg));
            }

            c.ret();
        }

        void EmitAllocate()
        {
            dynamic hReg = GetHostR(0, CurrentOperation.Sources);
            int ctx = (int)(CurrentOperation.Sources[1] as ConstOperand).Data;

            int type = (int)(CurrentOperation.Sources[2] as ConstOperand).Data;

            if (CurrentOperation.Sources[0] is IntReg)
            {
                if (type == 1)
                {
                    c.mov(hReg, GetPointerN(ctx));
                }
                else
                {
                    c.mov(GetPointerN(ctx), hReg);
                }
            }
            else
            {
                if (type == 1)
                {
                    c.vmovupd(hReg, GetPointerN(ctx));
                }
                else
                {
                    c.vmovupd(GetPointerN(ctx), hReg);
                }
            }
        }

        void EmitLzcnt()
        {
            PreferedSize = (CurrentOperation.Destinations[0] as IntReg).Size;

            dynamic dReg = GetHostR(0, CurrentOperation.Destinations);
            dynamic s0Reg = GetHostR(0, CurrentOperation.Sources);

            c.lzcnt(dReg, s0Reg);
        }

        void EmitBswap()
        {
            dynamic dReg = GetHostR(0, CurrentOperation.Destinations);

            c.bswap(dReg);
        }

        void EmitPinsr()
        {
            PreferedSize = (CurrentOperation.Sources[0] as IOperandSize).Size;

            dynamic des = GetHostR(0, CurrentOperation.Destinations);
            dynamic src = GetHostR(0, CurrentOperation.Sources);

            byte index = (byte)(CurrentOperation.Sources[1] as ConstOperand).Data;

            switch ((X86Instruction)CurrentOperation.Instruction)
            {
                case X86Instruction.Pinsrb: c.pinsrb(des, src, index); break;
                case X86Instruction.Pinsrw: c.pinsrw(des, src, index); break;
                case X86Instruction.Pinsrd: c.pinsrd(des, src, index); break;
                case X86Instruction.Pinsrq: c.pinsrq(des, src, index); break;
                default: throw new Exception();
            }
        }

        void EmitPext()
        {
            dynamic des = GetHostR(0, CurrentOperation.Destinations);

            dynamic src = GetHostR(0, CurrentOperation.Sources);
            byte index = (byte)(CurrentOperation.Sources[1] as ConstOperand).Data;

            switch ((X86Instruction)CurrentOperation.Instruction)
            {
                case X86Instruction.Pextrb: c.pextrb(des, src, index); break;
                case X86Instruction.Pextrw: c.pextrw(des, src, index); break;
                case X86Instruction.Pextrd: c.pextrd(des, src, index); break;
                case X86Instruction.Pextrq: c.pextrq(des, src, index); break;
                default: throw new Exception();
            }
        }

        void EmitMovaps()
        {
            dynamic des = GetHostR(0, CurrentOperation.Destinations);
            dynamic src = GetHostR(0, CurrentOperation.Sources);

            c.movaps(des, src);
        }

        void EmitVmovupd(bool IsLoad)
        {
            if (IsLoad)
            {
                dynamic des = GetHostR(0, CurrentOperation.Destinations);
                dynamic address = GetHostR(0, CurrentOperation.Sources);

                c.vmovupd(des, __[address]);
            }
            else
            {
                dynamic address = GetHostR(0, CurrentOperation.Sources);
                dynamic vector = GetHostR(1, CurrentOperation.Sources);

                c.vmovupd(__[address], vector);
            }
        }

        void EmitCvtsd2ss()
        {
            dynamic des = GetHostR(0, CurrentOperation.Destinations);
            dynamic src0 = GetHostR(0, CurrentOperation.Sources);

            c.cvtsd2ss(des, src0);
        }

        void EmitCvtss2sd()
        {
            dynamic des = GetHostR(0, CurrentOperation.Destinations);
            dynamic src0 = GetHostR(0, CurrentOperation.Sources);

            c.cvtss2sd(des, src0);
        }

        void EmitSqrtps()
        {
            dynamic des = GetHostR(0, CurrentOperation.Destinations);
            dynamic src0 = GetHostR(0, CurrentOperation.Sources);

            c.sqrtps(des, src0);
        }

        void EmitPopCnt()
        {
            dynamic des = GetHostR(0, CurrentOperation.Destinations);
            dynamic src0 = GetHostR(0, CurrentOperation.Sources);

            c.popcnt(des, src0);
        }

        void EmitSqrtpd()
        {
            dynamic des = GetHostR(0, CurrentOperation.Destinations);
            dynamic src0 = GetHostR(0, CurrentOperation.Sources);

            c.sqrtpd(des, src0);
        }

        void EmitVector2Src()
        {
            dynamic des = GetHostR(0, CurrentOperation.Destinations);
            dynamic src0 = GetHostR(0, CurrentOperation.Sources);
            dynamic src1 = GetHostR(1, CurrentOperation.Sources);

            switch ((X86Instruction)CurrentOperation.Instruction)
            {
                case X86Instruction.Vandps: c.vandps(des, src0, src1); break;
                case X86Instruction.Vxorps: c.vxorps(des, src0, src1); break;
                case X86Instruction.Vorps: c.vorps(des, src0, src1); break;
                case X86Instruction.Vmulps: c.vmulps(des, src0, src1); break;
                case X86Instruction.Vmulpd: c.vmulpd(des, src0, src1); break;
                case X86Instruction.Vaddps: c.vaddps(des, src0, src1); break;
                case X86Instruction.Vaddpd: c.vaddpd(des, src0, src1); break;
                case X86Instruction.Vdivps: c.vdivps(des, src0, src1); break;
                case X86Instruction.Vdivpd: c.vdivpd(des, src0, src1); break;
                case X86Instruction.Vsubps: c.vsubps(des, src0, src1); break;
                case X86Instruction.Vsubpd: c.vsubpd(des, src0, src1); break;
                case X86Instruction.Vmaxps: c.vmaxps(des, src0, src1); break;
                case X86Instruction.Vmaxpd: c.vmaxpd(des, src0, src1); break;
                case X86Instruction.Vminps: c.vminps(des, src0, src1); break;
                case X86Instruction.Vminpd: c.vminpd(des, src0, src1); break;
                case X86Instruction.Vpaddb: c.vpaddb(des, src0, src1); break;
                case X86Instruction.Vpaddw: c.vpaddw(des, src0, src1); break;
                case X86Instruction.Vpaddd: c.vpaddd(des, src0, src1); break;
                case X86Instruction.Vpaddq: c.vpaddq(des, src0, src1); break;
                case X86Instruction.Vpsubb: c.vpsubb(des, src0, src1); break;
                case X86Instruction.Vpsubw: c.vpsubw(des, src0, src1); break;
                case X86Instruction.Vpsubd: c.vpsubd(des, src0, src1); break;
                case X86Instruction.Vpsubq: c.vpsubq(des, src0, src1); break;

                default: throw new Exception();
            }
        }

        void EmitRound()
        {
            dynamic des = GetHostR(0, CurrentOperation.Destinations);
            dynamic src0 = GetHostR(0, CurrentOperation.Sources);

            int type = (int)(CurrentOperation.Sources[1] as ConstOperand).Data;

            switch ((X86Instruction)CurrentOperation.Instruction)
            {
                case X86Instruction.Vroundps: c.vroundps(des, src0, (byte)type); break;
                case X86Instruction.Vroundpd: c.vroundpd(des, src0, (byte)type); break;
                default: throw new Exception();
            }
        }

        void EmitVrsqrtps()
        {
            dynamic des = GetHostR(0, CurrentOperation.Destinations);
            dynamic src0 = GetHostR(0, CurrentOperation.Sources);

            c.vrsqrtps(des, src0);
        }

        void EmitAddSubtract_GetFlags()
        {
            PreferedSize = (CurrentOperation.Destinations[0] as IntReg).Size;

            dynamic src0 = GetHostR(0, CurrentOperation.Sources);
            dynamic src1 = GetHostR(1, CurrentOperation.Sources);

            dynamic result = GetHostR(0, CurrentOperation.Destinations);

            dynamic is_neg = GetHostR(1, CurrentOperation.Destinations, IntSize.Int8); 
            dynamic is_zero = GetHostR(2, CurrentOperation.Destinations, IntSize.Int8);
            dynamic is_carry = GetHostR(3, CurrentOperation.Destinations, IntSize.Int8);
            dynamic is_overflow = GetHostR(4, CurrentOperation.Destinations, IntSize.Int8);

            dynamic worker = GetHostRegister(PreferedSize, GetScrappedRegister(typeof(IntReg)));

            c.mov(worker, src0);

            c.add(worker, src1);

            c.setng(is_neg);
            c.setz(is_zero);
            c.setc(is_carry);
            c.seto(is_overflow);

            c.mov(result, worker);
        }
    }
}
