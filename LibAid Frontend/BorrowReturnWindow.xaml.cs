using System;
using System.Runtime.InteropServices;
using System.Windows;
using LibAidFrontend;

namespace LibAid_Frontend
{
    public partial class BorrowReturnWindow : Window
    {
        public BorrowReturnWindow()
        {
            InitializeComponent();
        }

        private void Borrow_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleBox.Text.Trim();
            string userIdText = UserIdBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(userIdText))
            {
                StatusText.Text = "Please provide both title and user ID.";
                return;
            }

            if (!int.TryParse(userIdText, out int userId))
            {
                StatusText.Text = "User ID must be a number.";
                return;
            }

            try
            {
                BackendInterop.borrowBook(title, userId);
                StatusText.Text = $"'{title}' successfully borrowed by User #{userId}.";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Error: {ex.Message}";
            }
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                StatusText.Text = "Please provide a book title.";
                return;
            }

            try
            {
                BackendInterop.returnBook(title);
                StatusText.Text = $"'{title}' successfully returned.";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Error: {ex.Message}";
            }
        }
    }
}
