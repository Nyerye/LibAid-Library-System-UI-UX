using System;
using System.Windows;
using LibAidFrontend;

namespace LibAid_Frontend
{
    public partial class AddBookWindow : Window
    {
        public AddBookWindow()
        {
            InitializeComponent();
            TitleBox.Focus(); // Auto-focus on title input
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleBox.Text.Trim();
            string author = AuthorBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author))
            {
                MessageBox.Show("Please enter both the title and author.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (BackendInterop.BookExists(title))
                {
                    MessageBox.Show($"A book with the title '{title}' already exists.", "Duplicate Book", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                BackendInterop.AddBook(title, author);
                MessageBox.Show($"Book '{title}' by '{author}' added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while adding book:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
