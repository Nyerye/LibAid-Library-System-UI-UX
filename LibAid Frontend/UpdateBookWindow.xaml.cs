using System;
using System.Windows;
using LibAidFrontend;

namespace LibAid_Frontend
{
    public partial class UpdateBookWindow : Window
    {
        private readonly string _originalTitle;

        public UpdateBookWindow(string originalTitle)
        {
            InitializeComponent();
            _originalTitle = originalTitle;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string newTitle = TitleBox.Text.Trim();
            string newAuthor = AuthorBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(newTitle) && string.IsNullOrWhiteSpace(newAuthor))
            {
                MessageBox.Show("Please enter a new title or author.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                BackendInterop.UpdateBook(_originalTitle, newTitle, newAuthor);
                MessageBox.Show("Book updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating book:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
