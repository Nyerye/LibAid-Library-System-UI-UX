/// <file>
/// BorrowReturnWindow.xaml.cs
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
/// Backend code for user when they borrow or return a book.
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
    /// Class that contains the backend code for the BorrowReturnWindow.
    /// Contains the event for when the user decides to borrow a book or return a book.
    /// </summary>
    public partial class BorrowReturnWindow : Window
    {
        /// <summary>
        /// Constructor for the BorrowReturnWindow.
        /// </summary>
        public BorrowReturnWindow()
        {
            InitializeComponent();
            LastNameBox.Focus();
        }

        /// <summary>
        /// Event handler for when the user clicks the "Borrow" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Borrow_Click(object sender, RoutedEventArgs e)
        {
            // Trim the trailing and leading whitespace from the input to prep for validation.
            string title = TitleBox.Text.Trim();
            string lastName = LastNameBox.Text.Trim();

            // Check if the title and last name are valid entries
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(lastName))
            {
                StatusText.Text = "Please provide both book title and user last name.";
                return;
            }

            // Confirm the user and title does indeed exist. Cant have a user that does not exist but somehow borrows a book and vice versa.
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

        /// <summary>
        /// Event handler for when the user clicks the "Return" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            // Trim the trailing and leading whitespace from the input to prep for validation.
            string title = TitleBox.Text.Trim();

            // Confirm the field was not empty.
            if (string.IsNullOrWhiteSpace(title))
            {
                StatusText.Text = "Please provide a book title.";
                return;
            }

            // Confirm the title they are returning does indeed exist. 
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
