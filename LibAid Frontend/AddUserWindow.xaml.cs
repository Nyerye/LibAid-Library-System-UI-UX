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
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            string first = FirstNameBox.Text.Trim();
            string last = LastNameBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(first) || string.IsNullOrWhiteSpace(last))
            {
                MessageBox.Show("Please enter both first and last names.");
                return;
            }

            try
            {
                BackendInterop.AddUser(first, last);
                MessageBox.Show($"User '{first} {last}' added.");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
