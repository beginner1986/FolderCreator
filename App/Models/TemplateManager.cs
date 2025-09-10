using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace FolderCreator.Models
{
    public static class TemplateManager
    {
        // Use AppData folder for user-specific template storage
        private static readonly string TemplatesDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "FolderCreator",
            "Templates");
            
        private static bool _isInitialized = false;
        private static string _initializationError = string.Empty;

        static TemplateManager()
        {
            try
            {
                // Create directory structure if it doesn't exist
                if (!Directory.Exists(TemplatesDirectory))
                {
                    Directory.CreateDirectory(TemplatesDirectory);
                }
                _isInitialized = true;
                
                // Log success for debugging
                System.Diagnostics.Debug.WriteLine($"Templates directory initialized at: {TemplatesDirectory}");
            }
            catch (Exception ex)
            {
                _initializationError = $"Failed to initialize Templates directory: {ex.Message}";
                System.Diagnostics.Debug.WriteLine(_initializationError);
                // Don't rethrow - let the methods handle the uninitialized state
            }
        }

        public static bool IsInitialized => _isInitialized;
        public static string InitializationError => _initializationError;

        public static void SaveTemplate(Template template)
        {
            if (!_isInitialized)
                throw new InvalidOperationException(_initializationError);

            var templatePath = Path.Combine(TemplatesDirectory, $"{template.Name}.json");
            var json = JsonSerializer.Serialize(template, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(templatePath, json);
        }

        public static Template? LoadTemplate(string templateName)
        {
            if (!_isInitialized)
                throw new InvalidOperationException(_initializationError);

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
            if (!_isInitialized)
                throw new InvalidOperationException(_initializationError);

            string filePath = Path.Combine(TemplatesDirectory, $"{templateName}.json");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static ObservableCollection<Template> GetAllTemplates()
        {
            var templates = new ObservableCollection<Template>();
            
            if (!_isInitialized)
            {
                System.Diagnostics.Debug.WriteLine(_initializationError);
                return templates; // Return empty collection instead of throwing
            }

            if (!Directory.Exists(TemplatesDirectory))
            {
                return templates;
            }

            foreach (var file in Directory.GetFiles(TemplatesDirectory, "*.json"))
            {
                try
                {
                    string json = File.ReadAllText(file);
                    var template = JsonSerializer.Deserialize<Template>(json);
                    if (template != null)
                    {
                        templates.Add(template);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading template from {file}: {ex.Message}");
                    // Continue with other templates
                }
            }
            return templates;
        }

        public static bool ApplyTemplate(Template template, string targetPath, Dictionary<string, string> variables)
        {
            if (!_isInitialized)
                throw new InvalidOperationException(_initializationError);

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
                newName = newName.Replace($"{{{{{variable.Key}}}}}", variable.Value);
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
