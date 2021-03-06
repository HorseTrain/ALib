using DCpu.CodeGen.RegisterAllocators;
using DCpu.IntermediateRepresentation;
using DCpu.Translation;
using System.Collections.Generic;
using System.Diagnostics;

using static DCpu.IntermediateRepresentation.OperandHelper;

namespace DCpu.CodeGen.X86
{
    using LLNode = LinkedListNode<Node>;

    static class PreAllocator
    {
        public static void RunPass(CompilerContext cctx, StackAllocator stackAlloc, out int maxCallArgs)
        {
            maxCallArgs = -1;

            CallConvName callConv = CallingConvention.GetCurrentCallConv();

            Operand[] preservedArgs = new Operand[CallingConvention.GetArgumentsOnRegsCount()];

            foreach (BasicBlock block in cctx.Cfg.Blocks)
            {
                LLNode nextNode;

                for (LLNode node = block.Operations.First; node != null; node = nextNode)
                {
                    nextNode = node.Next;

                    if (!(node.Value is Operation operation))
                    {
                        continue;
                    }

                    HandleConstantCopy(node, operation);

                    HandleSameDestSrc1Copy(node, operation);

                    HandleFixedRegisterCopy(node, operation);

                    switch (operation.Instruction)
                    {
                        case Instruction.Call:
                            // Get the maximum number of arguments used on a call.
                            // On windows, when a struct is returned from the call,
                            // we also need to pass the pointer where the struct
                            // should be written on the first argument.
                            int argsCount = operation.SourcesCount - 1;

                            if (operation.Destination != null && operation.Destination.Type == OperandType.V128)
                            {
                                argsCount++;
                            }

                            if (maxCallArgs < argsCount)
                            {
                                maxCallArgs = argsCount;
                            }

                            // Copy values to registers expected by the function
                            // being called, as mandated by the ABI.
                            if (callConv == CallConvName.Windows)
                            {
                                node = HandleCallWindowsAbi(stackAlloc, node, operation);
                            }
                            else /* if (callConv == CallConvName.SystemV) */
                            {
                                node = HandleCallSystemVAbi(node, operation);
                            }
                            break;

                        case Instruction.ConvertToFPUI:
                            HandleConvertToFPUI(node, operation);
                            break;

                        case Instruction.LoadArgument:
                            if (callConv == CallConvName.Windows)
                            {
                                HandleLoadArgumentWindowsAbi(cctx, node, preservedArgs, operation);
                            }
                            else /* if (callConv == CallConvName.SystemV) */
                            {
                                HandleLoadArgumentSystemVAbi(cctx, node, preservedArgs, operation);
                            }
                            break;

                        case Instruction.Negate:
                            if (!operation.GetSource(0).Type.IsInteger())
                            {
                                node = HandleNegate(node, operation);
                            }
                            break;

                        case Instruction.Return:
                            if (callConv == CallConvName.Windows)
                            {
                                HandleReturnWindowsAbi(cctx, node, preservedArgs, operation);
                            }
                            else /* if (callConv == CallConvName.SystemV) */
                            {
                                HandleReturnSystemVAbi(node, operation);
                            }
                            break;

                        case Instruction.VectorInsert8:
                            if (!HardwareCapabilities.SupportsSse41)
                            {
                                node = HandleVectorInsert8(node, operation);
                            }
                            break;
                    }
                }
            }
        }

        private static void HandleConstantCopy(LLNode node, Operation operation)
        {
            if (operation.SourcesCount == 0 || IsIntrinsic(operation.Instruction))
            {
                return;
            }

            Instruction inst = operation.Instruction;

            Operand src1 = operation.GetSource(0);
            Operand src2;

            if (src1.Kind == OperandKind.Constant)
            {
                if (!src1.Type.IsInteger())
                {
                    // Handle non-integer types (FP32, FP64 and V128).
                    // For instructions without an immediate operand, we do the following:
                    // - Insert a copy with the constant value (as integer) to a GPR.
                    // - Insert a copy from the GPR to a XMM register.
                    // - Replace the constant use with the XMM register.
                    src1 = AddXmmCopy(node, src1);

                    operation.SetSource(0, src1);
                }
                else if (!HasConstSrc1(inst))
                {
                    // Handle integer types.
                    // Most ALU instructions accepts a 32-bits immediate on the second operand.
                    // We need to ensure the following:
                    // - If the constant is on operand 1, we need to move it.
                    // -- But first, we try to swap operand 1 and 2 if the instruction is commutative.
                    // -- Doing so may allow us to encode the constant as operand 2 and avoid a copy.
                    // - If the constant is on operand 2, we check if the instruction supports it,
                    // if not, we also add a copy. 64-bits constants are usually not supported.
                    if (IsCommutative(inst))
                    {
                        src2 = operation.GetSource(1);

                        Operand temp = src1;

                        src1 = src2;
                        src2 = temp;

                        operation.SetSource(0, src1);
                        operation.SetSource(1, src2);
                    }

                    if (src1.Kind == OperandKind.Constant)
                    {
                        src1 = AddCopy(node, src1);

                        operation.SetSource(0, src1);
                    }
                }
            }

            if (operation.SourcesCount < 2)
            {
                return;
            }

            src2 = operation.GetSource(1);

            if (src2.Kind == OperandKind.Constant)
            {
                if (!src2.Type.IsInteger())
                {
                    src2 = AddXmmCopy(node, src2);

                    operation.SetSource(1, src2);
                }
                else if (!HasConstSrc2(inst) || IsLongConst(src2))
                {
                    src2 = AddCopy(node, src2);

                    operation.SetSource(1, src2);
                }
            }
        }

        private static LLNode HandleFixedRegisterCopy(LLNode node, Operation operation)
        {
            Operand dest = operation.Destination;

            LinkedList<Node> nodes = node.List;

            switch (operation.Instruction)
            {
                case Instruction.CompareAndSwap128:
                {
                    // Handle the many restrictions of the compare and exchange (16 bytes) instruction:
                    // - The expected value should be in RDX:RAX.
                    // - The new value to be written should be in RCX:RBX.
                    // - The value at the memory location is loaded to RDX:RAX.
                    void SplitOperand(Operand source, Operand lr, Operand hr)
                    {
                        nodes.AddBefore(node, new Operation(Instruction.VectorExtract, lr, source, Const(0)));
                        nodes.AddBefore(node, new Operation(Instruction.VectorExtract, hr, source, Const(1)));
                    }

                    Operand rax = Gpr(X86Register.Rax, OperandType.I64);
                    Operand rbx = Gpr(X86Register.Rbx, OperandType.I64);
                    Operand rcx = Gpr(X86Register.Rcx, OperandType.I64);
                    Operand rdx = Gpr(X86Register.Rdx, OperandType.I64);

                    SplitOperand(operation.GetSource(1), rax, rdx);
                    SplitOperand(operation.GetSource(2), rbx, rcx);

                    node = nodes.AddAfter(node, new Operation(Instruction.VectorCreateScalar, dest, rax));
                    node = nodes.AddAfter(node, new Operation(Instruction.VectorInsert,       dest, dest, rdx, Const(1)));

                    operation.SetDestinations(new Operand[] { rdx, rax });

                    operation.SetSources(new Operand[] { operation.GetSource(0), rdx, rax, rcx, rbx });

                    break;
                }

                case Instruction.CpuId:
                {
                    // Handle the many restrictions of the CPU Id instruction:
                    // - EAX controls the information returned by this instruction.
                    // - When EAX is 1, feature information is returned.
                    // - The information is written to registers EAX, EBX, ECX and EDX.
                    Debug.Assert(dest.Type == OperandType.I64);

                    Operand eax = Gpr(X86Register.Rax, OperandType.I32);
                    Operand ebx = Gpr(X86Register.Rbx, OperandType.I32);
                    Operand ecx = Gpr(X86Register.Rcx, OperandType.I32);
                    Operand edx = Gpr(X86Register.Rdx, OperandType.I32);

                    // Value 0x01 = Version, family and feature information.
                    nodes.AddBefore(node, new Operation(Instruction.Copy, eax, Const(1)));

                    // Copy results to the destination register.
                    // The values are split into 2 32-bits registers, we merge them
                    // into a single 64-bits register.
                    Operand rcx = Gpr(X86Register.Rcx, OperandType.I64);

                    node = nodes.AddAfter(node, new Operation(Instruction.ZeroExtend32, dest, edx));
                    node = nodes.AddAfter(node, new Operation(Instruction.ShiftLeft,    dest, dest, Const(32)));
                    node = nodes.AddAfter(node, new Operation(Instruction.BitwiseOr,    dest, dest, rcx));

                    operation.SetDestinations(new Operand[] { eax, ebx, ecx, edx });

                    operation.SetSources(new Operand[] { eax });

                    break;
                }

                case Instruction.Divide:
                case Instruction.DivideUI:
                {
                    // Handle the many restrictions of the division instructions:
                    // - The dividend is always in RDX:RAX.
                    // - The result is always in RAX.
                    // - Additionally it also writes the remainder in RDX.
                    if (dest.Type.IsInteger())
                    {
                        Operand src1 = operation.GetSource(0);

                        Operand rax = Gpr(X86Register.Rax, src1.Type);
                        Operand rdx = Gpr(X86Register.Rdx, src1.Type);

                        nodes.AddBefore(node, new Operation(Instruction.Copy,    rax, src1));
                        nodes.AddBefore(node, new Operation(Instruction.Clobber, rdx));

                        node = nodes.AddAfter(node, new Operation(Instruction.Copy, dest, rax));

                        operation.SetDestinations(new Operand[] { rdx, rax });

                        operation.SetSources(new Operand[] { rdx, rax, operation.GetSource(1) });

                        operation.Destination = rax;
                    }

                    break;
                }

                case Instruction.Extended:
                {
                    IntrinsicOperation intrinOp = (IntrinsicOperation)operation;

                    // PBLENDVB last operand is always implied to be XMM0 when VEX is not supported.
                    if (intrinOp.Intrinsic == Intrinsic.X86Pblendvb && !HardwareCapabilities.SupportsVexEncoding)
                    {
                        Operand xmm0 = Xmm(X86Register.Xmm0, OperandType.V128);

                        nodes.AddBefore(node, new Operation(Instruction.Copy, xmm0, operation.GetSource(2)));

                        operation.SetSource(2, xmm0);
                    }

                    break;
                }

                case Instruction.Multiply64HighSI:
                case Instruction.Multiply64HighUI:
                {
                    // Handle the many restrictions of the i64 * i64 = i128 multiply instructions:
                    // - The multiplicand is always in RAX.
                    // - The lower 64-bits of the result is always in RAX.
                    // - The higher 64-bits of the result is always in RDX.
                    Operand src1 = operation.GetSource(0);

                    Operand rax = Gpr(X86Register.Rax, src1.Type);
                    Operand rdx = Gpr(X86Register.Rdx, src1.Type);

                    nodes.AddBefore(node, new Operation(Instruction.Copy, rax, src1));

                    operation.SetSource(0, rax);

                    node = nodes.AddAfter(node, new Operation(Instruction.Copy, dest, rdx));

                    operation.SetDestinations(new Operand[] { rdx, rax });

                    break;
                }

                case Instruction.RotateRight:
                case Instruction.ShiftLeft:
                case Instruction.ShiftRightSI:
                case Instruction.ShiftRightUI:
                {
                    // The shift register is always implied to be CL (low 8-bits of RCX or ECX).
                    if (operation.GetSource(1).Kind == OperandKind.LocalVariable)
                    {
                        Operand rcx = Gpr(X86Register.Rcx, OperandType.I32);

                        nodes.AddBefore(node, new Operation(Instruction.Copy, rcx, operation.GetSource(1)));

                        operation.SetSource(1, rcx);
                    }

                    break;
                }
            }

            return node;
        }

        private static LLNode HandleSameDestSrc1Copy(LLNode node, Operation operation)
        {
            if (operation.Destination == null || operation.SourcesCount == 0)
            {
                return node;
            }

            Instruction inst = operation.Instruction;

            Operand dest = operation.Destination;
            Operand src1 = operation.GetSource(0);

            LinkedList<Node> nodes = node.List;

            // The multiply instruction (that maps to IMUL) is somewhat special, it has
            // a three operand form where the second source is a immediate value.
            bool threeOperandForm = inst == Instruction.Multiply && operation.GetSource(1).Kind == OperandKind.Constant;

            if (IsSameOperandDestSrc1(operation) && src1.Kind == OperandKind.LocalVariable && !threeOperandForm)
            {
                bool useNewLocal = false;

                for (int srcIndex = 1; srcIndex < operation.SourcesCount; srcIndex++)
                {
                    if (operation.GetSource(srcIndex) == dest)
                    {
                        useNewLocal = true;

                        break;
                    }
                }

                if (useNewLocal)
                {
                    // Dest is being used as some source already, we need to use a new
                    // local to store the temporary value, otherwise the value on dest
                    // local would be overwritten.
                    Operand temp = Local(dest.Type);

                    nodes.AddBefore(node, new Operation(Instruction.Copy, temp, src1));

                    operation.SetSource(0, temp);

                    node = nodes.AddAfter(node, new Operation(Instruction.Copy, dest, temp));

                    operation.Destination = temp;
                }
                else
                {
                    nodes.AddBefore(node, new Operation(Instruction.Copy, dest, src1));

                    operation.SetSource(0, dest);
                }
            }
            else if (inst == Instruction.ConditionalSelect)
            {
                Operand src2 = operation.GetSource(1);
                Operand src3 = operation.GetSource(2);

                if (src1 == dest || src2 == dest)
                {
                    Operand temp = Local(dest.Type);

                    nodes.AddBefore(node, new Operation(Instruction.Copy, temp, src3));

                    operation.SetSource(2, temp);

                    node = nodes.AddAfter(node, new Operation(Instruction.Copy, dest, temp));

                    operation.Destination = temp;
                }
                else
                {
                    nodes.AddBefore(node, new Operation(Instruction.Copy, dest, src3));

                    operation.SetSource(2, dest);
                }
            }

            return node;
        }

        private static LLNode HandleConvertToFPUI(LLNode node, Operation operation)
        {
            // Unsigned integer to FP conversions are not supported on X86.
            // We need to turn them into signed integer to FP conversions, and
            // adjust the final result.
            Operand dest   = operation.Destination;
            Operand source = operation.GetSource(0);

            Debug.Assert(source.Type.IsInteger(), $"Invalid source type \"{source.Type}\".");

            LinkedList<Node> nodes = node.List;

            LLNode currentNode = node;

            if (source.Type == OperandType.I32)
            {
                // For 32-bits integers, we can just zero-extend to 64-bits,
                // and then use the 64-bits signed conversion instructions.
                Operand zex = Local(OperandType.I64);

                node = nodes.AddAfter(node, new Operation(Instruction.ZeroExtend32, zex,  source));
                node = nodes.AddAfter(node, new Operation(Instruction.ConvertToFP,  dest, zex));
            }
            else /* if (source.Type == OperandType.I64) */
            {
                // For 64-bits integers, we need to do the following:
                // - Ensure that the integer has the most significant bit clear.
                // -- This can be done by shifting the value right by 1, that is, dividing by 2.
                // -- The least significant bit is lost in this case though.
                // - We can then convert the shifted value with a signed integer instruction.
                // - The result still needs to be corrected after that.
                // -- First, we need to multiply the result by 2, as we divided it by 2 before.
                // --- This can be done efficiently by adding the result to itself.
                // -- Then, we need to add the least significant bit that was shifted out.
                // --- We can convert the least significant bit to float, and add it to the result.
                Operand lsb  = Local(OperandType.I64);
                Operand half = Local(OperandType.I64);

                Operand lsbF = Local(dest.Type);

                node = nodes.AddAfter(node, new Operation(Instruction.Copy, lsb,  source));
                node = nodes.AddAfter(node, new Operation(Instruction.Copy, half, source));

                node = nodes.AddAfter(node, new Operation(Instruction.BitwiseAnd,   lsb,  lsb,  Const(1L)));
                node = nodes.AddAfter(node, new Operation(Instruction.ShiftRightUI, half, half, Const(1)));

                node = nodes.AddAfter(node, new Operation(Instruction.ConvertToFP, lsbF, lsb));
                node = nodes.AddAfter(node, new Operation(Instruction.ConvertToFP, dest, half));

                node = nodes.AddAfter(node, new Operation(Instruction.Add, dest, dest, dest));
                node = nodes.AddAfter(node, new Operation(Instruction.Add, dest, dest, lsbF));
            }

            Delete(currentNode, operation);

            return node;
        }

        private static LLNode HandleNegate(LLNode node, Operation operation)
        {
            // There's no SSE FP negate instruction, so we need to transform that into
            // a XOR of the value to be negated with a mask with the highest bit set.
            // This also produces -0 for a negation of the value 0.
            Operand dest   = operation.Destination;
            Operand source = operation.GetSource(0);

            Debug.Assert(dest.Type == OperandType.FP32 ||
                         dest.Type == OperandType.FP64, $"Invalid destination type \"{dest.Type}\".");

            LinkedList<Node> nodes = node.List;

            LLNode currentNode = node;

            Operand res = Local(dest.Type);

            node = nodes.AddAfter(node, new Operation(Instruction.VectorOne, res));

            if (dest.Type == OperandType.FP32)
            {
                node = nodes.AddAfter(node, new IntrinsicOperation(Intrinsic.X86Pslld, res, res, Const(31)));
            }
            else /* if (dest.Type == OperandType.FP64) */
            {
                node = nodes.AddAfter(node, new IntrinsicOperation(Intrinsic.X86Psllq, res, res, Const(63)));
            }

            node = nodes.AddAfter(node, new IntrinsicOperation(Intrinsic.X86Xorps, res, res, source));

            node = nodes.AddAfter(node, new Operation(Instruction.Copy, dest, res));

            Delete(currentNode, operation);

            return node;
        }

        private static LLNode HandleVectorInsert8(LLNode node, Operation operation)
        {
            // Handle vector insertion, when SSE 4.1 is not supported.
            Operand dest = operation.Destination;
            Operand src1 = operation.GetSource(0); // Vector
            Operand src2 = operation.GetSource(1); // Value
            Operand src3 = operation.GetSource(2); // Index

            Debug.Assert(src3.Kind == OperandKind.Constant);

            byte index = src3.AsByte();

            Debug.Assert(index < 16);

            LinkedList<Node> nodes = node.List;

            LLNode currentNode = node;

            Operand temp1 = Local(OperandType.I32);
            Operand temp2 = Local(OperandType.I32);

            node = nodes.AddAfter(node, new Operation(Instruction.Copy, temp2, src2));

            Operation vextOp = new Operation(Instruction.VectorExtract16, temp1, src1, Const(index >> 1));

            node = nodes.AddAfter(node, vextOp);

            if ((index & 1) != 0)
            {
                node = nodes.AddAfter(node, new Operation(Instruction.ZeroExtend8, temp1, temp1));
                node = nodes.AddAfter(node, new Operation(Instruction.ShiftLeft,   temp2, temp2, Const(8)));
                node = nodes.AddAfter(node, new Operation(Instruction.BitwiseOr,   temp1, temp1, temp2));
            }
            else
            {
                node = nodes.AddAfter(node, new Operation(Instruction.ZeroExtend8, temp2, temp2));
                node = nodes.AddAfter(node, new Operation(Instruction.BitwiseAnd,  temp1, temp1, Const(0xff00)));
                node = nodes.AddAfter(node, new Operation(Instruction.BitwiseOr,   temp1, temp1, temp2));
            }

            Operation vinsOp = new Operation(Instruction.VectorInsert16, dest, src1, temp1, Const(index >> 1));

            node = nodes.AddAfter(node, vinsOp);

            Delete(currentNode, operation);

            return node;
        }

        private static LLNode HandleCallWindowsAbi(StackAllocator stackAlloc, LLNode node, Operation operation)
        {
            Operand dest = operation.Destination;

            LinkedList<Node> nodes = node.List;

            // Handle struct arguments.
            int retArgs = 0;

            int stackAllocOffset = 0;

            int AllocateOnStack(int size)
            {
                // We assume that the stack allocator is initially empty (TotalSize = 0).
                // Taking that into account, we can reuse the space allocated for other
                // calls by keeping track of our own allocated size (stackAllocOffset).
                // If the space allocated is not big enough, then we just expand it.
                int offset = stackAllocOffset;

                if (stackAllocOffset + size > stackAlloc.TotalSize)
                {
                    stackAlloc.Allocate((stackAllocOffset + size) - stackAlloc.TotalSize);
                }

                stackAllocOffset += size;

                return offset;
            }

            Operand arg0Reg = null;

            if (dest != null && dest.Type == OperandType.V128)
            {
                int stackOffset = AllocateOnStack(dest.Type.GetSizeInBytes());

                arg0Reg = Gpr(CallingConvention.GetIntArgumentRegister(0), OperandType.I64);

                Operation allocOp = new Operation(Instruction.StackAlloc, arg0Reg, Const(stackOffset));

                nodes.AddBefore(node, allocOp);

                retArgs = 1;
            }

            int argsCount = operation.SourcesCount - 1;

            int maxArgs = CallingConvention.GetArgumentsOnRegsCount() - retArgs;

            if (argsCount > maxArgs)
            {
                argsCount = maxArgs;
            }

            Operand[] sources = new Operand[1 + retArgs + argsCount];

            sources[0] = operation.GetSource(0);

            if (arg0Reg != null)
            {
                sources[1] = arg0Reg;
            }

            for (int index = 1; index < operation.SourcesCount; index++)
            {
                Operand source = operation.GetSource(index);

                if (source.Type == OperandType.V128)
                {
                    Operand stackAddr = Local(OperandType.I64);

                    int stackOffset = AllocateOnStack(source.Type.GetSizeInBytes());

                    nodes.AddBefore(node, new Operation(Instruction.StackAlloc, stackAddr, Const(stackOffset)));

                    Operation storeOp = new Operation(Instruction.Store, null, stackAddr, source);

                    HandleConstantCopy(nodes.AddBefore(node, storeOp), storeOp);

                    operation.SetSource(index, stackAddr);
                }
            }

            // Handle arguments passed on registers.
            for (int index = 0; index < argsCount; index++)
            {
                Operand source = operation.GetSource(index + 1);

                Operand argReg;

                int argIndex = index + retArgs;

                if (source.Type.IsInteger())
                {
                    argReg = Gpr(CallingConvention.GetIntArgumentRegister(argIndex), source.Type);
                }
                else
                {
                    argReg = Xmm(CallingConvention.GetVecArgumentRegister(argIndex), source.Type);
                }

                Operation copyOp = new Operation(Instruction.Copy, argReg, source);

                HandleConstantCopy(nodes.AddBefore(node, copyOp), copyOp);

                sources[1 + retArgs + index] = argReg;
            }

            // The remaining arguments (those that are not passed on registers)
            // should be passed on the stack, we write them to the stack with "SpillArg".
            for (int index = argsCount; index < operation.SourcesCount - 1; index++)
            {
                Operand source = operation.GetSource(index + 1);

                Operand offset = new Operand((index + retArgs) * 8);

                Operation spillOp = new Operation(Instruction.SpillArg, null, offset, source);

                HandleConstantCopy(nodes.AddBefore(node, spillOp), spillOp);
            }

            if (dest != null)
            {
                if (dest.Type == OperandType.V128)
                {
                    Operand retValueAddr = Local(OperandType.I64);

                    nodes.AddBefore(node, new Operation(Instruction.Copy, retValueAddr, arg0Reg));

                    Operation loadOp = new Operation(Instruction.Load, dest, retValueAddr);

                    node = nodes.AddAfter(node, loadOp);

                    operation.Destination = null;
                }
                else
                {
                    Operand retReg = dest.Type.IsInteger()
                        ? Gpr(CallingConvention.GetIntReturnRegister(), dest.Type)
                        : Xmm(CallingConvention.GetVecReturnRegister(), dest.Type);

                    Operation copyOp = new Operation(Instruction.Copy, dest, retReg);

                    node = nodes.AddAfter(node, copyOp);

                    operation.Destination = retReg;
                }
            }

            operation.SetSources(sources);

            return node;
        }

        private static LLNode HandleCallSystemVAbi(LLNode node, Operation operation)
        {
            Operand dest = operation.Destination;

            LinkedList<Node> nodes = node.List;

            List<Operand> sources = new List<Operand>();

            sources.Add(operation.GetSource(0));

            int argsCount = operation.SourcesCount - 1;

            int intMax = CallingConvention.GetIntArgumentsOnRegsCount();
            int vecMax = CallingConvention.GetVecArgumentsOnRegsCount();

            int intCount = 0;
            int vecCount = 0;

            int stackOffset = 0;

            for (int index = 0; index < argsCount; index++)
            {
                Operand source = operation.GetSource(index + 1);

                bool passOnReg;

                if (source.Type.IsInteger())
                {
                    passOnReg = intCount < intMax;
                }
                else if (source.Type == OperandType.V128)
                {
                    passOnReg = intCount + 1 < intMax;
                }
                else
                {
                    passOnReg = vecCount < vecMax;
                }

                if (source.Type == OperandType.V128 && passOnReg)
                {
                    // V128 is a struct, we pass each half on a GPR if possible.
                    Operand argReg  = Gpr(CallingConvention.GetIntArgumentRegister(intCount++), OperandType.I64);
                    Operand argReg2 = Gpr(CallingConvention.GetIntArgumentRegister(intCount++), OperandType.I64);

                    nodes.AddBefore(node, new Operation(Instruction.VectorExtract, argReg,  source, Const(0)));
                    nodes.AddBefore(node, new Operation(Instruction.VectorExtract, argReg2, source, Const(1)));

                    continue;
                }

                if (passOnReg)
                {
                    Operand argReg = source.Type.IsInteger()
                        ? Gpr(CallingConvention.GetIntArgumentRegister(intCount++), source.Type)
                        : Xmm(CallingConvention.GetVecArgumentRegister(vecCount++), source.Type);

                    Operation copyOp = new Operation(Instruction.Copy, argReg, source);

                    HandleConstantCopy(nodes.AddBefore(node, copyOp), copyOp);

                    sources.Add(argReg);
                }
                else
                {
                    Operand offset = new Operand(stackOffset);

                    Operation spillOp = new Operation(Instruction.SpillArg, null, offset, source);

                    HandleConstantCopy(nodes.AddBefore(node, spillOp), spillOp);

                    stackOffset += source.Type.GetSizeInBytes();
                }
            }

            if (dest != null)
            {
                if (dest.Type == OperandType.V128)
                {
                    Operand retLReg = Gpr(CallingConvention.GetIntReturnRegister(),     OperandType.I64);
                    Operand retHReg = Gpr(CallingConvention.GetIntReturnRegisterHigh(), OperandType.I64);

                    node = nodes.AddAfter(node, new Operation(Instruction.VectorCreateScalar, dest, retLReg));
                    node = nodes.AddAfter(node, new Operation(Instruction.VectorInsert,       dest, dest, retHReg, Const(1)));

                    operation.Destination = null;
                }
                else
                {
                    Operand retReg = dest.Type.IsInteger()
                        ? Gpr(CallingConvention.GetIntReturnRegister(), dest.Type)
                        : Xmm(CallingConvention.GetVecReturnRegister(), dest.Type);

                    Operation copyOp = new Operation(Instruction.Copy, dest, retReg);

                    node = nodes.AddAfter(node, copyOp);

                    operation.Destination = retReg;
                }
            }

            operation.SetSources(sources.ToArray());

            return node;
        }

        private static void HandleLoadArgumentWindowsAbi(
            CompilerContext cctx,
            LLNode node,
            Operand[] preservedArgs,
            Operation operation)
        {
            Operand source = operation.GetSource(0);

            Debug.Assert(source.Kind == OperandKind.Constant, "Non-constant LoadArgument source kind.");

            int retArgs = cctx.FuncReturnType == OperandType.V128 ? 1 : 0;

            int index = source.AsInt32() + retArgs;

            if (index < CallingConvention.GetArgumentsOnRegsCount())
            {
                Operand dest = operation.Destination;

                if (preservedArgs[index] == null)
                {
                    Operand argReg, pArg;

                    if (dest.Type.IsInteger())
                    {
                        argReg = Gpr(CallingConvention.GetIntArgumentRegister(index), dest.Type);

                        pArg = Local(dest.Type);
                    }
                    else if (dest.Type == OperandType.V128)
                    {
                        argReg = Gpr(CallingConvention.GetIntArgumentRegister(index), OperandType.I64);

                        pArg = Local(OperandType.I64);
                    }
                    else
                    {
                        argReg = Xmm(CallingConvention.GetVecArgumentRegister(index), dest.Type);

                        pArg = Local(dest.Type);
                    }

                    Operation copyOp = new Operation(Instruction.Copy, pArg, argReg);

                    cctx.Cfg.Entry.Operations.AddFirst(copyOp);

                    preservedArgs[index] = pArg;
                }

                Operation argCopyOp = new Operation(dest.Type == OperandType.V128
                    ? Instruction.Load
                    : Instruction.Copy, dest, preservedArgs[index]);

                node.List.AddBefore(node, argCopyOp);

                Delete(node, operation);
            }
            else
            {
                // TODO: Pass on stack.
            }
        }

        private static void HandleLoadArgumentSystemVAbi(
            CompilerContext cctx,
            LLNode node,
            Operand[] preservedArgs,
            Operation operation)
        {
            Operand source = operation.GetSource(0);

            Debug.Assert(source.Kind == OperandKind.Constant, "Non-constant LoadArgument source kind.");

            int index = source.AsInt32();

            int intCount = 0;
            int vecCount = 0;

            for (int cIndex = 0; cIndex < index; cIndex++)
            {
                OperandType argType = cctx.FuncArgTypes[cIndex];

                if (argType.IsInteger())
                {
                    intCount++;
                }
                else if (argType == OperandType.V128)
                {
                    intCount += 2;
                }
                else
                {
                    vecCount++;
                }
            }

            bool passOnReg;

            if (source.Type.IsInteger())
            {
                passOnReg = intCount < CallingConvention.GetIntArgumentsOnRegsCount();
            }
            else if (source.Type == OperandType.V128)
            {
                passOnReg = intCount + 1 < CallingConvention.GetIntArgumentsOnRegsCount();
            }
            else
            {
                passOnReg = vecCount < CallingConvention.GetVecArgumentsOnRegsCount();
            }

            if (passOnReg)
            {
                Operand dest = operation.Destination;

                if (preservedArgs[index] == null)
                {
                    if (dest.Type == OperandType.V128)
                    {
                        // V128 is a struct, we pass each half on a GPR if possible.
                        Operand pArg = Local(OperandType.V128);

                        Operand argLReg = Gpr(CallingConvention.GetIntArgumentRegister(intCount),     OperandType.I64);
                        Operand argHReg = Gpr(CallingConvention.GetIntArgumentRegister(intCount + 1), OperandType.I64);

                        Operation copyL = new Operation(Instruction.VectorCreateScalar, pArg, argLReg);
                        Operation copyH = new Operation(Instruction.VectorInsert,       pArg, pArg, argHReg, Const(1));

                        cctx.Cfg.Entry.Operations.AddFirst(copyH);
                        cctx.Cfg.Entry.Operations.AddFirst(copyL);

                        preservedArgs[index] = pArg;
                    }
                    else
                    {
                        Operand pArg = Local(dest.Type);

                        Operand argReg = dest.Type.IsInteger()
                            ? Gpr(CallingConvention.GetIntArgumentRegister(intCount), dest.Type)
                            : Xmm(CallingConvention.GetVecArgumentRegister(vecCount), dest.Type);

                        Operation copyOp = new Operation(Instruction.Copy, pArg, argReg);

                        cctx.Cfg.Entry.Operations.AddFirst(copyOp);

                        preservedArgs[index] = pArg;
                    }
                }

                Operation argCopyOp = new Operation(Instruction.Copy, dest, preservedArgs[index]);

                node.List.AddBefore(node, argCopyOp);

                Delete(node, operation);
            }
            else
            {
                // TODO: Pass on stack.
            }
        }

        private static void HandleReturnWindowsAbi(
            CompilerContext cctx,
            LLNode node,
            Operand[] preservedArgs,
            Operation operation)
        {
            if (operation.SourcesCount == 0)
            {
                return;
            }

            Operand source = operation.GetSource(0);

            Operand retReg;

            if (source.Type.IsInteger())
            {
                retReg = Gpr(CallingConvention.GetIntReturnRegister(), source.Type);
            }
            else if (source.Type == OperandType.V128)
            {
                if (preservedArgs[0] == null)
                {
                    Operand preservedArg = Local(OperandType.I64);

                    Operand arg0 = Gpr(CallingConvention.GetIntArgumentRegister(0), OperandType.I64);

                    Operation copyOp = new Operation(Instruction.Copy, preservedArg, arg0);

                    cctx.Cfg.Entry.Operations.AddFirst(copyOp);

                    preservedArgs[0] = preservedArg;
                }

                retReg = preservedArgs[0];
            }
            else
            {
                retReg = Xmm(CallingConvention.GetVecReturnRegister(), source.Type);
            }

            if (source.Type == OperandType.V128)
            {
                Operation retStoreOp = new Operation(Instruction.Store, null, retReg, source);

                node.List.AddBefore(node, retStoreOp);
            }
            else
            {
                Operation retCopyOp = new Operation(Instruction.Copy, retReg, source);

                node.List.AddBefore(node, retCopyOp);
            }

            operation.SetSources(new Operand[0]);
        }

        private static void HandleReturnSystemVAbi(LLNode node, Operation operation)
        {
            if (operation.SourcesCount == 0)
            {
                return;
            }

            Operand source = operation.GetSource(0);

            if (source.Type == OperandType.V128)
            {
                Operand retLReg = Gpr(CallingConvention.GetIntReturnRegister(),     OperandType.I64);
                Operand retHReg = Gpr(CallingConvention.GetIntReturnRegisterHigh(), OperandType.I64);

                node.List.AddBefore(node, new Operation(Instruction.VectorExtract, retLReg, source, Const(0)));
                node.List.AddBefore(node, new Operation(Instruction.VectorExtract, retHReg, source, Const(1)));
            }
            else
            {
                Operand retReg = source.Type.IsInteger()
                    ? Gpr(CallingConvention.GetIntReturnRegister(), source.Type)
                    : Xmm(CallingConvention.GetVecReturnRegister(), source.Type);

                Operation retCopyOp = new Operation(Instruction.Copy, retReg, source);

                node.List.AddBefore(node, retCopyOp);
            }
        }

        private static Operand AddXmmCopy(LLNode node, Operand source)
        {
            Operand temp = Local(source.Type);

            Operand intConst = AddCopy(node, GetIntConst(source));

            Operation copyOp = new Operation(Instruction.VectorCreateScalar, temp, intConst);

            node.List.AddBefore(node, copyOp);

            return temp;
        }

        private static Operand AddCopy(LLNode node, Operand source)
        {
            Operand temp = Local(source.Type);

            Operation copyOp = new Operation(Instruction.Copy, temp, source);

            node.List.AddBefore(node, copyOp);

            return temp;
        }

        private static Operand GetIntConst(Operand value)
        {
            if (value.Type == OperandType.FP32)
            {
                return Const(value.AsInt32());
            }
            else if (value.Type == OperandType.FP64)
            {
                return Const(value.AsInt64());
            }

            return value;
        }

        private static bool IsLongConst(Operand operand)
        {
            long value = operand.Type == OperandType.I32
                ? operand.AsInt32()
                : operand.AsInt64();

            return !ConstFitsOnS32(value);
        }

        private static bool ConstFitsOnS32(long value)
        {
            return value == (int)value;
        }

        private static void Delete(LLNode node, Operation operation)
        {
            operation.Destination = null;

            for (int index = 0; index < operation.SourcesCount; index++)
            {
                operation.SetSource(index, null);
            }

            node.List.Remove(node);
        }

        private static Operand Gpr(X86Register register, OperandType type)
        {
            return Register((int)register, RegisterType.Integer, type);
        }

        private static Operand Xmm(X86Register register, OperandType type)
        {
            return Register((int)register, RegisterType.Vector, type);
        }

        private static bool IsSameOperandDestSrc1(Operation operation)
        {
            switch (operation.Instruction)
            {
                case Instruction.Add:
                case Instruction.Multiply:
                case Instruction.Subtract:
                    return !HardwareCapabilities.SupportsVexEncoding || operation.Destination.Type.IsInteger();

                case Instruction.BitwiseAnd:
                case Instruction.BitwiseExclusiveOr:
                case Instruction.BitwiseNot:
                case Instruction.BitwiseOr:
                case Instruction.ByteSwap:
                case Instruction.Negate:
                case Instruction.RotateRight:
                case Instruction.ShiftLeft:
                case Instruction.ShiftRightSI:
                case Instruction.ShiftRightUI:
                    return true;

                case Instruction.Divide:
                    return !HardwareCapabilities.SupportsVexEncoding && !operation.Destination.Type.IsInteger();

                case Instruction.VectorInsert:
                case Instruction.VectorInsert16:
                case Instruction.VectorInsert8:
                    return !HardwareCapabilities.SupportsVexEncoding;
            }

            return IsVexSameOperandDestSrc1(operation);
        }

        private static bool IsVexSameOperandDestSrc1(Operation operation)
        {
            if (IsIntrinsic(operation.Instruction))
            {
                bool isUnary = operation.SourcesCount < 2;

                bool hasVecDest = operation.Destination != null && operation.Destination.Type == OperandType.V128;

                return !HardwareCapabilities.SupportsVexEncoding && !isUnary && hasVecDest;
            }

            return false;
        }

        private static bool HasConstSrc1(Instruction inst)
        {
            switch (inst)
            {
                case Instruction.Copy:
                case Instruction.LoadArgument:
                case Instruction.Spill:
                case Instruction.SpillArg:
                    return true;
            }

            return false;
        }

        private static bool HasConstSrc2(Instruction inst)
        {
            switch (inst)
            {
                case Instruction.Add:
                case Instruction.BitwiseAnd:
                case Instruction.BitwiseExclusiveOr:
                case Instruction.BitwiseOr:
                case Instruction.CompareEqual:
                case Instruction.CompareGreater:
                case Instruction.CompareGreaterOrEqual:
                case Instruction.CompareGreaterOrEqualUI:
                case Instruction.CompareGreaterUI:
                case Instruction.CompareLess:
                case Instruction.CompareLessOrEqual:
                case Instruction.CompareLessOrEqualUI:
                case Instruction.CompareLessUI:
                case Instruction.CompareNotEqual:
                case Instruction.Multiply:
                case Instruction.RotateRight:
                case Instruction.ShiftLeft:
                case Instruction.ShiftRightSI:
                case Instruction.ShiftRightUI:
                case Instruction.Subtract:
                case Instruction.VectorExtract:
                case Instruction.VectorExtract16:
                case Instruction.VectorExtract8:
                    return true;
            }

            return false;
        }

        private static bool IsCommutative(Instruction inst)
        {
            switch (inst)
            {
                case Instruction.Add:
                case Instruction.BitwiseAnd:
                case Instruction.BitwiseExclusiveOr:
                case Instruction.BitwiseOr:
                case Instruction.CompareEqual:
                case Instruction.CompareNotEqual:
                case Instruction.Multiply:
                    return true;
            }

            return false;
        }

        private static bool IsIntrinsic(Instruction inst)
        {
            return inst == Instruction.Extended;
        }
    }
}