/// <file>
/// UpdateBookWindow.xaml.cs
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
/// COde that allows the right click function for Updating a Book entry
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
    /// Class that contains the code for Updating a book.
    /// Also has the events that handle the right click functionality for when a user right clicks on a book
    /// entry that is located in the Book section in the View Database window. 
    /// </summary>
    public partial class UpdateBookWindow : Window
    {
        private readonly string _originalTitle;

        /// <summary>
        /// Constructor for the UpdateBookWindow.
        /// </summary>
        /// <param name="originalTitle"></param>
        public UpdateBookWindow(string originalTitle)
        {
            InitializeComponent();
            _originalTitle = originalTitle;
        }

        /// <summary>
        /// Event handler for when the user clicks the "Update" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            // Trim the trailing and leading whitespace from the input to prep for validation.
            string newTitle = TitleBox.Text.Trim();
            string newAuthor = AuthorBox.Text.Trim();

            // Check if the title and author are empty. Handle gracefully and throw an error if so. 
            if (string.IsNullOrWhiteSpace(newTitle) && string.IsNullOrWhiteSpace(newAuthor))
            {
                MessageBox.Show("Please enter a new title or author.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Success path
            try
            {
                BackendInterop.UpdateBook(_originalTitle, newTitle, newAuthor);
                MessageBox.Show("Book updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }

            // Failure path
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating book:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
