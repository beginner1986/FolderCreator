using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace FolderCreator.Models
{
    public class Template : INotifyPropertyChanged
    {
        private string _name = "Nowy Szablon";
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
        public ObservableCollection<TemplateFolder> Folders { get; set; } = [];
        public List<string> Variables
        {
            get
            {
                var variableSet = new HashSet<string>();
                var regex = new Regex(@"\{(\w+)\}");
                void ExtractVariables(TemplateFolder folder)
                {
                    var matches = regex.Matches(folder.Name);
                    foreach (Match match in matches)
                    {
                        variableSet.Add(match.Groups[1].Value);
                    }
                    foreach (var subfolder in folder.Subfolders)
                    {
                        ExtractVariables(subfolder);
                    }
                }
                foreach (var folder in Folders)
                {
                    ExtractVariables(folder);
                }
                return variableSet.ToList();
            }
        }

        public void AddFolder(string path)
        {
            var parts = path.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

            if (!Folders.Any(f => f.Name == parts[0]))
            {
                Folders.Add(new TemplateFolder { Name = parts[0] });
            }

            if (parts.Length > 1)
            {
                var subPath = string.Join(System.IO.Path.DirectorySeparatorChar.ToString(), parts.Skip(1));
                var subFolder = Folders.First(f => f.Name == parts[0]);
                subFolder.AddSubfolder(subPath);
            }
            OnPropertyChanged(nameof(Variables));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
