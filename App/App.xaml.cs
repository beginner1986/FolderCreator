using FolderCreator.Models;
using FolderCreator.Views;
using System.Windows;

namespace FolderCreator
{
    public partial class App : Application
    {
        public App()
        {
            this.Startup += Application_Startup;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {                
                if (e.Args.Length > 0 && e.Args[0].Equals("-contextmenu", StringComparison.OrdinalIgnoreCase))
                {
                    // TODO: Handle context menu logic here
                }
                else
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in startup: {ex.Message}");
                MessageBox.Show($"Error during startup: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
