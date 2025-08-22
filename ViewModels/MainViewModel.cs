using FolderCreator.Models;
using System.Collections.ObjectModel;

namespace FolderCreator.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<Template> Templates { get; set; }

        public MainViewModel()
        {
            Templates = TemplateManager.GetAllTemplates();
        }
    }
}
