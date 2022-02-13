using ArmLIB;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

public unsafe class Aarch64DebugAssembler
{
    public List<int> Instructions = new List<int>();

    public uint AddIns(uint ins)
    {
        Instructions.Add((int)ins);

        return ins;
    }

    [DllImport("msvcrt.dll")]
    public static unsafe extern void* memcpy(void* dest, void* src, ulong count);

    public static unsafe T Convert<T, F>(F source) where T : unmanaged where F : unmanaged
    {
        Debug.Assert(sizeof(T) == sizeof(F));

        T _out = *((T*)&source);

        //memcpy(&_out, &source, (ulong)sizeof(T));

        return _out;
    }

    public byte[] GetBuffer()
    {
        byte[] Out = new byte[Instructions.Count * 4];

        fixed (int* src = Instructions.ToArray())
        {
            fixed (byte* des = Out)
            {
                memcpy(des, src, (ulong)Out.Length);
            }
        }

        return Out;
    }

    public void Emit(string code)
    {
        fixed (byte* dat = Testing.ks.Assemble(code, 0).Buffer)
        {
            int tmp = *(int*)dat;

            AddIns((uint)tmp);
        }
    }

    //SVE bitwise logical operations (predicated)
    public void emit_0(int size, int opc, int Pg, int Zm, int Zdn) => AddIns(0b00000100000110000000000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 7) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zm & 31) << 5) | (((uint)Zdn & 31) << 0));
    //SVE integer add/subtract vectors (predicated)
    public void emit_1(int size, int opc, int Pg, int Zm, int Zdn) => AddIns(0b00000100000000000000000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 7) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zm & 31) << 5) | (((uint)Zdn & 31) << 0));
    //SVE integer divide vectors (predicated)
    public void emit_2(int size, int R, int U, int Pg, int Zm, int Zdn) => AddIns(0b00000100000101000000000000000000 | (((uint)size & 3) << 22) | (((uint)R & 1) << 17) | (((uint)U & 1) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zm & 31) << 5) | (((uint)Zdn & 31) << 0));
    //SVE integer min/max/difference (predicated)
    public void emit_3(int size, int opc, int U, int Pg, int Zm, int Zdn) => AddIns(0b00000100000010000000000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 3) << 17) | (((uint)U & 1) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zm & 31) << 5) | (((uint)Zdn & 31) << 0));
    //SVE integer multiply vectors (predicated)
    public void emit_4(int size, int H, int U, int Pg, int Zm, int Zdn) => AddIns(0b00000100000100000000000000000000 | (((uint)size & 3) << 22) | (((uint)H & 1) << 17) | (((uint)U & 1) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zm & 31) << 5) | (((uint)Zdn & 31) << 0));
    //SVE bitwise logical reduction (predicated)
    public void emit_5(int size, int opc, int Pg, int Zn, int Vd) => AddIns(0b00000100000110000010000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 3) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Vd & 31) << 0));
    //SVE constructive prefix (predicated)
    public void emit_6(int size, int opc, int M, int Pg, int Zn, int Zd) => AddIns(0b00000100000100000010000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 3) << 17) | (((uint)M & 1) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE integer add reduction (predicated)
    public void emit_7(int size, int op, int U, int Pg, int Zn, int Vd) => AddIns(0b00000100000000000010000000000000 | (((uint)size & 3) << 22) | (((uint)op & 1) << 17) | (((uint)U & 1) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Vd & 31) << 0));
    //SVE integer min/max reduction (predicated)
    public void emit_8(int size, int op, int U, int Pg, int Zn, int Vd) => AddIns(0b00000100000010000010000000000000 | (((uint)size & 3) << 22) | (((uint)op & 1) << 17) | (((uint)U & 1) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Vd & 31) << 0));
    //SVE bitwise shift by immediate (predicated)
    public void emit_9(int tszh, int opc, int L, int U, int Pg, int tszl, int imm3, int Zdn) => AddIns(0b00000100000000001000000000000000 | (((uint)tszh & 3) << 22) | (((uint)opc & 3) << 18) | (((uint)L & 1) << 17) | (((uint)U & 1) << 16) | (((uint)Pg & 7) << 10) | (((uint)tszl & 3) << 8) | (((uint)imm3 & 7) << 5) | (((uint)Zdn & 31) << 0));
    //SVE bitwise shift by vector (predicated)
    public void emit_10(int size, int R, int L, int U, int Pg, int Zm, int Zdn) => AddIns(0b00000100000100001000000000000000 | (((uint)size & 3) << 22) | (((uint)R & 1) << 18) | (((uint)L & 1) << 17) | (((uint)U & 1) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zm & 31) << 5) | (((uint)Zdn & 31) << 0));
    //SVE bitwise shift by wide elements (predicated)
    public void emit_11(int size, int R, int L, int U, int Pg, int Zm, int Zdn) => AddIns(0b00000100000110001000000000000000 | (((uint)size & 3) << 22) | (((uint)R & 1) << 18) | (((uint)L & 1) << 17) | (((uint)U & 1) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zm & 31) << 5) | (((uint)Zdn & 31) << 0));
    //Barriers
    public void emit_12(int CRm, int op2, int Rt) => AddIns(0b11010101000000110011000000000000 | (((uint)CRm & 15) << 8) | (((uint)op2 & 7) << 5) | (((uint)Rt & 31) << 0));
    //Compare and branch (immediate)
    public void emit_13(int sf, int op, int imm19, int Rt) => AddIns(0b00110100000000000000000000000000 | (((uint)sf & 1) << 31) | (((uint)op & 1) << 24) | (((uint)imm19 & 524287) << 5) | (((uint)Rt & 31) << 0));
    //Conditional branch (immediate)
    public void emit_14(int o1, int imm19, int o0, int cond) => AddIns(0b01010100000000000000000000000000 | (((uint)o1 & 1) << 24) | (((uint)imm19 & 524287) << 5) | (((uint)o0 & 1) << 4) | (((uint)cond & 15) << 0));
    //Exception generation
    public void emit_15(int opc, int imm16, int op2, int LL) => AddIns(0b11010100000000000000000000000000 | (((uint)opc & 7) << 21) | (((uint)imm16 & 65535) << 5) | (((uint)op2 & 7) << 2) | (((uint)LL & 3) << 0));
    //Hints
    public void emit_16(int CRm, int op2) => AddIns(0b11010101000000110010000000011111 | (((uint)CRm & 15) << 8) | (((uint)op2 & 7) << 5));
    //PSTATE
    public void emit_17(int op1, int CRm, int op2, int Rt) => AddIns(0b11010101000000000100000000000000 | (((uint)op1 & 7) << 16) | (((uint)CRm & 15) << 8) | (((uint)op2 & 7) << 5) | (((uint)Rt & 31) << 0));
    //System instructions
    public void emit_18(int L, int op1, int CRn, int CRm, int op2, int Rt) => AddIns(0b11010101000010000000000000000000 | (((uint)L & 1) << 21) | (((uint)op1 & 7) << 16) | (((uint)CRn & 15) << 12) | (((uint)CRm & 15) << 8) | (((uint)op2 & 7) << 5) | (((uint)Rt & 31) << 0));
    //System instructions with register argument
    public void emit_19(int CRm, int op2, int Rt) => AddIns(0b11010101000000110001000000000000 | (((uint)CRm & 15) << 8) | (((uint)op2 & 7) << 5) | (((uint)Rt & 31) << 0));
    //System register move
    public void emit_20(int L, int o0, int op1, int CRn, int CRm, int op2, int Rt) => AddIns(0b11010101000100000000000000000000 | (((uint)L & 1) << 21) | (((uint)o0 & 1) << 19) | (((uint)op1 & 7) << 16) | (((uint)CRn & 15) << 12) | (((uint)CRm & 15) << 8) | (((uint)op2 & 7) << 5) | (((uint)Rt & 31) << 0));
    //Test and branch (immediate)
    public void emit_21(int b5, int op, int b40, int imm14, int Rt) => AddIns(0b00110110000000000000000000000000 | (((uint)b5 & 1) << 31) | (((uint)op & 1) << 24) | (((uint)b40 & 31) << 19) | (((uint)imm14 & 16383) << 5) | (((uint)Rt & 31) << 0));
    //Unconditional branch (immediate)
    public void emit_22(int op, int imm26) => AddIns(0b00010100000000000000000000000000 | (((uint)op & 1) << 31) | (((uint)imm26 & 67108863) << 0));
    //Unconditional branch (register)
    public void emit_23(int opc, int op2, int op3, int Rn, int op4) => AddIns(0b11010110000000000000000000000000 | (((uint)opc & 15) << 21) | (((uint)op2 & 31) << 16) | (((uint)op3 & 63) << 10) | (((uint)Rn & 31) << 5) | (((uint)op4 & 31) << 0));
    //SVE bitwise unary operations (predicated)
    public void emit_24(int size, int opc, int Pg, int Zn, int Zd) => AddIns(0b00000100000110001010000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 7) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE integer unary operations (predicated)
    public void emit_25(int size, int opc, int Pg, int Zn, int Zd) => AddIns(0b00000100000100001010000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 7) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //Advanced SIMD load/store multiple structures
    public void emit_26(int Q, int L, int opcode, int size, int Rn, int Rt) => AddIns(0b00001100000000000000000000000000 | (((uint)Q & 1) << 30) | (((uint)L & 1) << 22) | (((uint)opcode & 15) << 12) | (((uint)size & 3) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //Advanced SIMD load/store multiple structures (post-indexed)
    public void emit_27(int Q, int L, int Rm, int opcode, int size, int Rn, int Rt) => AddIns(0b00001100100000000000000000000000 | (((uint)Q & 1) << 30) | (((uint)L & 1) << 22) | (((uint)Rm & 31) << 16) | (((uint)opcode & 15) << 12) | (((uint)size & 3) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //Advanced SIMD load/store single structure
    public void emit_28(int Q, int L, int R, int opcode, int S, int size, int Rn, int Rt) => AddIns(0b00001101000000000000000000000000 | (((uint)Q & 1) << 30) | (((uint)L & 1) << 22) | (((uint)R & 1) << 21) | (((uint)opcode & 7) << 13) | (((uint)S & 1) << 12) | (((uint)size & 3) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //Advanced SIMD load/store single structure (post-indexed)
    public void emit_29(int Q, int L, int R, int Rm, int opcode, int S, int size, int Rn, int Rt) => AddIns(0b00001101100000000000000000000000 | (((uint)Q & 1) << 30) | (((uint)L & 1) << 22) | (((uint)R & 1) << 21) | (((uint)Rm & 31) << 16) | (((uint)opcode & 7) << 13) | (((uint)S & 1) << 12) | (((uint)size & 3) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //Atomic memory operations
    public void emit_30(int size, int V, int A, int R, int Rs, int o3, int opc, int Rn, int Rt) => AddIns(0b00111000001000000000000000000000 | (((uint)size & 3) << 30) | (((uint)V & 1) << 26) | (((uint)A & 1) << 23) | (((uint)R & 1) << 22) | (((uint)Rs & 31) << 16) | (((uint)o3 & 1) << 15) | (((uint)opc & 7) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //Compare and swap
    public void emit_31(int size, int L, int Rs, int o0, int Rt2, int Rn, int Rt) => AddIns(0b00001000101000000000000000000000 | (((uint)size & 3) << 30) | (((uint)L & 1) << 22) | (((uint)Rs & 31) << 16) | (((uint)o0 & 1) << 15) | (((uint)Rt2 & 31) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //Compare and swap pair
    public void emit_32(int sz, int L, int Rs, int o0, int Rt2, int Rn, int Rt) => AddIns(0b00001000001000000000000000000000 | (((uint)sz & 1) << 30) | (((uint)L & 1) << 22) | (((uint)Rs & 31) << 16) | (((uint)o0 & 1) << 15) | (((uint)Rt2 & 31) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //LDAPR/STLR (unscaled immediate)
    public void emit_33(int size, int opc, int imm9, int Rn, int Rt) => AddIns(0b00011001000000000000000000000000 | (((uint)size & 3) << 30) | (((uint)opc & 3) << 22) | (((uint)imm9 & 511) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //Load register (literal)
    public void emit_34(int opc, int V, int imm19, int Rt) => AddIns(0b00011000000000000000000000000000 | (((uint)opc & 3) << 30) | (((uint)V & 1) << 26) | (((uint)imm19 & 524287) << 5) | (((uint)Rt & 31) << 0));
    //Load/store exclusive pair
    public void emit_35(int sz, int L, int Rs, int o0, int Rt2, int Rn, int Rt) => AddIns(0b10001000001000000000000000000000 | (((uint)sz & 1) << 30) | (((uint)L & 1) << 22) | (((uint)Rs & 31) << 16) | (((uint)o0 & 1) << 15) | (((uint)Rt2 & 31) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //Load/store exclusive register
    public void emit_36(int size, int L, int Rs, int o0, int Rt2, int Rn, int Rt) => AddIns(0b00001000000000000000000000000000 | (((uint)size & 3) << 30) | (((uint)L & 1) << 22) | (((uint)Rs & 31) << 16) | (((uint)o0 & 1) << 15) | (((uint)Rt2 & 31) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //Load/store memory tags
    public void emit_37(int opc, int imm9, int op2, int Rn, int Rt) => AddIns(0b11011001001000000000000000000000 | (((uint)opc & 3) << 22) | (((uint)imm9 & 511) << 12) | (((uint)op2 & 3) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));//Load/store no-allocate pair (offset)
    public void emit_38(int opc, int V, int L, int imm7, int Rt2, int Rn, int Rt) => AddIns(0b00101000000000000000000000000000 | (((uint)opc & 3) << 30) | (((uint)V & 1) << 26) | (((uint)L & 1) << 22) | (((uint)imm7 & 127) << 15) | (((uint)Rt2 & 31) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //Load/store ordered
    public void emit_39(int size, int L, int Rs, int o0, int Rt2, int Rn, int Rt) => AddIns(0b00001000100000000000000000000000 | (((uint)size & 3) << 30) | (((uint)L & 1) << 22) | (((uint)Rs & 31) << 16) | (((uint)o0 & 1) << 15) | (((uint)Rt2 & 31) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //Load/store register (immediate post-indexed)
    public void emit_40(int size, int V, int opc, int imm9, int Rn, int Rt) => AddIns(0b00111000000000000000010000000000 | (((uint)size & 3) << 30) | (((uint)V & 1) << 26) | (((uint)opc & 3) << 22) | (((uint)imm9 & 511) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //Load/store register (immediate pre-indexed)
    public void emit_41(int size, int V, int opc, int imm9, int Rn, int Rt) => AddIns(0b00111000000000000000110000000000 | (((uint)size & 3) << 30) | (((uint)V & 1) << 26) | (((uint)opc & 3) << 22) | (((uint)imm9 & 511) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //Load/store register (pac)
    public void emit_42(int size, int V, int M, int S, int imm9, int W, int Rn, int Rt) => AddIns(0b00111000001000000000010000000000 | (((uint)size & 3) << 30) | (((uint)V & 1) << 26) | (((uint)M & 1) << 23) | (((uint)S & 1) << 22) | (((uint)imm9 & 511) << 12) | (((uint)W & 1) << 11) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //Load/store register (register offset)
    public void emit_43(int size, int V, int opc, int Rm, int option, int S, int Rn, int Rt) => AddIns(0b00111000001000000000100000000000 | (((uint)size & 3) << 30) | (((uint)V & 1) << 26) | (((uint)opc & 3) << 22) | (((uint)Rm & 31) << 16) | (((uint)option & 7) << 13) | (((uint)S & 1) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //Load/store register (unprivileged)
    public void emit_44(int size, int V, int opc, int imm9, int Rn, int Rt) => AddIns(0b00111000000000000000100000000000 | (((uint)size & 3) << 30) | (((uint)V & 1) << 26) | (((uint)opc & 3) << 22) | (((uint)imm9 & 511) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //Load/store register (unscaled immediate)
    public void emit_45(int size, int V, int opc, int imm9, int Rn, int Rt) => AddIns(0b00111000000000000000000000000000 | (((uint)size & 3) << 30) | (((uint)V & 1) << 26) | (((uint)opc & 3) << 22) | (((uint)imm9 & 511) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //Load/store register (unsigned immediate)
    public void emit_46(int size, int V, int opc, int imm12, int Rn, int Rt) => AddIns(0b00111001000000000000000000000000 | (((uint)size & 3) << 30) | (((uint)V & 1) << 26) | (((uint)opc & 3) << 22) | (((uint)imm12 & 4095) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //Load/store register pair (offset)
    public void emit_47(int opc, int V, int L, int imm7, int Rt2, int Rn, int Rt) => AddIns(0b00101001000000000000000000000000 | (((uint)opc & 3) << 30) | (((uint)V & 1) << 26) | (((uint)L & 1) << 22) | (((uint)imm7 & 127) << 15) | (((uint)Rt2 & 31) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //Load/store register pair (post-indexed)
    public void emit_48(int opc, int V, int L, int imm7, int Rt2, int Rn, int Rt) => AddIns(0b00101000100000000000000000000000 | (((uint)opc & 3) << 30) | (((uint)V & 1) << 26) | (((uint)L & 1) << 22) | (((uint)imm7 & 127) << 15) | (((uint)Rt2 & 31) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //Load/store register pair (pre-indexed)
    public void emit_49(int opc, int V, int L, int imm7, int Rt2, int Rn, int Rt) => AddIns(0b00101001100000000000000000000000 | (((uint)opc & 3) << 30) | (((uint)V & 1) << 26) | (((uint)L & 1) << 22) | (((uint)imm7 & 127) << 15) | (((uint)Rt2 & 31) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rt & 31) << 0));
    //SVE integer multiply-accumulate writing addend (predicated)
    public void emit_50(int size, int Zm, int op, int Pg, int Zn, int Zda) => AddIns(0b00000100000000000100000000000000 | (((uint)size & 3) << 22) | (((uint)Zm & 31) << 16) | (((uint)op & 1) << 13) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zda & 31) << 0));
    //SVE integer multiply-add writing multiplicand (predicated)
    public void emit_51(int size, int Zm, int op, int Pg, int Za, int Zdn) => AddIns(0b00000100000000001100000000000000 | (((uint)size & 3) << 22) | (((uint)Zm & 31) << 16) | (((uint)op & 1) << 13) | (((uint)Pg & 7) << 10) | (((uint)Za & 31) << 5) | (((uint)Zdn & 31) << 0));
    //SVE integer add/subtract vectors (unpredicated)
    public void emit_52(int size, int Zm, int opc, int Zn, int Zd) => AddIns(0b00000100001000000000000000000000 | (((uint)size & 3) << 22) | (((uint)Zm & 31) << 16) | (((uint)opc & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //Add/subtract (immediate)
    public void emit_53(int sf, int op, int S, int sh, int imm12, int Rn, int Rd) => AddIns(0b00010001000000000000000000000000 | (((uint)sf & 1) << 31) | (((uint)op & 1) << 30) | (((uint)S & 1) << 29) | (((uint)sh & 1) << 22) | (((uint)imm12 & 4095) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Add/subtract (immediate, with tags)
    public void emit_54(int sf, int op, int S, int o2, int uimm6, int op3, int uimm4, int Rn, int Rd) => AddIns(0b00010001100000000000000000000000 | (((uint)sf & 1) << 31) | (((uint)op & 1) << 30) | (((uint)S & 1) << 29) | (((uint)o2 & 1) << 22) | (((uint)uimm6 & 63) << 16) | (((uint)op3 & 3) << 14) | (((uint)uimm4 & 15) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Bitfield
    public void emit_55(int sf, int opc, int N, int immr, int imms, int Rn, int Rd) => AddIns(0b00010011000000000000000000000000 | (((uint)sf & 1) << 31) | (((uint)opc & 3) << 29) | (((uint)N & 1) << 22) | (((uint)immr & 63) << 16) | (((uint)imms & 63) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Extract
    public void emit_56(int sf, int op21, int N, int o0, int Rm, int imms, int Rn, int Rd) => AddIns(0b00010011100000000000000000000000 | (((uint)sf & 1) << 31) | (((uint)op21 & 3) << 29) | (((uint)N & 1) << 22) | (((uint)o0 & 1) << 21) | (((uint)Rm & 31) << 16) | (((uint)imms & 63) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Logical (immediate)
    public void emit_57(int sf, int opc, int N, int immr, int imms, int Rn, int Rd) => AddIns(0b00010010000000000000000000000000 | (((uint)sf & 1) << 31) | (((uint)opc & 3) << 29) | (((uint)N & 1) << 22) | (((uint)immr & 63) << 16) | (((uint)imms & 63) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Move wide (immediate)
    public void emit_58(int sf, int opc, int hw, int imm16, int Rd) => AddIns(0b00010010100000000000000000000000 | (((uint)sf & 1) << 31) | (((uint)opc & 3) << 29) | (((uint)hw & 3) << 21) | (((uint)imm16 & 65535) << 5) | (((uint)Rd & 31) << 0));
    //PC-rel. addressing
    public void emit_59(int op, int immlo, int immhi, int Rd) => AddIns(0b00010000000000000000000000000000 | (((uint)op & 1) << 31) | (((uint)immlo & 3) << 29) | (((uint)immhi & 524287) << 5) | (((uint)Rd & 31) << 0));
    //SVE bitwise logical operations (unpredicated)
    public void emit_60(int opc, int Zm, int Zn, int Zd) => AddIns(0b00000100001000000011000000000000 | (((uint)opc & 3) << 22) | (((uint)Zm & 31) << 16) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE index generation (immediate start, immediate increment)
    public void emit_61(int size, int imm5b, int imm5, int Zd) => AddIns(0b00000100001000000100000000000000 | (((uint)size & 3) << 22) | (((uint)imm5b & 31) << 16) | (((uint)imm5 & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE index generation (immediate start, register increment)
    public void emit_62(int size, int Rm, int imm5, int Zd) => AddIns(0b00000100001000000100100000000000 | (((uint)size & 3) << 22) | (((uint)Rm & 31) << 16) | (((uint)imm5 & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE index generation (register start, immediate increment)
    public void emit_63(int size, int imm5, int Rn, int Zd) => AddIns(0b00000100001000000100010000000000 | (((uint)size & 3) << 22) | (((uint)imm5 & 31) << 16) | (((uint)Rn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE index generation (register start, register increment)
    public void emit_64(int size, int Rm, int Rn, int Zd) => AddIns(0b00000100001000000100110000000000 | (((uint)size & 3) << 22) | (((uint)Rm & 31) << 16) | (((uint)Rn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE stack frame adjustment
    public void emit_65(int op, int Rn, int imm6, int Rd) => AddIns(0b00000100001000000101000000000000 | (((uint)op & 1) << 22) | (((uint)Rn & 31) << 16) | (((uint)imm6 & 63) << 5) | (((uint)Rd & 31) << 0));
    //SVE stack frame size
    public void emit_66(int op, int opc2, int imm6, int Rd) => AddIns(0b00000100101000000101000000000000 | (((uint)op & 1) << 22) | (((uint)opc2 & 31) << 16) | (((uint)imm6 & 63) << 5) | (((uint)Rd & 31) << 0));
    //Add/subtract (extended register)
    public void emit_67(int sf, int op, int S, int opt, int Rm, int option, int imm3, int Rn, int Rd) => AddIns(0b00001011001000000000000000000000 | (((uint)sf & 1) << 31) | (((uint)op & 1) << 30) | (((uint)S & 1) << 29) | (((uint)opt & 3) << 22) | (((uint)Rm & 31) << 16) | (((uint)option & 7) << 13) | (((uint)imm3 & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Add/subtract (shifted register)
    public void emit_68(int sf, int op, int S, int shift, int Rm, int imm6, int Rn, int Rd) => AddIns(0b00001011000000000000000000000000 | (((uint)sf & 1) << 31) | (((uint)op & 1) << 30) | (((uint)S & 1) << 29) | (((uint)shift & 3) << 22) | (((uint)Rm & 31) << 16) | (((uint)imm6 & 63) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Add/subtract (with carry)
    public void emit_69(int sf, int op, int S, int Rm, int Rn, int Rd) => AddIns(0b00011010000000000000000000000000 | (((uint)sf & 1) << 31) | (((uint)op & 1) << 30) | (((uint)S & 1) << 29) | (((uint)Rm & 31) << 16) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Conditional compare (immediate)
    public void emit_70(int sf, int op, int S, int imm5, int cond, int o2, int Rn, int o3, int nzcv) => AddIns(0b00011010010000000000100000000000 | (((uint)sf & 1) << 31) | (((uint)op & 1) << 30) | (((uint)S & 1) << 29) | (((uint)imm5 & 31) << 16) | (((uint)cond & 15) << 12) | (((uint)o2 & 1) << 10) | (((uint)Rn & 31) << 5) | (((uint)o3 & 1) << 4) | (((uint)nzcv & 15) << 0));
    //Conditional compare (register)
    public void emit_71(int sf, int op, int S, int Rm, int cond, int o2, int Rn, int o3, int nzcv) => AddIns(0b00011010010000000000000000000000 | (((uint)sf & 1) << 31) | (((uint)op & 1) << 30) | (((uint)S & 1) << 29) | (((uint)Rm & 31) << 16) | (((uint)cond & 15) << 12) | (((uint)o2 & 1) << 10) | (((uint)Rn & 31) << 5) | (((uint)o3 & 1) << 4) | (((uint)nzcv & 15) << 0));
    //Conditional select
    public void emit_72(int sf, int op, int S, int Rm, int cond, int op2, int Rn, int Rd) => AddIns(0b00011010100000000000000000000000 | (((uint)sf & 1) << 31) | (((uint)op & 1) << 30) | (((uint)S & 1) << 29) | (((uint)Rm & 31) << 16) | (((uint)cond & 15) << 12) | (((uint)op2 & 3) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Data-processing (1 source)
    public void emit_73(int sf, int S, int opcode2, int opcode, int Rn, int Rd) => AddIns(0b01011010110000000000000000000000 | (((uint)sf & 1) << 31) | (((uint)S & 1) << 29) | (((uint)opcode2 & 31) << 16) | (((uint)opcode & 63) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Data-processing (2 source)
    public void emit_74(int sf, int S, int Rm, int opcode, int Rn, int Rd) => AddIns(0b00011010110000000000000000000000 | (((uint)sf & 1) << 31) | (((uint)S & 1) << 29) | (((uint)Rm & 31) << 16) | (((uint)opcode & 63) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Data-processing (3 source)
    public void emit_75(int sf, int op54, int op31, int Rm, int o0, int Ra, int Rn, int Rd) => AddIns(0b00011011000000000000000000000000 | (((uint)sf & 1) << 31) | (((uint)op54 & 3) << 29) | (((uint)op31 & 7) << 21) | (((uint)Rm & 31) << 16) | (((uint)o0 & 1) << 15) | (((uint)Ra & 31) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Evaluate into flags
    public void emit_76(int sf, int op, int S, int opcode2, int sz, int Rn, int o3, int mask) => AddIns(0b00011010000000000000100000000000 | (((uint)sf & 1) << 31) | (((uint)op & 1) << 30) | (((uint)S & 1) << 29) | (((uint)opcode2 & 63) << 15) | (((uint)sz & 1) << 14) | (((uint)Rn & 31) << 5) | (((uint)o3 & 1) << 4) | (((uint)mask & 15) << 0));
    //Logical (shifted register)
    public void emit_77(int sf, int opc, int shift, int N, int Rm, int imm6, int Rn, int Rd) => AddIns(0b00001010000000000000000000000000 | (((uint)sf & 1) << 31) | (((uint)opc & 3) << 29) | (((uint)shift & 3) << 22) | (((uint)N & 1) << 21) | (((uint)Rm & 31) << 16) | (((uint)imm6 & 63) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Rotate right into flags
    public void emit_78(int sf, int op, int S, int imm6, int Rn, int o2, int mask) => AddIns(0b00011010000000000000010000000000 | (((uint)sf & 1) << 31) | (((uint)op & 1) << 30) | (((uint)S & 1) << 29) | (((uint)imm6 & 63) << 15) | (((uint)Rn & 31) << 5) | (((uint)o2 & 1) << 4) | (((uint)mask & 15) << 0));
    //SVE bitwise shift by immediate (unpredicated)
    public void emit_79(int tszh, int tszl, int imm3, int opc, int Zn, int Zd) => AddIns(0b00000100001000001001000000000000 | (((uint)tszh & 3) << 22) | (((uint)tszl & 3) << 19) | (((uint)imm3 & 7) << 16) | (((uint)opc & 3) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE bitwise shift by wide elements (unpredicated)
    public void emit_80(int size, int Zm, int opc, int Zn, int Zd) => AddIns(0b00000100001000001000000000000000 | (((uint)size & 3) << 22) | (((uint)Zm & 31) << 16) | (((uint)opc & 3) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //Advanced SIMD across lanes
    public void emit_81(int Q, int U, int size, int opcode, int Rn, int Rd) => AddIns(0b00001110001100000000100000000000 | (((uint)Q & 1) << 30) | (((uint)U & 1) << 29) | (((uint)size & 3) << 22) | (((uint)opcode & 31) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD copy
    public void emit_82(int Q, int op, int imm5, int imm4, int Rn, int Rd) => AddIns(0b00001110000000000000010000000000 | (((uint)Q & 1) << 30) | (((uint)op & 1) << 29) | (((uint)imm5 & 31) << 16) | (((uint)imm4 & 15) << 11) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD extract
    public void emit_83(int Q, int op2, int Rm, int imm4, int Rn, int Rd) => AddIns(0b00101110000000000000000000000000 | (((uint)Q & 1) << 30) | (((uint)op2 & 3) << 22) | (((uint)Rm & 31) << 16) | (((uint)imm4 & 15) << 11) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD modified immediate
    public void emit_84(int Q, int op, int a, int b, int c, int cmode, int o2, int d, int e, int f, int g, int h, int Rd) => AddIns(0b00001111000000000000010000000000 | (((uint)Q & 1) << 30) | (((uint)op & 1) << 29) | (((uint)a & 1) << 18) | (((uint)b & 1) << 17) | (((uint)c & 1) << 16) | (((uint)cmode & 15) << 12) | (((uint)o2 & 1) << 11) | (((uint)d & 1) << 9) | (((uint)e & 1) << 8) | (((uint)f & 1) << 7) | (((uint)g & 1) << 6) | (((uint)h & 1) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD permute
    public void emit_85(int Q, int size, int Rm, int opcode, int Rn, int Rd) => AddIns(0b00001110000000000000100000000000 | (((uint)Q & 1) << 30) | (((uint)size & 3) << 22) | (((uint)Rm & 31) << 16) | (((uint)opcode & 7) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD scalar copy
    public void emit_86(int op, int imm5, int imm4, int Rn, int Rd) => AddIns(0b01011110000000000000010000000000 | (((uint)op & 1) << 29) | (((uint)imm5 & 31) << 16) | (((uint)imm4 & 15) << 11) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));//Advanced SIMD scalar pairwise
    public void emit_87(int U, int size, int opcode, int Rn, int Rd) => AddIns(0b01011110001100000000100000000000 | (((uint)U & 1) << 29) | (((uint)size & 3) << 22) | (((uint)opcode & 31) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD scalar shift by immediate
    public void emit_88(int U, int immh, int immb, int opcode, int Rn, int Rd) => AddIns(0b01011111000000000000010000000000 | (((uint)U & 1) << 29) | (((uint)immh & 15) << 19) | (((uint)immb & 7) << 16) | (((uint)opcode & 31) << 11) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD scalar three different
    public void emit_89(int U, int size, int Rm, int opcode, int Rn, int Rd) => AddIns(0b01011110001000000000000000000000 | (((uint)U & 1) << 29) | (((uint)size & 3) << 22) | (((uint)Rm & 31) << 16) | (((uint)opcode & 15) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD scalar three same
    public void emit_90(int U, int size, int Rm, int opcode, int Rn, int Rd) => AddIns(0b01011110001000000000010000000000 | (((uint)U & 1) << 29) | (((uint)size & 3) << 22) | (((uint)Rm & 31) << 16) | (((uint)opcode & 31) << 11) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD scalar three same FP16
    public void emit_91(int U, int a, int Rm, int opcode, int Rn, int Rd) => AddIns(0b01011110010000000000010000000000 | (((uint)U & 1) << 29) | (((uint)a & 1) << 23) | (((uint)Rm & 31) << 16) | (((uint)opcode & 7) << 11) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD scalar three same extra
    public void emit_92(int U, int size, int Rm, int opcode, int Rn, int Rd) => AddIns(0b01011110000000001000010000000000 | (((uint)U & 1) << 29) | (((uint)size & 3) << 22) | (((uint)Rm & 31) << 16) | (((uint)opcode & 15) << 11) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD scalar two-register miscellaneous
    public void emit_93(int U, int size, int opcode, int Rn, int Rd) => AddIns(0b01011110001000000000100000000000 | (((uint)U & 1) << 29) | (((uint)size & 3) << 22) | (((uint)opcode & 31) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD scalar two-register miscellaneous FP16
    public void emit_94(int U, int a, int opcode, int Rn, int Rd) => AddIns(0b01011110011110000000100000000000 | (((uint)U & 1) << 29) | (((uint)a & 1) << 23) | (((uint)opcode & 31) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD scalar x indexed element
    public void emit_95(int U, int size, int L, int M, int Rm, int opcode, int H, int Rn, int Rd) => AddIns(0b01011111000000000000000000000000 | (((uint)U & 1) << 29) | (((uint)size & 3) << 22) | (((uint)L & 1) << 21) | (((uint)M & 1) << 20) | (((uint)Rm & 15) << 16) | (((uint)opcode & 15) << 12) | (((uint)H & 1) << 11) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD shift by immediate
    public void emit_96(int Q, int U, int immh, int immb, int opcode, int Rn, int Rd) => AddIns(0b00001111000000000000010000000000 | (((uint)Q & 1) << 30) | (((uint)U & 1) << 29) | (((uint)immh & 15) << 19) | (((uint)immb & 7) << 16) | (((uint)opcode & 31) << 11) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD table lookup
    public void emit_97(int Q, int op2, int Rm, int len, int op, int Rn, int Rd) => AddIns(0b00001110000000000000000000000000 | (((uint)Q & 1) << 30) | (((uint)op2 & 3) << 22) | (((uint)Rm & 31) << 16) | (((uint)len & 3) << 13) | (((uint)op & 1) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD three different
    public void emit_98(int Q, int U, int size, int Rm, int opcode, int Rn, int Rd) => AddIns(0b00001110001000000000000000000000 | (((uint)Q & 1) << 30) | (((uint)U & 1) << 29) | (((uint)size & 3) << 22) | (((uint)Rm & 31) << 16) | (((uint)opcode & 15) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD three same
    public void emit_99(int Q, int U, int size, int Rm, int opcode, int Rn, int Rd) => AddIns(0b00001110001000000000010000000000 | (((uint)Q & 1) << 30) | (((uint)U & 1) << 29) | (((uint)size & 3) << 22) | (((uint)Rm & 31) << 16) | (((uint)opcode & 31) << 11) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD three same (FP16)
    public void emit_100(int Q, int U, int a, int Rm, int opcode, int Rn, int Rd) => AddIns(0b00001110010000000000010000000000 | (((uint)Q & 1) << 30) | (((uint)U & 1) << 29) | (((uint)a & 1) << 23) | (((uint)Rm & 31) << 16) | (((uint)opcode & 7) << 11) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD three-register extension
    public void emit_101(int Q, int U, int size, int Rm, int opcode, int Rn, int Rd) => AddIns(0b00001110000000001000010000000000 | (((uint)Q & 1) << 30) | (((uint)U & 1) << 29) | (((uint)size & 3) << 22) | (((uint)Rm & 31) << 16) | (((uint)opcode & 15) << 11) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD two-register miscellaneous
    public void emit_102(int Q, int U, int size, int opcode, int Rn, int Rd) => AddIns(0b00001110001000000000100000000000 | (((uint)Q & 1) << 30) | (((uint)U & 1) << 29) | (((uint)size & 3) << 22) | (((uint)opcode & 31) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD two-register miscellaneous (FP16)
    public void emit_103(int Q, int U, int a, int opcode, int Rn, int Rd) => AddIns(0b00001110011110000000100000000000 | (((uint)Q & 1) << 30) | (((uint)U & 1) << 29) | (((uint)a & 1) << 23) | (((uint)opcode & 31) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Advanced SIMD vector x indexed element
    public void emit_104(int Q, int U, int size, int L, int M, int Rm, int opcode, int H, int Rn, int Rd) => AddIns(0b00001111000000000000000000000000 | (((uint)Q & 1) << 30) | (((uint)U & 1) << 29) | (((uint)size & 3) << 22) | (((uint)L & 1) << 21) | (((uint)M & 1) << 20) | (((uint)Rm & 15) << 16) | (((uint)opcode & 15) << 12) | (((uint)H & 1) << 11) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Conversion between floating-point and fixed-point
    public void emit_105(int sf, int S, int ptype, int rmode, int opcode, int scale, int Rn, int Rd) => AddIns(0b00011110000000000000000000000000 | (((uint)sf & 1) << 31) | (((uint)S & 1) << 29) | (((uint)ptype & 3) << 22) | (((uint)rmode & 3) << 19) | (((uint)opcode & 7) << 16) | (((uint)scale & 63) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Conversion between floating-point and integer
    public void emit_106(int sf, int S, int ptype, int rmode, int opcode, int Rn, int Rd) => AddIns(0b00011110001000000000000000000000 | (((uint)sf & 1) << 31) | (((uint)S & 1) << 29) | (((uint)ptype & 3) << 22) | (((uint)rmode & 3) << 19) | (((uint)opcode & 7) << 16) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Cryptographic AES
    public void emit_107(int size, int opcode, int Rn, int Rd) => AddIns(0b01001110001010000000100000000000 | (((uint)size & 3) << 22) | (((uint)opcode & 31) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Cryptographic four-register
    public void emit_108(int Op0, int Rm, int Ra, int Rn, int Rd) => AddIns(0b11001110000000000000000000000000 | (((uint)Op0 & 3) << 21) | (((uint)Rm & 31) << 16) | (((uint)Ra & 31) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Cryptographic three-register SHA
    public void emit_109(int size, int Rm, int opcode, int Rn, int Rd) => AddIns(0b01011110000000000000000000000000 | (((uint)size & 3) << 22) | (((uint)Rm & 31) << 16) | (((uint)opcode & 7) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Cryptographic three-register SHA 512
    public void emit_110(int Rm, int O, int opcode, int Rn, int Rd) => AddIns(0b11001110011000001000000000000000 | (((uint)Rm & 31) << 16) | (((uint)O & 1) << 14) | (((uint)opcode & 3) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Cryptographic three-register, imm2
    public void emit_111(int Rm, int imm2, int opcode, int Rn, int Rd) => AddIns(0b11001110010000001000000000000000 | (((uint)Rm & 31) << 16) | (((uint)imm2 & 3) << 12) | (((uint)opcode & 3) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Cryptographic three-register, imm6
    public void emit_112(int Rm, int imm6, int Rn, int Rd) => AddIns(0b11001110100000000000000000000000 | (((uint)Rm & 31) << 16) | (((uint)imm6 & 63) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Cryptographic two-register SHA
    public void emit_113(int size, int opcode, int Rn, int Rd) => AddIns(0b01011110001010000000100000000000 | (((uint)size & 3) << 22) | (((uint)opcode & 31) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Cryptographic two-register SHA 512
    public void emit_114(int opcode, int Rn, int Rd) => AddIns(0b11001110110000001000000000000000 | (((uint)opcode & 3) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Floating-point compare
    public void emit_115(int M, int S, int ptype, int Rm, int op, int Rn, int opcode2) => AddIns(0b00011110001000000010000000000000 | (((uint)M & 1) << 31) | (((uint)S & 1) << 29) | (((uint)ptype & 3) << 22) | (((uint)Rm & 31) << 16) | (((uint)op & 3) << 14) | (((uint)Rn & 31) << 5) | (((uint)opcode2 & 31) << 0));
    //Floating-point conditional compare
    public void emit_116(int M, int S, int ptype, int Rm, int cond, int Rn, int op, int nzcv) => AddIns(0b00011110001000000000010000000000 | (((uint)M & 1) << 31) | (((uint)S & 1) << 29) | (((uint)ptype & 3) << 22) | (((uint)Rm & 31) << 16) | (((uint)cond & 15) << 12) | (((uint)Rn & 31) << 5) | (((uint)op & 1) << 4) | (((uint)nzcv & 15) << 0));
    //Floating-point conditional select
    public void emit_117(int M, int S, int ptype, int Rm, int cond, int Rn, int Rd) => AddIns(0b00011110001000000000110000000000 | (((uint)M & 1) << 31) | (((uint)S & 1) << 29) | (((uint)ptype & 3) << 22) | (((uint)Rm & 31) << 16) | (((uint)cond & 15) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Floating-point data-processing (1 source)
    public void emit_118(int M, int S, int ptype, int opcode, int Rn, int Rd) => AddIns(0b00011110001000000100000000000000 | (((uint)M & 1) << 31) | (((uint)S & 1) << 29) | (((uint)ptype & 3) << 22) | (((uint)opcode & 63) << 15) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Floating-point data-processing (2 source)
    public void emit_119(int M, int S, int ptype, int Rm, int opcode, int Rn, int Rd) => AddIns(0b00011110001000000000100000000000 | (((uint)M & 1) << 31) | (((uint)S & 1) << 29) | (((uint)ptype & 3) << 22) | (((uint)Rm & 31) << 16) | (((uint)opcode & 15) << 12) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Floating-point data-processing (3 source)
    public void emit_120(int M, int S, int ptype, int o1, int Rm, int o0, int Ra, int Rn, int Rd) => AddIns(0b00011111000000000000000000000000 | (((uint)M & 1) << 31) | (((uint)S & 1) << 29) | (((uint)ptype & 3) << 22) | (((uint)o1 & 1) << 21) | (((uint)Rm & 31) << 16) | (((uint)o0 & 1) << 15) | (((uint)Ra & 31) << 10) | (((uint)Rn & 31) << 5) | (((uint)Rd & 31) << 0));
    //Floating-point immediate
    public void emit_121(int M, int S, int ptype, int imm8, int imm5, int Rd) => AddIns(0b00011110001000000001000000000000 | (((uint)M & 1) << 31) | (((uint)S & 1) << 29) | (((uint)ptype & 3) << 22) | (((uint)imm8 & 255) << 13) | (((uint)imm5 & 31) << 5) | (((uint)Rd & 31) << 0));
    //SVE address generation
    public void emit_122(int opc, int Zm, int msz, int Zn, int Zd) => AddIns(0b00000100001000001010000000000000 | (((uint)opc & 3) << 22) | (((uint)Zm & 31) << 16) | (((uint)msz & 3) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //Reserved
    public void emit_123(int imm16) => AddIns(0b00000000000000000000000000000000 | (((uint)imm16 & 65535) << 0));
    //SVE constructive prefix (unpredicated)
    public void emit_124(int opc, int opc2, int Zn, int Zd) => AddIns(0b00000100001000001011110000000000 | (((uint)opc & 3) << 22) | (((uint)opc2 & 31) << 16) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE floating-point exponential accelerator
    public void emit_125(int size, int opc, int Zn, int Zd) => AddIns(0b00000100001000001011100000000000 | (((uint)size & 3) << 22) | (((uint)opc & 31) << 16) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE floating-point trig select coefficient
    public void emit_126(int size, int Zm, int op, int Zn, int Zd) => AddIns(0b00000100001000001011000000000000 | (((uint)size & 3) << 22) | (((uint)Zm & 31) << 16) | (((uint)op & 1) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE element count
    public void emit_127(int size, int imm4, int op, int pattern, int Rd) => AddIns(0b00000100001000001110000000000000 | (((uint)size & 3) << 22) | (((uint)imm4 & 15) << 16) | (((uint)op & 1) << 10) | (((uint)pattern & 31) << 5) | (((uint)Rd & 31) << 0));
    //SVE inc/dec register by element count
    public void emit_128(int size, int imm4, int D, int pattern, int Rdn) => AddIns(0b00000100001100001110000000000000 | (((uint)size & 3) << 22) | (((uint)imm4 & 15) << 16) | (((uint)D & 1) << 10) | (((uint)pattern & 31) << 5) | (((uint)Rdn & 31) << 0));
    //SVE inc/dec vector by element count
    public void emit_129(int size, int imm4, int D, int pattern, int Zdn) => AddIns(0b00000100001100001100000000000000 | (((uint)size & 3) << 22) | (((uint)imm4 & 15) << 16) | (((uint)D & 1) << 10) | (((uint)pattern & 31) << 5) | (((uint)Zdn & 31) << 0));
    //SVE saturating inc/dec register by element count
    public void emit_130(int size, int sf, int imm4, int D, int U, int pattern, int Rdn) => AddIns(0b00000100001000001111000000000000 | (((uint)size & 3) << 22) | (((uint)sf & 1) << 20) | (((uint)imm4 & 15) << 16) | (((uint)D & 1) << 11) | (((uint)U & 1) << 10) | (((uint)pattern & 31) << 5) | (((uint)Rdn & 31) << 0));
    //SVE saturating inc/dec vector by element count
    public void emit_131(int size, int imm4, int D, int U, int pattern, int Zdn) => AddIns(0b00000100001000001100000000000000 | (((uint)size & 3) << 22) | (((uint)imm4 & 15) << 16) | (((uint)D & 1) << 11) | (((uint)U & 1) << 10) | (((uint)pattern & 31) << 5) | (((uint)Zdn & 31) << 0));
    //SVE extract vector (immediate offset, destructive)
    public void emit_132(int imm8h, int imm8l, int Zm, int Zdn) => AddIns(0b00000101001000000000000000000000 | (((uint)imm8h & 31) << 16) | (((uint)imm8l & 7) << 10) | (((uint)Zm & 31) << 5) | (((uint)Zdn & 31) << 0));
    //SVE permute vector segments
    public void emit_133(int op, int Zm, int opc2, int Zn, int Zd) => AddIns(0b00000101101000000000000000000000 | (((uint)op & 1) << 22) | (((uint)Zm & 31) << 16) | (((uint)opc2 & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE bitwise logical with immediate (unpredicated)
    public void emit_134(int opc, int imm13, int Zdn) => AddIns(0b00000101000000000000000000000000 | (((uint)opc & 3) << 22) | (((uint)imm13 & 8191) << 5) | (((uint)Zdn & 31) << 0));
    //SVE broadcast bitmask immediate
    public void emit_135(int imm13, int Zd) => AddIns(0b00000101110000000000000000000000 | (((uint)imm13 & 8191) << 5) | (((uint)Zd & 31) << 0));
    //SVE copy floating-point immediate (predicated)
    public void emit_136(int size, int Pg, int imm8, int Zd) => AddIns(0b00000101000100001100000000000000 | (((uint)size & 3) << 22) | (((uint)Pg & 15) << 16) | (((uint)imm8 & 255) << 5) | (((uint)Zd & 31) << 0));
    //SVE copy integer immediate (predicated)
    public void emit_137(int size, int Pg, int M, int sh, int imm8, int Zd) => AddIns(0b00000101000100000000000000000000 | (((uint)size & 3) << 22) | (((uint)Pg & 15) << 16) | (((uint)M & 1) << 14) | (((uint)sh & 1) << 13) | (((uint)imm8 & 255) << 5) | (((uint)Zd & 31) << 0));
    //SVE broadcast indexed element
    public void emit_138(int imm2, int tsz, int Zn, int Zd) => AddIns(0b00000101001000000010000000000000 | (((uint)imm2 & 3) << 22) | (((uint)tsz & 31) << 16) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE table lookup
    public void emit_139(int size, int Zm, int Zn, int Zd) => AddIns(0b00000101001000000011000000000000 | (((uint)size & 3) << 22) | (((uint)Zm & 31) << 16) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE broadcast general register
    public void emit_140(int size, int Rn, int Zd) => AddIns(0b00000101001000000011100000000000 | (((uint)size & 3) << 22) | (((uint)Rn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE insert SIMD&FP scalar register
    public void emit_141(int size, int Vm, int Zdn) => AddIns(0b00000101001101000011100000000000 | (((uint)size & 3) << 22) | (((uint)Vm & 31) << 5) | (((uint)Zdn & 31) << 0));
    //SVE insert general register
    public void emit_142(int size, int Rm, int Zdn) => AddIns(0b00000101001001000011100000000000 | (((uint)size & 3) << 22) | (((uint)Rm & 31) << 5) | (((uint)Zdn & 31) << 0));
    //SVE reverse vector elements
    public void emit_143(int size, int Zn, int Zd) => AddIns(0b00000101001110000011100000000000 | (((uint)size & 3) << 22) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE unpack vector elements
    public void emit_144(int size, int U, int H, int Zn, int Zd) => AddIns(0b00000101001100000011100000000000 | (((uint)size & 3) << 22) | (((uint)U & 1) << 17) | (((uint)H & 1) << 16) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE permute predicate elements
    public void emit_145(int size, int Pm, int opc, int H, int Pn, int Pd) => AddIns(0b00000101001000000100000000000000 | (((uint)size & 3) << 22) | (((uint)Pm & 15) << 16) | (((uint)opc & 3) << 11) | (((uint)H & 1) << 10) | (((uint)Pn & 15) << 5) | (((uint)Pd & 15) << 0));
    //SVE reverse predicate elements
    public void emit_146(int size, int Pn, int Pd) => AddIns(0b00000101001101000100000000000000 | (((uint)size & 3) << 22) | (((uint)Pn & 15) << 5) | (((uint)Pd & 15) << 0));
    //SVE unpack predicate elements
    public void emit_147(int H, int Pn, int Pd) => AddIns(0b00000101001100000100000000000000 | (((uint)H & 1) << 16) | (((uint)Pn & 15) << 5) | (((uint)Pd & 15) << 0));
    //SVE permute vector elements
    public void emit_148(int size, int Zm, int opc, int Zn, int Zd) => AddIns(0b00000101001000000110000000000000 | (((uint)size & 3) << 22) | (((uint)Zm & 31) << 16) | (((uint)opc & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE compress active elements
    public void emit_149(int size, int Pg, int Zn, int Zd) => AddIns(0b00000101001000011000000000000000 | (((uint)size & 3) << 22) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE conditionally broadcast element to vector
    public void emit_150(int size, int B, int Pg, int Zm, int Zdn) => AddIns(0b00000101001010001000000000000000 | (((uint)size & 3) << 22) | (((uint)B & 1) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zm & 31) << 5) | (((uint)Zdn & 31) << 0));
    //SVE conditionally extract element to SIMD&FP scalar
    public void emit_151(int size, int B, int Pg, int Zm, int Vdn) => AddIns(0b00000101001010101000000000000000 | (((uint)size & 3) << 22) | (((uint)B & 1) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zm & 31) << 5) | (((uint)Vdn & 31) << 0));
    //SVE conditionally extract element to general register
    public void emit_152(int size, int B, int Pg, int Zm, int Rdn) => AddIns(0b00000101001100001010000000000000 | (((uint)size & 3) << 22) | (((uint)B & 1) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zm & 31) << 5) | (((uint)Rdn & 31) << 0));
    //SVE copy SIMD&FP scalar register to vector (predicated)
    public void emit_153(int size, int Pg, int Vn, int Zd) => AddIns(0b00000101001000001000000000000000 | (((uint)size & 3) << 22) | (((uint)Pg & 7) << 10) | (((uint)Vn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE copy general register to vector (predicated)
    public void emit_154(int size, int Pg, int Rn, int Zd) => AddIns(0b00000101001010001010000000000000 | (((uint)size & 3) << 22) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE extract element to SIMD&FP scalar register
    public void emit_155(int size, int B, int Pg, int Zn, int Vd) => AddIns(0b00000101001000101000000000000000 | (((uint)size & 3) << 22) | (((uint)B & 1) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Vd & 31) << 0));
    //SVE extract element to general register
    public void emit_156(int size, int B, int Pg, int Zn, int Rd) => AddIns(0b00000101001000001010000000000000 | (((uint)size & 3) << 22) | (((uint)B & 1) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Rd & 31) << 0));
    //SVE reverse within elements
    public void emit_157(int size, int opc, int Pg, int Zn, int Zd) => AddIns(0b00000101001001001000000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 3) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE vector splice (destructive)
    public void emit_158(int size, int Pg, int Zm, int Zdn) => AddIns(0b00000101001011001000000000000000 | (((uint)size & 3) << 22) | (((uint)Pg & 7) << 10) | (((uint)Zm & 31) << 5) | (((uint)Zdn & 31) << 0));
    //SVE select vector elements (predicated)
    public void emit_159(int size, int Zm, int Pg, int Zn, int Zd) => AddIns(0b00000101001000001100000000000000 | (((uint)size & 3) << 22) | (((uint)Zm & 31) << 16) | (((uint)Pg & 15) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE integer compare vectors
    public void emit_160(int size, int Zm, int op, int o2, int Pg, int Zn, int ne, int Pd) => AddIns(0b00100100000000000000000000000000 | (((uint)size & 3) << 22) | (((uint)Zm & 31) << 16) | (((uint)op & 1) << 15) | (((uint)o2 & 1) << 13) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)ne & 1) << 4) | (((uint)Pd & 15) << 0));
    //SVE integer compare with wide elements
    public void emit_161(int size, int Zm, int U, int lt, int Pg, int Zn, int ne, int Pd) => AddIns(0b00100100000000000100000000000000 | (((uint)size & 3) << 22) | (((uint)Zm & 31) << 16) | (((uint)U & 1) << 15) | (((uint)lt & 1) << 13) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)ne & 1) << 4) | (((uint)Pd & 15) << 0));
    //SVE integer compare with unsigned immediate
    public void emit_162(int size, int imm7, int lt, int Pg, int Zn, int ne, int Pd) => AddIns(0b00100100001000000000000000000000 | (((uint)size & 3) << 22) | (((uint)imm7 & 127) << 14) | (((uint)lt & 1) << 13) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)ne & 1) << 4) | (((uint)Pd & 15) << 0));
    //SVE predicate logical operations
    public void emit_163(int op, int S, int Pm, int Pg, int o2, int Pn, int o3, int Pd) => AddIns(0b00100101000000000100000000000000 | (((uint)op & 1) << 23) | (((uint)S & 1) << 22) | (((uint)Pm & 15) << 16) | (((uint)Pg & 15) << 10) | (((uint)o2 & 1) << 9) | (((uint)Pn & 15) << 5) | (((uint)o3 & 1) << 4) | (((uint)Pd & 15) << 0));
    //SVE propagate break from previous partition
    public void emit_164(int op, int S, int Pm, int Pg, int Pn, int B, int Pd) => AddIns(0b00100101000000001100000000000000 | (((uint)op & 1) << 23) | (((uint)S & 1) << 22) | (((uint)Pm & 15) << 16) | (((uint)Pg & 15) << 10) | (((uint)Pn & 15) << 5) | (((uint)B & 1) << 4) | (((uint)Pd & 15) << 0));
    //SVE partition break condition
    public void emit_165(int B, int S, int Pg, int Pn, int M, int Pd) => AddIns(0b00100101000100000100000000000000 | (((uint)B & 1) << 23) | (((uint)S & 1) << 22) | (((uint)Pg & 15) << 10) | (((uint)Pn & 15) << 5) | (((uint)M & 1) << 4) | (((uint)Pd & 15) << 0));
    //SVE propagate break to next partition
    public void emit_166(int S, int Pg, int Pn, int Pdm) => AddIns(0b00100101000110000100000000000000 | (((uint)S & 1) << 22) | (((uint)Pg & 15) << 10) | (((uint)Pn & 15) << 5) | (((uint)Pdm & 15) << 0));
    //SVE predicate first active
    public void emit_167(int op, int S, int Pg, int Pdn) => AddIns(0b00100101000110001100000000000000 | (((uint)op & 1) << 23) | (((uint)S & 1) << 22) | (((uint)Pg & 15) << 5) | (((uint)Pdn & 15) << 0));
    //SVE predicate initialize
    public void emit_168(int size, int S, int pattern, int Pd) => AddIns(0b00100101000110001110000000000000 | (((uint)size & 3) << 22) | (((uint)S & 1) << 16) | (((uint)pattern & 31) << 5) | (((uint)Pd & 15) << 0));
    //SVE predicate next active
    public void emit_169(int size, int Pg, int Pdn) => AddIns(0b00100101000110011100010000000000 | (((uint)size & 3) << 22) | (((uint)Pg & 15) << 5) | (((uint)Pdn & 15) << 0));
    //SVE predicate read from FFR (predicated)
    public void emit_170(int op, int S, int Pg, int Pd) => AddIns(0b00100101000110001111000000000000 | (((uint)op & 1) << 23) | (((uint)S & 1) << 22) | (((uint)Pg & 15) << 5) | (((uint)Pd & 15) << 0));
    //SVE predicate read from FFR (unpredicated)
    public void emit_171(int op, int S, int Pd) => AddIns(0b00100101000110011111000000000000 | (((uint)op & 1) << 23) | (((uint)S & 1) << 22) | (((uint)Pd & 15) << 0));
    //SVE predicate test
    public void emit_172(int op, int S, int Pg, int Pn, int opc2) => AddIns(0b00100101000100001100000000000000 | (((uint)op & 1) << 23) | (((uint)S & 1) << 22) | (((uint)Pg & 15) << 10) | (((uint)Pn & 15) << 5) | (((uint)opc2 & 15) << 0));
    //SVE predicate zero
    public void emit_173(int op, int S, int Pd) => AddIns(0b00100101000110001110010000000000 | (((uint)op & 1) << 23) | (((uint)S & 1) << 22) | (((uint)Pd & 15) << 0));
    //SVE integer compare with signed immediate
    public void emit_174(int size, int imm5, int op, int o2, int Pg, int Zn, int ne, int Pd) => AddIns(0b00100101000000000000000000000000 | (((uint)size & 3) << 22) | (((uint)imm5 & 31) << 16) | (((uint)op & 1) << 15) | (((uint)o2 & 1) << 13) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)ne & 1) << 4) | (((uint)Pd & 15) << 0));
    //SVE predicate count
    public void emit_175(int size, int opc, int Pg, int Pn, int Rd) => AddIns(0b00100101001000001000000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 7) << 16) | (((uint)Pg & 15) << 10) | (((uint)Pn & 15) << 5) | (((uint)Rd & 31) << 0));
    //SVE inc/dec register by predicate count
    public void emit_176(int size, int op, int D, int opc2, int Pm, int Rdn) => AddIns(0b00100101001011001000100000000000 | (((uint)size & 3) << 22) | (((uint)op & 1) << 17) | (((uint)D & 1) << 16) | (((uint)opc2 & 3) << 9) | (((uint)Pm & 15) << 5) | (((uint)Rdn & 31) << 0));
    //SVE inc/dec vector by predicate count
    public void emit_177(int size, int op, int D, int opc2, int Pm, int Zdn) => AddIns(0b00100101001011001000000000000000 | (((uint)size & 3) << 22) | (((uint)op & 1) << 17) | (((uint)D & 1) << 16) | (((uint)opc2 & 3) << 9) | (((uint)Pm & 15) << 5) | (((uint)Zdn & 31) << 0));
    //SVE saturating inc/dec register by predicate count
    public void emit_178(int size, int D, int U, int sf, int op, int Pm, int Rdn) => AddIns(0b00100101001010001000100000000000 | (((uint)size & 3) << 22) | (((uint)D & 1) << 17) | (((uint)U & 1) << 16) | (((uint)sf & 1) << 10) | (((uint)op & 1) << 9) | (((uint)Pm & 15) << 5) | (((uint)Rdn & 31) << 0));
    //SVE saturating inc/dec vector by predicate count
    public void emit_179(int size, int D, int U, int opc, int Pm, int Zdn) => AddIns(0b00100101001010001000000000000000 | (((uint)size & 3) << 22) | (((uint)D & 1) << 17) | (((uint)U & 1) << 16) | (((uint)opc & 3) << 9) | (((uint)Pm & 15) << 5) | (((uint)Zdn & 31) << 0));
    //SVE FFR initialise
    public void emit_180(int opc) => AddIns(0b00100101001011001001000000000000 | (((uint)opc & 3) << 22));
    //SVE FFR write from predicate
    public void emit_181(int opc, int Pn) => AddIns(0b00100101001010001001000000000000 | (((uint)opc & 3) << 22) | (((uint)Pn & 15) << 5));
    //SVE conditionally terminate scalars
    public void emit_182(int op, int sz, int Rm, int Rn, int ne) => AddIns(0b00100101001000000010000000000000 | (((uint)op & 1) << 23) | (((uint)sz & 1) << 22) | (((uint)Rm & 31) << 16) | (((uint)Rn & 31) << 5) | (((uint)ne & 1) << 4));
    //SVE integer compare scalar count and limit
    public void emit_183(int size, int Rm, int sf, int U, int lt, int Rn, int eq, int Pd) => AddIns(0b00100101001000000000000000000000 | (((uint)size & 3) << 22) | (((uint)Rm & 31) << 16) | (((uint)sf & 1) << 12) | (((uint)U & 1) << 11) | (((uint)lt & 1) << 10) | (((uint)Rn & 31) << 5) | (((uint)eq & 1) << 4) | (((uint)Pd & 15) << 0));
    //SVE broadcast floating-point immediate (unpredicated)
    public void emit_184(int size, int opc, int o2, int imm8, int Zd) => AddIns(0b00100101001110011100000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 3) << 17) | (((uint)o2 & 1) << 13) | (((uint)imm8 & 255) << 5) | (((uint)Zd & 31) << 0));
    //SVE broadcast integer immediate (unpredicated)
    public void emit_185(int size, int opc, int sh, int imm8, int Zd) => AddIns(0b00100101001110001100000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 3) << 17) | (((uint)sh & 1) << 13) | (((uint)imm8 & 255) << 5) | (((uint)Zd & 31) << 0));
    //SVE integer add/subtract immediate (unpredicated)
    public void emit_186(int size, int opc, int sh, int imm8, int Zdn) => AddIns(0b00100101001000001100000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 7) << 16) | (((uint)sh & 1) << 13) | (((uint)imm8 & 255) << 5) | (((uint)Zdn & 31) << 0));
    //SVE integer min/max immediate (unpredicated)
    public void emit_187(int size, int opc, int o2, int imm8, int Zdn) => AddIns(0b00100101001010001100000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 7) << 16) | (((uint)o2 & 1) << 13) | (((uint)imm8 & 255) << 5) | (((uint)Zdn & 31) << 0));
    //SVE integer multiply immediate (unpredicated)
    public void emit_188(int size, int opc, int o2, int imm8, int Zdn) => AddIns(0b00100101001100001100000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 7) << 16) | (((uint)o2 & 1) << 13) | (((uint)imm8 & 255) << 5) | (((uint)Zdn & 31) << 0));
    //SVE integer dot product (unpredicated)
    public void emit_189(int size, int Zm, int U, int Zn, int Zda) => AddIns(0b01000100000000000000000000000000 | (((uint)size & 3) << 22) | (((uint)Zm & 31) << 16) | (((uint)U & 1) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zda & 31) << 0));
    //SVE mixed sign dot product
    public void emit_190(int size, int Zm, int Zn, int Zda) => AddIns(0b01000100000000000111100000000000 | (((uint)size & 3) << 22) | (((uint)Zm & 31) << 16) | (((uint)Zn & 31) << 5) | (((uint)Zda & 31) << 0));
    //SVE integer dot product (indexed)
    public void emit_191(int size, int opc, int U, int Zn, int Zda) => AddIns(0b01000100001000000000000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 31) << 16) | (((uint)U & 1) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zda & 31) << 0));
    //SVE mixed sign dot product (indexed)
    public void emit_192(int size, int opc, int U, int Zn, int Zda) => AddIns(0b01000100001000000001100000000000 | (((uint)size & 3) << 22) | (((uint)opc & 31) << 16) | (((uint)U & 1) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zda & 31) << 0));
    //SVE integer matrix multiply accumulate
    public void emit_193(int uns, int Zm, int Zn, int Zd) => AddIns(0b01000101000000001001100000000000 | (((uint)uns & 3) << 22) | (((uint)Zm & 31) << 16) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE floating-point complex add (predicated)
    public void emit_194(int size, int rot, int Pg, int Zm, int Zdn) => AddIns(0b01100100000000001000000000000000 | (((uint)size & 3) << 22) | (((uint)rot & 1) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zm & 31) << 5) | (((uint)Zdn & 31) << 0));
    //SVE floating-point convert precision odd elements
    public void emit_195(int opc, int opc2, int Pg, int Zn, int Zd) => AddIns(0b01100100000010001010000000000000 | (((uint)opc & 3) << 22) | (((uint)opc2 & 3) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE floating-point complex multiply-add (predicated)
    public void emit_196(int size, int Zm, int rot, int Pg, int Zn, int Zda) => AddIns(0b01100100000000000000000000000000 | (((uint)size & 3) << 22) | (((uint)Zm & 31) << 16) | (((uint)rot & 3) << 13) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zda & 31) << 0));
    //SVE floating-point multiply-add (indexed)
    public void emit_197(int size, int opc, int op, int Zn, int Zda) => AddIns(0b01100100001000000000000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 31) << 16) | (((uint)op & 1) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zda & 31) << 0));//SVE floating-point complex multiply-add (indexed)
    public void emit_198(int size, int opc, int rot, int Zn, int Zda) => AddIns(0b01100100001000000001000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 31) << 16) | (((uint)rot & 3) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zda & 31) << 0));
    //SVE floating-point multiply (indexed)
    public void emit_199(int size, int opc, int Zn, int Zd) => AddIns(0b01100100001000000010000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 31) << 16) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE BFloat16 floating-point dot product (indexed)
    public void emit_200(int op, int i2, int Zm, int Zn, int Zda) => AddIns(0b01100100001000000100000000000000 | (((uint)op & 1) << 22) | (((uint)i2 & 3) << 19) | (((uint)Zm & 7) << 16) | (((uint)Zn & 31) << 5) | (((uint)Zda & 31) << 0));
    //SVE floating-point multiply-add long (indexed)
    public void emit_201(int o2, int i3h, int Zm, int op, int i3l, int T, int Zn, int Zda) => AddIns(0b01100100101000000100000000000000 | (((uint)o2 & 1) << 22) | (((uint)i3h & 3) << 19) | (((uint)Zm & 7) << 16) | (((uint)op & 1) << 13) | (((uint)i3l & 1) << 11) | (((uint)T & 1) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zda & 31) << 0));
    //SVE BFloat16 floating-point dot product
    public void emit_202(int op, int Zm, int Zn, int Zda) => AddIns(0b01100100001000001000000000000000 | (((uint)op & 1) << 22) | (((uint)Zm & 31) << 16) | (((uint)Zn & 31) << 5) | (((uint)Zda & 31) << 0));
    //SVE floating-point multiply-add long
    public void emit_203(int o2, int Zm, int op, int T, int Zn, int Zda) => AddIns(0b01100100101000001000000000000000 | (((uint)o2 & 1) << 22) | (((uint)Zm & 31) << 16) | (((uint)op & 1) << 13) | (((uint)T & 1) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zda & 31) << 0));
    //SVE floating point matrix multiply accumulate
    public void emit_204(int opc, int Zm, int Zn, int Zda) => AddIns(0b01100100001000001110010000000000 | (((uint)opc & 3) << 22) | (((uint)Zm & 31) << 16) | (((uint)Zn & 31) << 5) | (((uint)Zda & 31) << 0));
    //SVE floating-point recursive reduction
    public void emit_205(int size, int opc, int Pg, int Zn, int Vd) => AddIns(0b01100101000000000010000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 7) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Vd & 31) << 0));
    //SVE floating-point reciprocal estimate (unpredicated)
    public void emit_206(int size, int opc, int Zn, int Zd) => AddIns(0b01100101000010000011000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 7) << 16) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE floating-point compare with zero
    public void emit_207(int size, int eq, int lt, int Pg, int Zn, int ne, int Pd) => AddIns(0b01100101000100000010000000000000 | (((uint)size & 3) << 22) | (((uint)eq & 1) << 17) | (((uint)lt & 1) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)ne & 1) << 4) | (((uint)Pd & 15) << 0));
    //SVE floating-point serial reduction (predicated)
    public void emit_208(int size, int opc, int Pg, int Zm, int Vdn) => AddIns(0b01100101000110000010000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 3) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zm & 31) << 5) | (((uint)Vdn & 31) << 0));
    //SVE floating-point arithmetic (unpredicated)
    public void emit_209(int size, int Zm, int opc, int Zn, int Zd) => AddIns(0b01100101000000000000000000000000 | (((uint)size & 3) << 22) | (((uint)Zm & 31) << 16) | (((uint)opc & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE floating-point arithmetic (predicated)
    public void emit_210(int size, int opc, int Pg, int Zm, int Zdn) => AddIns(0b01100101000000001000000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 15) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zm & 31) << 5) | (((uint)Zdn & 31) << 0));//SVE floating-point arithmetic with immediate (predicated)
    public void emit_211(int size, int opc, int Pg, int i1, int Zdn) => AddIns(0b01100101000110001000000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 7) << 16) | (((uint)Pg & 7) << 10) | (((uint)i1 & 1) << 5) | (((uint)Zdn & 31) << 0));
    //SVE floating-point trig multiply-add coefficient
    public void emit_212(int size, int imm3, int Zm, int Zdn) => AddIns(0b01100101000100001000000000000000 | (((uint)size & 3) << 22) | (((uint)imm3 & 7) << 16) | (((uint)Zm & 31) << 5) | (((uint)Zdn & 31) << 0));
    //SVE floating-point convert precision
    public void emit_213(int opc, int opc2, int Pg, int Zn, int Zd) => AddIns(0b01100101000010001010000000000000 | (((uint)opc & 3) << 22) | (((uint)opc2 & 3) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE floating-point convert to integer
    public void emit_214(int opc, int opc2, int U, int Pg, int Zn, int Zd) => AddIns(0b01100101000110001010000000000000 | (((uint)opc & 3) << 22) | (((uint)opc2 & 3) << 17) | (((uint)U & 1) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE floating-point round to integral value
    public void emit_215(int size, int opc, int Pg, int Zn, int Zd) => AddIns(0b01100101000000001010000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 7) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE floating-point unary operations
    public void emit_216(int size, int opc, int Pg, int Zn, int Zd) => AddIns(0b01100101000011001010000000000000 | (((uint)size & 3) << 22) | (((uint)opc & 3) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE integer convert to floating-point
    public void emit_217(int opc, int opc2, int U, int Pg, int Zn, int Zd) => AddIns(0b01100101000100001010000000000000 | (((uint)opc & 3) << 22) | (((uint)opc2 & 3) << 17) | (((uint)U & 1) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zd & 31) << 0));
    //SVE floating-point compare vectors
    public void emit_218(int size, int Zm, int op, int o2, int Pg, int Zn, int o3, int Pd) => AddIns(0b01100101000000000100000000000000 | (((uint)size & 3) << 22) | (((uint)Zm & 31) << 16) | (((uint)op & 1) << 15) | (((uint)o2 & 1) << 13) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)o3 & 1) << 4) | (((uint)Pd & 15) << 0));
    //SVE floating-point multiply-accumulate writing addend
    public void emit_219(int size, int Zm, int opc, int Pg, int Zn, int Zda) => AddIns(0b01100101001000000000000000000000 | (((uint)size & 3) << 22) | (((uint)Zm & 31) << 16) | (((uint)opc & 3) << 13) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zda & 31) << 0));
    //SVE floating-point multiply-accumulate writing multiplicand
    public void emit_220(int size, int Za, int opc, int Pg, int Zm, int Zdn) => AddIns(0b01100101001000001000000000000000 | (((uint)size & 3) << 22) | (((uint)Za & 31) << 16) | (((uint)opc & 3) << 13) | (((uint)Pg & 7) << 10) | (((uint)Zm & 31) << 5) | (((uint)Zdn & 31) << 0));
    //SVE 32-bit gather load (scalar plus 32-bit unscaled offsets)
    public void emit_221(int opc, int xs, int Zm, int U, int ff, int Pg, int Rn, int Zt) => AddIns(0b10000100000000000000000000000000 | (((uint)opc & 3) << 23) | (((uint)xs & 1) << 22) | (((uint)Zm & 31) << 16) | (((uint)U & 1) << 14) | (((uint)ff & 1) << 13) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE 32-bit gather load (vector plus immediate)
    public void emit_222(int msz, int imm5, int U, int ff, int Pg, int Zn, int Zt) => AddIns(0b10000100001000001000000000000000 | (((uint)msz & 3) << 23) | (((uint)imm5 & 31) << 16) | (((uint)U & 1) << 14) | (((uint)ff & 1) << 13) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE 32-bit gather load halfwords (scalar plus 32-bit scaled offsets)
    public void emit_223(int xs, int Zm, int U, int ff, int Pg, int Rn, int Zt) => AddIns(0b10000100101000000000000000000000 | (((uint)xs & 1) << 22) | (((uint)Zm & 31) << 16) | (((uint)U & 1) << 14) | (((uint)ff & 1) << 13) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE 32-bit gather load words (scalar plus 32-bit scaled offsets)
    public void emit_224(int xs, int Zm, int U, int ff, int Pg, int Rn, int Zt) => AddIns(0b10000101001000000000000000000000 | (((uint)xs & 1) << 22) | (((uint)Zm & 31) << 16) | (((uint)U & 1) << 14) | (((uint)ff & 1) << 13) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE 32-bit gather prefetch (scalar plus 32-bit scaled offsets)
    public void emit_225(int xs, int Zm, int msz, int Pg, int Rn, int prfop) => AddIns(0b10000100001000000000000000000000 | (((uint)xs & 1) << 22) | (((uint)Zm & 31) << 16) | (((uint)msz & 3) << 13) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)prfop & 15) << 0));
    //SVE 32-bit gather prefetch (vector plus immediate)
    public void emit_226(int msz, int imm5, int Pg, int Zn, int prfop) => AddIns(0b10000100000000001110000000000000 | (((uint)msz & 3) << 23) | (((uint)imm5 & 31) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)prfop & 15) << 0));
    //SVE contiguous prefetch (scalar plus immediate)
    public void emit_227(int imm6, int msz, int Pg, int Rn, int prfop) => AddIns(0b10000101110000000000000000000000 | (((uint)imm6 & 63) << 16) | (((uint)msz & 3) << 13) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)prfop & 15) << 0));
    //SVE contiguous prefetch (scalar plus scalar)
    public void emit_228(int msz, int Rm, int Pg, int Rn, int prfop) => AddIns(0b10000100000000001100000000000000 | (((uint)msz & 3) << 23) | (((uint)Rm & 31) << 16) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)prfop & 15) << 0));//SVE load and broadcast element
    public void emit_229(int dtypeh, int imm6, int dtypel, int Pg, int Rn, int Zt) => AddIns(0b10000100010000001000000000000000 | (((uint)dtypeh & 3) << 23) | (((uint)imm6 & 63) << 16) | (((uint)dtypel & 3) << 13) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE load predicate register
    public void emit_230(int imm9h, int imm9l, int Rn, int Pt) => AddIns(0b10000101100000000000000000000000 | (((uint)imm9h & 63) << 16) | (((uint)imm9l & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Pt & 15) << 0));
    //SVE load vector register
    public void emit_231(int imm9h, int imm9l, int Rn, int Zt) => AddIns(0b10000101100000000100000000000000 | (((uint)imm9h & 63) << 16) | (((uint)imm9l & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE contiguous first-fault load (scalar plus scalar)
    public void emit_232(int dtype, int Rm, int Pg, int Rn, int Zt) => AddIns(0b10100100000000000110000000000000 | (((uint)dtype & 15) << 21) | (((uint)Rm & 31) << 16) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE contiguous load (scalar plus immediate)
    public void emit_233(int dtype, int imm4, int Pg, int Rn, int Zt) => AddIns(0b10100100000000001010000000000000 | (((uint)dtype & 15) << 21) | (((uint)imm4 & 15) << 16) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE contiguous load (scalar plus scalar)
    public void emit_234(int dtype, int Rm, int Pg, int Rn, int Zt) => AddIns(0b10100100000000000100000000000000 | (((uint)dtype & 15) << 21) | (((uint)Rm & 31) << 16) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE contiguous non-fault load (scalar plus immediate)
    public void emit_235(int dtype, int imm4, int Pg, int Rn, int Zt) => AddIns(0b10100100000100001010000000000000 | (((uint)dtype & 15) << 21) | (((uint)imm4 & 15) << 16) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE contiguous non-temporal load (scalar plus immediate)
    public void emit_236(int msz, int imm4, int Pg, int Rn, int Zt) => AddIns(0b10100100000000001110000000000000 | (((uint)msz & 3) << 23) | (((uint)imm4 & 15) << 16) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE contiguous non-temporal load (scalar plus scalar)
    public void emit_237(int msz, int Rm, int Pg, int Rn, int Zt) => AddIns(0b10100100000000001100000000000000 | (((uint)msz & 3) << 23) | (((uint)Rm & 31) << 16) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE load and broadcast quadword (scalar plus immediate)
    public void emit_238(int msz, int ssz, int imm4, int Pg, int Rn, int Zt) => AddIns(0b10100100000000000010000000000000 | (((uint)msz & 3) << 23) | (((uint)ssz & 3) << 21) | (((uint)imm4 & 15) << 16) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE load and broadcast quadword (scalar plus scalar)
    public void emit_239(int msz, int ssz, int Rm, int Pg, int Rn, int Zt) => AddIns(0b10100100000000000000000000000000 | (((uint)msz & 3) << 23) | (((uint)ssz & 3) << 21) | (((uint)Rm & 31) << 16) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE load multiple structures (scalar plus immediate)
    public void emit_240(int msz, int opc, int imm4, int Pg, int Rn, int Zt) => AddIns(0b10100100000000001110000000000000 | (((uint)msz & 3) << 23) | (((uint)opc & 3) << 21) | (((uint)imm4 & 15) << 16) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE load multiple structures (scalar plus scalar)
    public void emit_241(int msz, int opc, int Rm, int Pg, int Rn, int Zt) => AddIns(0b10100100000000001100000000000000 | (((uint)msz & 3) << 23) | (((uint)opc & 3) << 21) | (((uint)Rm & 31) << 16) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE 64-bit gather load (scalar plus 32-bit unpacked scaled offsets)
    public void emit_242(int opc, int xs, int Zm, int U, int ff, int Pg, int Rn, int Zt) => AddIns(0b11000100001000000000000000000000 | (((uint)opc & 3) << 23) | (((uint)xs & 1) << 22) | (((uint)Zm & 31) << 16) | (((uint)U & 1) << 14) | (((uint)ff & 1) << 13) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE 64-bit gather load (scalar plus 64-bit scaled offsets)
    public void emit_243(int opc, int Zm, int U, int ff, int Pg, int Rn, int Zt) => AddIns(0b11000100011000001000000000000000 | (((uint)opc & 3) << 23) | (((uint)Zm & 31) << 16) | (((uint)U & 1) << 14) | (((uint)ff & 1) << 13) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE 64-bit gather load (scalar plus 64-bit unscaled offsets)
    public void emit_244(int msz, int Zm, int U, int ff, int Pg, int Rn, int Zt) => AddIns(0b11000100010000001000000000000000 | (((uint)msz & 3) << 23) | (((uint)Zm & 31) << 16) | (((uint)U & 1) << 14) | (((uint)ff & 1) << 13) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE 64-bit gather load (scalar plus unpacked 32-bit unscaled offsets)
    public void emit_245(int msz, int xs, int Zm, int U, int ff, int Pg, int Rn, int Zt) => AddIns(0b11000100000000000000000000000000 | (((uint)msz & 3) << 23) | (((uint)xs & 1) << 22) | (((uint)Zm & 31) << 16) | (((uint)U & 1) << 14) | (((uint)ff & 1) << 13) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE 64-bit gather load (vector plus immediate)
    public void emit_246(int msz, int imm5, int U, int ff, int Pg, int Zn, int Zt) => AddIns(0b11000100001000001000000000000000 | (((uint)msz & 3) << 23) | (((uint)imm5 & 31) << 16) | (((uint)U & 1) << 14) | (((uint)ff & 1) << 13) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE 64-bit gather prefetch (scalar plus 64-bit scaled offsets)
    public void emit_247(int Zm, int msz, int Pg, int Rn, int prfop) => AddIns(0b11000100011000001000000000000000 | (((uint)Zm & 31) << 16) | (((uint)msz & 3) << 13) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)prfop & 15) << 0));//SVE 64-bit gather prefetch (scalar plus unpacked 32-bit scaled offsets)
    public void emit_248(int xs, int Zm, int msz, int Pg, int Rn, int prfop) => AddIns(0b11000100001000000000000000000000 | (((uint)xs & 1) << 22) | (((uint)Zm & 31) << 16) | (((uint)msz & 3) << 13) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)prfop & 15) << 0));
    //SVE 64-bit gather prefetch (vector plus immediate)
    public void emit_249(int msz, int imm5, int Pg, int Zn, int prfop) => AddIns(0b11000100000000001110000000000000 | (((uint)msz & 3) << 23) | (((uint)imm5 & 31) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)prfop & 15) << 0));
    //SVE contiguous store (scalar plus scalar)
    public void emit_250(int opc, int o2, int Rm, int Pg, int Rn, int Zt) => AddIns(0b11100100000000000100000000000000 | (((uint)opc & 7) << 22) | (((uint)o2 & 1) << 21) | (((uint)Rm & 31) << 16) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE store predicate register
    public void emit_251(int imm9h, int imm9l, int Rn, int Pt) => AddIns(0b11100101100000000000000000000000 | (((uint)imm9h & 63) << 16) | (((uint)imm9l & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Pt & 15) << 0));
    //SVE store vector register
    public void emit_252(int imm9h, int imm9l, int Rn, int Zt) => AddIns(0b11100101100000000100000000000000 | (((uint)imm9h & 63) << 16) | (((uint)imm9l & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE contiguous non-temporal store (scalar plus scalar)
    public void emit_253(int msz, int Rm, int Pg, int Rn, int Zt) => AddIns(0b11100100000000000110000000000000 | (((uint)msz & 3) << 23) | (((uint)Rm & 31) << 16) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE store multiple structures (scalar plus scalar)
    public void emit_254(int msz, int opc, int Rm, int Pg, int Rn, int Zt) => AddIns(0b11100100000000000110000000000000 | (((uint)msz & 3) << 23) | (((uint)opc & 3) << 21) | (((uint)Rm & 31) << 16) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE 32-bit scatter store (vector plus immediate)
    public void emit_255(int msz, int imm5, int Pg, int Zn, int Zt) => AddIns(0b11100100011000001010000000000000 | (((uint)msz & 3) << 23) | (((uint)imm5 & 31) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE 64-bit scatter store (scalar plus 64-bit scaled offsets)
    public void emit_256(int msz, int Zm, int Pg, int Rn, int Zt) => AddIns(0b11100100001000001010000000000000 | (((uint)msz & 3) << 23) | (((uint)Zm & 31) << 16) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE 64-bit scatter store (scalar plus 64-bit unscaled offsets)
    public void emit_257(int msz, int Zm, int Pg, int Rn, int Zt) => AddIns(0b11100100000000001010000000000000 | (((uint)msz & 3) << 23) | (((uint)Zm & 31) << 16) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE 64-bit scatter store (vector plus immediate)
    public void emit_258(int msz, int imm5, int Pg, int Zn, int Zt) => AddIns(0b11100100010000001010000000000000 | (((uint)msz & 3) << 23) | (((uint)imm5 & 31) << 16) | (((uint)Pg & 7) << 10) | (((uint)Zn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE contiguous non-temporal store (scalar plus immediate)
    public void emit_259(int msz, int imm4, int Pg, int Rn, int Zt) => AddIns(0b11100100000100001110000000000000 | (((uint)msz & 3) << 23) | (((uint)imm4 & 15) << 16) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE contiguous store (scalar plus immediate)
    public void emit_260(int msz, int size, int imm4, int Pg, int Rn, int Zt) => AddIns(0b11100100000000001110000000000000 | (((uint)msz & 3) << 23) | (((uint)size & 3) << 21) | (((uint)imm4 & 15) << 16) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE store multiple structures (scalar plus immediate)
    public void emit_261(int msz, int opc, int imm4, int Pg, int Rn, int Zt) => AddIns(0b11100100000100001110000000000000 | (((uint)msz & 3) << 23) | (((uint)opc & 3) << 21) | (((uint)imm4 & 15) << 16) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE 32-bit scatter store (scalar plus 32-bit scaled offsets)
    public void emit_262(int msz, int Zm, int xs, int Pg, int Rn, int Zt) => AddIns(0b11100100011000001000000000000000 | (((uint)msz & 3) << 23) | (((uint)Zm & 31) << 16) | (((uint)xs & 1) << 14) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE 32-bit scatter store (scalar plus 32-bit unscaled offsets)
    public void emit_263(int msz, int Zm, int xs, int Pg, int Rn, int Zt) => AddIns(0b11100100010000001000000000000000 | (((uint)msz & 3) << 23) | (((uint)Zm & 31) << 16) | (((uint)xs & 1) << 14) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE 64-bit scatter store (scalar plus unpacked 32-bit scaled offsets)
    public void emit_264(int msz, int Zm, int xs, int Pg, int Rn, int Zt) => AddIns(0b11100100001000001000000000000000 | (((uint)msz & 3) << 23) | (((uint)Zm & 31) << 16) | (((uint)xs & 1) << 14) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
    //SVE 64-bit scatter store (scalar plus unpacked 32-bit unscaled offsets)
    public void emit_265(int msz, int Zm, int xs, int Pg, int Rn, int Zt) => AddIns(0b11100100000000001000000000000000 | (((uint)msz & 3) << 23) | (((uint)Zm & 31) << 16) | (((uint)xs & 1) << 14) | (((uint)Pg & 7) << 10) | (((uint)Rn & 31) << 5) | (((uint)Zt & 31) << 0));
}