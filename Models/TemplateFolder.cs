using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace FolderCreator.Models
{
    public class TemplateFolder : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private bool _isEditing;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                if (_isEditing != value)
                {
                    _isEditing = value;
                    OnPropertyChanged(nameof(IsEditing));
                }
            }
        }

        public ObservableCollection<TemplateFolder> Subfolders { get; set; } = [];

        public void AddSubfolder(string path)
        {
            var parts = path.Split(['/', '\\'], StringSplitOptions.RemoveEmptyEntries);

            if (!Subfolders.Any(f => f.Name == parts[0]))
            {
                Subfolders.Add(new TemplateFolder { Name = parts[0] });
            }

            if (parts.Length > 1)
            {
                var subPath = string.Join(System.IO.Path.DirectorySeparatorChar.ToString(), parts.Skip(1));
                var subFolder = Subfolders.First(f => f.Name == parts[0]);
                subFolder.AddSubfolder(subPath);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
