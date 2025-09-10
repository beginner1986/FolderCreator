using FolderCreator.Models;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FolderCreator.Views
{
    public partial class ContextMenuWindow : Window
    {
        private readonly List<string> _templateNames;

        public ContextMenuWindow(List<string> templateNames)
        {
            InitializeComponent();
            _templateNames = templateNames;
            this.Loaded += ContextMenuWindow_Loaded;
            PopulateMenuItems();
        }

        private void ContextMenuWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (GetCursorPos(out POINT mousePos))
            {
                // Get the PresentationSource for this window to handle DPI scaling.
                PresentationSource source = PresentationSource.FromVisual(this);
                if (source != null)
                {
                    Matrix transform = source.CompositionTarget.TransformFromDevice;
                    Point wpfMousePos = transform.Transform(new Point(mousePos.X, mousePos.Y));
                    this.Left = wpfMousePos.X;
                    this.Top = wpfMousePos.Y;
                }
                else
                {
                    // Fallback for cases where PresentationSource is not available.
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

            // TODO: apply template to selected folder

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
