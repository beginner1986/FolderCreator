using FolderCreator.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FolderCreator.Views
{
    public partial class SetVariables : Window
    {
        private SetVariablesViewModel _viewModel;

        public Dictionary<string, string> Variables
        {
            get
            {
                return _viewModel.Variables.ToDictionary(v => v.Key, v => v.Value);
            }
        }

        public SetVariables(List<string> variableNames)
        {
            InitializeComponent();
            _viewModel = new SetVariablesViewModel(variableNames);
            DataContext = _viewModel;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
