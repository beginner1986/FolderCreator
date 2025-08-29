using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FolderCreator.ViewModels
{
    public class SetVariablesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<VariableViewModel> _variables;

        public ObservableCollection<VariableViewModel> Variables
        {
            get => _variables;
            set
            {
                _variables = value;
                OnPropertyChanged(nameof(Variables));
            }
        }

        public SetVariablesViewModel(List<string> variableNames)
        {
            Variables = new ObservableCollection<VariableViewModel>(
                variableNames.Select(name => new VariableViewModel { Key = name })
            );
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class VariableViewModel : INotifyPropertyChanged
    {
        private string _key;
        private string _value = string.Empty;

        public string Key
        {
            get => _key;
            set
            {
                _key = value;
                OnPropertyChanged(nameof(Key));
            }
        }

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
