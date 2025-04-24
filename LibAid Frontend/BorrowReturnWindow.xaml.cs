using System;
using System.Windows;
using LibAidFrontend;

namespace LibAid_Frontend
{
    public partial class BorrowReturnWindow : Window
    {
        public BorrowReturnWindow()
        {
            InitializeComponent();
            LastNameBox.Focus();
        }

        private void Borrow_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleBox.Text.Trim();
            string lastName = LastNameBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(lastName))
            {
                StatusText.Text = "Please provide both book title and user last name.";
                return;
            }

            try
            {
                if (!BackendInterop.UserExists(lastName))
                {
                    StatusText.Text = $"User with last name '{lastName}' does not exist.";
                    return;
                }

                if (!BackendInterop.BookExists(title))
                {
                    StatusText.Text = $"Book '{title}' does not exist.";
                    return;
                }

                BackendInterop.BorrowBook(lastName, title);
                StatusText.Text = $"'{title}' successfully borrowed by {lastName}.";
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
                if (!BackendInterop.BookExists(title))
                {
                    StatusText.Text = $"Book '{title}' does not exist.";
                    return;
                }

                BackendInterop.ReturnBook(title);
                StatusText.Text = $"'{title}' successfully returned.";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Error: {ex.Message}";
            }
        }
    }
}
