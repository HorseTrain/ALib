using System.Runtime.InteropServices;
using System.Security;

namespace Compiler.Tools.Memory
{
    public static unsafe class WindowsAlloc
    {
        [DllImport("kernel32.dll")]
        public static extern void* VirtualAlloc(void* addr, ulong size, int type, int protect);
        [DllImport("kernel32.dll")]
        public static extern bool VirtualProtect(void* addr, ulong size, int new_protect, int* old_protect);
        [DllImport("kernel32.dll")]
        public static extern bool VirtualFree(void* addr, ulong size, int type);
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false), SuppressUnmanagedCodeSecurity]
        public static unsafe extern void* CopyMemory(void* dest, void* src, ulong count);
    }
}
