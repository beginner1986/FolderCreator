using FolderCreator.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FolderCreator.Views
{
    public partial class ContextMenuWindow : Window
    {
        private readonly List<string> _templateNames;

        public ContextMenuWindow(List<string> templateNames)
        {
            InitializeComponent();
            _templateNames = templateNames;

            // Optional: Position the window where the mouse is.
            this.Left = Mouse.GetPosition(this).X;
            this.Top = Mouse.GetPosition(this).Y;

            PopulateMenuItems();
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

            // TODO: apply template to selected folder

            this.Close();
        }

        private void Window_Deactivated(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
