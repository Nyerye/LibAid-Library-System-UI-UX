using System;
using System.Windows;
using LibAidFrontend;

namespace LibAid_Frontend
{
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
