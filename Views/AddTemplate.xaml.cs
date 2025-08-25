using FolderCreator.Models;
using System.Windows;
using System.Windows.Controls;

namespace FolderCreator.Views
{
    public partial class AddTemplate : Window
    {
        public Template CurrentTemplate { get; set; }

        public AddTemplate()
        {
            InitializeComponent();
            CurrentTemplate = new Template();
            DataContext = this;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string path = NewFolderTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(path))
            {
                CurrentTemplate.AddFolder(path);
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
                CurrentTemplate.Folders.Remove(folder);
            }
        }

        private void SaveTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            SortFolders();

            TemplateManager.SaveTemplate(CurrentTemplate);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SortFolders()
        {
            var sortedFolders = CurrentTemplate.Folders.OrderBy(f => f.Name).ToList();
            foreach (var folder in sortedFolders)
            {
                folder.Subfolders = new System.Collections.ObjectModel.ObservableCollection<TemplateFolder>(folder.Subfolders.OrderBy(sf => sf.Name));
            }
            CurrentTemplate.Folders.Clear();
            foreach (var folder in sortedFolders)
            {
                CurrentTemplate.Folders.Add(folder);
            }
        }
    }
}
