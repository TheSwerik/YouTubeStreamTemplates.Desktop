using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YouTubeStreamTemplates.Exceptions;
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
            path = path.Trim();
            if (string.IsNullOrWhiteSpace(path)) throw new EmptyPathException();
            if (!File.Exists(path)) throw new InvalidPathException(path);

            var id = Path.GetFileNameWithoutExtension(path);
            var template = Templates.FirstOrDefault(t => t.Id.Equals(id));
            if (template != null) Templates.Remove(template);

            template = await GetTemplateFromPath(path);
            Templates.Add(template);
            return template;
        }

        #endregion

        #region Helper Methods

        private async Task<Template> GetTemplateFromPath(string path)
        {
            var id = Path.GetFileNameWithoutExtension(path);
            var lines = await File.ReadAllLinesAsync(path);

            return Templates.FirstOrDefault(t => t.Id.Equals(id))
                   ?? new Template(lines.GetValue("Name"))
                      {
                          Id = id,
                          Name = lines.GetValue("Name"),
                          Title = lines.GetValue("Title"),
                          Category = (Category) int.Parse(lines.GetValue("Category")),
                          Description = lines.GetValue("Description").Replace(LineSeparator, "\n"),
                          StartTime = DateTime.Parse(lines.GetValue("StartTime")),
                          EndTime = DateTime.Parse(lines.GetValue("EndTime")),
                          Language = lines.GetValue("Language"),
                          Tags = lines.GetValue("Tags").Split(",").ToList(),
                          ThumbnailsPath = lines.GetValue("ThumbnailsPath")
                      };
        }

        #endregion
    }
}