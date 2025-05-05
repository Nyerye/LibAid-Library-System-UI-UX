using System;
using System.Text.RegularExpressions;
using System.Windows;
using LibAidFrontend;

namespace LibAid_Frontend
{
    public partial class AddUserWindow : Window
    {
        public AddUserWindow()
        {
            InitializeComponent();
            FirstNameBox.Focus();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            string first = FirstNameBox.Text.Trim();
            string last = LastNameBox.Text.Trim();

            if (!Regex.IsMatch(first, @"^[A-Za-z]+$") || !Regex.IsMatch(last, @"^[A-Za-z]+$"))
            {
                MessageBox.Show("Only alphabetical characters are allowed for first and last names.",
                                "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {

                BackendInterop.AddUser(first, last);
                MessageBox.Show($"User '{first} {last}' added successfully.", "Success",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while adding user:\n{ex.Message}", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
