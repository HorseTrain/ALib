using Compiler.Intermediate;
using Compiler.Intermediate.Extensions.X86;
using System;
using System.Collections.Generic;
using System.Linq;
using static Iced.Intel.AssemblerRegisters;

namespace Compiler.Backend.X86
{
    delegate void EmitAllocate(int Host, int Guest, bool IsLoad);

    class EmitContextStore
    {
        public List<(int, RequestMode)> GuestRegisters          { get; set; }
        public List<int> Locked                                 { get; set; }
    }

    enum RequestMode
    {
        Null = 0,
        Read = 1 << 0,
        Write = 1 << 1,
        ReadWrite = Read | Write,
    }

    class GuestRegister
    {
        public const int UnloadedReg = int.MinValue;

        public int Guest    { get; set; } = UnloadedReg;
        public bool Loaded  => Guest != UnloadedReg;

        public RequestMode Mode { get; set; }
    }

    class EmitContext
    {
        public GuestRegister[] Registers    { get; set; }
        public EmitAllocate emit            { get; set; }

        public HashSet<int> Locked          { get; set; }

        public EmitContext(EmitAllocate allocate, int HostCount)
        {
            emit = allocate;

            Registers = new GuestRegister[HostCount];

            for (int i = 0; i < Registers.Length; ++i)
            {
                Registers[i] = new GuestRegister();
            }

            Locked = new HashSet<int>();
        }
        
        public void Lock(int register)
        {
            Locked.Add(register);
        }

        void Unlock(int register)
        {
            Locked.Remove(register);
        }

        void LoadRegister(int Host, int Guest, RequestMode Mode)
        {
            UnloadRegister(Host);

            GuestRegister register = Registers[Host];

            register.Mode |= Mode;
            register.Guest = Guest;

            Lock(Host);

            if (Mode.HasFlag(RequestMode.Read))
                emit(Host, Guest, true);
        }

        void UnloadRegister(int Host)
        {
            GuestRegister register = Registers[Host];

            if (!register.Loaded)
                return;

            if (register.Mode.HasFlag(RequestMode.Write))
                emit(Host, register.Guest, false);

            Unlock(Host);

            register.Guest = GuestRegister.UnloadedReg;
            register.Mode = RequestMode.Null;
        }

        public int RequestRegister(int Guest, RequestMode Mode)
        {
            for (int i = 0; i < Registers.Length; ++i)
            {
                if (Registers[i].Guest == Guest)
                {
                    Lock(i);

                    Registers[i].Mode |= Mode;

                    return i;
                }
            }

            for (int i = 0; i < Registers.Length; ++i)
            {
                if (!Registers[i].Loaded)
                {
                    LoadRegister(i, Guest, Mode);

                    return i;
                }
            }

            for (int i = 0; i < Registers.Length; ++i)
            {
                if (!Locked.Contains(i))
                {
                    LoadRegister(i, Guest, Mode);

                    return i;
                }
            }

            throw new Exception();
        }

        public void ForceAllocate(int Host, int Guest, RequestMode Mode)
        {
            if (Locked.Contains(Host))
                throw new Exception();

            LoadRegister(Host, Guest, Mode);
        }

        public void UnloadAll()
        {
            for (int i = 0; i < Registers.Length; ++i)
                UnloadRegister(i);
        }

        public void UnlockAll()
        {
            Locked = new HashSet<int>();
        }

        public EmitContextStore CreateContext()
        {
            EmitContextStore Out = new EmitContextStore();

            Out.GuestRegisters = new List<(int, RequestMode)>();

            Out.Locked = Locked.ToList();

            foreach (GuestRegister register in Registers)
            {
                Out.GuestRegisters.Add((register.Guest, register.Mode));
            }

            return Out;
        }

        public void LoadContext(EmitContextStore store)
        {
            for (int i = 0; i < store.GuestRegisters.Count; ++i)
            {
                Registers[i].Guest = store.GuestRegisters[i].Item1;
                Registers[i].Mode = store.GuestRegisters[i].Item2;
            }

            Locked = new HashSet<int>();

            foreach (int Lock in store.Locked)
            {
                Locked.Add(Lock);
            }
        }
    }

    public class RegisterAllocator
    {
        //NOTE: it is up to the user to prevent collisions.
        EmitContext gpAllocator             { get; set; }
        EmitContext xmmAllocator            { get; set; }

        OperationBlock AllocatedCode        { get; set; }

        ControlFlowGraph SourceCFG                           { get; set; }
        Dictionary<ControlFlowNode, ConstOperand> NewLabels  { get; set; }

        void EmitAllocateGP(int Host, int Guest, bool IsLoad)
        {
            if (Guest < 0)
                return;

            AllocatedCode.Emit(InstructionType.Normal, (int)Instruction.AllocateRegister, new IOperand[] { }, new IOperand[] { IntReg.Create(IntSize.Int64, Host), ConstOperand.Create(Guest), ConstOperand.Create(IsLoad) });
        }

        void EmitAllocateVector(int Host, int Guest, bool IsLoad)
        {
            if (Guest < 0)
                return;

            AllocatedCode.Emit(InstructionType.Normal, (int)Instruction.AllocateRegister, new IOperand[] { }, new IOperand[] { Xmm.Create(Host), ConstOperand.Create(Guest), ConstOperand.Create(IsLoad) });
        }

        void UnloadAll()
        {
            gpAllocator.UnloadAll();
            xmmAllocator.UnloadAll();
        }

        void UnlockAll()
        {
            gpAllocator.UnlockAll();
            xmmAllocator.UnlockAll();
        }

        public RegisterAllocator(ControlFlowGraph SourceCFG)
        {
            this.SourceCFG = SourceCFG;
        }

        void LockTroubleRegisters()
        {
            gpAllocator.ForceAllocate(X86Assembler.IndexOf(rax), -2, RequestMode.Null);
            gpAllocator.ForceAllocate(X86Assembler.IndexOf(rcx), -3, RequestMode.Null);
            gpAllocator.ForceAllocate(X86Assembler.IndexOf(rdx), -4, RequestMode.Null);
        }

        public Operation CurrentOperation;

        int ScrapMin = -1;

        bool EmitNode(ControlFlowNode Source, bool JumpToNext, bool HandleJumpingOutside)
        {
            AllocatedCode.MarkLabel(NewLabels[Source]);

            if (Source.Length == 0)
                return false;

            for (int i = 0; i < Source.Length; ++i)
            {
                Operation operation = Source.GetOperation(i);

                CurrentOperation = operation;

                if (operation.IsInstruction(Instruction.Return) || operation.IsInstruction(Instruction.HardJump))
                {
                    UnloadAll();

                    AllocatedCode.Emit(InstructionType.Normal, (int)operation.Instruction, new IOperand[0], operation.Sources);
                }
                else if (operation.IsInstruction(Instruction.JumpIf))
                {
                    if (HandleJumpingOutside)
                    {
                        return true;
                    }

                    List<IOperand> Sources = AllocateRegisters(operation.Sources, RequestMode.Read);

                    UnloadAll();

                    AllocatedCode.Emit(InstructionType.Normal, (int)Instruction.JumpIf, new IOperand[0], new IOperand[] { NewLabels[Source.Branching], Sources[1] });

                    if (JumpToNext)
                        AllocatedCode.Emit(InstructionType.Normal, (int)Instruction.JumpIf, new IOperand[0], new IOperand[] { NewLabels[Source.Leading], ConstOperand.Create(1) });
                }
                else if (operation.IsInstruction(Instruction.Call))
                {
                    UnloadAll();

                    //Do not modify.
                    AllocatedCode.Emit(InstructionType.Normal, (int)Instruction.Call, operation.Destinations, operation.Sources);
                }
                else
                {
                    if (operation.Type == InstructionType.Normal && X86Assembler.RAProblemInstruction((Instruction)CurrentOperation.Instruction))
                    {
                        LockTroubleRegisters();
                    }

                    List<IOperand> Sources = AllocateRegisters(operation.Sources, RequestMode.Read);
                    List<IOperand> Destinations = AllocateRegisters(operation.Destinations, RequestMode.Write);

                    AllocatedCode.Emit(operation.Type, operation.Instruction, Destinations.ToArray(), Sources.ToArray());
                }

                UnlockAll();
            }

            return false;
        }

        List<IOperand> AllocateRegisters( IOperand[] Sources, RequestMode Mode)
        {
            List<IOperand> Out = new List<IOperand>();

            foreach (IOperand Source in Sources)
            {
                switch (Source)
                {
                    case IntReg ir:
                        {
                            int AllocatedRegIndex = gpAllocator.RequestRegister(ir.Reg, Mode);

                            IntReg AllocatedReg = IntReg.Create(ir.Size, AllocatedRegIndex);

                            Out.Add(AllocatedReg);

                        }; break;

                    case ConstOperand co:
                        {
                            if ((co.Data >= int.MaxValue && CurrentOperation.Type == InstructionType.Normal && !CurrentOperation.IsInstruction(Instruction.JumpIf)))
                            {
                                int Scrap = gpAllocator.RequestRegister(ScrapMin--, RequestMode.Null);

                                AllocatedCode.Emit(Instruction.Copy, IntReg.Create(co.Size, Scrap), ConstOperand.Create(co.Data, co.Size));

                                Out.Add(IntReg.Create(co.Size, Scrap));
                            }
                            else
                            {
                                Out.Add(co);
                            }
                        }; break;

                    case Xmm xmm:
                        {
                            bool AllNeeded = X86Assembler.RAProblemInstruction((X86Instruction)CurrentOperation.Instruction) && CurrentOperation.Type == InstructionType.X86;

                            int AllocatedRegIndex = xmmAllocator.RequestRegister(xmm.Reg, AllNeeded ? RequestMode.ReadWrite : Mode);

                            Xmm AllocatedReg = Xmm.Create(AllocatedRegIndex);

                            Out.Add(AllocatedReg);
                        }; break;
                    default: throw new Exception();
                }
            }

            return Out;
        }

        (EmitContextStore, EmitContextStore) GetRegisterContextStore() => (gpAllocator.CreateContext(), xmmAllocator.CreateContext());

        void LoadRegisterContextStore((EmitContextStore gp, EmitContextStore xmm) stores)
        {
            gpAllocator.LoadContext(stores.gp);
            xmmAllocator.LoadContext(stores.xmm);
        }

        public ControlFlowGraph Allocate()
        {
            NewLabels = new Dictionary<ControlFlowNode, ConstOperand>();
            AllocatedCode = new OperationBlock();

            gpAllocator = new EmitContext(EmitAllocateGP, X86Assembler._64.Length);
            xmmAllocator = new EmitContext(EmitAllocateVector, X86Assembler._xmm.Length);

            foreach (var node in SourceCFG.Nodes)
            {
                NewLabels.Add(node.Value, AllocatedCode.CreateLabel());
            }

            ControlFlowNode[] Nodes = SourceCFG.Nodes.Values.ToArray();

            for (int i = 0; i < Nodes.Length; ++i)
            {
                ControlFlowNode CurrentNode = Nodes[i];
                ControlFlowNode NextNode = (i + 1 < Nodes.Length) ? Nodes[i + 1] : null;

                bool JumpToNext = NextNode == null || CurrentNode.Leading != NextNode;

                EmitNode(CurrentNode, JumpToNext, false);
            }

            return new ControlFlowGraph(AllocatedCode);
        }
    }
}
