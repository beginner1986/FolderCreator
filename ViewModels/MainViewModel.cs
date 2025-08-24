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

        public MainViewModel()
        {
            Templates = TemplateManager.GetAllTemplates();

            AddTemplateCommand = new RelayCommand(ShowWindow, CanShowWindow);

            // For testing purposes, add a sample template if none exist
            if (Templates.Count == 0)
            {
                var sampleTemplate = new Template
                {
                    Name = "Sample Template"
                };

                sampleTemplate.AddFolder("Project/{{ProjectName}}/src");
                sampleTemplate.AddFolder("Project/{{ProjectName}}/docs");
                sampleTemplate.AddFolder("Project/{{ProjectName}}/tests");

                Templates.Add(sampleTemplate);
                TemplateManager.SaveTemplate(sampleTemplate);
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
