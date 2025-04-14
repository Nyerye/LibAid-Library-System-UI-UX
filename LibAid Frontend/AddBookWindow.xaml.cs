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
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleBox.Text.Trim();
            string author = AuthorBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author))
            {
                MessageBox.Show("Please enter both the title and author.");
                return;
            }

            try
            {
                BackendInterop.AddBook(title, author);
                MessageBox.Show($"Book '{title}' by '{author}' added.");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
