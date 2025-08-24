using System.Collections.Generic;
using System.Linq;

namespace FolderCreator.Models
{
    public class TemplateFolder
    {
        public string Name { get; set; } = string.Empty;
        public List<TemplateFolder> Subfolders { get; set; } = [];

        public void AddSubfolder(string path)
        {
            var parts = path.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

            if (!Subfolders.Any(f => f.Name == parts[0]))
                Subfolders.Add(new TemplateFolder { Name = parts[0] });

            if (parts.Length > 1)
            {
                var subPath = string.Join(System.IO.Path.DirectorySeparatorChar.ToString(), parts.Skip(1));
                var subFolder = Subfolders.First(f => f.Name == parts[0]);
                subFolder.AddSubfolder(subPath);
            }
        }
    }
}
