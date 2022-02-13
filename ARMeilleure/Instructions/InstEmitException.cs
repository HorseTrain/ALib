using DCpu.Decoders;
using DCpu.Translation;
using System;

using static DCpu.IntermediateRepresentation.OperandHelper;

namespace DCpu.Instructions
{
    static partial class InstEmit
    {
        public static void Brk(ArmEmitterContext context)
        {
            EmitExceptionCall(context, NativeInterface.Break);
        }

        public static void Svc(ArmEmitterContext context)
        {
            //EmitExceptionCall(context, NativeInterface.SupervisorCall);

            context.Return(context.BitwiseOr(Const( (long)((OpCodeException)context.CurrOp).Id << 48), Const(context.CurrOp.Address + 4))); ;
        } 

        private static void EmitExceptionCall(ArmEmitterContext context, _Void_U64_S32 func)
        {
            OpCodeException op = (OpCodeException)context.CurrOp;

            context.StoreToContext();

            context.Call(func, Const(op.Address), Const(op.Id));

            context.LoadFromContext();

            if (context.CurrBlock.Next == null)
            {
                context.Return(Const(op.Address + 4));
            }
        }

        public static void Und(ArmEmitterContext context)
        {
            OpCode op = context.CurrOp;

            Delegate dlg = new _Void_U64_S32(NativeInterface.Undefined);

            context.StoreToContext();

            context.Call(dlg, Const(op.Address), Const(op.RawOpCode));

            context.LoadFromContext();

            if (context.CurrBlock.Next == null)
            {
                context.Return(Const(op.Address + 4));
            }
        }
    }
}