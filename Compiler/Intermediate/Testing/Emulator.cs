using SharpDisasm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Intermediate.Testing
{
    public static class Emulator
    {
        public static ulong Test(OperationBlock Source, ulong[] Data)
        {
            int i = 0;

            while (true)
            {
                bool advance = true;

                Operation operation = Source.RawOperations[i];

                ulong GetSource(int index)
                {
                    IOperand operand = operation.Sources[index];

                    switch (operand)
                    {
                        case IntReg op: return Data[op.Reg];
                        case ConstOperand c: return c.Data;
                        default: throw new Exception();
                    }
                }

                int GetDes(int index)
                {
                    return (operation.Destinations[index] as IOperandReg).Reg;
                }

                switch (operation.Type)
                {
                    case InstructionType.Normal:
                        {
                            switch ((Instruction)operation.Instruction)
                            {
                                case Instruction.Add: Data[GetDes(0)] = GetSource(0) + GetSource(1); break;
                                case Instruction.LogicalAnd: Data[GetDes(0)] = GetSource(0) & GetSource(1); break;
                                case Instruction.Subtract: Data[GetDes(0)] = GetSource(0) - GetSource(1); break;
                                case Instruction.Multiply: Data[GetDes(0)] = GetSource(0) * GetSource(1); break;

                                case Instruction.LogicalShiftLeft: Data[GetDes(0)] = GetSource(0) << ((int)GetSource(1) & 255); break;
                                case Instruction.LogicalShiftRight: Data[GetDes(0)] = GetSource(0) >> ((int)GetSource(1) & 255); break;
                                case Instruction.LogicalShiftRightSigned: Data[GetDes(0)] = (ulong)((long)GetSource(0) >> ((int)GetSource(1) & 255)); break;

                                case Instruction.Return: return 0;

                                default: throw new Exception();
                            }


                            break;
                        }

                    default: throw new Exception();
                }

                if (advance)
                    ++i;
            }

            throw new Exception();
        }

        public static ulong TestWithRegisterAllocator(OperationBlock Source, ulong[] Context, int RegisterCount)
        {
            int i = 0;

            ulong[] Cpu = new ulong[RegisterCount];

            while (true)
            {
                bool advance = true;

                Operation operation = Source.RawOperations[i];

                ulong GetSource(int index)
                {
                    IOperand operand = operation.Sources[index];

                    switch (operand)
                    {
                        case IntReg op: return Cpu[op.Reg];
                        case ConstOperand c: return c.Data;
                        default: throw new Exception();
                    }
                }

                int GetDes(int index)
                {
                    return (operation.Destinations[index] as IOperandReg).Reg;
                }

                switch (operation.Type)
                {
                    case InstructionType.Normal:
                        {
                            switch ((Instruction)operation.Instruction)
                            {
                                case Instruction.Add: Cpu[GetDes(0)] = GetSource(0) + GetSource(1); break;
                                case Instruction.Return: return GetSource(0);

                                case Instruction.AllocateRegister:
                                    {
                                        int dreg = (operation.Sources[0] as IOperandReg).Reg;
                                        int ctx = (int)(operation.Sources[1] as ConstOperand).Data;
                                        int type = (int)(operation.Sources[2] as ConstOperand).Data;

                                        if (type == 1)
                                        {
                                            Cpu[dreg] = Context[ctx];
                                        }
                                        else
                                        {
                                            Context[ctx] = Cpu[dreg];
                                        }

                                    }; break;

                                default: throw new Exception();
                            }


                            break;
                        }

                    default: throw new Exception();
                }

                if (advance)
                    ++i;
            }

            throw new Exception();
        }
    }
}
