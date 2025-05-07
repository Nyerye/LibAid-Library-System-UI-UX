/// <file>
/// AddUserWindow.xaml.cs
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
/// Backend code for user when they add a user to the system.
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
    /// Class that contains the backend code for the AddUserWindow
    /// Also contains the event handler for when a user clicks the "Add User" button.
    /// </summary>
    public partial class AddUserWindow : Window
    {
        /// <summary>
        /// Constructor for the AddUserWindow.
        /// </summary>
        public AddUserWindow()
        {
            InitializeComponent();
            FirstNameBox.Focus();
        }

        /// <summary>
        /// Event handler for when the user clicks the "Add User" button.
        /// Checks to confirm that the first and last names are valid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            // Trim whitespace from the input fields so its prepped for validation
            string first = FirstNameBox.Text.Trim();
            string last = LastNameBox.Text.Trim();

            // If the regex fails, throw an error gracefully and allow user to enter input again.
            if (!Regex.IsMatch(first, @"^[A-Za-z]+$") || !Regex.IsMatch(last, @"^[A-Za-z]+$"))
            {
                MessageBox.Show("Only alphabetical characters are allowed for first and last names.",
                                "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Call the improted function to add them if regex succeeds. Give a message if successful
            try
            {

                BackendInterop.AddUser(first, last);
                MessageBox.Show($"User '{first} {last}' added successfully.", "Success",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }

            // If the user fials to be made, throw an error.
            catch (Exception ex)
            {
                MessageBox.Show($"Error while adding user:\n{ex.Message}", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
