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
                // Check if TemplateManager is initialized properly
                if (!TemplateManager.IsInitialized)
                {
                    MessageBox.Show($"Error initializing template system: {TemplateManager.InitializationError}", 
                        "Initialization Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    // Continue anyway to show main window
                }
                
                if (e.Args.Length > 0 && e.Args[0].Equals("-contextmenu", StringComparison.OrdinalIgnoreCase))
                {
                    // Even if there's an initialization error, try to get templates
                    List<Template> templates = TemplateManager.GetAllTemplates().ToList();
                    if (templates.Count == 0)
                    {
                        MessageBox.Show("No templates found or templates couldn't be loaded.", 
                            "No Templates", MessageBoxButton.OK, MessageBoxImage.Information);
                        // Fall back to showing main window
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        return;
                    }

                    List<string> names = templates.Select(t => t.Name).ToList();
                    ContextMenuWindow contextMenuWindow = new ContextMenuWindow(names);
                    contextMenuWindow.Show();
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
                
                // Always try to show main window if possible
                try
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                }
                catch
                {
                    // If we can't even show the main window, just exit
                    this.Shutdown(1);
                }
            }
        }
    }
}
