/// <file>
/// MainWindow.xaml.cs
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
/// Code for the main window of the application.
/// </description>
/// <references>
/// Deitel, P., & Deitel, H. (2017). *C# 6 for Programmers Sixth Edition* 
/// (Sixth, Ser. Deitel Development Series). Pearson Education.
/// </references>
/// 

using System;
using System.Windows;
using LibAidFrontend;

namespace LibAid_Frontend
{
    /// <summary>
    /// Class that contains the backend code for the MainWindow.
    /// Also contains the events for each of the corresponding buttons:
    ///     Add User
    ///     Add Book
    ///     View Database
    ///     Borrow/Return (code is here, no longer used)
    ///     Undo Last Action
    ///     Exit Program
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddUserWindow();
            win.ShowDialog();
        }
        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddBookWindow();
            win.ShowDialog();
        }
        private void ViewDatabase_Click(object sender, RoutedEventArgs e)
        {
            var win = new ViewDatabaseWindow();
            win.ShowDialog();
        }

        private void BorrowReturn_Click(object sender, RoutedEventArgs e)
        {
            var win = new BorrowReturnWindow();
            win.ShowDialog();
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BackendInterop.UndoLastAction();
                MessageBox.Show("Last action has been undone.", "Undo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error performing undo:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


    }
}
