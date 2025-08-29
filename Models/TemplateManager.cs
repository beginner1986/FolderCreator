using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace FolderCreator.Models
{
    public static class TemplateManager
    {
        private static readonly string TemplatesDirectory = "Templates";

        static TemplateManager()
        {
            if (!Directory.Exists(TemplatesDirectory))
            {
                Directory.CreateDirectory(TemplatesDirectory);
            }
        }

        public static void SaveTemplate(Template template)
        {
            var templatePath = Path.Combine(TemplatesDirectory, $"{template.Name}.json");
            var json = JsonSerializer.Serialize(template, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(templatePath, json);
        }

        public static Template? LoadTemplate(string templateName)
        {
            string filePath = Path.Combine(TemplatesDirectory, $"{templateName}.json");
            if (!File.Exists(filePath))
            {
                return null;
            }

            string json = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<Template>(json);
        }

        public static void DeleteTemplate(string templateName)
        {
            string filePath = Path.Combine(TemplatesDirectory, $"{templateName}.json");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static ObservableCollection<Template> GetAllTemplates()
        {
            var templates = new ObservableCollection<Template>();
            if (!Directory.Exists(TemplatesDirectory))
            {
                return templates;
            }

            foreach (var file in Directory.GetFiles(TemplatesDirectory, "*.json"))
            {
                string json = File.ReadAllText(file);
                var template = JsonSerializer.Deserialize<Template>(json);
                if (template != null)
                {
                    templates.Add(template);
                }
            }
            return templates;
        }

        public static bool ApplyTemplate(Template template, string targetPath, Dictionary<string, string> variables)
        {
            Template processedTemplate = new Template
            {
                Name = template.Name,
                Folders = new ObservableCollection<TemplateFolder>()
            };

            foreach (var folder in template.Folders)
            {
                var processedFolder = folder.CloneWithVariables(variables);
                processedTemplate.Folders.Add(processedFolder);
            }

            try
            {
                foreach (var folder in processedTemplate.Folders)
                {
                    CreateFolderRecursively(folder, targetPath);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void CreateFolderRecursively(TemplateFolder folder, string parentPath)
        {
            string folderPath = Path.Combine(parentPath, folder.Name);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            foreach (var subfolder in folder.Subfolders)
            {
                CreateFolderRecursively(subfolder, folderPath);
            }
        }

        private static TemplateFolder CloneWithVariables(this TemplateFolder folder, Dictionary<string, string> variables)
        {
            string newName = folder.Name;
            foreach (var variable in variables)
            {
                newName = Regex.Replace(newName, Regex.Escape($"{{{{{variable.Key}}}}}"), variable.Value);
            }
            var newFolder = new TemplateFolder { Name = newName };
            foreach (var subfolder in folder.Subfolders)
            {
                newFolder.Subfolders.Add(subfolder.CloneWithVariables(variables));
            }
            return newFolder;
        }
    }
}
