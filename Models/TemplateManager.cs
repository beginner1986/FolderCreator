using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

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

        public static IEnumerable<string?> GetTemplateNames()
        {
            if (!Directory.Exists(TemplatesDirectory))
            {
                return Enumerable.Empty<string>();
            }

            return Directory.GetFiles(TemplatesDirectory, "*.json")
                            .Select(Path.GetFileNameWithoutExtension);
        }
    }
}
