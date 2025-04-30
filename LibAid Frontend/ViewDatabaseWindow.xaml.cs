using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LibAidFrontend;

namespace LibAid_Frontend
{
    public partial class ViewDatabaseWindow : Window
    {
        private string _selectedLine;
        private string _selectedType; 

        public ViewDatabaseWindow()
        {
            InitializeComponent();
        }

        private void ViewBooks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] lines = File.ReadAllLines("database.txt");
                OutputBox.Text = "Books:\n\n";
                OutputBox.AppendText("Title                 | Author           | Status\n");
                OutputBox.AppendText("----------------------|------------------|----------\n");

                foreach (string line in lines)
                {
                    if (line.StartsWith("BOOK,"))
                    {
                        string[] parts = line.Split(',');

                        if (parts.Length >= 7 && parts[6] == "0")
                        {
                            string rawTitle = parts[2];
                            string rawAuthor = parts[3];
                            string status = parts[4] == "1" ? "Borrowed" : "Available";

                            string title = (rawTitle.Length > 21 ? rawTitle.Substring(0, 21) + "…" : rawTitle).PadRight(22);
                            string author = (rawAuthor.Length > 17 ? rawAuthor.Substring(0, 17) + "…" : rawAuthor).PadRight(18);
                            status = status.PadRight(10);

                            OutputBox.AppendText($"{title}|{author}|{status}\n");
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
                OutputBox.Text = "Users:\n\n";
                OutputBox.AppendText("ID    | First Name       | Last Name\n");
                OutputBox.AppendText("------|------------------|-----------------\n");

                foreach (string line in lines)
                {
                    if (line.StartsWith("USER,"))
                    {
                        string[] parts = line.Split(',');

                        if (parts.Length >= 5 && parts[4] == "0") // not deleted
                        {
                            string id = parts[1].PadRight(5);
                            string first = parts[2].PadRight(16);
                            string last = parts[3].PadRight(15);

                            OutputBox.AppendText($"{id} | {first} | {last}\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading users: " + ex.Message);
            }
        }

        private void OutputBox_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point clickPoint = e.GetPosition(OutputBox);
            int charIndex = OutputBox.GetCharacterIndexFromPoint(clickPoint, true);

            if (charIndex < 0) return;

            string[] lines = OutputBox.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            int currentIndex = 0;

            foreach (var line in lines)
            {
                int lineLength = line.Length + 1; // +1 for newline
                if (charIndex < currentIndex + lineLength)
                {
                    _selectedLine = line.Trim();
                    break;
                }
                currentIndex += lineLength;
            }

            if (_selectedLine.StartsWith("📖"))
                _selectedType = "BOOK";
            else if (_selectedLine.StartsWith("🧑"))
                _selectedType = "USER";
            else
                _selectedType = null;
        }


        private void OutputBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_selectedLine) || _selectedType == null)
            {
                e.Handled = true;
                return;
            }

            var menu = new ContextMenu();

            var updateItem = new MenuItem { Header = "Update" };
            updateItem.Click += (s, args) => HandleUpdate();

            var deleteItem = new MenuItem { Header = "Delete" };
            deleteItem.Click += (s, args) => HandleDelete();

            menu.Items.Add(updateItem);
            menu.Items.Add(deleteItem);

            OutputBox.ContextMenu = menu;
        }

        private void HandleUpdate()
        {
            if (_selectedType == "BOOK")
            {
                string title = ExtractBookTitle(_selectedLine);
                var win = new UpdateBookWindow(title); // You’ll build this window
                win.ShowDialog();
            }
            else if (_selectedType == "USER")
            {
                string lastName = ExtractUserLastName(_selectedLine);
                var win = new UpdateUserWindow(lastName); // You’ll build this window
                win.ShowDialog();
            }
            _selectedLine = null;
            _selectedType = null;

        }

        private void HandleDelete()
        {
            if (_selectedType == "BOOK")
            {
                string title = ExtractBookTitle(_selectedLine);
                if (MessageBox.Show($"Permanently delete '{title}'?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    BackendInterop.HardDeleteBook(title);
                    ViewBooks_Click(null, null);
                }
            }
            else if (_selectedType == "USER")
            {
                string lastName = ExtractUserLastName(_selectedLine);
                if (MessageBox.Show($"Permanently delete user '{lastName}'?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    BackendInterop.HardDeleteUser(lastName);
                    ViewUsers_Click(null, null);
                }
            }
            _selectedLine = null;
            _selectedType = null;

        }
        private string ExtractBookTitle(string line)
        {
            // Format: 📖 <title> by <author> — Status
            var byIndex = line.IndexOf(" by ");
            return byIndex > 0 ? line.Substring(2, byIndex - 2).Trim() : string.Empty;
        }

        private string ExtractUserLastName(string line)
        {
            // Format: 🧑 User #<id> — <first> <last>
            var dashIndex = line.IndexOf("—");
            if (dashIndex < 0) return "";
            string fullName = line.Substring(dashIndex + 1).Trim();
            string[] parts = fullName.Split(' ');
            return parts.Length >= 2 ? parts[1] : "";
        }

    }
}
