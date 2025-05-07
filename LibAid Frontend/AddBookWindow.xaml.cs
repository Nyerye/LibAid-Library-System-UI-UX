/// <file>
/// AddBookWindow.xaml.cs
/// </file>
/// <project>
/// LibAId v3.0.12
/// </project>
/// <author>
/// Nicholas Reilly
/// </author>
/// <date>
/// 2025-03-27
/// </date>
/// <description>
/// Backend code for user when they add a book to the system.
/// </description>
/// <references>
/// Deitel, P., & Deitel, H. (2017). *C# 6 for Programmers Sixth Edition* 
/// (Sixth, Ser. Deitel Development Series). Pearson Education.
/// </references>
/// 

using System;
using System.Text.RegularExpressions;
using System.Windows;
using LibAidFrontend;

namespace LibAid_Frontend
{
    /// <summary>
    /// Class that contains the backend code for the AddBookWindow.
    /// Has the event handler for when a user clicks the "Add Book" button.
    /// </summary>
    public partial class AddBookWindow : Window
    {
        /// <summary>
        /// Constructor for the AddBookWindow.
        /// </summary>
        public AddBookWindow()
        {
            InitializeComponent();
            TitleBox.Focus();
        }

        /// <summary>
        /// Event handler for when the user clicks the "Add Book" button.
        /// Checks a regex to confirm both the author and the title are valid.
        /// Handles gracefully should it not be valid
        /// </summary>
        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            // Remove any trailing and leading whitespace from the input to prep for valdiation.
            string title = TitleBox.Text.Trim();
            string author = AuthorBox.Text.Trim();

            // Check if the title and author are valid entries
            if (!Regex.IsMatch(title, @"^[A-Za-z0-9 ,.'-]+$") ||
                !Regex.IsMatch(author, @"^[A-Za-z ,.'-]+$"))
            {
                MessageBox.Show("Title and author must only contain letters, numbers, and basic punctuation.",
                                "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check if the title exists. If it does, throw it out. Duplicate entrys are not allowed.
            try
            {
                if (BackendInterop.BookExists(title))
                {
                    MessageBox.Show($"A book with the title '{title}' already exists.",
                                    "Duplicate Book", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                BackendInterop.AddBook(title, author);
                MessageBox.Show($"Book '{title}' by '{author}' added successfully.", "Success",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while adding book:\n{ex.Message}", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
