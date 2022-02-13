using DCpu.CodeGen;
using DCpu.CodeGen.X86;
using DCpu.Diagnostics;
using DCpu.IntermediateRepresentation;
using System;
using System.Runtime.InteropServices;

namespace DCpu.Translation
{
    static class Compiler
    {
        public static T Compile<T>(
            ControlFlowGraph cfg,
            OperandType[]    funcArgTypes,
            OperandType      funcReturnType,
            CompilerOptions  options)
        {
            Logger.StartPass(PassName.Dominance);

            Dominance.FindDominators(cfg);
            Dominance.FindDominanceFrontiers(cfg);

            Logger.EndPass(PassName.Dominance);

            Logger.StartPass(PassName.SsaConstruction);

            if ((options & CompilerOptions.SsaForm) != 0)
            {
                Ssa.Construct(cfg);
            }
            else
            {
                RegisterToLocal.Rename(cfg);
            }

            Logger.EndPass(PassName.SsaConstruction, cfg);

            CompilerContext cctx = new CompilerContext(cfg, funcArgTypes, funcReturnType, options);

            CompiledFunction func = CodeGenerator.Generate(cctx);

            IntPtr codePtr = JitCache.Map(func);

            return Marshal.GetDelegateForFunctionPointer<T>(codePtr);
        }
    }
}