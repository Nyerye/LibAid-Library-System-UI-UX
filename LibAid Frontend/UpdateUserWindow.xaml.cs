/// <file>
/// UpdateUserWindow.xaml.cs
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
/// Code for the UpdateUserWindow to allow the user to upate a user entry.
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
    /// Class that is used to update a user entry in the database.
    /// Contains the events for the right click feature to allow a user to update a user entry
    /// in the User section of the View Database window.
    /// </summary>
    public partial class UpdateUserWindow : Window
    {
        /// <summary>
        /// The original last name of the user being updated. Private and readonly to not modify the original value. 
        /// </summary>
        private readonly string _originalLastName;

        /// <summary>
        /// Constructor for the UpdateUserWindow.
        /// </summary>
        /// <param name="originalLastName"></param>
        public UpdateUserWindow(string originalLastName)
        {
            InitializeComponent();
            _originalLastName = originalLastName;
        }

        /// <summary>
        /// Event handler for when the user clicks the "Update" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            // Trim the trailing and leading whitespace from the input to prep for validation.
            string newFirst = FirstNameBox.Text.Trim();
            string newLast = LastNameBox.Text.Trim();

            // Check if the new first and last names are empty. Handle gracefully and throw an error if so.
            if (string.IsNullOrWhiteSpace(newFirst) && string.IsNullOrWhiteSpace(newLast))
            {
                MessageBox.Show("Please enter a new first or last name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //Success path
            try
            {
                BackendInterop.UpdateUser(_originalLastName, newFirst, newLast);
                MessageBox.Show("User updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }

            // Falure path
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating user:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
