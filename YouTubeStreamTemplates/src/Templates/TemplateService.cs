using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YouTubeStreamTemplates.LiveStreaming;

namespace YouTubeStreamTemplates.Templates
{
    public class TemplateService
    {
        private const string LineSeparator = "■\\n■";
        private List<string> _templatePaths;

        #region Initialisation

        public TemplateService()
        {
            _templatePaths = new List<string>();
            Templates = new List<Template>();
        }

        #endregion

        public List<Template> Templates { get; }

        #region Public Methods

        // public async Task<List<string>> GetTemplatePaths()
        // {
        //     var openFileDialog = new OpenFileDialog();
        //     openFileDialog.Filter = "Template files (*.tlpt)|*.tlpt|All files (*.*)|*.*";
        //     if (openFileDialog.ShowDialog()) return openFileDialog.FileName;
        //     throw new Exception();
        // }

        public async Task<Template> LoadTemplate(string path)
        {
            var name = Path.GetFileNameWithoutExtension(path);
            Console.WriteLine(name);
            var template =
                Templates.FirstOrDefault(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (template != null) return template;

            template = await GetTemplateFromPath(path);
            Templates.Add(template);
            return template;
        }

        public async Task<Template> GetTemplateFromPath(string path)
        {
            var name = Path.GetFileNameWithoutExtension(path);
            var template = new Template(name);
            var lines = await File.ReadAllLinesAsync(path);

            template.Title = lines.GetValue("Title");
            template.Category = (Category) int.Parse(lines.GetValue("Category"));
            template.Description = lines.GetValue("Description").Replace(LineSeparator, "\n");
            template.StartTime = DateTime.Parse(lines.GetValue("StartTime"));
            template.EndTime = DateTime.Parse(lines.GetValue("EndTime"));
            template.Language = lines.GetValue("Language");
            template.Tags = lines.GetValue("Tags").Split(",").ToList();
            template.ThumbnailsPath = lines.GetValue("ThumbnailsPath");

            return template;
        }

        #endregion
    }
}