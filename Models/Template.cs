using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace FolderCreator.Models
{
    public class Template
    {
        public string Name { get; set; } = "New Template";
        public ObservableCollection<TemplateFolder> Folders { get; set; } = [];
        public List<string> Variables
        {
            get
            {
                var regex = new Regex(@"\{\{(.*?)\}\}");
                var variables = new HashSet<string>();

                foreach (var folder in Folders)
                {
                    var matches = regex.Matches(folder.Name);
                    foreach (Match match in matches)
                    {
                        variables.Add(match.Groups[1].Value);
                    }
                }

                return variables.ToList();
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
        }
    }
}
