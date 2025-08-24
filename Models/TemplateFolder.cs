namespace FolderCreator.Models
{
    public class TemplateFolder
    {
        public string Name { get; set; } = string.Empty;
        public List<TemplateFolder> Subfolders { get; set; } = [];

        public TemplateFolder(string path)
        {
            var parts = path.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0)
            {
                Name = parts[0];
                if (parts.Length > 1)
                {
                    var subPath = string.Join(System.IO.Path.DirectorySeparatorChar.ToString(), parts.Skip(1));
                    Subfolders.Add(new TemplateFolder(subPath));
                }
            }
        }
    }
}
