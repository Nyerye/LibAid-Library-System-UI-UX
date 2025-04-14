using System.Runtime.InteropServices;

namespace LibAidFrontend
{
    public static class BackendInterop
    {
        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void InitSystem();

        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AddUser(string firstName, string lastName);

        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CleanupSystem();
    }
}
