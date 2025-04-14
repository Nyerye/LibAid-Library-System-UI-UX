using System.Windows;

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



    }
}
