using ArmLIB.Dissasembler.Aarch64.LowLevel;
using ArmLIB.Emulator.Aarch64.Translation;
using Gee.External.Capstone.Arm64;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public class MiddleMan
    {
        public Mnemonic HighLevelName       { get; set; }
        public LowLevelClassNames ClassName { get; set; }
        public LowLevelNames LowLevelName   { get; set; }
        public OpCodeCreate CreateOP        { get; set; }
        public InstEmit Emit                { get; set; }

        public MiddleMan(Mnemonic HighLevelName, LowLevelClassNames ClassName, LowLevelNames InstructionName, InstEmit Emit, OpCodeCreate CreateOP)
        {
            this.HighLevelName = HighLevelName;
            this.ClassName = ClassName;
            this.LowLevelName = InstructionName;
            this.CreateOP = CreateOP;
            this.Emit = Emit;
        }

        static MiddleMan()
        {
            HashSet<(int, int)> Keys = new HashSet<(int, int)>();

            foreach (MiddleMan man in MiddleMen)
            {
                (int, int) key = new ((int)man.ClassName, (int)man.LowLevelName);

                if (Keys.Contains(key))
                {
                    Console.WriteLine($"Invalid Combination {(LowLevelClassNames)key.Item1} {(LowLevelNames)key.Item2}");

                    throw new Exception();
                }

                Keys.Add(key);
            }
        }

        public static List<MiddleMan> MiddleMen = new List<MiddleMan>()
        {
            //Aarch 64
            Create(Mnemonic.adc,        LowLevelClassNames.Add_subtract_with_carry,                             LowLevelNames.ADC,                          InstEmit64.Adc,                         OpCodeALU2Src.Create),
            Create(Mnemonic.adcs,       LowLevelClassNames.Add_subtract_with_carry,                             LowLevelNames.ADCS,                         InstEmit64.Adcs,                        OpCodeALU2Src.Create),
            Create(Mnemonic.add,        LowLevelClassNames.Add_subtract_extended_register,                      LowLevelNames.ADD_extended_register,        InstEmit64.Add,                         OpCodeALUExtend.Create),
            Create(Mnemonic.add,        LowLevelClassNames.Add_subtract_immediate,                              LowLevelNames.ADD_immediate,                InstEmit64.Add,                         OpCodeALUImm.Create),
            Create(Mnemonic.add,        LowLevelClassNames.Add_subtract_shifted_register,                       LowLevelNames.ADD_shifted_register,         InstEmit64.Add,                         OpCodeALUShiftedReg.Create),
            Create(Mnemonic.adds,       LowLevelClassNames.Add_subtract_extended_register,                      LowLevelNames.ADDS_extended_register,       InstEmit64.Adds,                        OpCodeALUExtend.Create),
            Create(Mnemonic.adds,       LowLevelClassNames.Add_subtract_immediate,                              LowLevelNames.ADDS_immediate,               InstEmit64.Adds,                        OpCodeALUImm.Create),
            Create(Mnemonic.adds,       LowLevelClassNames.Add_subtract_shifted_register,                       LowLevelNames.ADDS_shifted_register,        InstEmit64.Adds,                        OpCodeALUShiftedReg.Create),
            Create(Mnemonic.adr,        LowLevelClassNames.PC_rel__addressing,                                  LowLevelNames.ADR,                          InstEmit64.Adr,                         OpCodePcRelAddressing.Create),
            Create(Mnemonic.adrp,       LowLevelClassNames.PC_rel__addressing,                                  LowLevelNames.ADRP,                         InstEmit64.Adrp,                        OpCodePcRelAddressing.Create),
            Create(Mnemonic.and,        LowLevelClassNames.Logical_immediate,                                   LowLevelNames.AND_immediate,                InstEmit64.And,                         OpCodeALUImm.Create),
            Create(Mnemonic.and,        LowLevelClassNames.Logical_shifted_register,                            LowLevelNames.AND_shifted_register,         InstEmit64.And,                         OpCodeALUShiftedReg.Create),
            Create(Mnemonic.ands,       LowLevelClassNames.Logical_immediate,                                   LowLevelNames.ANDS_immediate,               InstEmit64.Ands,                        OpCodeALUImm.Create),
            Create(Mnemonic.ands,       LowLevelClassNames.Logical_shifted_register,                            LowLevelNames.ANDS_shifted_register,        InstEmit64.Ands,                        OpCodeALUShiftedReg.Create),
            Create(Mnemonic.asrv,       LowLevelClassNames.Data_processing_2_source,                            LowLevelNames.ASRV,                         InstEmit64.Asrv,                        OpCodeALU2Src.Create),
            Create(Mnemonic.b,          LowLevelClassNames.Conditional_branch_immediate,                        LowLevelNames.B_cond,                       InstEmit64.B_Cond,                      OpCodeConditionalBranchImm.Create),
            Create(Mnemonic.b,          LowLevelClassNames.Unconditional_branch_immediate,                      LowLevelNames.B,                            InstEmit64.B_Uncond,                    OpCodeUnconditionalBranchImm.Create),
            Create(Mnemonic.bfm,        LowLevelClassNames.Bitfield,                                            LowLevelNames.BFM,                          InstEmit64.Bfm,                         OpCodeBitfield.Create),
            Create(Mnemonic.bic,        LowLevelClassNames.Logical_shifted_register,                            LowLevelNames.BIC_shifted_register,         InstEmit64.Bic,                         OpCodeALUShiftedReg.Create),
            Create(Mnemonic.bics,       LowLevelClassNames.Logical_shifted_register,                            LowLevelNames.BICS_shifted_register,        InstEmit64.Bics,                        OpCodeALUShiftedReg.Create),
            Create(Mnemonic.bl,         LowLevelClassNames.Unconditional_branch_immediate,                      LowLevelNames.BL,                           InstEmit64.BL_Uncond,                   OpCodeUnconditionalBranchImm.Create),
            Create(Mnemonic.blr,        LowLevelClassNames.Unconditional_branch_register,                       LowLevelNames.BLR,                          InstEmit64.Blr,                         OpCodeUnconditionalBranchRegister.Create),
            Create(Mnemonic.br,         LowLevelClassNames.Unconditional_branch_register,                       LowLevelNames.BR,                           InstEmit64.Br,                          OpCodeUnconditionalBranchRegister.Create),
            Create(Mnemonic.cbnz,       LowLevelClassNames.Compare_and_branch_immediate,                        LowLevelNames.CBNZ,                         InstEmit64.Cbnz,                        OpCodeCompareAndBranchImm.Create),
            Create(Mnemonic.cbz,        LowLevelClassNames.Compare_and_branch_immediate,                        LowLevelNames.CBZ,                          InstEmit64.Cbz,                         OpCodeCompareAndBranchImm.Create),
            Create(Mnemonic.ccmn,       LowLevelClassNames.Conditional_compare_immediate,                       LowLevelNames.CCMN_immediate,               InstEmit64.Ccmn,                        OpCodeConditionalCompare.Create),
            Create(Mnemonic.ccmn,       LowLevelClassNames.Conditional_compare_register,                        LowLevelNames.CCMN_register,                InstEmit64.Ccmn,                        OpCodeConditionalCompare.Create),
            Create(Mnemonic.ccmp,       LowLevelClassNames.Conditional_compare_immediate,                       LowLevelNames.CCMP_immediate,               InstEmit64.Ccmp,                        OpCodeConditionalCompare.Create),
            Create(Mnemonic.ccmp,       LowLevelClassNames.Conditional_compare_register,                        LowLevelNames.CCMP_register,                InstEmit64.Ccmp,                        OpCodeConditionalCompare.Create),
            Create(Mnemonic.clrex,      LowLevelClassNames.Barriers,                                            LowLevelNames.CLREX,                        InstEmit64.Clrex,                       OpCodeRedundant.Create),
            Create(Mnemonic.cls,        LowLevelClassNames.Data_processing_1_source,                            LowLevelNames.CLS,                          InstEmit64.Cls,                         OpCodeALU1Src.Create),
            Create(Mnemonic.clz,        LowLevelClassNames.Data_processing_1_source,                            LowLevelNames.CLZ,                          InstEmit64.Clz,                         OpCodeALU1Src.Create),
            Create(Mnemonic.csel,       LowLevelClassNames.Conditional_select,                                  LowLevelNames.CSEL,                         InstEmit64.Csel,                        OpCodeConditionalSelect.Create),
            Create(Mnemonic.csinc,      LowLevelClassNames.Conditional_select,                                  LowLevelNames.CSINC,                        InstEmit64.Csinc,                       OpCodeConditionalSelect.Create),
            Create(Mnemonic.csinv,      LowLevelClassNames.Conditional_select,                                  LowLevelNames.CSINV,                        InstEmit64.Csinv,                       OpCodeConditionalSelect.Create),
            Create(Mnemonic.csneg,      LowLevelClassNames.Conditional_select,                                  LowLevelNames.CSNEG,                        InstEmit64.Csneg,                       OpCodeConditionalSelect.Create),
            Create(Mnemonic.dc,         LowLevelClassNames.System_instructions,                                 LowLevelNames.SYS,                          InstEmit64.DoNothing,                   OpCodeRedundant.Create),
            Create(Mnemonic.dmb,        LowLevelClassNames.Barriers,                                            LowLevelNames.DMB,                          InstEmit64.DoNothing,                   OpCodeRedundant.Create),
            Create(Mnemonic.dsb,        LowLevelClassNames.Barriers,                                            LowLevelNames.DSB,                          InstEmit64.DoNothing,                   OpCodeRedundant.Create),
            Create(Mnemonic.eon,        LowLevelClassNames.Logical_shifted_register,                            LowLevelNames.EON_shifted_register,         InstEmit64.Eon,                         OpCodeALUShiftedReg.Create),
            Create(Mnemonic.eor,        LowLevelClassNames.Logical_immediate,                                   LowLevelNames.EOR_immediate,                InstEmit64.Eor,                         OpCodeALUImm.Create),
            Create(Mnemonic.eor,        LowLevelClassNames.Logical_shifted_register,                            LowLevelNames.EOR_shifted_register,         InstEmit64.Eor,                         OpCodeALUShiftedReg.Create),
            Create(Mnemonic.extr,       LowLevelClassNames.Extract,                                             LowLevelNames.EXTR,                         InstEmit64.Extr,                        OpCodeExtract.Create),
            Create(Mnemonic.ldar,       LowLevelClassNames.Load_store_ordered,                                  LowLevelNames.LDAR,                         InstEmit64.Lda,                         OpCodeMemoryExclusive.Create),
            Create(Mnemonic.ldarb,      LowLevelClassNames.Load_store_ordered,                                  LowLevelNames.LDARB,                        InstEmit64.Lda,                         OpCodeMemoryExclusive.Create),
            Create(Mnemonic.ldarh,      LowLevelClassNames.Load_store_ordered,                                  LowLevelNames.LDARH,                        InstEmit64.Lda,                         OpCodeMemoryExclusive.Create),
            Create(Mnemonic.ldaxr,      LowLevelClassNames.Load_store_exclusive_register,                       LowLevelNames.LDAXR,                        InstEmit64.Ldx,                         OpCodeMemoryExclusive.Create),
            Create(Mnemonic.ldaxrb,     LowLevelClassNames.Load_store_exclusive_register,                       LowLevelNames.LDAXRB,                       InstEmit64.Ldx,                         OpCodeMemoryExclusive.Create),
            Create(Mnemonic.ldaxrh,     LowLevelClassNames.Load_store_exclusive_register,                       LowLevelNames.LDAXRH,                       InstEmit64.Ldx,                         OpCodeMemoryExclusive.Create),
            Create(Mnemonic.ldp,        LowLevelClassNames.Load_store_register_pair_offset,                     LowLevelNames.LDP,                          InstEmit64.Ldp,                         OpCodeMemoryPair.Create),
            Create(Mnemonic.ldp,        LowLevelClassNames.Load_store_register_pair_post_indexed,               LowLevelNames.LDP,                          InstEmit64.Ldp,                         OpCodeMemoryPair.Create),
            Create(Mnemonic.ldp,        LowLevelClassNames.Load_store_register_pair_pre_indexed,                LowLevelNames.LDP,                          InstEmit64.Ldp,                         OpCodeMemoryPair.Create),
            Create(Mnemonic.ldpsw,      LowLevelClassNames.Load_store_register_pair_offset,                     LowLevelNames.LDPSW,                        InstEmit64.Ldp,                         OpCodeMemoryPair.Create),
            Create(Mnemonic.ldpsw,      LowLevelClassNames.Load_store_register_pair_post_indexed,               LowLevelNames.LDPSW,                        InstEmit64.Ldp,                         OpCodeMemoryPair.Create),
            Create(Mnemonic.ldpsw,      LowLevelClassNames.Load_store_register_pair_pre_indexed,                LowLevelNames.LDPSW,                        InstEmit64.Ldp,                         OpCodeMemoryPair.Create),
            Create(Mnemonic.ldr,        LowLevelClassNames.Load_store_register_immediate_post_indexed,          LowLevelNames.LDR_immediate,                InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldr,        LowLevelClassNames.Load_store_register_immediate_pre_indexed,           LowLevelNames.LDR_immediate,                InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldr,        LowLevelClassNames.Load_store_register_register_offset,                 LowLevelNames.LDR_register,                 InstEmit64.Ldr,                         OpCodeMemoryReg.Create),
            Create(Mnemonic.ldr,        LowLevelClassNames.Load_store_register_unsigned_immediate,              LowLevelNames.LDR_immediate,                InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldrb,       LowLevelClassNames.Load_store_register_immediate_post_indexed,          LowLevelNames.LDRB_immediate,               InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldrb,       LowLevelClassNames.Load_store_register_immediate_pre_indexed,           LowLevelNames.LDRB_immediate,               InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldrb,       LowLevelClassNames.Load_store_register_register_offset,                 LowLevelNames.LDRB_register,                InstEmit64.Ldr,                         OpCodeMemoryReg.Create),
            Create(Mnemonic.ldrb,       LowLevelClassNames.Load_store_register_unsigned_immediate,              LowLevelNames.LDRB_immediate,               InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldrh,       LowLevelClassNames.Load_store_register_immediate_post_indexed,          LowLevelNames.LDRH_immediate,               InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldrh,       LowLevelClassNames.Load_store_register_immediate_pre_indexed,           LowLevelNames.LDRH_immediate,               InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldrh,       LowLevelClassNames.Load_store_register_register_offset,                 LowLevelNames.LDRH_register,                InstEmit64.Ldr,                         OpCodeMemoryReg.Create),
            Create(Mnemonic.ldrh,       LowLevelClassNames.Load_store_register_unsigned_immediate,              LowLevelNames.LDRH_immediate,               InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldrsb,      LowLevelClassNames.Load_store_register_immediate_post_indexed,          LowLevelNames.LDRSB_immediate,              InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldrsb,      LowLevelClassNames.Load_store_register_immediate_pre_indexed,           LowLevelNames.LDRSB_immediate,              InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldrsb,      LowLevelClassNames.Load_store_register_register_offset,                 LowLevelNames.LDRSB_register,               InstEmit64.Ldr,                         OpCodeMemoryReg.Create),
            Create(Mnemonic.ldrsb,      LowLevelClassNames.Load_store_register_unsigned_immediate,              LowLevelNames.LDRSB_immediate,              InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldrsh,      LowLevelClassNames.Load_store_register_immediate_post_indexed,          LowLevelNames.LDRSH_immediate,              InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldrsh,      LowLevelClassNames.Load_store_register_immediate_pre_indexed,           LowLevelNames.LDRSH_immediate,              InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldrsh,      LowLevelClassNames.Load_store_register_register_offset,                 LowLevelNames.LDRSH_register,               InstEmit64.Ldr,                         OpCodeMemoryReg.Create),
            Create(Mnemonic.ldrsh,      LowLevelClassNames.Load_store_register_unsigned_immediate,              LowLevelNames.LDRSH_immediate,              InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldrsw,      LowLevelClassNames.Load_store_register_immediate_post_indexed,          LowLevelNames.LDRSW_immediate,              InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldrsw,      LowLevelClassNames.Load_store_register_immediate_pre_indexed,           LowLevelNames.LDRSW_immediate,              InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldrsw,      LowLevelClassNames.Load_store_register_register_offset,                 LowLevelNames.LDRSW_register,               InstEmit64.Ldr,                         OpCodeMemoryReg.Create),
            Create(Mnemonic.ldrsw,      LowLevelClassNames.Load_store_register_unsigned_immediate,              LowLevelNames.LDRSW_immediate,              InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldur,       LowLevelClassNames.Load_store_register_unscaled_immediate,              LowLevelNames.LDUR,                         InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldurb,      LowLevelClassNames.Load_store_register_unscaled_immediate,              LowLevelNames.LDURB,                        InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldurh,      LowLevelClassNames.Load_store_register_unscaled_immediate,              LowLevelNames.LDURH,                        InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldursb,     LowLevelClassNames.Load_store_register_unscaled_immediate,              LowLevelNames.LDURSB,                       InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldursh,     LowLevelClassNames.Load_store_register_unscaled_immediate,              LowLevelNames.LDURSH,                       InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldursw,     LowLevelClassNames.Load_store_register_unscaled_immediate,              LowLevelNames.LDURSW,                       InstEmit64.Ldr,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.ldxr,       LowLevelClassNames.Load_store_exclusive_register,                       LowLevelNames.LDXR,                         InstEmit64.Ldx,                         OpCodeMemoryExclusive.Create),
            Create(Mnemonic.ldxrb,      LowLevelClassNames.Load_store_exclusive_register,                       LowLevelNames.LDXRB,                        InstEmit64.Ldx,                         OpCodeMemoryExclusive.Create),
            Create(Mnemonic.ldxrh,      LowLevelClassNames.Load_store_exclusive_register,                       LowLevelNames.LDXRH,                        InstEmit64.Ldx,                         OpCodeMemoryExclusive.Create),
            Create(Mnemonic.lslv,       LowLevelClassNames.Data_processing_2_source,                            LowLevelNames.LSLV,                         InstEmit64.Lslv,                        OpCodeALU2Src.Create),
            Create(Mnemonic.lsrv,       LowLevelClassNames.Data_processing_2_source,                            LowLevelNames.LSRV,                         InstEmit64.Lsrv,                        OpCodeALU2Src.Create),
            Create(Mnemonic.madd,       LowLevelClassNames.Data_processing_3_source,                            LowLevelNames.MADD,                         InstEmit64.Madd,                        OpCodeALU3Src.Create),
            Create(Mnemonic.movk,       LowLevelClassNames.Move_wide_immediate,                                 LowLevelNames.MOVK,                         InstEmit64.Movk,                        OpCodeMoveWide.Create),
            Create(Mnemonic.movn,       LowLevelClassNames.Move_wide_immediate,                                 LowLevelNames.MOVN,                         InstEmit64.Movn,                        OpCodeMoveWide.Create),
            Create(Mnemonic.movz,       LowLevelClassNames.Move_wide_immediate,                                 LowLevelNames.MOVZ,                         InstEmit64.Movz,                        OpCodeMoveWide.Create),
            Create(Mnemonic.mrs,        LowLevelClassNames.System_register_move,                                LowLevelNames.MRS,                          InstEmit64.Mrs,                         OpCodeSys.Create),
            Create(Mnemonic.msr,        LowLevelClassNames.System_register_move,                                LowLevelNames.MSR_register,                 InstEmit64.Msr,                         OpCodeSys.Create),
            Create(Mnemonic.msub,       LowLevelClassNames.Data_processing_3_source,                            LowLevelNames.MSUB,                         InstEmit64.Msub,                        OpCodeALU3Src.Create),
            Create(Mnemonic.nop,        LowLevelClassNames.Hints,                                               LowLevelNames.HINT,                         InstEmit64.DoNothing,                   OpCodeRedundant.Create),
            Create(Mnemonic.orn,        LowLevelClassNames.Logical_shifted_register,                            LowLevelNames.ORN_shifted_register,         InstEmit64.Orn,                         OpCodeALUShiftedReg.Create),
            Create(Mnemonic.orr,        LowLevelClassNames.Logical_immediate,                                   LowLevelNames.ORR_immediate,                InstEmit64.Orr,                         OpCodeALUImm.Create),
            Create(Mnemonic.orr,        LowLevelClassNames.Logical_shifted_register,                            LowLevelNames.ORR_shifted_register,         InstEmit64.Orr,                         OpCodeALUShiftedReg.Create),
            Create(Mnemonic.prfm,       LowLevelClassNames.Load_store_register_unsigned_immediate,              LowLevelNames.PRFM_immediate,               InstEmit64.DoNothing,                   OpCodeRedundant.Create),
            Create(Mnemonic.rbit,       LowLevelClassNames.Data_processing_1_source,                            LowLevelNames.RBIT,                         InstEmit64.Rbit,                        OpCodeALU1Src.Create),
            Create(Mnemonic.ret,        LowLevelClassNames.Unconditional_branch_register,                       LowLevelNames.RET,                          InstEmit64.Ret,                         OpCodeUnconditionalBranchRegister.Create),
            Create(Mnemonic.rev,        LowLevelClassNames.Data_processing_1_source,                            LowLevelNames.REV,                          InstEmit64.Rev,                         OpCodeALU1Src.Create),
            Create(Mnemonic.rev16,      LowLevelClassNames.Data_processing_1_source,                            LowLevelNames.REV16,                        InstEmit64.Rev16,                       OpCodeALU1Src.Create),
            Create(Mnemonic.rev32,      LowLevelClassNames.Data_processing_1_source,                            LowLevelNames.REV32,                        InstEmit64.Rev32,                       OpCodeALU1Src.Create),
            Create(Mnemonic.rorv,       LowLevelClassNames.Data_processing_2_source,                            LowLevelNames.RORV,                         InstEmit64.Rorv,                        OpCodeALU2Src.Create),
            Create(Mnemonic.sbc,        LowLevelClassNames.Add_subtract_with_carry,                             LowLevelNames.SBC,                          InstEmit64.Sbc,                         OpCodeALU2Src.Create),
            Create(Mnemonic.sbcs,       LowLevelClassNames.Add_subtract_with_carry,                             LowLevelNames.SBCS,                         InstEmit64.Sbcs,                        OpCodeALU2Src.Create),
            Create(Mnemonic.sbfm,       LowLevelClassNames.Bitfield,                                            LowLevelNames.SBFM,                         InstEmit64.Sbfm,                        OpCodeBitfield.Create),
            Create(Mnemonic.sdiv,       LowLevelClassNames.Data_processing_2_source,                            LowLevelNames.SDIV,                         InstEmit64.Sdiv,                        OpCodeALU2Src.Create),
            Create(Mnemonic.smaddl,     LowLevelClassNames.Data_processing_3_source,                            LowLevelNames.SMADDL,                       InstEmit64.Smaddl,                      OpCodeALU3Src.Create),
            Create(Mnemonic.smsubl,     LowLevelClassNames.Data_processing_3_source,                            LowLevelNames.SMSUBL,                       InstEmit64.Smsubl,                      OpCodeALU3Src.Create),
            Create(Mnemonic.smulh,      LowLevelClassNames.Data_processing_3_source,                            LowLevelNames.SMULH,                        InstEmit64.Smulh,                       OpCodeALU2Src.Create), //eh?
            Create(Mnemonic.stlr,       LowLevelClassNames.Load_store_ordered,                                  LowLevelNames.STLR,                         InstEmit64.Stl,                         OpCodeMemoryExclusive.Create),
            Create(Mnemonic.stlrb,      LowLevelClassNames.Load_store_ordered,                                  LowLevelNames.STLRB,                        InstEmit64.Stl,                         OpCodeMemoryExclusive.Create),
            Create(Mnemonic.stlrh,      LowLevelClassNames.Load_store_ordered,                                  LowLevelNames.STLRH,                        InstEmit64.Stl,                         OpCodeMemoryExclusive.Create),
            Create(Mnemonic.stlxr,      LowLevelClassNames.Load_store_exclusive_register,                       LowLevelNames.STLXR,                        InstEmit64.Stx,                         OpCodeMemoryExclusive.Create),
            Create(Mnemonic.stlxrb,     LowLevelClassNames.Load_store_exclusive_register,                       LowLevelNames.STLXRB,                       InstEmit64.Stx,                         OpCodeMemoryExclusive.Create),
            Create(Mnemonic.stlxrh,     LowLevelClassNames.Load_store_exclusive_register,                       LowLevelNames.STLXRH,                       InstEmit64.Stx,                         OpCodeMemoryExclusive.Create),
            Create(Mnemonic.stp,        LowLevelClassNames.Load_store_register_pair_offset,                     LowLevelNames.STP,                          InstEmit64.Stp,                         OpCodeMemoryPair.Create),
            Create(Mnemonic.stp,        LowLevelClassNames.Load_store_register_pair_post_indexed,               LowLevelNames.STP,                          InstEmit64.Stp,                         OpCodeMemoryPair.Create),
            Create(Mnemonic.stp,        LowLevelClassNames.Load_store_register_pair_pre_indexed,                LowLevelNames.STP,                          InstEmit64.Stp,                         OpCodeMemoryPair.Create),
            Create(Mnemonic.str,        LowLevelClassNames.Load_store_register_immediate_post_indexed,          LowLevelNames.STR_immediate,                InstEmit64.Str,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.str,        LowLevelClassNames.Load_store_register_immediate_pre_indexed,           LowLevelNames.STR_immediate,                InstEmit64.Str,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.str,        LowLevelClassNames.Load_store_register_register_offset,                 LowLevelNames.STR_register,                 InstEmit64.Str,                         OpCodeMemoryReg.Create),
            Create(Mnemonic.str,        LowLevelClassNames.Load_store_register_unsigned_immediate,              LowLevelNames.STR_immediate,                InstEmit64.Str,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.strb,       LowLevelClassNames.Load_store_register_immediate_post_indexed,          LowLevelNames.STRB_immediate,               InstEmit64.Str,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.strb,       LowLevelClassNames.Load_store_register_immediate_pre_indexed,           LowLevelNames.STRB_immediate,               InstEmit64.Str,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.strb,       LowLevelClassNames.Load_store_register_register_offset,                 LowLevelNames.STRB_register,                InstEmit64.Str,                         OpCodeMemoryReg.Create),
            Create(Mnemonic.strb,       LowLevelClassNames.Load_store_register_unsigned_immediate,              LowLevelNames.STRB_immediate,               InstEmit64.Str,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.strh,       LowLevelClassNames.Load_store_register_immediate_post_indexed,          LowLevelNames.STRH_immediate,               InstEmit64.Str,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.strh,       LowLevelClassNames.Load_store_register_immediate_pre_indexed,           LowLevelNames.STRH_immediate,               InstEmit64.Str,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.strh,       LowLevelClassNames.Load_store_register_register_offset,                 LowLevelNames.STRH_register,                InstEmit64.Str,                         OpCodeMemoryReg.Create),
            Create(Mnemonic.strh,       LowLevelClassNames.Load_store_register_unsigned_immediate,              LowLevelNames.STRH_immediate,               InstEmit64.Str,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.stur,       LowLevelClassNames.Load_store_register_unscaled_immediate,              LowLevelNames.STUR,                         InstEmit64.Str,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.sturb,      LowLevelClassNames.Load_store_register_unscaled_immediate,              LowLevelNames.STURB,                        InstEmit64.Str,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.sturh,      LowLevelClassNames.Load_store_register_unscaled_immediate,              LowLevelNames.STURH,                        InstEmit64.Str,                         OpCodeMemoryImm.Create),
            Create(Mnemonic.stxr,       LowLevelClassNames.Load_store_exclusive_register,                       LowLevelNames.STXR,                         InstEmit64.Stx,                         OpCodeMemoryExclusive.Create),
            Create(Mnemonic.stxrb,      LowLevelClassNames.Load_store_exclusive_register,                       LowLevelNames.STXRB,                        InstEmit64.Stx,                         OpCodeMemoryExclusive.Create),
            Create(Mnemonic.stxrh,      LowLevelClassNames.Load_store_exclusive_register,                       LowLevelNames.STXRH,                        InstEmit64.Stx,                         OpCodeMemoryExclusive.Create),
            Create(Mnemonic.sub,        LowLevelClassNames.Add_subtract_extended_register,                      LowLevelNames.SUB_extended_register,        InstEmit64.Sub,                         OpCodeALUExtend.Create),
            Create(Mnemonic.sub,        LowLevelClassNames.Add_subtract_immediate,                              LowLevelNames.SUB_immediate,                InstEmit64.Sub,                         OpCodeALUImm.Create),
            Create(Mnemonic.sub,        LowLevelClassNames.Add_subtract_shifted_register,                       LowLevelNames.SUB_shifted_register,         InstEmit64.Sub,                         OpCodeALUShiftedReg.Create),
            Create(Mnemonic.subs,       LowLevelClassNames.Add_subtract_extended_register,                      LowLevelNames.SUBS_extended_register,       InstEmit64.Subs,                        OpCodeALUExtend.Create),
            Create(Mnemonic.subs,       LowLevelClassNames.Add_subtract_immediate,                              LowLevelNames.SUBS_immediate,               InstEmit64.Subs,                        OpCodeALUImm.Create),
            Create(Mnemonic.subs,       LowLevelClassNames.Add_subtract_shifted_register,                       LowLevelNames.SUBS_shifted_register,        InstEmit64.Subs,                        OpCodeALUShiftedReg.Create),
            Create(Mnemonic.svc,        LowLevelClassNames.Exception_generation,                                LowLevelNames.SVC,                          InstEmit64.Svc,                         OpCodeExceptionGeneration.Create),
            Create(Mnemonic.tbnz,       LowLevelClassNames.Test_and_branch_immediate,                           LowLevelNames.TBNZ,                         InstEmit64.Tbnz,                        OpCodeTestBitAndBranch.Create),
            Create(Mnemonic.tbz,        LowLevelClassNames.Test_and_branch_immediate,                           LowLevelNames.TBZ,                          InstEmit64.Tbz,                         OpCodeTestBitAndBranch.Create),
            Create(Mnemonic.ubfm,       LowLevelClassNames.Bitfield,                                            LowLevelNames.UBFM,                         InstEmit64.Ubfm,                        OpCodeBitfield.Create),
            Create(Mnemonic.udiv,       LowLevelClassNames.Data_processing_2_source,                            LowLevelNames.UDIV,                         InstEmit64.Udiv,                        OpCodeALU2Src.Create),
            Create(Mnemonic.umaddl,     LowLevelClassNames.Data_processing_3_source,                            LowLevelNames.UMADDL,                       InstEmit64.Umaddl,                      OpCodeALU3Src.Create),
            Create(Mnemonic.umsubl,     LowLevelClassNames.Data_processing_3_source,                            LowLevelNames.UMSUBL,                       InstEmit64.Umsubl,                      OpCodeALU3Src.Create),
            Create(Mnemonic.umulh,      LowLevelClassNames.Data_processing_3_source,                            LowLevelNames.UMULH,                        InstEmit64.Umulh,                       OpCodeALU2Src.Create), //eh?

            //Vector
            Create(Mnemonic.add,        LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.ADD_vector,                   InstEmit64.Add_Vector,                  SIMDOpCodeVector2Src.CreateIS),
            Create(Mnemonic.and,        LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.AND_vector,                   InstEmit64.And_Vector,                  SIMDOpCodeVector2Src.CreateL),
            Create(Mnemonic.bic,        LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.BIC_vector_register,          InstEmit64.Bic_Vector,                  SIMDOpCodeVector2Src.CreateL),
            Create(Mnemonic.bsl,        LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.BSL,                          InstEmit64.Bsl,                         SIMDOpCodeVector2Src.CreateL),
            Create(Mnemonic.cmeq,       LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.CMEQ_register,                InstEmit64.Cmeq,                        SIMDOpCodeVector2Src.CreateIS),
            Create(Mnemonic.cmhi,       LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.CMHI_register,                InstEmit64.Cmhi,                        SIMDOpCodeVector2Src.CreateIS),
            Create(Mnemonic.cnt,        LowLevelClassNames.Advanced_SIMD_two_register_miscellaneous,            LowLevelNames.CNT,                          InstEmit64.Cnt,                         SIMDOpCodeVector1Src.CreateL),
            Create(Mnemonic.dup,        LowLevelClassNames.Advanced_SIMD_copy,                                  LowLevelNames.DUP_element,                  InstEmit64.Dup_Vector_Element,          SIMDOpCodeInsertElement.CreateV),
            Create(Mnemonic.dup,        LowLevelClassNames.Advanced_SIMD_copy,                                  LowLevelNames.DUP_general,                  InstEmit64.Dup_Gp,                      SIMDOpCodeDUP.Create),
            Create(Mnemonic.dup,        LowLevelClassNames.Advanced_SIMD_scalar_copy,                           LowLevelNames.DUP_element,                  InstEmit64.Dup_Element,                 SIMDOpCodeInsertElement.CreateF),
            Create(Mnemonic.eor,        LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.EOR_vector,                   InstEmit64.Eor_Vector,                  SIMDOpCodeVector2Src.CreateL),
            Create(Mnemonic.ext,        LowLevelClassNames.Advanced_SIMD_extract,                               LowLevelNames.EXT,                          InstEmit64.Ext,                         SIMDOpCodeVectorExtract.CreateEXT),
            Create(Mnemonic.fabs,       LowLevelClassNames.Advanced_SIMD_two_register_miscellaneous,            LowLevelNames.FABS_vector,                  null,                                   SIMDOpCodeVector1Src.CreateF),
            Create(Mnemonic.fabs,       LowLevelClassNames.Floating_point_data_processing_1_source,             LowLevelNames.FABS_scalar,                  InstEmit64.Fabs_Scalar,                 SIMDOpCodeScalar1Src.CreateF),
            Create(Mnemonic.fadd,       LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.FADD_vector,                  InstEmit64.Fadd_Vector,                 SIMDOpCodeVector2Src.CreateF),
            Create(Mnemonic.fadd,       LowLevelClassNames.Floating_point_data_processing_2_source,             LowLevelNames.FADD_scalar,                  InstEmit64.Fadd_Scalar,                 SIMDOpCodeScalar2Src.CreateF),
            Create(Mnemonic.faddp,      LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.FADDP_vector,                 InstEmit64.Faddp_Vector,                SIMDOpCodeVector2Src.CreateF),
            Create(Mnemonic.fccmp,      LowLevelClassNames.Floating_point_conditional_compare,                  LowLevelNames.FCCMP,                        InstEmit64.Fccmp,                       SIMDOpCodeFloatCompare.CreateC),
            Create(Mnemonic.fccmpe,     LowLevelClassNames.Floating_point_conditional_compare,                  LowLevelNames.FCCMPE,                       null,                                   SIMDOpCodeFloatCompare.CreateC),
            Create(Mnemonic.fcmeq,      LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.FCMEQ_register,               null,                                   SIMDOpCodeVector2Src.CreateF),
            Create(Mnemonic.fcmeq,      LowLevelClassNames.Advanced_SIMD_two_register_miscellaneous,            LowLevelNames.FCMEQ_zero,                   null,                                   SIMDOpCodeVector1Src.CreateFZ),
            Create(Mnemonic.fcmge,      LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.FCMGE_register,               null,                                   SIMDOpCodeVector2Src.CreateF),
            Create(Mnemonic.fcmge,      LowLevelClassNames.Advanced_SIMD_two_register_miscellaneous,            LowLevelNames.FCMGE_zero,                   null,                                   SIMDOpCodeVector1Src.CreateFZ),
            Create(Mnemonic.fcmgt,      LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.FCMGT_register,               null,                                   SIMDOpCodeVector2Src.CreateF),
            Create(Mnemonic.fcmp,       LowLevelClassNames.Floating_point_compare,                              LowLevelNames.FCMP,                         InstEmit64.Fcmp,                        SIMDOpCodeFloatCompare.CreateNC),
            Create(Mnemonic.fcmpe,      LowLevelClassNames.Floating_point_compare,                              LowLevelNames.FCMPE,                        null,                                   SIMDOpCodeFloatCompare.CreateNC),
            Create(Mnemonic.fcsel,      LowLevelClassNames.Floating_point_conditional_select,                   LowLevelNames.FCSEL,                        InstEmit64.Fcsel,                       SIMDOpCodeFloatConditionalSelect.Create),
            Create(Mnemonic.fcvt,       LowLevelClassNames.Floating_point_data_processing_1_source,             LowLevelNames.FCVT,                         InstEmit64.Fcvt,                        SIMDOpCodeScalar1Src.CreateFCVT),
            Create(Mnemonic.fcvtas,     LowLevelClassNames.Conversion_between_floating_point_and_fixed_point,   LowLevelNames.FCVTAS_scalar,                InstEmit64.Fcvtas_Scalar,               SIMDOpCodeIntegerFloatConversionScalar.CreateFCVTF),
            Create(Mnemonic.fcvtas,     LowLevelClassNames.Conversion_between_floating_point_and_integer,       LowLevelNames.FCVTAS_scalar,                InstEmit64.Fcvtas_Scalar,               SIMDOpCodeIntegerFloatConversionScalar.CreateFCVT),
            Create(Mnemonic.fcvtau,     LowLevelClassNames.Conversion_between_floating_point_and_fixed_point,   LowLevelNames.FCVTAU_scalar,                InstEmit64.Fcvtau_Scalar,               SIMDOpCodeIntegerFloatConversionScalar.CreateFCVTF),
            Create(Mnemonic.fcvtau,     LowLevelClassNames.Conversion_between_floating_point_and_integer,       LowLevelNames.FCVTAU_scalar,                InstEmit64.Fcvtau_Scalar,               SIMDOpCodeIntegerFloatConversionScalar.CreateFCVT),
            Create(Mnemonic.fcvtms,     LowLevelClassNames.Conversion_between_floating_point_and_fixed_point,   LowLevelNames.FCVTMS_scalar,                InstEmit64.Fcvtms_Scalar,               SIMDOpCodeIntegerFloatConversionScalar.CreateFCVTF),
            Create(Mnemonic.fcvtms,     LowLevelClassNames.Conversion_between_floating_point_and_integer,       LowLevelNames.FCVTMS_scalar,                InstEmit64.Fcvtms_Scalar,               SIMDOpCodeIntegerFloatConversionScalar.CreateFCVT),
            Create(Mnemonic.fcvtmu,     LowLevelClassNames.Conversion_between_floating_point_and_fixed_point,   LowLevelNames.FCVTMU_scalar,                InstEmit64.Fcvtmu_Scalar,               SIMDOpCodeIntegerFloatConversionScalar.CreateFCVTF),
            Create(Mnemonic.fcvtmu,     LowLevelClassNames.Conversion_between_floating_point_and_integer,       LowLevelNames.FCVTMU_scalar,                InstEmit64.Fcvtmu_Scalar,               SIMDOpCodeIntegerFloatConversionScalar.CreateFCVT),
            Create(Mnemonic.fcvtns,     LowLevelClassNames.Conversion_between_floating_point_and_fixed_point,   LowLevelNames.FCVTNS_scalar,                null,                                   SIMDOpCodeIntegerFloatConversionScalar.CreateFCVTF),
            Create(Mnemonic.fcvtns,     LowLevelClassNames.Conversion_between_floating_point_and_integer,       LowLevelNames.FCVTNS_scalar,                null,                                   SIMDOpCodeIntegerFloatConversionScalar.CreateFCVT),
            Create(Mnemonic.fcvtnu,     LowLevelClassNames.Conversion_between_floating_point_and_fixed_point,   LowLevelNames.FCVTNU_scalar,                null,                                   SIMDOpCodeIntegerFloatConversionScalar.CreateFCVTF),
            Create(Mnemonic.fcvtnu,     LowLevelClassNames.Conversion_between_floating_point_and_integer,       LowLevelNames.FCVTNU_scalar,                null,                                   SIMDOpCodeIntegerFloatConversionScalar.CreateFCVT),
            Create(Mnemonic.fcvtps,     LowLevelClassNames.Conversion_between_floating_point_and_fixed_point,   LowLevelNames.FCVTPS_scalar,                InstEmit64.Fcvtps_Scalar,               SIMDOpCodeIntegerFloatConversionScalar.CreateFCVTF),
            Create(Mnemonic.fcvtps,     LowLevelClassNames.Conversion_between_floating_point_and_integer,       LowLevelNames.FCVTPS_scalar,                InstEmit64.Fcvtps_Scalar,               SIMDOpCodeIntegerFloatConversionScalar.CreateFCVT),
            Create(Mnemonic.fcvtpu,     LowLevelClassNames.Conversion_between_floating_point_and_fixed_point,   LowLevelNames.FCVTPU_scalar,                InstEmit64.Fcvtpu_Scalar,               SIMDOpCodeIntegerFloatConversionScalar.CreateFCVTF),
            Create(Mnemonic.fcvtpu,     LowLevelClassNames.Conversion_between_floating_point_and_integer,       LowLevelNames.FCVTPU_scalar,                InstEmit64.Fcvtpu_Scalar,               SIMDOpCodeIntegerFloatConversionScalar.CreateFCVT),
            Create(Mnemonic.fcvtzs,     LowLevelClassNames.Advanced_SIMD_two_register_miscellaneous,            LowLevelNames.FCVTZS_vector_integer,        null,                                   SIMDOpCodeVector1Src.CreateF),
            Create(Mnemonic.fcvtzs,     LowLevelClassNames.Conversion_between_floating_point_and_fixed_point,   LowLevelNames.FCVTZS_scalar_fixed_point,    InstEmit64.Fcvtzs_Scalar,               SIMDOpCodeIntegerFloatConversionScalar.CreateFCVTF),
            Create(Mnemonic.fcvtzs,     LowLevelClassNames.Conversion_between_floating_point_and_integer,       LowLevelNames.FCVTZS_scalar_integer,        InstEmit64.Fcvtzs_Scalar,               SIMDOpCodeIntegerFloatConversionScalar.CreateFCVT),
            Create(Mnemonic.fcvtzu,     LowLevelClassNames.Conversion_between_floating_point_and_fixed_point,   LowLevelNames.FCVTZU_scalar_fixed_point,    InstEmit64.Fcvtzu_Scalar,               SIMDOpCodeIntegerFloatConversionScalar.CreateFCVTF),
            Create(Mnemonic.fcvtzu,     LowLevelClassNames.Conversion_between_floating_point_and_integer,       LowLevelNames.FCVTZU_scalar_integer,        InstEmit64.Fcvtzu_Scalar,               SIMDOpCodeIntegerFloatConversionScalar.CreateFCVT),
            Create(Mnemonic.fdiv,       LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.FDIV_vector,                  InstEmit64.Fdiv_Vector,                 SIMDOpCodeVector2Src.CreateF),
            Create(Mnemonic.fdiv,       LowLevelClassNames.Floating_point_data_processing_2_source,             LowLevelNames.FDIV_scalar,                  InstEmit64.Fdiv_Scalar,                 SIMDOpCodeScalar2Src.CreateF),
            Create(Mnemonic.fmax,       LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.FMAX_vector,                  InstEmit64.Fmax_Vector,                 SIMDOpCodeVector2Src.CreateF),
            Create(Mnemonic.fmax,       LowLevelClassNames.Floating_point_data_processing_2_source,             LowLevelNames.FMAX_scalar,                  InstEmit64.Fmax_Scalar,                 SIMDOpCodeScalar2Src.CreateF),
            Create(Mnemonic.fmaxnm,     LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.FMAXNM_vector,                null,                                   SIMDOpCodeVector2Src.CreateF),
            Create(Mnemonic.fmaxnm,     LowLevelClassNames.Floating_point_data_processing_2_source,             LowLevelNames.FMAXNM_scalar,                InstEmit64.Fmax_Scalar,                 SIMDOpCodeScalar2Src.CreateF),
            Create(Mnemonic.fmin,       LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.FMIN_vector,                  InstEmit64.Fmin_Vector,                 SIMDOpCodeVector2Src.CreateF),
            Create(Mnemonic.fmin,       LowLevelClassNames.Floating_point_data_processing_2_source,             LowLevelNames.FMIN_scalar,                  InstEmit64.Fmin_Scalar,                 SIMDOpCodeScalar2Src.CreateF),
            Create(Mnemonic.fminnm,     LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.FMINNMP_vector,               null,                                   SIMDOpCodeVector2Src.CreateF),
            Create(Mnemonic.fminnm,     LowLevelClassNames.Floating_point_data_processing_2_source,             LowLevelNames.FMINNM_scalar,                InstEmit64.Fmin_Scalar,                 SIMDOpCodeScalar2Src.CreateF),
            Create(Mnemonic.fmla,       LowLevelClassNames.Advanced_SIMD_scalar_x_indexed_element,              LowLevelNames.FMLA_by_element,              InstEmit64.Fmla_Scalar_Element,         SIMDOpCodeScalar2SrcElement.CreateF),
            Create(Mnemonic.fmla,       LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.FMLA_vector,                  InstEmit64.Fmla_Vector,                 SIMDOpCodeVector2Src.CreateF),
            Create(Mnemonic.fmla,       LowLevelClassNames.Advanced_SIMD_vector_x_indexed_element,              LowLevelNames.FMLA_by_element,              InstEmit64.Fmla_Vector_Element,         SIMDOpCodeVector2SrcElement.CreateF),
            Create(Mnemonic.fmls,       LowLevelClassNames.Advanced_SIMD_scalar_x_indexed_element,              LowLevelNames.FMLS_by_element,              InstEmit64.Fmls_Scalar_Element,         SIMDOpCodeScalar2SrcElement.CreateF),
            Create(Mnemonic.fmls,       LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.FMLS_vector,                  InstEmit64.Fmls_Vector,                 SIMDOpCodeVector2Src.CreateF),
            Create(Mnemonic.fmls,       LowLevelClassNames.Advanced_SIMD_vector_x_indexed_element,              LowLevelNames.FMLS_by_element,              InstEmit64.Fmls_Vector_Element,         SIMDOpCodeVector2SrcElement.CreateF),
            Create(Mnemonic.fmov,       LowLevelClassNames.Conversion_between_floating_point_and_integer,       LowLevelNames.FMOV_general,                 InstEmit64.Fmov_Scalar_Gp,              SIMDOpCodeFmov.Create),
            Create(Mnemonic.fmov,       LowLevelClassNames.Floating_point_immediate,                            LowLevelNames.FMOV_scalar_immediate,        InstEmit64.Fmov_Scalar_Imm,             SIMDOpCodeFmovImm.Create),
            Create(Mnemonic.fmul,       LowLevelClassNames.Advanced_SIMD_scalar_x_indexed_element,              LowLevelNames.FMUL_by_element,              InstEmit64.Fmul_Scalar_Element,         SIMDOpCodeScalar2SrcElement.CreateF),
            Create(Mnemonic.fmul,       LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.FMUL_vector,                  InstEmit64.Fmul_Vector,                 SIMDOpCodeVector2Src.CreateF),
            Create(Mnemonic.fmul,       LowLevelClassNames.Advanced_SIMD_vector_x_indexed_element,              LowLevelNames.FMUL_by_element,              InstEmit64.Fmul_Vector_Element,         SIMDOpCodeVector2SrcElement.CreateF),
            Create(Mnemonic.fmul,       LowLevelClassNames.Floating_point_data_processing_2_source,             LowLevelNames.FMUL_scalar,                  InstEmit64.Fmul_Scalar,                 SIMDOpCodeScalar2Src.CreateF),
            Create(Mnemonic.fneg,       LowLevelClassNames.Advanced_SIMD_two_register_miscellaneous,            LowLevelNames.FNEG_vector,                  null,                                   SIMDOpCodeVector1Src.CreateF),
            Create(Mnemonic.fneg,       LowLevelClassNames.Floating_point_data_processing_1_source,             LowLevelNames.FNEG_scalar,                  InstEmit64.Fneg_Scalar,                 SIMDOpCodeScalar1Src.CreateF),
            Create(Mnemonic.fnmul,      LowLevelClassNames.Floating_point_data_processing_2_source,             LowLevelNames.FNMUL_scalar,                 InstEmit64.Fnmul_Scalar,                SIMDOpCodeScalar2Src.CreateF),
            Create(Mnemonic.frecpe,     LowLevelClassNames.Advanced_SIMD_two_register_miscellaneous,            LowLevelNames.FRECPE,                       null,                                   SIMDOpCodeVector1Src.CreateF),
            Create(Mnemonic.frintm,     LowLevelClassNames.Floating_point_data_processing_1_source,             LowLevelNames.FRINTM_scalar,                InstEmit64.Frintm,                      SIMDOpCodeScalar1Src.CreateF),
            Create(Mnemonic.frintp,     LowLevelClassNames.Floating_point_data_processing_1_source,             LowLevelNames.FRINTP_scalar,                null,                                   SIMDOpCodeScalar1Src.CreateF),
            Create(Mnemonic.frsqrte,    LowLevelClassNames.Advanced_SIMD_two_register_miscellaneous,            LowLevelNames.FRSQRTE,                      InstEmit64.Frsqrte_Vector,              SIMDOpCodeVector1Src.CreateF),
            Create(Mnemonic.frsqrts,    LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.FRSQRTS,                      InstEmit64.Frsqrts_Vector,              SIMDOpCodeVector2Src.CreateF),
            Create(Mnemonic.fsqrt,      LowLevelClassNames.Advanced_SIMD_two_register_miscellaneous,            LowLevelNames.FSQRT_vector,                 null,                                   SIMDOpCodeVector1Src.CreateF),
            Create(Mnemonic.fsqrt,      LowLevelClassNames.Floating_point_data_processing_1_source,             LowLevelNames.FSQRT_scalar,                 InstEmit64.Fsqrt_Scalar,                SIMDOpCodeScalar1Src.CreateF),
            Create(Mnemonic.fsub,       LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.FSUB_vector,                  InstEmit64.Fsub_Vector,                 SIMDOpCodeVector2Src.CreateF),
            Create(Mnemonic.fsub,       LowLevelClassNames.Floating_point_data_processing_2_source,             LowLevelNames.FSUB_scalar,                  InstEmit64.Fsub_Scalar,                 SIMDOpCodeScalar2Src.CreateF),
            Create(Mnemonic.ins,        LowLevelClassNames.Advanced_SIMD_copy,                                  LowLevelNames.INS_element,                  InstEmit64.Ins_Element,                 SIDMOpCodeInsertVectorElement.Create),
            Create(Mnemonic.ins,        LowLevelClassNames.Advanced_SIMD_copy,                                  LowLevelNames.INS_general,                  InstEmit64.Ins_General,                 SIMDOpCodeInsertElement.CreateT),
            Create(Mnemonic.ld1,        LowLevelClassNames.Advanced_SIMD_load_store_single_structure,           LowLevelNames.LD1_single_structure,         InstEmit64.Ldm,                         SIMDOpCodeVectorMemoryMultiple.Create),
            Create(Mnemonic.ldp,        LowLevelClassNames.Load_store_register_pair_offset,                     LowLevelNames.LDP_SIMD_FP,                  InstEmit64.Ldp_Vector,                  OpCodeMemoryPair.Create),
            Create(Mnemonic.ldp,        LowLevelClassNames.Load_store_register_pair_post_indexed,               LowLevelNames.LDP_SIMD_FP,                  InstEmit64.Ldp_Vector,                  OpCodeMemoryPair.Create),
            Create(Mnemonic.ldp,        LowLevelClassNames.Load_store_register_pair_pre_indexed,                LowLevelNames.LDP_SIMD_FP,                  InstEmit64.Ldp_Vector,                  OpCodeMemoryPair.Create),
            Create(Mnemonic.ldr,        LowLevelClassNames.Load_store_register_immediate_post_indexed,          LowLevelNames.LDR_immediate_SIMD_FP,        InstEmit64.Ldr_Vector,                  OpCodeMemoryImm.Create),
            Create(Mnemonic.ldr,        LowLevelClassNames.Load_store_register_immediate_pre_indexed,           LowLevelNames.LDR_immediate_SIMD_FP,        InstEmit64.Ldr_Vector,                  OpCodeMemoryImm.Create),
            Create(Mnemonic.ldr,        LowLevelClassNames.Load_store_register_register_offset,                 LowLevelNames.LDR_register_SIMD_FP,         InstEmit64.Ldr_Vector,                  OpCodeMemoryReg.Create),
            Create(Mnemonic.ldr,        LowLevelClassNames.Load_store_register_unsigned_immediate,              LowLevelNames.LDR_immediate_SIMD_FP,        InstEmit64.Ldr_Vector,                  OpCodeMemoryImm.Create),
            Create(Mnemonic.ldur,       LowLevelClassNames.Load_store_register_unscaled_immediate,              LowLevelNames.LDUR_SIMD_FP,                 InstEmit64.Ldr_Vector,                  OpCodeMemoryImm.Create),
            Create(Mnemonic.movi,       LowLevelClassNames.Advanced_SIMD_modified_immediate,                    LowLevelNames.MOVI,                         InstEmit64.Movi,                        SIMDOpCodeMovImm.Create),
            Create(Mnemonic.mul,        LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.MUL_vector,                   null,                                   SIMDOpCodeVector2Src.CreateIS),
            Create(Mnemonic.neg,        LowLevelClassNames.Advanced_SIMD_two_register_miscellaneous,            LowLevelNames.NEG_vector,                   null,                                   SIMDOpCodeVector1Src.CreateIS),
            Create(Mnemonic.orn,        LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.ORN_vector,                   InstEmit64.Orn_Vector,                  SIMDOpCodeVector2Src.CreateL),
            Create(Mnemonic.orr,        LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.ORR_vector_register,          InstEmit64.Orr_Vector,                  SIMDOpCodeVector2Src.CreateL),
            Create(Mnemonic.scvtf,      LowLevelClassNames.Advanced_SIMD_scalar_two_register_miscellaneous,     LowLevelNames.SCVTF_vector_integer,         InstEmit64.Scvtf_Scalar,                SIMDOpCodeIntegerFloatConversionScalar.CreateCVTFV),
            Create(Mnemonic.scvtf,      LowLevelClassNames.Advanced_SIMD_two_register_miscellaneous,            LowLevelNames.SCVTF_vector_integer,         InstEmit64.Scvtf_Vector,                SIMDOpCodeVector1Src.CreateFSZ),
            Create(Mnemonic.scvtf,      LowLevelClassNames.Conversion_between_floating_point_and_integer,       LowLevelNames.SCVTF_scalar_integer,         InstEmit64.Scvtf_Scalar,                SIMDOpCodeIntegerFloatConversionScalar.CreateCVTF),
            Create(Mnemonic.stp,        LowLevelClassNames.Load_store_register_pair_offset,                     LowLevelNames.STP_SIMD_FP,                  InstEmit64.Stp_Vector,                  OpCodeMemoryPair.Create),
            Create(Mnemonic.stp,        LowLevelClassNames.Load_store_register_pair_post_indexed,               LowLevelNames.STP_SIMD_FP,                  InstEmit64.Stp_Vector,                  OpCodeMemoryPair.Create),
            Create(Mnemonic.stp,        LowLevelClassNames.Load_store_register_pair_pre_indexed,                LowLevelNames.STP_SIMD_FP,                  InstEmit64.Stp_Vector,                  OpCodeMemoryPair.Create),
            Create(Mnemonic.str,        LowLevelClassNames.Load_store_register_immediate_post_indexed,          LowLevelNames.STR_immediate_SIMD_FP,        InstEmit64.Str_Vector,                  OpCodeMemoryImm.Create),
            Create(Mnemonic.str,        LowLevelClassNames.Load_store_register_immediate_pre_indexed,           LowLevelNames.STR_immediate_SIMD_FP,        InstEmit64.Str_Vector,                  OpCodeMemoryImm.Create),
            Create(Mnemonic.str,        LowLevelClassNames.Load_store_register_register_offset,                 LowLevelNames.STR_register_SIMD_FP,         InstEmit64.Str_Vector,                  OpCodeMemoryReg.Create),
            Create(Mnemonic.str,        LowLevelClassNames.Load_store_register_unsigned_immediate,              LowLevelNames.STR_immediate_SIMD_FP,        InstEmit64.Str_Vector,                  OpCodeMemoryImm.Create),
            Create(Mnemonic.stur,       LowLevelClassNames.Load_store_register_unscaled_immediate,              LowLevelNames.STUR_SIMD_FP,                 InstEmit64.Str_Vector,                  OpCodeMemoryImm.Create),
            Create(Mnemonic.sub,        LowLevelClassNames.Advanced_SIMD_three_same,                            LowLevelNames.SUB_vector,                   null,                                   SIMDOpCodeVector2Src.CreateIS),
            Create(Mnemonic.tbl,        LowLevelClassNames.Advanced_SIMD_table_lookup,                          LowLevelNames.TBL,                          InstEmit64.Tbl,                         SIMDOpCodeVectorTableLookup.Create),
            Create(Mnemonic.uaddlv,     LowLevelClassNames.Advanced_SIMD_across_lanes,                          LowLevelNames.UADDLV,                       InstEmit64.Uaddlv,                      SIMDOpCodeVector1Src.CreateVDS),
            Create(Mnemonic.ucvtf,      LowLevelClassNames.Advanced_SIMD_scalar_two_register_miscellaneous,     LowLevelNames.UCVTF_vector_integer,         InstEmit64.Ucvtf_Scalar,                SIMDOpCodeIntegerFloatConversionScalar.CreateCVTFV),
            Create(Mnemonic.ucvtf,      LowLevelClassNames.Advanced_SIMD_two_register_miscellaneous,            LowLevelNames.UCVTF_vector_integer,         InstEmit64.Ucvtf_Vector,                SIMDOpCodeVector1Src.CreateFSZ),
            Create(Mnemonic.ucvtf,      LowLevelClassNames.Conversion_between_floating_point_and_integer,       LowLevelNames.UCVTF_scalar_integer,         InstEmit64.Ucvtf_Scalar,                SIMDOpCodeIntegerFloatConversionScalar.CreateCVTF),
            Create(Mnemonic.umov,       LowLevelClassNames.Advanced_SIMD_copy,                                  LowLevelNames.UMOV,                         InstEmit64.Umov_Element,                SIMDOpCodeInsertElement.CreateU),
            Create(Mnemonic.zip1,       LowLevelClassNames.Advanced_SIMD_permute,                               LowLevelNames.ZIP1,                         InstEmit64.Zip1,                        SIMDOpCodeVector2Src.CreateIS),
            Create(Mnemonic.zip2,       LowLevelClassNames.Advanced_SIMD_permute,                               LowLevelNames.ZIP2,                         InstEmit64.Zip2,                        SIMDOpCodeVector2Src.CreateIS),
        };

        //static MiddleMan Create(Mnemonic Mnemonic, LowLevelClassNames ClassName, LowLevelNames LowLevelName, OpCodeCreate Create) => new MiddleMan(Mnemonic, ClassName, LowLevelName, null, Create);
        static MiddleMan Create(Mnemonic Mnemonic, LowLevelClassNames ClassName, LowLevelNames LowLevelName, InstEmit Emit,OpCodeCreate Create) => new MiddleMan(Mnemonic, ClassName, LowLevelName, Emit, Create);

        public static string GetName(int RawIns)
        {
#if ANDROID
            return "undefined:";
#else
            using (CapstoneArm64Disassembler disassembler = CapstoneArm64Disassembler.CreateArm64Disassembler(Arm64DisassembleMode.LittleEndian))
            {
                //Console.WriteLine(LowLevelInstructionTableCollection.GetOpHex(RawIns));

                var ins = disassembler.Disassemble(BitConverter.GetBytes(RawIns));

                if (ins == null || ins.Length == 0)
                    return "undefined";

                string ss = Testing.GetIns(ins[0]);

                return ss;
            }
#endif
        }

        public static AOpCode GetAOpCode(long Address, int RawInstruction)
        {
            LowLevelAOpCode lowLevel = LowLevelInstructionTable.GetOpCode(RawInstruction);

            foreach (MiddleMan man in MiddleMen)
            {
                if (man.ClassName == lowLevel.ClassName && man.LowLevelName == lowLevel.LowLevelName)
                {
                    AOpCode opCode = man.CreateOP(lowLevel, Address, man.HighLevelName);

                    opCode.Emit = man.Emit;
                    opCode.RawData = lowLevel;

                    //Console.WriteLine(opCode);

                    return opCode;
                }
            }

            AOpCode Out = new AOpCode(Address, RawInstruction, Mnemonic.undefined);

            //if (lowLevel.ClassName == LowLevelClassNames.Advanced_SIMD_two_register_miscellaneous)
            //Console.WriteLine($"Undefined Instruction: {Out} {lowLevel.ClassName} {lowLevel.LowLevelName}");

            return Out;
        }
    }
}
