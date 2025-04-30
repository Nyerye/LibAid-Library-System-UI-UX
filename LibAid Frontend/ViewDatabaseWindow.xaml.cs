using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LibAidFrontend;
using Microsoft.VisualBasic;

namespace LibAid_Frontend
{
    public partial class ViewDatabaseWindow : Window
    {
        private string _selectedLine;
        private string _currentViewType; // "BOOK" or "USER"

        public ViewDatabaseWindow()
        {
            InitializeComponent();
        }

        private void ViewBooks_Click(object sender, RoutedEventArgs e)
        {
            _currentViewType = "BOOK";
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
            _currentViewType = "USER";
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
                        if (parts.Length >= 5 && parts[4] == "0")
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
                int lineLength = line.Length + 1;
                if (charIndex < currentIndex + lineLength)
                {
                    _selectedLine = line.Trim();
                    break;
                }
                currentIndex += lineLength;
            }
        }

        private void OutputBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_selectedLine) || string.IsNullOrWhiteSpace(_currentViewType))
            {
                e.Handled = true;
                return;
            }

            var menu = new ContextMenu();

            // Update
            var updateItem = new MenuItem { Header = "Update" };
            updateItem.Click += (s, args) => HandleUpdate();
            menu.Items.Add(updateItem);

            // Delete
            var deleteItem = new MenuItem { Header = "Delete" };
            deleteItem.Click += (s, args) => HandleDelete();
            menu.Items.Add(deleteItem);

            if (_currentViewType == "BOOK")
            {
                // Borrow
                var borrowItem = new MenuItem { Header = "Borrow" };
                borrowItem.Click += (s, args) => HandleBorrow();
                menu.Items.Add(borrowItem);

                // Return
                var returnItem = new MenuItem { Header = "Return" };
                returnItem.Click += (s, args) => HandleReturn();
                menu.Items.Add(returnItem);
            }

            OutputBox.ContextMenu = menu;
        }

        private void HandleUpdate()
        {
            if (_currentViewType == "BOOK")
            {
                string title = ExtractBookTitle(_selectedLine);
                var win = new UpdateBookWindow(title);
                win.ShowDialog();
                ViewBooks_Click(null, null);
            }
            else if (_currentViewType == "USER")
            {
                string lastName = ExtractUserLastName(_selectedLine);
                var win = new UpdateUserWindow(lastName);
                win.ShowDialog();
                ViewUsers_Click(null, null);
            }
            _selectedLine = null;
        }

        private void HandleDelete()
        {
            if (_currentViewType == "BOOK")
            {
                string title = ExtractBookTitle(_selectedLine);
                if (MessageBox.Show($"Permanently delete '{title}'?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    BackendInterop.HardDeleteBook(title);
                    ViewBooks_Click(null, null);
                }
            }
            else if (_currentViewType == "USER")
            {
                string lastName = ExtractUserLastName(_selectedLine);
                if (MessageBox.Show($"Permanently delete user '{lastName}'?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    BackendInterop.HardDeleteUser(lastName);
                    ViewUsers_Click(null, null);
                }
            }
            _selectedLine = null;
        }

        private void HandleBorrow()
        {
            string title = ExtractBookTitle(_selectedLine);
            string lastName = PromptUser("Enter user's last name to borrow this book:");
            if (string.IsNullOrWhiteSpace(lastName)) return;

            try
            {
                BackendInterop.BorrowBook(lastName, title);
                MessageBox.Show($"'{title}' borrowed by {lastName}.");
                ViewBooks_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void HandleReturn()
        {
            string title = ExtractBookTitle(_selectedLine);
            try
            {
                BackendInterop.ReturnBook(title);
                MessageBox.Show($"'{title}' returned successfully.");
                ViewBooks_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private string ExtractBookTitle(string line)
        {
            string[] columns = line.Split('|');
            return columns.Length > 0 ? columns[0].Trim() : "";
        }

        private string ExtractUserLastName(string line)
        {
            string[] columns = line.Split('|');
            return columns.Length > 2 ? columns[2].Trim() : "";
        }

        private string PromptUser(string message)
        {
            return Interaction.InputBox(message, "Input Required", "");
        }
    }
}
