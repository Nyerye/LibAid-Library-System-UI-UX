using System;
using System.Windows;
using LibAidFrontend;

namespace LibAid_Frontend
{
    public partial class AddUserWindow : Window
    {
        public AddUserWindow()
        {
            InitializeComponent();
            FirstNameBox.Focus(); // Autofocus first name input
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            string first = FirstNameBox.Text.Trim();
            string last = LastNameBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(first) || string.IsNullOrWhiteSpace(last))
            {
                MessageBox.Show("Please enter both first and last names.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (BackendInterop.UserExists(last))
                {
                    MessageBox.Show($"A user with the last name '{last}' already exists.", "Duplicate User", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                BackendInterop.AddUser(first, last);
                MessageBox.Show($"User '{first} {last}' added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while adding user:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
