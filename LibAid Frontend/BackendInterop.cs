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

        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AddBook(string title, string author);

        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BorrowBook([MarshalAs(UnmanagedType.LPStr)] string title, int userId);

        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ReturnBook([MarshalAs(UnmanagedType.LPStr)] string title);

    }
}
