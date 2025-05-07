/// <file>
/// BackendInterop.cs
/// </file>
/// <project>
/// LibAid v3.0.12
/// </project>
/// <author>
/// Nicholas Reilly
/// </author>
/// <date>
/// 2025-03-27
/// </date>
/// <description>
/// C# method creation that takes in the DLL we made in C. 
/// </description>
/// <references>
/// Deitel, P., & Deitel, H. (2017). *C# 6 for Programmers Sixth Edition* 
/// (Sixth, Ser. Deitel Development Series). Pearson Education.
/// </references>
/// 

using System.Runtime.InteropServices;

namespace LibAidFrontend
{
    /// <summary>
    /// Class that contains the P/Invoke signatures for the functions in the C++ DLL.
    /// </summary>
    public static class BackendInterop
    {
        /// <summary>
        /// Initializes the system by calling the InitSystem function from the DLL.
        /// </summary>
        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void InitSystem();

        /// <summary>
        /// Cleans up the system by calling the CleanupSystem function from the DLL.
        /// </summary>
        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CleanupSystem();

        /// <summary>
        /// Adds a user to the system by calling the AddUser function from the DLL.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AddUser(string firstName, string lastName);

        /// <summary>
        /// Adds a book to the system by calling the AddBook function from the DLL.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="author"></param>
        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AddBook(string title, string author);

        /// <summary>
        /// Borrows a book for a user by calling the BorrowBook function from the DLL.
        /// </summary>
        /// <param name="userLastName"></param>
        /// <param name="bookTitle"></param>
        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BorrowBook(string userLastName, string bookTitle);

        /// <summary>
        /// Returns a book for a user by calling the ReturnBook function from the DLL.
        /// </summary>
        /// <param name="bookTitle"></param>
        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ReturnBook(string bookTitle);

        /// <summary>
        /// Removes a user from the system by calling the RemoveUser function from the DLL.
        /// </summary>
        /// <param name="lastName"></param>
        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RemoveUser(string lastName);

        /// <summary>
        /// Removes a book from the system by calling the RemoveBook function from the DLL.
        /// </summary>
        /// <param name="title"></param>
        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RemoveBook(string title);

        /// <summary>
        /// Updates a user's information by calling the UpdateUser function from the DLL.
        /// </summary>
        /// <param name="oldLastName"></param>
        /// <param name="newFirstName"></param>
        /// <param name="newLastName"></param>
        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UpdateUser(string oldLastName, string newFirstName, string newLastName);

        /// <summary>
        /// Updates a book's information by calling the UpdateBook function from the DLL.
        /// </summary>
        /// <param name="oldTitle"></param>
        /// <param name="newTitle"></param>
        /// <param name="newAuthor"></param>
        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UpdateBook(string oldTitle, string newTitle, string newAuthor);

        /// <summary>
        /// Checks if a user exists in the system by calling the UserExists function from the DLL.
        /// </summary>
        /// <param name="lastName"></param>
        /// <returns></returns>
        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)] 
        public static extern bool UserExists(string lastName);

        /// <summary>
        /// Checks if a book exists in the system by calling the BookExists function from the DLL.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool BookExists(string title);

        /// <summary>
        /// Checks if a user has borrowed a book by calling the UserHasBorrowedBook function from the DLL.
        /// </summary>
        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UndoLastAction();

        /// <summary>
        /// Prints the list of users in the system by calling the PrintUsers function from the DLL.
        /// </summary>
        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PrintUsers();

        /// <summary>
        /// Prints the list of books in the system by calling the PrintBooks function from the DLL.
        /// </summary>
        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PrintBooks();

        /// <summary>
        /// Permanently deletes a user from the system by calling the HardDeleteUser function from the DLL.
        /// </summary>
        /// <param name="lastName"></param>
        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void HardDeleteUser(string lastName);

        /// <summary>
        /// Permanently deletes a book from the system by calling the HardDeleteBook function from the DLL.
        /// </summary>
        /// <param name="title"></param>
        [DllImport("LibAidBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void HardDeleteBook(string title);

    }
}
