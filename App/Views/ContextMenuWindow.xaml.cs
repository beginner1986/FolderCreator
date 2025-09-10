using FolderCreator.Models;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FolderCreator.Views
{
    public partial class ContextMenuWindow : Window
    {
        private readonly List<string> _templateNames;
        private readonly string _targetPath;

        public ContextMenuWindow(string targetPath, List<string> templateNames)
        {
            InitializeComponent();
            _templateNames = templateNames;
            _targetPath = targetPath;
            this.Loaded += ContextMenuWindow_Loaded;
            PopulateMenuItems();
        }

        private void ContextMenuWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (GetCursorPos(out POINT mousePos))
            {
                var source = PresentationSource.FromVisual(this);
                if (source?.CompositionTarget != null)
                {
                    var matrix = source.CompositionTarget.TransformToDevice;
                    this.Left = mousePos.X / matrix.M11;
                    this.Top = mousePos.Y / matrix.M22;
                }
                else
                {
                    this.Left = mousePos.X;
                    this.Top = mousePos.Y;
                }
            }
        }

        private void PopulateMenuItems()
        {
            foreach (var name in _templateNames)
            {
                var menuItem = new MenuItem
                {
                    Header = name
                };
                menuItem.Click += MenuItem_Click;
                MenuStackPanel.Children.Add(menuItem);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var clickedMenuItem = (MenuItem)sender;
            string selectedTemplateName = (string)clickedMenuItem.Header;

            Template? selectedTemplate = TemplateManager.GetAllTemplates().FirstOrDefault(t => t.Name == selectedTemplateName);
            if (selectedTemplate != null) {
                try
                {
                    Dictionary<string, string> variables = [];

                    if (selectedTemplate.Variables.Count > 0)
                    {
                        SetVariables setVariables = new(selectedTemplate.Variables);
                        if (setVariables.ShowDialog() == true)
                        {
                            variables = setVariables.Variables;
                        }
                        else
                        {
                            this.Close();
                            return;
                        }
                    }

                    bool isSuccess = TemplateManager.ApplyTemplate(selectedTemplate, _targetPath, variables);
                    if (isSuccess)
                    {
                        MessageBox.Show("Foldery zostały poprawnie utworzone.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Wystąpił błąd podczas tworzenia folderów.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            this.Close();
        }

        private void Window_Deactivated(object sender, System.EventArgs e)
        {
            this.Close();
        }

        #region P/Invoke
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }
        #endregion
    }
}
