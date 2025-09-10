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
                    MessageBox.Show($"Błąd inicjalizacji menadżera szablonów: {TemplateManager.InitializationError}",
                        "Błąd inicjalizacji", MessageBoxButton.OK, MessageBoxImage.Error);
                    // Continue anyway to show main window
                }
                
                if (e.Args.Length > 1 && e.Args[0].Equals("-contextmenu", StringComparison.OrdinalIgnoreCase))
                {
                    // Even if there's an initialization error, try to get templates
                    List<Template> templates = TemplateManager.GetAllTemplates().ToList();
                    string targetPath = e.Args[1];

                    if (templates.Count == 0)
                    {
                        MessageBox.Show("Nie znaleziono szablonów lub nie można wczytać szablonów.", 
                            "Brak szablonów", MessageBoxButton.OK, MessageBoxImage.Information);
                        // Fall back to showing main window
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        return;
                    }

                    List<string> names = templates.Select(t => t.Name).ToList();
                    ContextMenuWindow contextMenuWindow = new ContextMenuWindow(targetPath, names);
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
                System.Diagnostics.Debug.WriteLine($"Błąd przy uruchamianiu aplikacji: {ex.Message}");
                MessageBox.Show($"Błąd przy uruchamianiu aplikacji: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                
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
