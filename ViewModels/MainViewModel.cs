using FolderCreator.Commands;
using FolderCreator.Models;
using FolderCreator.Views;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FolderCreator.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<Template> Templates { get; set; }

        public ICommand AddTemplateCommand { get; set; }
        public ICommand DeleteTemplateCommand { get; set; }
        public ICommand EditTemplateCommand { get; set; }
        public ICommand UseTemplateCommand { get; set; }

        public MainViewModel()
        {
            Templates = TemplateManager.GetAllTemplates();

            AddTemplateCommand = new RelayCommand(ShowWindow, CanShowWindow);
            DeleteTemplateCommand = new RelayCommand(DeleteTemplate, CanDeleteTemplate);
            EditTemplateCommand = new RelayCommand(EditTemplate, CanEditTemplate);
            UseTemplateCommand = new RelayCommand(UseTemplate, CanUseTemplate);

            // For testing purposes, add a sample template if none exist
            if (Templates.Count == 0)
            {
                var sampleTemplate = new Template
                {
                    Name = "Pierwszy Szablon"
                };

                sampleTemplate.AddFolder("Projekt/{{Nazwa}}/src");
                sampleTemplate.AddFolder("Projekt/{{Nazwa}}/docs");
                sampleTemplate.AddFolder("Projekt/{{Nazwa}}/tests");

                Templates.Add(sampleTemplate);
                TemplateManager.SaveTemplate(sampleTemplate);
            }
        }

        private bool CanDeleteTemplate(object? obj)
        {
            return obj is Template;
        }

        private void DeleteTemplate(object? obj)
        {
            if (obj is Template template)
            {
                TemplateManager.DeleteTemplate(template.Name);
                Templates.Remove(template);
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

                    // Show SetVariables window if there are variables
                    if (template.Variables.Count > 0)
                    {
                        SetVariables setVariables = new(template.Variables);
                        if (setVariables.ShowDialog() == true)
                        {
                            variables = setVariables.Variables;                          
                        }
                        else
                        {
                            return; // User cancelled the operation
                        }
                    }

                    TemplateManager.ApplyTemplate(template, folderName, variables);
                }
            }
        }
    }
}
