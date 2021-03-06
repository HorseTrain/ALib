namespace DCpu.CodeGen.X86
{
    enum X86Instruction
    {
        Add,
        Addpd,
        Addps,
        Addsd,
        Addss,
        And,
        Andnpd,
        Andnps,
        Bsr,
        Bswap,
        Call,
        Cmovcc,
        Cmp,
        Cmppd,
        Cmpps,
        Cmpsd,
        Cmpss,
        Cmpxchg16b,
        Comisd,
        Comiss,
        Cpuid,
        Cvtdq2pd,
        Cvtdq2ps,
        Cvtpd2dq,
        Cvtpd2ps,
        Cvtps2dq,
        Cvtps2pd,
        Cvtsd2si,
        Cvtsd2ss,
        Cvtsi2sd,
        Cvtsi2ss,
        Cvtss2sd,
        Div,
        Divpd,
        Divps,
        Divsd,
        Divss,
        Haddpd,
        Haddps,
        Idiv,
        Imul,
        Imul128,
        Insertps,
        Lea,
        Maxpd,
        Maxps,
        Maxsd,
        Maxss,
        Minpd,
        Minps,
        Minsd,
        Minss,
        Mov,
        Mov16,
        Mov8,
        Movd,
        Movdqu,
        Movhlps,
        Movlhps,
        Movq,
        Movsd,
        Movss,
        Movsx16,
        Movsx32,
        Movsx8,
        Movzx16,
        Movzx8,
        Mul128,
        Mulpd,
        Mulps,
        Mulsd,
        Mulss,
        Neg,
        Not,
        Or,
        Paddb,
        Paddd,
        Paddq,
        Paddw,
        Pand,
        Pandn,
        Pavgb,
        Pavgw,
        Pblendvb,
        Pcmpeqb,
        Pcmpeqd,
        Pcmpeqq,
        Pcmpeqw,
        Pcmpgtb,
        Pcmpgtd,
        Pcmpgtq,
        Pcmpgtw,
        Pextrb,
        Pextrd,
        Pextrq,
        Pextrw,
        Pinsrb,
        Pinsrd,
        Pinsrq,
        Pinsrw,
        Pmaxsb,
        Pmaxsd,
        Pmaxsw,
        Pmaxub,
        Pmaxud,
        Pmaxuw,
        Pminsb,
        Pminsd,
        Pminsw,
        Pminub,
        Pminud,
        Pminuw,
        Pmovsxbw,
        Pmovsxdq,
        Pmovsxwd,
        Pmovzxbw,
        Pmovzxdq,
        Pmovzxwd,
        Pmulld,
        Pmullw,
        Pop,
        Popcnt,
        Por,
        Pshufb,
        Pshufd,
        Pslld,
        Pslldq,
        Psllq,
        Psllw,
        Psrad,
        Psraw,
        Psrld,
        Psrlq,
        Psrldq,
        Psrlw,
        Psubb,
        Psubd,
        Psubq,
        Psubw,
        Punpckhbw,
        Punpckhdq,
        Punpckhqdq,
        Punpckhwd,
        Punpcklbw,
        Punpckldq,
        Punpcklqdq,
        Punpcklwd,
        Push,
        Pxor,
        Rcpps,
        Rcpss,
        Ror,
        Roundpd,
        Roundps,
        Roundsd,
        Roundss,
        Rsqrtps,
        Rsqrtss,
        Sar,
        Setcc,
        Shl,
        Shr,
        Shufpd,
        Shufps,
        Sqrtpd,
        Sqrtps,
        Sqrtsd,
        Sqrtss,
        Sub,
        Subpd,
        Subps,
        Subsd,
        Subss,
        Test,
        Unpckhpd,
        Unpckhps,
        Unpcklpd,
        Unpcklps,
        Vpblendvb,
        Xor,
        Xorpd,
        Xorps,

        Count
    }
}