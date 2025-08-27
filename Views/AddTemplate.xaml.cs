using FolderCreator.Models;
using FolderCreator.ViewModels;
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

        private async void SaveTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            SortFolders();

            // Get the MainViewModel instance
            if (Application.Current.MainWindow?.DataContext is MainViewModel mainViewModel)
            {
                // Check if the template already exists in the collection
                var existingTemplate = mainViewModel.Templates.FirstOrDefault(t => t.Name == CurrentTemplate.Name);

                if (existingTemplate != null)
                {
                    // Ask the user if they want to replace the existing template
                    MessageBoxResult result = MessageBox.Show($"Szablon o nazwie '{CurrentTemplate.Name}' już istnieje. Czy chcesz go zastąpić?", "Zastąpić istniejący szablon?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        // If the user chooses to replace, update the template
                        TemplateManager.SaveTemplate(CurrentTemplate);

                        // Replace the existing template in the collection
                        int index = mainViewModel.Templates.IndexOf(existingTemplate);
                        mainViewModel.Templates[index] = CurrentTemplate;
                    }
                    else
                    {
                        // If the user chooses to cancel, do nothing
                        return;
                    }
                }
                else
                {
                    // If the template doesn't exist, save it
                    TemplateManager.SaveTemplate(CurrentTemplate);

                    // Add the new template to the collection
                    mainViewModel.Templates.Add(CurrentTemplate);
                }
            }

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

        private void RemoveSubfolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is TemplateFolder subfolder)
            {
                // Find parent folder
                TemplateFolder? parent = FindParentFolder(CurrentTemplate.Folders, subfolder);
                if (parent != null)
                {
                    parent.Subfolders.Remove(subfolder);
                }
                else
                {
                    // If not found in subfolders, try top-level folders
                    CurrentTemplate.Folders.Remove(subfolder);
                }
            }
        }

        // Helper method to find the parent of a subfolder
        private TemplateFolder? FindParentFolder(System.Collections.ObjectModel.ObservableCollection<TemplateFolder> folders, TemplateFolder target)
        {
            foreach (var folder in folders)
            {
                if (folder.Subfolders.Contains(target))
                    return folder;
                var parent = FindParentFolder(folder.Subfolders, target);
                if (parent != null)
                    return parent;
            }
            return null;
        }
    }
}
