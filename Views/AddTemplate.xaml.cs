using FolderCreator.Models;
using FolderCreator.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FolderCreator.Views
{
    public partial class AddTemplate : Window
    {
        public Template CurrentTemplate { get; set; }
        private TemplateFolder? CurrentlyEditing { get; set; }

        public AddTemplate() : this(null, true)
        {
        }

        public AddTemplate(Template? template) : this(template, false)
        {
        }

        public AddTemplate(Template? template, bool isNew)
        {
            InitializeComponent();
            CurrentTemplate = template ?? new Template();
            DataContext = this;

            if (isNew)
            {
                Title = "Nowy szablon";
            }
            else
            {
                Title = "Edytuj szablon";
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is TemplateFolder folder)
            {
                StartEditing(folder);
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

            if (Application.Current.MainWindow?.DataContext is MainViewModel mainViewModel)
            {
                var existingTemplate = mainViewModel.Templates.FirstOrDefault(t => t.Name == CurrentTemplate.Name);

                if (existingTemplate != null)
                {
                    MessageBoxResult result = MessageBox.Show($"Szablon o nazwie '{CurrentTemplate.Name}' już istnieje. Czy chcesz go zastąpić?", "Zastąpić istniejący szablon?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        TemplateManager.SaveTemplate(CurrentTemplate);

                        int index = mainViewModel.Templates.IndexOf(existingTemplate);
                        mainViewModel.Templates[index] = CurrentTemplate;

                        mainViewModel.FilteredTemplates = new System.Collections.ObjectModel.ObservableCollection<Template>(mainViewModel.Templates);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    TemplateManager.SaveTemplate(CurrentTemplate);

                    mainViewModel.Templates.Add(CurrentTemplate);

                    mainViewModel.FilteredTemplates = new System.Collections.ObjectModel.ObservableCollection<Template>(mainViewModel.Templates);
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
                TemplateFolder? parent = FindParentFolder(CurrentTemplate.Folders, subfolder);
                if (parent != null)
                {
                    parent.Subfolders.Remove(subfolder);
                }
                else
                {
                    CurrentTemplate.Folders.Remove(subfolder);
                }
            }
        }

        private static TemplateFolder? FindParentFolder(System.Collections.ObjectModel.ObservableCollection<TemplateFolder> folders, TemplateFolder target)
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

        private void EditSubfolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is TemplateFolder subfolder)
            {
                StartEditing(subfolder);
            }
        }

        private void TreeViewItem_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is TreeViewItem item && item.Header is TemplateFolder subfolder)
            {
                StartEditing(subfolder);
                e.Handled = true; // Prevent further processing of the double-click
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.DataContext is TemplateFolder subfolder)
            {
                StopEditing(subfolder);
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender is TextBox textBox && textBox.DataContext is TemplateFolder subfolder)
                {
                    StopEditing(subfolder);
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (CurrentlyEditing != null)
                {
                    StopEditing(CurrentlyEditing);
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void AddSubfolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is TemplateFolder folder)
            {
                folder.AddSubfolder("Nowy Folder");
                SortFolders();
                var newSubfolder = folder.Subfolders.FirstOrDefault(sf => sf.Name == "Nowy Folder");
                if (newSubfolder != null)
                {
                    StartEditing(newSubfolder);
                }
            }
        }

        private T? FindVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = System.Windows.Media.VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }

                    T? childItem = FindVisualChild<T>(child);
                    if (childItem != null) return childItem;
                }
            }

            return null;
        }

        private void StartEditing(TemplateFolder folder)
        {
            if (CurrentlyEditing != null)
            {
                CurrentlyEditing.IsEditing = false;
            }
            CurrentlyEditing = folder;
            CurrentlyEditing.IsEditing = true;
        }

        private void StopEditing(TemplateFolder folder)
        {
            if (CurrentlyEditing == folder)
            {
                folder.IsEditing = false;
                CurrentlyEditing = null;
            }
        }
    }
}
