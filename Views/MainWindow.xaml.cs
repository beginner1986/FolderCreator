using FolderCreator.Models;
using FolderCreator.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FolderCreator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Template template = new Template
            {
                Name = "DefaultTemplate",
                Folders = new List<TemplateFolder>
                {
                    new TemplateFolder
                    {
                        Path = "Folder1"
                    },
                    new TemplateFolder
                    {
                        Path = "Folder2"
                    }
                },
            };

            TemplateManager.SaveTemplate(template);

            MainViewModel mainViewModel = new MainViewModel();
            this.DataContext = mainViewModel;
        }
    }
}