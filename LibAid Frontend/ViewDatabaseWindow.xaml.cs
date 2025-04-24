using System;
using System.IO;
using System.Windows;

namespace LibAid_Frontend
{
    public partial class ViewDatabaseWindow : Window
    {
        public ViewDatabaseWindow()
        {
            InitializeComponent();
        }

        private void ViewBooks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] lines = File.ReadAllLines("database.txt");
                OutputBox.Text = "=== All Books ===\n\n";

                foreach (string line in lines)
                {
                    if (line.StartsWith("BOOK,"))
                    {
                        string[] parts = line.Split(',');

                        if (parts.Length >= 7)
                        {
                            string title = parts[2];
                            string author = parts[3];
                            bool isBorrowed = parts[4] == "1";
                            string borrowedBy = parts[5];
                            bool isDeleted = parts[6] == "1";

                            if (isDeleted)
                                continue; // Skip soft-deleted books

                            string status = isBorrowed
                                ? $"Borrowed by User {borrowedBy}"
                                : "Available";

                            OutputBox.AppendText($"📖 {title} by {author} — {status}\n");
                        }
                        else
                        {
                            OutputBox.AppendText("⚠️ Error parsing line: " + line + "\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading books: " + ex.Message);
            }
        }

        private void ViewUsers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] lines = File.ReadAllLines("database.txt");
                OutputBox.Text = "=== All Users ===\n\n";

                foreach (string line in lines)
                {
                    if (line.StartsWith("USER,"))
                    {
                        string[] parts = line.Split(',');

                        if (parts.Length >= 5)
                        {
                            string id = parts[1];
                            string first = parts[2];
                            string last = parts[3];
                            bool isDeleted = parts[4] == "1";

                            if (isDeleted)
                                continue; // Skip soft-deleted users

                            OutputBox.AppendText($"🧑 User #{id} — {first} {last}\n");
                        }
                        else
                        {
                            OutputBox.AppendText("⚠️ Error parsing line: " + line + "\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading users: " + ex.Message);
            }
        }
    }
}
