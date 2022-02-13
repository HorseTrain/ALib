using System;

namespace DCpu.State
{
    public class InstExceptionEventArgs : EventArgs
    {
        public ulong Address { get; }
        public int   Id      { get; }

        public InstExceptionEventArgs(ulong address, int id)
        {
            Address = address;
            Id      = id;
        }
    }
}