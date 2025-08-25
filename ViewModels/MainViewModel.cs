using FolderCreator.Commands;
using FolderCreator.Models;
using FolderCreator.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FolderCreator.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<Template> Templates { get; set; }

        public ICommand AddTemplateCommand { get; set; }
        public ICommand DeleteTemplateCommand { get; set; }

        public MainViewModel()
        {
            Templates = TemplateManager.GetAllTemplates();

            AddTemplateCommand = new RelayCommand(ShowWindow, CanShowWindow);
            DeleteTemplateCommand = new RelayCommand(DeleteTemplate, CanDeleteTemplate);

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
            AddTemplate addTemplate = new AddTemplate();

            addTemplate.ShowDialog();
        }
    }
}
