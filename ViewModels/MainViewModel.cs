using FolderCreator.Commands;
using FolderCreator.Models;
using FolderCreator.Views;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace FolderCreator.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    FilterTemplates();
                }
            }
        }

        private ObservableCollection<Template> _templates;
        public ObservableCollection<Template> Templates
        {
            get => _templates;
            set
            {
                _templates = value;
                OnPropertyChanged(nameof(Templates));
            }
        }

        private ObservableCollection<Template> _filteredTemplates;
        public ObservableCollection<Template> FilteredTemplates
        {
            get => _filteredTemplates;
            set
            {
                _filteredTemplates = value;
                OnPropertyChanged(nameof(FilteredTemplates));
            }
        }

        public ICommand AddTemplateCommand { get; set; }
        public ICommand DeleteTemplateCommand { get; set; }
        public ICommand EditTemplateCommand { get; set; }
        public ICommand UseTemplateCommand { get; set; }

        public MainViewModel()
        {
            _templates = TemplateManager.GetAllTemplates();
            FilteredTemplates = new ObservableCollection<Template>(_templates);

            AddTemplateCommand = new RelayCommand(ShowWindow, CanShowWindow);
            DeleteTemplateCommand = new RelayCommand(DeleteTemplate, CanDeleteTemplate);
            EditTemplateCommand = new RelayCommand(EditTemplate, CanEditTemplate);
            UseTemplateCommand = new RelayCommand(UseTemplate, CanUseTemplate);
        }

        private bool CanDeleteTemplate(object? obj)
        {
            return obj is Template;
        }

        private void DeleteTemplate(object? obj)
        {
            if (obj is Template template)
            {
                MessageBoxResult result = MessageBox.Show($"Czy na pewno chcesz usunąć szablon '{template.Name}'?", "Potwierdzenie usunięcia", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    TemplateManager.DeleteTemplate(template.Name);
                    Templates.Remove(template);
                    FilterTemplates();
                }
            }
        }

        private bool CanShowWindow(object? obj)
        {
            return true;
        }

        private void ShowWindow(object? obj)
        {
            AddTemplate addTemplate = new();

            addTemplate.ShowDialog();
        }

        private bool CanEditTemplate(object? obj)
        {
            return obj is Template;
        }

        private void EditTemplate(object? obj)
        {
            if (obj is Template template)
            {
                AddTemplate addTemplate = new(template);
                addTemplate.ShowDialog();
            }
        }

        private bool CanUseTemplate(object? obj)
        {
            return obj is Template;
        }

        private void UseTemplate(object? obj)
        {
            if (obj is Template template)
            {
                var folderDialog = new OpenFolderDialog
                {
                    Title = "Wybierz lokalizację do utworzenia folderów",
                    Multiselect = false
                };

                if (folderDialog.ShowDialog() == true)
                {
                    var folderName = folderDialog.FolderName;
                    Dictionary<string, string> variables = [];

                    if (template.Variables.Count > 0)
                    {
                        SetVariables setVariables = new(template.Variables);
                        if (setVariables.ShowDialog() == true)
                        {
                            variables = setVariables.Variables;
                        }
                        else
                        {
                            return;
                        }
                    }

                    bool isSuccess = TemplateManager.ApplyTemplate(template, folderName, variables);
                    if (isSuccess)
                    {
                        MessageBox.Show("Foldery zostały pomyślnie utworzone.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                        System.Diagnostics.Process.Start("explorer.exe", folderName);
                    }
                    else
                    {
                        MessageBox.Show("Wystąpił błąd podczas tworzenia folderów.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void FilterTemplates()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                FilteredTemplates = new ObservableCollection<Template>(Templates);
            }
            else
            {
                FilteredTemplates = new ObservableCollection<Template>(Templates.Where(t => t.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || t.Folders.Any(f => f.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || ContainsTextRecursive(f, SearchText))));
            }
        }

        private bool ContainsTextRecursive(TemplateFolder folder, string searchText)
        {
            if (folder.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return folder.Subfolders.Any(sf => ContainsTextRecursive(sf, searchText));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
