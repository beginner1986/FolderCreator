using FolderCreator.ViewModels;
using System.Windows;

namespace FolderCreator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainViewModel mainViewModel = new MainViewModel();
            this.DataContext = mainViewModel;

            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TemplatesTreeView_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Delete)
            {
                if (DataContext is MainViewModel mainViewModel && mainViewModel.DeleteTemplateCommand.CanExecute(TemplatesTreeView.SelectedItem))
                {
                    mainViewModel.DeleteTemplateCommand.Execute(TemplatesTreeView.SelectedItem);
                }
            }

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (DataContext is MainViewModel mainViewModel && mainViewModel.EditTemplateCommand.CanExecute(TemplatesTreeView.SelectedItem))
                {
                    mainViewModel.EditTemplateCommand.Execute(TemplatesTreeView.SelectedItem);
                }
            }
        }
    }
}