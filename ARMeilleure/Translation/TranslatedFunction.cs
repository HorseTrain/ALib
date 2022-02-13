using System.Threading;

namespace DCpu.Translation
{
    class TranslatedFunction
    {
        private const int MinCallsForRejit = 100;

        public GuestFunction _func;

        private bool _rejit;
        private int  _callCount;

        public TranslatedFunction(GuestFunction func, bool rejit)
        {
            _func  = func;
            _rejit = rejit;
        }

        public ulong Execute(State.ExecutionContext context)
        {
            return _func(context.NativeContextPtr);
        }

        public bool ShouldRejit()
        {
            return _rejit && Interlocked.Increment(ref _callCount) == MinCallsForRejit;
        }
    }
}