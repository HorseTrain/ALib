using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.LowLevel
{
    public class LowLevelAOpCode
    {
        public int RawInstruction;

        public LowLevelNames LowLevelName;
        public LowLevelClassNames ClassName;

        public int size;
        public int opc;
        public int Pg;
        public int Zm;
        public int Zdn;
        public int R;
        public int U;
        public int H;
        public int Zn;
        public int Vd;
        public int M;
        public int Zd;
        public int op;
        public int tszh;
        public int L;
        public int tszl;
        public int imm3;
        public int CRm;
        public int op2;
        public int Rt;
        public int sf;
        public int imm19;
        public int o1;
        public int o0;
        public int cond;
        public int imm16;
        public int LL;
        public int op1;
        public int CRn;
        public int b5;
        public int b40;
        public int imm14;
        public int imm26;
        public int op3;
        public int Rn;
        public int op4;
        public int Q;
        public int opcode;
        public int Rm;
        public int S;
        public int V;
        public int A;
        public int Rs;
        public int o3;
        public int Rt2;
        public int sz;
        public int imm9;
        public int imm7;
        public int W;
        public int option;
        public int imm12;
        public int Zda;
        public int Za;
        public int sh;
        public int Rd;
        public int o2;
        public int uimm6;
        public int uimm4;
        public int N;
        public int immr;
        public int imms;
        public int op21;
        public int hw;
        public int immlo;
        public int immhi;
        public int imm5b;
        public int imm5;
        public int imm6;
        public int opc2;
        public int opt;
        public int shift;
        public int nzcv;
        public int opcode2;
        public int op54;
        public int op31;
        public int Ra;
        public int mask;
        public int imm4;
        public int a;
        public int b;
        public int c;
        public int cmode;
        public int d;
        public int e;
        public int f;
        public int g;
        public int h;
        public int immh;
        public int immb;
        public int len;
        public int ptype;
        public int rmode;
        public int scale;
        public int Op0;
        public int O;
        public int imm2;
        public int imm8;
        public int msz;
        public int pattern;
        public int D;
        public int Rdn;
        public int imm8h;
        public int imm8l;
        public int imm13;
        public int tsz;
        public int Vm;
        public int Pm;
        public int Pn;
        public int Pd;
        public int B;
        public int Vdn;
        public int Vn;
        public int ne;
        public int lt;
        public int Pdm;
        public int Pdn;
        public int eq;
        public int uns;
        public int rot;
        public int i2;
        public int i3h;
        public int i3l;
        public int T;
        public int i1;
        public int xs;
        public int ff;
        public int Zt;
        public int prfop;
        public int dtypeh;
        public int dtypel;
        public int imm9h;
        public int imm9l;
        public int Pt;
        public int dtype;
        public int ssz;

        public override string ToString()
        {
            return $"{ClassName}: {LowLevelName}";
        }

        public static LowLevelAOpCode Create_Unallocated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.LowLevelName = Name;

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_bitwise_logical_operations_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_bitwise_logical_operations_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zm = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_add_subtract_vectors_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_add_subtract_vectors_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zm = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_divide_vectors_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_divide_vectors_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.R = (RawOpCode >> 17) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zm = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_min_max_difference_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_min_max_difference_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 17) & ((1 << 2) - 1);
            Out.U = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zm = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_multiply_vectors_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_multiply_vectors_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.H = (RawOpCode >> 17) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zm = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_bitwise_logical_reduction_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_bitwise_logical_reduction_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Vd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_constructive_prefix_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_constructive_prefix_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 17) & ((1 << 2) - 1);
            Out.M = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_add_reduction_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_add_reduction_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.op = (RawOpCode >> 17) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Vd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_min_max_reduction_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_min_max_reduction_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.op = (RawOpCode >> 17) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Vd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_bitwise_shift_by_immediate_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_bitwise_shift_by_immediate_predicated;

            Out.LowLevelName = Name;
            Out.tszh = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 18) & ((1 << 2) - 1);
            Out.L = (RawOpCode >> 17) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.tszl = (RawOpCode >> 8) & ((1 << 2) - 1);
            Out.imm3 = (RawOpCode >> 5) & ((1 << 3) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_bitwise_shift_by_vector_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_bitwise_shift_by_vector_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.R = (RawOpCode >> 18) & ((1 << 1) - 1);
            Out.L = (RawOpCode >> 17) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zm = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_bitwise_shift_by_wide_elements_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_bitwise_shift_by_wide_elements_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.R = (RawOpCode >> 18) & ((1 << 1) - 1);
            Out.L = (RawOpCode >> 17) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zm = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Barriers(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Barriers;

            Out.LowLevelName = Name;
            Out.CRm = (RawOpCode >> 8) & ((1 << 4) - 1);
            Out.op2 = (RawOpCode >> 5) & ((1 << 3) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Compare_and_branch_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Compare_and_branch_immediate;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.op = (RawOpCode >> 24) & ((1 << 1) - 1);
            Out.imm19 = (RawOpCode >> 5) & ((1 << 19) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Conditional_branch_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Conditional_branch_immediate;

            Out.LowLevelName = Name;
            Out.o1 = (RawOpCode >> 24) & ((1 << 1) - 1);
            Out.imm19 = (RawOpCode >> 5) & ((1 << 19) - 1);
            Out.o0 = (RawOpCode >> 4) & ((1 << 1) - 1);
            Out.cond = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Exception_generation(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Exception_generation;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 21) & ((1 << 3) - 1);
            Out.imm16 = (RawOpCode >> 5) & ((1 << 16) - 1);
            Out.op2 = (RawOpCode >> 2) & ((1 << 3) - 1);
            Out.LL = (RawOpCode >> 0) & ((1 << 2) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Hints(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Hints;

            Out.LowLevelName = Name;
            Out.CRm = (RawOpCode >> 8) & ((1 << 4) - 1);
            Out.op2 = (RawOpCode >> 5) & ((1 << 3) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_PSTATE(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.PSTATE;

            Out.LowLevelName = Name;
            Out.op1 = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.CRm = (RawOpCode >> 8) & ((1 << 4) - 1);
            Out.op2 = (RawOpCode >> 5) & ((1 << 3) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_System_instructions(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.System_instructions;

            Out.LowLevelName = Name;
            Out.L = (RawOpCode >> 21) & ((1 << 1) - 1);
            Out.op1 = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.CRn = (RawOpCode >> 12) & ((1 << 4) - 1);
            Out.CRm = (RawOpCode >> 8) & ((1 << 4) - 1);
            Out.op2 = (RawOpCode >> 5) & ((1 << 3) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_System_instructions_with_register_argument(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.System_instructions_with_register_argument;

            Out.LowLevelName = Name;
            Out.CRm = (RawOpCode >> 8) & ((1 << 4) - 1);
            Out.op2 = (RawOpCode >> 5) & ((1 << 3) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_System_register_move(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.System_register_move;

            Out.LowLevelName = Name;
            Out.L = (RawOpCode >> 21) & ((1 << 1) - 1);
            Out.o0 = (RawOpCode >> 19) & ((1 << 1) - 1);
            Out.op1 = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.CRn = (RawOpCode >> 12) & ((1 << 4) - 1);
            Out.CRm = (RawOpCode >> 8) & ((1 << 4) - 1);
            Out.op2 = (RawOpCode >> 5) & ((1 << 3) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Test_and_branch_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Test_and_branch_immediate;

            Out.LowLevelName = Name;
            Out.b5 = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.op = (RawOpCode >> 24) & ((1 << 1) - 1);
            Out.b40 = (RawOpCode >> 19) & ((1 << 5) - 1);
            Out.imm14 = (RawOpCode >> 5) & ((1 << 14) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Unconditional_branch_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Unconditional_branch_immediate;

            Out.LowLevelName = Name;
            Out.op = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.imm26 = (RawOpCode >> 0) & ((1 << 26) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Unconditional_branch_register(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Unconditional_branch_register;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 21) & ((1 << 4) - 1);
            Out.op2 = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.op3 = (RawOpCode >> 10) & ((1 << 6) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.op4 = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_bitwise_unary_operations_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_bitwise_unary_operations_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_unary_operations_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_unary_operations_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_load_store_multiple_structures(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_load_store_multiple_structures;

            Out.LowLevelName = Name;
            Out.Q = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.L = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.opcode = (RawOpCode >> 12) & ((1 << 4) - 1);
            Out.size = (RawOpCode >> 10) & ((1 << 2) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_load_store_multiple_structures_post_indexed(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_load_store_multiple_structures_post_indexed;

            Out.LowLevelName = Name;
            Out.Q = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.L = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opcode = (RawOpCode >> 12) & ((1 << 4) - 1);
            Out.size = (RawOpCode >> 10) & ((1 << 2) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_load_store_single_structure(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_load_store_single_structure;

            Out.LowLevelName = Name;
            Out.Q = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.L = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.R = (RawOpCode >> 21) & ((1 << 1) - 1);
            Out.opcode = (RawOpCode >> 13) & ((1 << 3) - 1);
            Out.S = (RawOpCode >> 12) & ((1 << 1) - 1);
            Out.size = (RawOpCode >> 10) & ((1 << 2) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_load_store_single_structure_post_indexed(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_load_store_single_structure_post_indexed;

            Out.LowLevelName = Name;
            Out.Q = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.L = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.R = (RawOpCode >> 21) & ((1 << 1) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opcode = (RawOpCode >> 13) & ((1 << 3) - 1);
            Out.S = (RawOpCode >> 12) & ((1 << 1) - 1);
            Out.size = (RawOpCode >> 10) & ((1 << 2) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Atomic_memory_operations(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Atomic_memory_operations;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 30) & ((1 << 2) - 1);
            Out.V = (RawOpCode >> 26) & ((1 << 1) - 1);
            Out.A = (RawOpCode >> 23) & ((1 << 1) - 1);
            Out.R = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Rs = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.o3 = (RawOpCode >> 15) & ((1 << 1) - 1);
            Out.opc = (RawOpCode >> 12) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Compare_and_swap(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Compare_and_swap;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 30) & ((1 << 2) - 1);
            Out.L = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Rs = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.o0 = (RawOpCode >> 15) & ((1 << 1) - 1);
            Out.Rt2 = (RawOpCode >> 10) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Compare_and_swap_pair(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Compare_and_swap_pair;

            Out.LowLevelName = Name;
            Out.sz = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.L = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Rs = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.o0 = (RawOpCode >> 15) & ((1 << 1) - 1);
            Out.Rt2 = (RawOpCode >> 10) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_LDAPR_STLR_unscaled_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.LDAPR_STLR_unscaled_immediate;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 30) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.imm9 = (RawOpCode >> 12) & ((1 << 9) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Load_register_literal(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Load_register_literal;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 30) & ((1 << 2) - 1);
            Out.V = (RawOpCode >> 26) & ((1 << 1) - 1);
            Out.imm19 = (RawOpCode >> 5) & ((1 << 19) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Load_store_exclusive_pair(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Load_store_exclusive_pair;

            Out.LowLevelName = Name;
            Out.sz = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.L = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Rs = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.o0 = (RawOpCode >> 15) & ((1 << 1) - 1);
            Out.Rt2 = (RawOpCode >> 10) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Load_store_exclusive_register(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Load_store_exclusive_register;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 30) & ((1 << 2) - 1);
            Out.L = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Rs = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.o0 = (RawOpCode >> 15) & ((1 << 1) - 1);
            Out.Rt2 = (RawOpCode >> 10) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Load_store_memory_tags(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Load_store_memory_tags;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.imm9 = (RawOpCode >> 12) & ((1 << 9) - 1);
            Out.op2 = (RawOpCode >> 10) & ((1 << 2) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Load_store_no_allocate_pair_offset(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Load_store_no_allocate_pair_offset;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 30) & ((1 << 2) - 1);
            Out.V = (RawOpCode >> 26) & ((1 << 1) - 1);
            Out.L = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.imm7 = (RawOpCode >> 15) & ((1 << 7) - 1);
            Out.Rt2 = (RawOpCode >> 10) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Load_store_ordered(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Load_store_ordered;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 30) & ((1 << 2) - 1);
            Out.L = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Rs = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.o0 = (RawOpCode >> 15) & ((1 << 1) - 1);
            Out.Rt2 = (RawOpCode >> 10) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Load_store_register_immediate_post_indexed(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Load_store_register_immediate_post_indexed;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 30) & ((1 << 2) - 1);
            Out.V = (RawOpCode >> 26) & ((1 << 1) - 1);
            Out.opc = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.imm9 = (RawOpCode >> 12) & ((1 << 9) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Load_store_register_immediate_pre_indexed(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Load_store_register_immediate_pre_indexed;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 30) & ((1 << 2) - 1);
            Out.V = (RawOpCode >> 26) & ((1 << 1) - 1);
            Out.opc = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.imm9 = (RawOpCode >> 12) & ((1 << 9) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Load_store_register_pac(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Load_store_register_pac;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 30) & ((1 << 2) - 1);
            Out.V = (RawOpCode >> 26) & ((1 << 1) - 1);
            Out.M = (RawOpCode >> 23) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.imm9 = (RawOpCode >> 12) & ((1 << 9) - 1);
            Out.W = (RawOpCode >> 11) & ((1 << 1) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Load_store_register_register_offset(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Load_store_register_register_offset;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 30) & ((1 << 2) - 1);
            Out.V = (RawOpCode >> 26) & ((1 << 1) - 1);
            Out.opc = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.option = (RawOpCode >> 13) & ((1 << 3) - 1);
            Out.S = (RawOpCode >> 12) & ((1 << 1) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Load_store_register_unprivileged(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Load_store_register_unprivileged;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 30) & ((1 << 2) - 1);
            Out.V = (RawOpCode >> 26) & ((1 << 1) - 1);
            Out.opc = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.imm9 = (RawOpCode >> 12) & ((1 << 9) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Load_store_register_unscaled_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Load_store_register_unscaled_immediate;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 30) & ((1 << 2) - 1);
            Out.V = (RawOpCode >> 26) & ((1 << 1) - 1);
            Out.opc = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.imm9 = (RawOpCode >> 12) & ((1 << 9) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Load_store_register_unsigned_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Load_store_register_unsigned_immediate;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 30) & ((1 << 2) - 1);
            Out.V = (RawOpCode >> 26) & ((1 << 1) - 1);
            Out.opc = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.imm12 = (RawOpCode >> 10) & ((1 << 12) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Load_store_register_pair_offset(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Load_store_register_pair_offset;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 30) & ((1 << 2) - 1);
            Out.V = (RawOpCode >> 26) & ((1 << 1) - 1);
            Out.L = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.imm7 = (RawOpCode >> 15) & ((1 << 7) - 1);
            Out.Rt2 = (RawOpCode >> 10) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Load_store_register_pair_post_indexed(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Load_store_register_pair_post_indexed;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 30) & ((1 << 2) - 1);
            Out.V = (RawOpCode >> 26) & ((1 << 1) - 1);
            Out.L = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.imm7 = (RawOpCode >> 15) & ((1 << 7) - 1);
            Out.Rt2 = (RawOpCode >> 10) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Load_store_register_pair_pre_indexed(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Load_store_register_pair_pre_indexed;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 30) & ((1 << 2) - 1);
            Out.V = (RawOpCode >> 26) & ((1 << 1) - 1);
            Out.L = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.imm7 = (RawOpCode >> 15) & ((1 << 7) - 1);
            Out.Rt2 = (RawOpCode >> 10) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_multiply_accumulate_writing_addend_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_multiply_accumulate_writing_addend_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.op = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zda = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_multiply_add_writing_multiplicand_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_multiply_add_writing_multiplicand_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.op = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Za = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_add_subtract_vectors_unpredicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_add_subtract_vectors_unpredicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opc = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Add_subtract_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Add_subtract_immediate;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.op = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.sh = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.imm12 = (RawOpCode >> 10) & ((1 << 12) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Add_subtract_immediate_with_tags(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Add_subtract_immediate_with_tags;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.op = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.o2 = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.uimm6 = (RawOpCode >> 16) & ((1 << 6) - 1);
            Out.op3 = (RawOpCode >> 14) & ((1 << 2) - 1);
            Out.uimm4 = (RawOpCode >> 10) & ((1 << 4) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Bitfield(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Bitfield;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.opc = (RawOpCode >> 29) & ((1 << 2) - 1);
            Out.N = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.immr = (RawOpCode >> 16) & ((1 << 6) - 1);
            Out.imms = (RawOpCode >> 10) & ((1 << 6) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Extract(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Extract;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.op21 = (RawOpCode >> 29) & ((1 << 2) - 1);
            Out.N = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.o0 = (RawOpCode >> 21) & ((1 << 1) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.imms = (RawOpCode >> 10) & ((1 << 6) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Logical_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Logical_immediate;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.opc = (RawOpCode >> 29) & ((1 << 2) - 1);
            Out.N = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.immr = (RawOpCode >> 16) & ((1 << 6) - 1);
            Out.imms = (RawOpCode >> 10) & ((1 << 6) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Move_wide_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Move_wide_immediate;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.opc = (RawOpCode >> 29) & ((1 << 2) - 1);
            Out.hw = (RawOpCode >> 21) & ((1 << 2) - 1);
            Out.imm16 = (RawOpCode >> 5) & ((1 << 16) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_PC_rel__addressing(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.PC_rel__addressing;

            Out.LowLevelName = Name;
            Out.op = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.immlo = (RawOpCode >> 29) & ((1 << 2) - 1);
            Out.immhi = (RawOpCode >> 5) & ((1 << 19) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_bitwise_logical_operations_unpredicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_bitwise_logical_operations_unpredicated;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_index_generation_immediate_start_immediate_increment(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_index_generation_immediate_start_immediate_increment;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.imm5b = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.imm5 = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_index_generation_immediate_start_register_increment(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_index_generation_immediate_start_register_increment;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.imm5 = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_index_generation_register_start_immediate_increment(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_index_generation_register_start_immediate_increment;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.imm5 = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_index_generation_register_start_register_increment(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_index_generation_register_start_register_increment;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_stack_frame_adjustment(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_stack_frame_adjustment;

            Out.LowLevelName = Name;
            Out.op = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Rn = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.imm6 = (RawOpCode >> 5) & ((1 << 6) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_stack_frame_size(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_stack_frame_size;

            Out.LowLevelName = Name;
            Out.op = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.opc2 = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.imm6 = (RawOpCode >> 5) & ((1 << 6) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Add_subtract_extended_register(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Add_subtract_extended_register;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.op = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.opt = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.option = (RawOpCode >> 13) & ((1 << 3) - 1);
            Out.imm3 = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Add_subtract_shifted_register(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Add_subtract_shifted_register;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.op = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.shift = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.imm6 = (RawOpCode >> 10) & ((1 << 6) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Add_subtract_with_carry(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Add_subtract_with_carry;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.op = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Conditional_compare_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Conditional_compare_immediate;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.op = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.imm5 = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.cond = (RawOpCode >> 12) & ((1 << 4) - 1);
            Out.o2 = (RawOpCode >> 10) & ((1 << 1) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.o3 = (RawOpCode >> 4) & ((1 << 1) - 1);
            Out.nzcv = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Conditional_compare_register(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Conditional_compare_register;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.op = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.cond = (RawOpCode >> 12) & ((1 << 4) - 1);
            Out.o2 = (RawOpCode >> 10) & ((1 << 1) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.o3 = (RawOpCode >> 4) & ((1 << 1) - 1);
            Out.nzcv = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Conditional_select(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Conditional_select;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.op = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.cond = (RawOpCode >> 12) & ((1 << 4) - 1);
            Out.op2 = (RawOpCode >> 10) & ((1 << 2) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Data_processing_1_source(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Data_processing_1_source;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.opcode2 = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opcode = (RawOpCode >> 10) & ((1 << 6) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Data_processing_2_source(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Data_processing_2_source;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opcode = (RawOpCode >> 10) & ((1 << 6) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Data_processing_3_source(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Data_processing_3_source;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.op54 = (RawOpCode >> 29) & ((1 << 2) - 1);
            Out.op31 = (RawOpCode >> 21) & ((1 << 3) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.o0 = (RawOpCode >> 15) & ((1 << 1) - 1);
            Out.Ra = (RawOpCode >> 10) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Evaluate_into_flags(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Evaluate_into_flags;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.op = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.opcode2 = (RawOpCode >> 15) & ((1 << 6) - 1);
            Out.sz = (RawOpCode >> 14) & ((1 << 1) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.o3 = (RawOpCode >> 4) & ((1 << 1) - 1);
            Out.mask = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Logical_shifted_register(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Logical_shifted_register;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.opc = (RawOpCode >> 29) & ((1 << 2) - 1);
            Out.shift = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.N = (RawOpCode >> 21) & ((1 << 1) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.imm6 = (RawOpCode >> 10) & ((1 << 6) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Rotate_right_into_flags(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Rotate_right_into_flags;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.op = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.imm6 = (RawOpCode >> 15) & ((1 << 6) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.o2 = (RawOpCode >> 4) & ((1 << 1) - 1);
            Out.mask = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_bitwise_shift_by_immediate_unpredicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_bitwise_shift_by_immediate_unpredicated;

            Out.LowLevelName = Name;
            Out.tszh = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.tszl = (RawOpCode >> 19) & ((1 << 2) - 1);
            Out.imm3 = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.opc = (RawOpCode >> 10) & ((1 << 2) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_bitwise_shift_by_wide_elements_unpredicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_bitwise_shift_by_wide_elements_unpredicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opc = (RawOpCode >> 10) & ((1 << 2) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_across_lanes(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_across_lanes;

            Out.LowLevelName = Name;
            Out.Q = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opcode = (RawOpCode >> 12) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_copy(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_copy;

            Out.LowLevelName = Name;
            Out.Q = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.op = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.imm5 = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.imm4 = (RawOpCode >> 11) & ((1 << 4) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_extract(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_extract;

            Out.LowLevelName = Name;
            Out.Q = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.op2 = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.imm4 = (RawOpCode >> 11) & ((1 << 4) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_modified_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_modified_immediate;

            Out.LowLevelName = Name;
            Out.Q = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.op = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.a = (RawOpCode >> 18) & ((1 << 1) - 1);
            Out.b = (RawOpCode >> 17) & ((1 << 1) - 1);
            Out.c = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.cmode = (RawOpCode >> 12) & ((1 << 4) - 1);
            Out.o2 = (RawOpCode >> 11) & ((1 << 1) - 1);
            Out.d = (RawOpCode >> 9) & ((1 << 1) - 1);
            Out.e = (RawOpCode >> 8) & ((1 << 1) - 1);
            Out.f = (RawOpCode >> 7) & ((1 << 1) - 1);
            Out.g = (RawOpCode >> 6) & ((1 << 1) - 1);
            Out.h = (RawOpCode >> 5) & ((1 << 1) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_permute(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_permute;

            Out.LowLevelName = Name;
            Out.Q = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opcode = (RawOpCode >> 12) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_scalar_copy(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_scalar_copy;

            Out.LowLevelName = Name;
            Out.op = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.imm5 = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.imm4 = (RawOpCode >> 11) & ((1 << 4) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_scalar_pairwise(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_scalar_pairwise;

            Out.LowLevelName = Name;
            Out.U = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opcode = (RawOpCode >> 12) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_scalar_shift_by_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_scalar_shift_by_immediate;

            Out.LowLevelName = Name;
            Out.U = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.immh = (RawOpCode >> 19) & ((1 << 4) - 1);
            Out.immb = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.opcode = (RawOpCode >> 11) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_scalar_three_different(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_scalar_three_different;

            Out.LowLevelName = Name;
            Out.U = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opcode = (RawOpCode >> 12) & ((1 << 4) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_scalar_three_same(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_scalar_three_same;

            Out.LowLevelName = Name;
            Out.U = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opcode = (RawOpCode >> 11) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_scalar_three_same_FP16(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_scalar_three_same_FP16;

            Out.LowLevelName = Name;
            Out.U = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.a = (RawOpCode >> 23) & ((1 << 1) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opcode = (RawOpCode >> 11) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_scalar_three_same_extra(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_scalar_three_same_extra;

            Out.LowLevelName = Name;
            Out.U = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opcode = (RawOpCode >> 11) & ((1 << 4) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_scalar_two_register_miscellaneous(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_scalar_two_register_miscellaneous;

            Out.LowLevelName = Name;
            Out.U = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opcode = (RawOpCode >> 12) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_scalar_two_register_miscellaneous_FP16(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_scalar_two_register_miscellaneous_FP16;

            Out.LowLevelName = Name;
            Out.U = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.a = (RawOpCode >> 23) & ((1 << 1) - 1);
            Out.opcode = (RawOpCode >> 12) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_scalar_x_indexed_element(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_scalar_x_indexed_element;

            Out.LowLevelName = Name;
            Out.U = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.L = (RawOpCode >> 21) & ((1 << 1) - 1);
            Out.M = (RawOpCode >> 20) & ((1 << 1) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.opcode = (RawOpCode >> 12) & ((1 << 4) - 1);
            Out.H = (RawOpCode >> 11) & ((1 << 1) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_shift_by_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_shift_by_immediate;

            Out.LowLevelName = Name;
            Out.Q = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.immh = (RawOpCode >> 19) & ((1 << 4) - 1);
            Out.immb = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.opcode = (RawOpCode >> 11) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_table_lookup(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_table_lookup;

            Out.LowLevelName = Name;
            Out.Q = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.op2 = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.len = (RawOpCode >> 13) & ((1 << 2) - 1);
            Out.op = (RawOpCode >> 12) & ((1 << 1) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_three_different(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_three_different;

            Out.LowLevelName = Name;
            Out.Q = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opcode = (RawOpCode >> 12) & ((1 << 4) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_three_same(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_three_same;

            Out.LowLevelName = Name;
            Out.Q = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opcode = (RawOpCode >> 11) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_three_same_FP16(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_three_same_FP16;

            Out.LowLevelName = Name;
            Out.Q = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.a = (RawOpCode >> 23) & ((1 << 1) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opcode = (RawOpCode >> 11) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_three_register_extension(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_three_register_extension;

            Out.LowLevelName = Name;
            Out.Q = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opcode = (RawOpCode >> 11) & ((1 << 4) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_two_register_miscellaneous(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_two_register_miscellaneous;

            Out.LowLevelName = Name;
            Out.Q = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opcode = (RawOpCode >> 12) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_two_register_miscellaneous_FP16(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_two_register_miscellaneous_FP16;

            Out.LowLevelName = Name;
            Out.Q = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.a = (RawOpCode >> 23) & ((1 << 1) - 1);
            Out.opcode = (RawOpCode >> 12) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Advanced_SIMD_vector_x_indexed_element(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Advanced_SIMD_vector_x_indexed_element;

            Out.LowLevelName = Name;
            Out.Q = (RawOpCode >> 30) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.L = (RawOpCode >> 21) & ((1 << 1) - 1);
            Out.M = (RawOpCode >> 20) & ((1 << 1) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.opcode = (RawOpCode >> 12) & ((1 << 4) - 1);
            Out.H = (RawOpCode >> 11) & ((1 << 1) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Conversion_between_floating_point_and_fixed_point(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Conversion_between_floating_point_and_fixed_point;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.ptype = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.rmode = (RawOpCode >> 19) & ((1 << 2) - 1);
            Out.opcode = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.scale = (RawOpCode >> 10) & ((1 << 6) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Conversion_between_floating_point_and_integer(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Conversion_between_floating_point_and_integer;

            Out.LowLevelName = Name;
            Out.sf = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.ptype = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.rmode = (RawOpCode >> 19) & ((1 << 2) - 1);
            Out.opcode = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Cryptographic_AES(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Cryptographic_AES;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opcode = (RawOpCode >> 12) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Cryptographic_four_register(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Cryptographic_four_register;

            Out.LowLevelName = Name;
            Out.Op0 = (RawOpCode >> 21) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Ra = (RawOpCode >> 10) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Cryptographic_three_register_SHA(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Cryptographic_three_register_SHA;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opcode = (RawOpCode >> 12) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Cryptographic_three_register_SHA_512(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Cryptographic_three_register_SHA_512;

            Out.LowLevelName = Name;
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.O = (RawOpCode >> 14) & ((1 << 1) - 1);
            Out.opcode = (RawOpCode >> 10) & ((1 << 2) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Cryptographic_three_register_imm2(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Cryptographic_three_register_imm2;

            Out.LowLevelName = Name;
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.imm2 = (RawOpCode >> 12) & ((1 << 2) - 1);
            Out.opcode = (RawOpCode >> 10) & ((1 << 2) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Cryptographic_three_register_imm6(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Cryptographic_three_register_imm6;

            Out.LowLevelName = Name;
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.imm6 = (RawOpCode >> 10) & ((1 << 6) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Cryptographic_two_register_SHA(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Cryptographic_two_register_SHA;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opcode = (RawOpCode >> 12) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Cryptographic_two_register_SHA_512(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Cryptographic_two_register_SHA_512;

            Out.LowLevelName = Name;
            Out.opcode = (RawOpCode >> 10) & ((1 << 2) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Floating_point_compare(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Floating_point_compare;

            Out.LowLevelName = Name;
            Out.M = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.ptype = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.op = (RawOpCode >> 14) & ((1 << 2) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.opcode2 = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Floating_point_conditional_compare(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Floating_point_conditional_compare;

            Out.LowLevelName = Name;
            Out.M = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.ptype = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.cond = (RawOpCode >> 12) & ((1 << 4) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.op = (RawOpCode >> 4) & ((1 << 1) - 1);
            Out.nzcv = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Floating_point_conditional_select(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Floating_point_conditional_select;

            Out.LowLevelName = Name;
            Out.M = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.ptype = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.cond = (RawOpCode >> 12) & ((1 << 4) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Floating_point_data_processing_1_source(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Floating_point_data_processing_1_source;

            Out.LowLevelName = Name;
            Out.M = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.ptype = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opcode = (RawOpCode >> 15) & ((1 << 6) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Floating_point_data_processing_2_source(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Floating_point_data_processing_2_source;

            Out.LowLevelName = Name;
            Out.M = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.ptype = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opcode = (RawOpCode >> 12) & ((1 << 4) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Floating_point_data_processing_3_source(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Floating_point_data_processing_3_source;

            Out.LowLevelName = Name;
            Out.M = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.ptype = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.o1 = (RawOpCode >> 21) & ((1 << 1) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.o0 = (RawOpCode >> 15) & ((1 << 1) - 1);
            Out.Ra = (RawOpCode >> 10) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Floating_point_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Floating_point_immediate;

            Out.LowLevelName = Name;
            Out.M = (RawOpCode >> 31) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 29) & ((1 << 1) - 1);
            Out.ptype = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.imm8 = (RawOpCode >> 13) & ((1 << 8) - 1);
            Out.imm5 = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_address_generation(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_address_generation;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.msz = (RawOpCode >> 10) & ((1 << 2) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_Reserved(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.Reserved;

            Out.LowLevelName = Name;
            Out.imm16 = (RawOpCode >> 0) & ((1 << 16) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_constructive_prefix_unpredicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_constructive_prefix_unpredicated;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc2 = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_exponential_accelerator(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_exponential_accelerator;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_trig_select_coefficient(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_trig_select_coefficient;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.op = (RawOpCode >> 10) & ((1 << 1) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_element_count(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_element_count;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.imm4 = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.op = (RawOpCode >> 10) & ((1 << 1) - 1);
            Out.pattern = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_inc_dec_register_by_element_count(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_inc_dec_register_by_element_count;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.imm4 = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.D = (RawOpCode >> 10) & ((1 << 1) - 1);
            Out.pattern = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_inc_dec_vector_by_element_count(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_inc_dec_vector_by_element_count;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.imm4 = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.D = (RawOpCode >> 10) & ((1 << 1) - 1);
            Out.pattern = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_saturating_inc_dec_register_by_element_count(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_saturating_inc_dec_register_by_element_count;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.sf = (RawOpCode >> 20) & ((1 << 1) - 1);
            Out.imm4 = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.D = (RawOpCode >> 11) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 10) & ((1 << 1) - 1);
            Out.pattern = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_saturating_inc_dec_vector_by_element_count(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_saturating_inc_dec_vector_by_element_count;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.imm4 = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.D = (RawOpCode >> 11) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 10) & ((1 << 1) - 1);
            Out.pattern = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_extract_vector_immediate_offset_destructive(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_extract_vector_immediate_offset_destructive;

            Out.LowLevelName = Name;
            Out.imm8h = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.imm8l = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zm = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_permute_vector_segments(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_permute_vector_segments;

            Out.LowLevelName = Name;
            Out.op = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opc2 = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_bitwise_logical_with_immediate_unpredicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_bitwise_logical_with_immediate_unpredicated;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.imm13 = (RawOpCode >> 5) & ((1 << 13) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_broadcast_bitmask_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_broadcast_bitmask_immediate;

            Out.LowLevelName = Name;
            Out.imm13 = (RawOpCode >> 5) & ((1 << 13) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_copy_floating_point_immediate_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_copy_floating_point_immediate_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.imm8 = (RawOpCode >> 5) & ((1 << 8) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_copy_integer_immediate_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_copy_integer_immediate_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.M = (RawOpCode >> 14) & ((1 << 1) - 1);
            Out.sh = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.imm8 = (RawOpCode >> 5) & ((1 << 8) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_broadcast_indexed_element(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_broadcast_indexed_element;

            Out.LowLevelName = Name;
            Out.imm2 = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.tsz = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_table_lookup(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_table_lookup;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_broadcast_general_register(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_broadcast_general_register;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_insert_SIMD_FP_scalar_register(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_insert_SIMD_FP_scalar_register;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Vm = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_insert_general_register(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_insert_general_register;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_reverse_vector_elements(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_reverse_vector_elements;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_unpack_vector_elements(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_unpack_vector_elements;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.U = (RawOpCode >> 17) & ((1 << 1) - 1);
            Out.H = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_permute_predicate_elements(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_permute_predicate_elements;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Pm = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.opc = (RawOpCode >> 11) & ((1 << 2) - 1);
            Out.H = (RawOpCode >> 10) & ((1 << 1) - 1);
            Out.Pn = (RawOpCode >> 5) & ((1 << 4) - 1);
            Out.Pd = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_reverse_predicate_elements(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_reverse_predicate_elements;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Pn = (RawOpCode >> 5) & ((1 << 4) - 1);
            Out.Pd = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_unpack_predicate_elements(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_unpack_predicate_elements;

            Out.LowLevelName = Name;
            Out.H = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.Pn = (RawOpCode >> 5) & ((1 << 4) - 1);
            Out.Pd = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_permute_vector_elements(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_permute_vector_elements;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opc = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_compress_active_elements(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_compress_active_elements;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_conditionally_broadcast_element_to_vector(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_conditionally_broadcast_element_to_vector;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.B = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zm = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_conditionally_extract_element_to_SIMD_FP_scalar(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_conditionally_extract_element_to_SIMD_FP_scalar;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.B = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zm = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Vdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_conditionally_extract_element_to_general_register(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_conditionally_extract_element_to_general_register;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.B = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zm = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_copy_SIMD_FP_scalar_register_to_vector_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_copy_SIMD_FP_scalar_register_to_vector_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Vn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_copy_general_register_to_vector_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_copy_general_register_to_vector_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_extract_element_to_SIMD_FP_scalar_register(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_extract_element_to_SIMD_FP_scalar_register;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.B = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Vd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_extract_element_to_general_register(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_extract_element_to_general_register;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.B = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_reverse_within_elements(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_reverse_within_elements;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_vector_splice_destructive(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_vector_splice_destructive;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zm = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_select_vector_elements_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_select_vector_elements_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 4) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_compare_vectors(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_compare_vectors;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.op = (RawOpCode >> 15) & ((1 << 1) - 1);
            Out.o2 = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.ne = (RawOpCode >> 4) & ((1 << 1) - 1);
            Out.Pd = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_compare_with_wide_elements(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_compare_with_wide_elements;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.U = (RawOpCode >> 15) & ((1 << 1) - 1);
            Out.lt = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.ne = (RawOpCode >> 4) & ((1 << 1) - 1);
            Out.Pd = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_compare_with_unsigned_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_compare_with_unsigned_immediate;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.imm7 = (RawOpCode >> 14) & ((1 << 7) - 1);
            Out.lt = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.ne = (RawOpCode >> 4) & ((1 << 1) - 1);
            Out.Pd = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_predicate_logical_operations(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_predicate_logical_operations;

            Out.LowLevelName = Name;
            Out.op = (RawOpCode >> 23) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Pm = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 4) - 1);
            Out.o2 = (RawOpCode >> 9) & ((1 << 1) - 1);
            Out.Pn = (RawOpCode >> 5) & ((1 << 4) - 1);
            Out.o3 = (RawOpCode >> 4) & ((1 << 1) - 1);
            Out.Pd = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_propagate_break_from_previous_partition(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_propagate_break_from_previous_partition;

            Out.LowLevelName = Name;
            Out.op = (RawOpCode >> 23) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Pm = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 4) - 1);
            Out.Pn = (RawOpCode >> 5) & ((1 << 4) - 1);
            Out.B = (RawOpCode >> 4) & ((1 << 1) - 1);
            Out.Pd = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_partition_break_condition(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_partition_break_condition;

            Out.LowLevelName = Name;
            Out.B = (RawOpCode >> 23) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 4) - 1);
            Out.Pn = (RawOpCode >> 5) & ((1 << 4) - 1);
            Out.M = (RawOpCode >> 4) & ((1 << 1) - 1);
            Out.Pd = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_propagate_break_to_next_partition(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_propagate_break_to_next_partition;

            Out.LowLevelName = Name;
            Out.S = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 4) - 1);
            Out.Pn = (RawOpCode >> 5) & ((1 << 4) - 1);
            Out.Pdm = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_predicate_first_active(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_predicate_first_active;

            Out.LowLevelName = Name;
            Out.op = (RawOpCode >> 23) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 5) & ((1 << 4) - 1);
            Out.Pdn = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_predicate_initialize(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_predicate_initialize;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.S = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.pattern = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Pd = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_predicate_next_active(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_predicate_next_active;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 5) & ((1 << 4) - 1);
            Out.Pdn = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_predicate_read_from_FFR_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_predicate_read_from_FFR_predicated;

            Out.LowLevelName = Name;
            Out.op = (RawOpCode >> 23) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 5) & ((1 << 4) - 1);
            Out.Pd = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_predicate_read_from_FFR_unpredicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_predicate_read_from_FFR_unpredicated;

            Out.LowLevelName = Name;
            Out.op = (RawOpCode >> 23) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Pd = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_predicate_test(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_predicate_test;

            Out.LowLevelName = Name;
            Out.op = (RawOpCode >> 23) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 4) - 1);
            Out.Pn = (RawOpCode >> 5) & ((1 << 4) - 1);
            Out.opc2 = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_predicate_zero(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_predicate_zero;

            Out.LowLevelName = Name;
            Out.op = (RawOpCode >> 23) & ((1 << 1) - 1);
            Out.S = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Pd = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_compare_with_signed_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_compare_with_signed_immediate;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.imm5 = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.op = (RawOpCode >> 15) & ((1 << 1) - 1);
            Out.o2 = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.ne = (RawOpCode >> 4) & ((1 << 1) - 1);
            Out.Pd = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_predicate_count(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_predicate_count;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 4) - 1);
            Out.Pn = (RawOpCode >> 5) & ((1 << 4) - 1);
            Out.Rd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_inc_dec_register_by_predicate_count(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_inc_dec_register_by_predicate_count;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.op = (RawOpCode >> 17) & ((1 << 1) - 1);
            Out.D = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.opc2 = (RawOpCode >> 9) & ((1 << 2) - 1);
            Out.Pm = (RawOpCode >> 5) & ((1 << 4) - 1);
            Out.Rdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_inc_dec_vector_by_predicate_count(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_inc_dec_vector_by_predicate_count;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.op = (RawOpCode >> 17) & ((1 << 1) - 1);
            Out.D = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.opc2 = (RawOpCode >> 9) & ((1 << 2) - 1);
            Out.Pm = (RawOpCode >> 5) & ((1 << 4) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_saturating_inc_dec_register_by_predicate_count(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_saturating_inc_dec_register_by_predicate_count;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.D = (RawOpCode >> 17) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.sf = (RawOpCode >> 10) & ((1 << 1) - 1);
            Out.op = (RawOpCode >> 9) & ((1 << 1) - 1);
            Out.Pm = (RawOpCode >> 5) & ((1 << 4) - 1);
            Out.Rdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_saturating_inc_dec_vector_by_predicate_count(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_saturating_inc_dec_vector_by_predicate_count;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.D = (RawOpCode >> 17) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.opc = (RawOpCode >> 9) & ((1 << 2) - 1);
            Out.Pm = (RawOpCode >> 5) & ((1 << 4) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_FFR_initialise(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_FFR_initialise;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 22) & ((1 << 2) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_FFR_write_from_predicate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_FFR_write_from_predicate;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Pn = (RawOpCode >> 5) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_conditionally_terminate_scalars(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_conditionally_terminate_scalars;

            Out.LowLevelName = Name;
            Out.op = (RawOpCode >> 23) & ((1 << 1) - 1);
            Out.sz = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.ne = (RawOpCode >> 4) & ((1 << 1) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_compare_scalar_count_and_limit(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_compare_scalar_count_and_limit;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.sf = (RawOpCode >> 12) & ((1 << 1) - 1);
            Out.U = (RawOpCode >> 11) & ((1 << 1) - 1);
            Out.lt = (RawOpCode >> 10) & ((1 << 1) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.eq = (RawOpCode >> 4) & ((1 << 1) - 1);
            Out.Pd = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_broadcast_floating_point_immediate_unpredicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_broadcast_floating_point_immediate_unpredicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 17) & ((1 << 2) - 1);
            Out.o2 = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.imm8 = (RawOpCode >> 5) & ((1 << 8) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_broadcast_integer_immediate_unpredicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_broadcast_integer_immediate_unpredicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 17) & ((1 << 2) - 1);
            Out.sh = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.imm8 = (RawOpCode >> 5) & ((1 << 8) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_add_subtract_immediate_unpredicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_add_subtract_immediate_unpredicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.sh = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.imm8 = (RawOpCode >> 5) & ((1 << 8) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_min_max_immediate_unpredicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_min_max_immediate_unpredicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.o2 = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.imm8 = (RawOpCode >> 5) & ((1 << 8) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_multiply_immediate_unpredicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_multiply_immediate_unpredicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.o2 = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.imm8 = (RawOpCode >> 5) & ((1 << 8) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_dot_product_unpredicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_dot_product_unpredicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.U = (RawOpCode >> 10) & ((1 << 1) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zda = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_mixed_sign_dot_product(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_mixed_sign_dot_product;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zda = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_dot_product_indexed(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_dot_product_indexed;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.U = (RawOpCode >> 10) & ((1 << 1) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zda = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_mixed_sign_dot_product_indexed(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_mixed_sign_dot_product_indexed;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.U = (RawOpCode >> 10) & ((1 << 1) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zda = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_matrix_multiply_accumulate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_matrix_multiply_accumulate;

            Out.LowLevelName = Name;
            Out.uns = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_complex_add_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_complex_add_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.rot = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zm = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_convert_precision_odd_elements(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_convert_precision_odd_elements;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc2 = (RawOpCode >> 16) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_complex_multiply_add_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_complex_multiply_add_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.rot = (RawOpCode >> 13) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zda = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_multiply_add_indexed(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_multiply_add_indexed;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.op = (RawOpCode >> 10) & ((1 << 1) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zda = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_complex_multiply_add_indexed(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_complex_multiply_add_indexed;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.rot = (RawOpCode >> 10) & ((1 << 2) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zda = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_multiply_indexed(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_multiply_indexed;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_BFloat16_floating_point_dot_product_indexed(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_BFloat16_floating_point_dot_product_indexed;

            Out.LowLevelName = Name;
            Out.op = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.i2 = (RawOpCode >> 19) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zda = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_multiply_add_long_indexed(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_multiply_add_long_indexed;

            Out.LowLevelName = Name;
            Out.o2 = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.i3h = (RawOpCode >> 19) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.op = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.i3l = (RawOpCode >> 11) & ((1 << 1) - 1);
            Out.T = (RawOpCode >> 10) & ((1 << 1) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zda = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_BFloat16_floating_point_dot_product(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_BFloat16_floating_point_dot_product;

            Out.LowLevelName = Name;
            Out.op = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zda = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_multiply_add_long(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_multiply_add_long;

            Out.LowLevelName = Name;
            Out.o2 = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.op = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.T = (RawOpCode >> 10) & ((1 << 1) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zda = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_matrix_multiply_accumulate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_matrix_multiply_accumulate;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zda = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_recursive_reduction(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_recursive_reduction;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Vd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_reciprocal_estimate_unpredicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_reciprocal_estimate_unpredicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_compare_with_zero(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_compare_with_zero;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.eq = (RawOpCode >> 17) & ((1 << 1) - 1);
            Out.lt = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.ne = (RawOpCode >> 4) & ((1 << 1) - 1);
            Out.Pd = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_serial_reduction_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_serial_reduction_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zm = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Vdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_arithmetic_unpredicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_arithmetic_unpredicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opc = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_arithmetic_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_arithmetic_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zm = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_arithmetic_with_immediate_predicated(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_arithmetic_with_immediate_predicated;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.i1 = (RawOpCode >> 5) & ((1 << 1) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_trig_multiply_add_coefficient(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_trig_multiply_add_coefficient;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.imm3 = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.Zm = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_convert_precision(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_convert_precision;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc2 = (RawOpCode >> 16) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_convert_to_integer(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_convert_to_integer;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc2 = (RawOpCode >> 17) & ((1 << 2) - 1);
            Out.U = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_round_to_integral_value(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_round_to_integral_value;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 3) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_unary_operations(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_unary_operations;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 16) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_integer_convert_to_floating_point(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_integer_convert_to_floating_point;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.opc2 = (RawOpCode >> 17) & ((1 << 2) - 1);
            Out.U = (RawOpCode >> 16) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zd = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_compare_vectors(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_compare_vectors;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.op = (RawOpCode >> 15) & ((1 << 1) - 1);
            Out.o2 = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.o3 = (RawOpCode >> 4) & ((1 << 1) - 1);
            Out.Pd = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_multiply_accumulate_writing_addend(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_multiply_accumulate_writing_addend;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opc = (RawOpCode >> 13) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zda = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_floating_point_multiply_accumulate_writing_multiplicand(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_floating_point_multiply_accumulate_writing_multiplicand;

            Out.LowLevelName = Name;
            Out.size = (RawOpCode >> 22) & ((1 << 2) - 1);
            Out.Za = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.opc = (RawOpCode >> 13) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zm = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zdn = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_32_bit_gather_load_scalar_plus_32_bit_unscaled_offsets(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_32_bit_gather_load_scalar_plus_32_bit_unscaled_offsets;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.xs = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.U = (RawOpCode >> 14) & ((1 << 1) - 1);
            Out.ff = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_32_bit_gather_load_vector_plus_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_32_bit_gather_load_vector_plus_immediate;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.imm5 = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.U = (RawOpCode >> 14) & ((1 << 1) - 1);
            Out.ff = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_32_bit_gather_load_halfwords_scalar_plus_32_bit_scaled_offsets(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_32_bit_gather_load_halfwords_scalar_plus_32_bit_scaled_offsets;

            Out.LowLevelName = Name;
            Out.xs = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.U = (RawOpCode >> 14) & ((1 << 1) - 1);
            Out.ff = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_32_bit_gather_load_words_scalar_plus_32_bit_scaled_offsets(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_32_bit_gather_load_words_scalar_plus_32_bit_scaled_offsets;

            Out.LowLevelName = Name;
            Out.xs = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.U = (RawOpCode >> 14) & ((1 << 1) - 1);
            Out.ff = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_32_bit_gather_prefetch_scalar_plus_32_bit_scaled_offsets(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_32_bit_gather_prefetch_scalar_plus_32_bit_scaled_offsets;

            Out.LowLevelName = Name;
            Out.xs = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.msz = (RawOpCode >> 13) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.prfop = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_32_bit_gather_prefetch_vector_plus_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_32_bit_gather_prefetch_vector_plus_immediate;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.imm5 = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.prfop = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_contiguous_prefetch_scalar_plus_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_contiguous_prefetch_scalar_plus_immediate;

            Out.LowLevelName = Name;
            Out.imm6 = (RawOpCode >> 16) & ((1 << 6) - 1);
            Out.msz = (RawOpCode >> 13) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.prfop = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_contiguous_prefetch_scalar_plus_scalar(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_contiguous_prefetch_scalar_plus_scalar;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.prfop = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_load_and_broadcast_element(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_load_and_broadcast_element;

            Out.LowLevelName = Name;
            Out.dtypeh = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.imm6 = (RawOpCode >> 16) & ((1 << 6) - 1);
            Out.dtypel = (RawOpCode >> 13) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_load_predicate_register(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_load_predicate_register;

            Out.LowLevelName = Name;
            Out.imm9h = (RawOpCode >> 16) & ((1 << 6) - 1);
            Out.imm9l = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Pt = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_load_vector_register(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_load_vector_register;

            Out.LowLevelName = Name;
            Out.imm9h = (RawOpCode >> 16) & ((1 << 6) - 1);
            Out.imm9l = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_contiguous_first_fault_load_scalar_plus_scalar(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_contiguous_first_fault_load_scalar_plus_scalar;

            Out.LowLevelName = Name;
            Out.dtype = (RawOpCode >> 21) & ((1 << 4) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_contiguous_load_scalar_plus_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_contiguous_load_scalar_plus_immediate;

            Out.LowLevelName = Name;
            Out.dtype = (RawOpCode >> 21) & ((1 << 4) - 1);
            Out.imm4 = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_contiguous_load_scalar_plus_scalar(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_contiguous_load_scalar_plus_scalar;

            Out.LowLevelName = Name;
            Out.dtype = (RawOpCode >> 21) & ((1 << 4) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_contiguous_non_fault_load_scalar_plus_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_contiguous_non_fault_load_scalar_plus_immediate;

            Out.LowLevelName = Name;
            Out.dtype = (RawOpCode >> 21) & ((1 << 4) - 1);
            Out.imm4 = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_contiguous_non_temporal_load_scalar_plus_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_contiguous_non_temporal_load_scalar_plus_immediate;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.imm4 = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_contiguous_non_temporal_load_scalar_plus_scalar(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_contiguous_non_temporal_load_scalar_plus_scalar;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_load_and_broadcast_quadword_scalar_plus_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_load_and_broadcast_quadword_scalar_plus_immediate;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.ssz = (RawOpCode >> 21) & ((1 << 2) - 1);
            Out.imm4 = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_load_and_broadcast_quadword_scalar_plus_scalar(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_load_and_broadcast_quadword_scalar_plus_scalar;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.ssz = (RawOpCode >> 21) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_load_multiple_structures_scalar_plus_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_load_multiple_structures_scalar_plus_immediate;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 21) & ((1 << 2) - 1);
            Out.imm4 = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_load_multiple_structures_scalar_plus_scalar(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_load_multiple_structures_scalar_plus_scalar;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 21) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_64_bit_gather_load_scalar_plus_32_bit_unpacked_scaled_offsets(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_64_bit_gather_load_scalar_plus_32_bit_unpacked_scaled_offsets;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.xs = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.U = (RawOpCode >> 14) & ((1 << 1) - 1);
            Out.ff = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_64_bit_gather_load_scalar_plus_64_bit_scaled_offsets(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_64_bit_gather_load_scalar_plus_64_bit_scaled_offsets;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.U = (RawOpCode >> 14) & ((1 << 1) - 1);
            Out.ff = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_64_bit_gather_load_scalar_plus_64_bit_unscaled_offsets(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_64_bit_gather_load_scalar_plus_64_bit_unscaled_offsets;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.U = (RawOpCode >> 14) & ((1 << 1) - 1);
            Out.ff = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_64_bit_gather_load_scalar_plus_unpacked_32_bit_unscaled_offsets(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_64_bit_gather_load_scalar_plus_unpacked_32_bit_unscaled_offsets;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.xs = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.U = (RawOpCode >> 14) & ((1 << 1) - 1);
            Out.ff = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_64_bit_gather_load_vector_plus_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_64_bit_gather_load_vector_plus_immediate;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.imm5 = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.U = (RawOpCode >> 14) & ((1 << 1) - 1);
            Out.ff = (RawOpCode >> 13) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_64_bit_gather_prefetch_scalar_plus_64_bit_scaled_offsets(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_64_bit_gather_prefetch_scalar_plus_64_bit_scaled_offsets;

            Out.LowLevelName = Name;
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.msz = (RawOpCode >> 13) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.prfop = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_64_bit_gather_prefetch_scalar_plus_unpacked_32_bit_scaled_offsets(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_64_bit_gather_prefetch_scalar_plus_unpacked_32_bit_scaled_offsets;

            Out.LowLevelName = Name;
            Out.xs = (RawOpCode >> 22) & ((1 << 1) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.msz = (RawOpCode >> 13) & ((1 << 2) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.prfop = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_64_bit_gather_prefetch_vector_plus_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_64_bit_gather_prefetch_vector_plus_immediate;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.imm5 = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.prfop = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_contiguous_store_scalar_plus_scalar(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_contiguous_store_scalar_plus_scalar;

            Out.LowLevelName = Name;
            Out.opc = (RawOpCode >> 22) & ((1 << 3) - 1);
            Out.o2 = (RawOpCode >> 21) & ((1 << 1) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_store_predicate_register(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_store_predicate_register;

            Out.LowLevelName = Name;
            Out.imm9h = (RawOpCode >> 16) & ((1 << 6) - 1);
            Out.imm9l = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Pt = (RawOpCode >> 0) & ((1 << 4) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_store_vector_register(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_store_vector_register;

            Out.LowLevelName = Name;
            Out.imm9h = (RawOpCode >> 16) & ((1 << 6) - 1);
            Out.imm9l = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_contiguous_non_temporal_store_scalar_plus_scalar(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_contiguous_non_temporal_store_scalar_plus_scalar;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_store_multiple_structures_scalar_plus_scalar(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_store_multiple_structures_scalar_plus_scalar;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 21) & ((1 << 2) - 1);
            Out.Rm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_32_bit_scatter_store_vector_plus_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_32_bit_scatter_store_vector_plus_immediate;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.imm5 = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_64_bit_scatter_store_scalar_plus_64_bit_scaled_offsets(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_64_bit_scatter_store_scalar_plus_64_bit_scaled_offsets;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_64_bit_scatter_store_scalar_plus_64_bit_unscaled_offsets(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_64_bit_scatter_store_scalar_plus_64_bit_unscaled_offsets;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_64_bit_scatter_store_vector_plus_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_64_bit_scatter_store_vector_plus_immediate;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.imm5 = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Zn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_contiguous_non_temporal_store_scalar_plus_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_contiguous_non_temporal_store_scalar_plus_immediate;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.imm4 = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_contiguous_store_scalar_plus_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_contiguous_store_scalar_plus_immediate;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.size = (RawOpCode >> 21) & ((1 << 2) - 1);
            Out.imm4 = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_store_multiple_structures_scalar_plus_immediate(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_store_multiple_structures_scalar_plus_immediate;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.opc = (RawOpCode >> 21) & ((1 << 2) - 1);
            Out.imm4 = (RawOpCode >> 16) & ((1 << 4) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_32_bit_scatter_store_scalar_plus_32_bit_scaled_offsets(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_32_bit_scatter_store_scalar_plus_32_bit_scaled_offsets;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.xs = (RawOpCode >> 14) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_32_bit_scatter_store_scalar_plus_32_bit_unscaled_offsets(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_32_bit_scatter_store_scalar_plus_32_bit_unscaled_offsets;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.xs = (RawOpCode >> 14) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_64_bit_scatter_store_scalar_plus_unpacked_32_bit_scaled_offsets(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_64_bit_scatter_store_scalar_plus_unpacked_32_bit_scaled_offsets;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.xs = (RawOpCode >> 14) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }

        public static LowLevelAOpCode Create_SVE_64_bit_scatter_store_scalar_plus_unpacked_32_bit_unscaled_offsets(int RawOpCode, LowLevelNames Name)
        {
            LowLevelAOpCode Out = new LowLevelAOpCode();

            Out.ClassName = LowLevelClassNames.SVE_64_bit_scatter_store_scalar_plus_unpacked_32_bit_unscaled_offsets;

            Out.LowLevelName = Name;
            Out.msz = (RawOpCode >> 23) & ((1 << 2) - 1);
            Out.Zm = (RawOpCode >> 16) & ((1 << 5) - 1);
            Out.xs = (RawOpCode >> 14) & ((1 << 1) - 1);
            Out.Pg = (RawOpCode >> 10) & ((1 << 3) - 1);
            Out.Rn = (RawOpCode >> 5) & ((1 << 5) - 1);
            Out.Zt = (RawOpCode >> 0) & ((1 << 5) - 1);

            return Out;
        }
    }
}
