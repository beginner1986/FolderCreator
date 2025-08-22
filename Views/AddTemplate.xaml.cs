using FolderCreator.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace FolderCreator.Views
{
    public partial class AddTemplate : Window
    {
        public ObservableCollection<TemplateFolder> Folders { get; set; }

        public AddTemplate()
        {
            InitializeComponent();
            Folders = [];
            DataContext = this;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string path = NewFolderTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(path))
            {
                Folders.Add(new TemplateFolder { Path = path });
                NewFolderTextBox.Clear();
                SortFolders();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle edit logic here
            if (sender is Button button && button.Tag is string folder)
            {
                // You can implement edit functionality here, such as opening a dialog to edit the folder's properties
                MessageBox.Show($"Editing {folder}");
                SortFolders();
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is TemplateFolder folder)
            {
                Folders.Remove(folder);
            }
        }

        private void SaveTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            // Save template logic here
            SortFolders();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SortFolders()
        {
            var sortedFolders = Folders.OrderBy(f => f.Path).ToList();
            Folders.Clear();
            foreach (var folder in sortedFolders)
            {
                Folders.Add(folder);
            }
        }
    }
}
