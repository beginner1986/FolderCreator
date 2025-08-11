using System.Text.RegularExpressions;

namespace FolderCreator.Models
{
    public class Template
    {
        public string Name { get; set; } = string.Empty;
        public List<TemplateFolder> Folders { get; set; } = [];
        public List<string> Variables
        {
            get
            {
                var regex = new Regex(@"\{\{(.*?)\}\}");
                var variables = new HashSet<string>();

                foreach (var folder in Folders)
                {
                    var matches = regex.Matches(folder.Path);
                    foreach (Match match in matches)
                    {
                        variables.Add(match.Groups[1].Value);
                    }
                }

                return variables.ToList();
            }
        }
    }
}
