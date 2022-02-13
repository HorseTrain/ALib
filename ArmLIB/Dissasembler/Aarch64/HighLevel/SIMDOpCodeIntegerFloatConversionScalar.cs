using ArmLIB.Dissasembler.Aarch64.LowLevel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class SIMDOpCodeIntegerFloatConversionScalar : OpCodeSIMD, IOpCodeRn
    {
        public OpCodeSize DesSize       { get; set; }
        public OpCodeSize SourceSize    { get; set; }

        public int Rn                       { get; set; }

        public int FixedShift               { get; set; }

        public SIMDInstructionMode Mode     { get; set; }

        public static SIMDOpCodeIntegerFloatConversionScalar CreateCVTF(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeIntegerFloatConversionScalar(lowLevelAOpCode, Address, Name, SIMDInstructionMode.IntToFloat);
        public static SIMDOpCodeIntegerFloatConversionScalar CreateCVTFV(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeIntegerFloatConversionScalar(lowLevelAOpCode, Address, Name, SIMDInstructionMode.IntToFloatVector);
        public static SIMDOpCodeIntegerFloatConversionScalar CreateFCVT(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeIntegerFloatConversionScalar(lowLevelAOpCode, Address, Name, SIMDInstructionMode.FloatToInt);
        public static SIMDOpCodeIntegerFloatConversionScalar CreateFCVTF(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name) => new SIMDOpCodeIntegerFloatConversionScalar(lowLevelAOpCode, Address, Name, SIMDInstructionMode.FloatToFixed);

        public SIMDOpCodeIntegerFloatConversionScalar(LowLevelAOpCode lowLevelAOpCode, long Address, Mnemonic Name, SIMDInstructionMode Mode) : base(lowLevelAOpCode, Address, Name)
        {
            Rn = lowLevelAOpCode.Rn;

            this.Mode = Mode;

            if (Mode == SIMDInstructionMode.IntToFloatVector)
            {
                DesSize = (OpCodeSize)(((lowLevelAOpCode.RawInstruction >> 22)& 1) + 2);
                SourceSize = DesSize;
            }
            else if (Mode == SIMDInstructionMode.IntToFloat)
            {
                SourceSize = DecodingHelpers.GetIntALUSize(lowLevelAOpCode.sf);
                DesSize = (OpCodeSize)(lowLevelAOpCode.ptype + 2);
            }
            else if (Mode == SIMDInstructionMode.FloatToInt)
            {
                SourceSize = (OpCodeSize)(lowLevelAOpCode.ptype + 2); 
                DesSize = DecodingHelpers.GetIntALUSize(lowLevelAOpCode.sf);
            }
            else if (Mode == SIMDInstructionMode.FloatToFixed)
            {
                SourceSize = (OpCodeSize)(lowLevelAOpCode.ptype + 2);
                DesSize = DecodingHelpers.GetIntALUSize(lowLevelAOpCode.sf);

                FixedShift = 64 - ((lowLevelAOpCode.RawInstruction >> 10) & 0x3F);
            }
            else
            {
                throw new Exception();
            }
        }

        public override string ToString()
        {
            if (Mode == SIMDInstructionMode.IntToFloatVector)
            {
                return $"{Name} {LoggerTools.GetRegister(DesSize, Rd, false, true)}, {LoggerTools.GetRegister(SourceSize, Rn, false, true)}";
            }
            else if (Mode == SIMDInstructionMode.FloatToInt)
            {
                return $"{Name} {LoggerTools.GetRegister(DesSize, Rd, false, true)}, {LoggerTools.GetRegister(SourceSize, Rn)}";
            }
            else if (Mode == SIMDInstructionMode.IntToFloat)
            {
                return $"{Name} {LoggerTools.GetRegister(DesSize, Rd)}, {LoggerTools.GetRegister(SourceSize, Rn, false, true)}";
            }
            else if (Mode == SIMDInstructionMode.FloatToFixed)
            {
                return $"{Name} {LoggerTools.GetRegister(DesSize, Rd)}, {LoggerTools.GetRegister(SourceSize, Rn, false, true)}, {LoggerTools.GetImm(FixedShift)}";
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
