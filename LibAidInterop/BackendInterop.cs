using System;
using System.Runtime.InteropServices;

namespace LibAidInterop
{
    public static class BackendInterop
    {
#if !DEBUG || !NETDESIGNTIME
        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void InitSystem();

        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AddUser(string firstName, string lastName);

        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CleanupSystem();
#endif
    }
}
