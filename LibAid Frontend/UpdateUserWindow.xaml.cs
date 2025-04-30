using System;
using System.Windows;
using LibAidFrontend;

namespace LibAid_Frontend
{
    public partial class UpdateUserWindow : Window
    {
        private readonly string _originalLastName;

        public UpdateUserWindow(string originalLastName)
        {
            InitializeComponent();
            _originalLastName = originalLastName;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string newFirst = FirstNameBox.Text.Trim();
            string newLast = LastNameBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(newFirst) && string.IsNullOrWhiteSpace(newLast))
            {
                MessageBox.Show("Please enter a new first or last name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                BackendInterop.UpdateUser(_originalLastName, newFirst, newLast);
                MessageBox.Show("User updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating user:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
