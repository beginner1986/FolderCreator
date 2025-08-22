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
        }

        private bool CanShowWindow(object? obj)
        {
            return true;
        }

        private void ShowWindow(object? obj)
        {
            AddTemplate addTemplate = new AddTemplate();

            addTemplate.Show();
        }
    }
}
