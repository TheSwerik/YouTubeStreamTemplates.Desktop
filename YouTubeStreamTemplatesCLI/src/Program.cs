using System;
using System.Linq;
using YouTubeStreamTemplates.Settings;
using YouTubeStreamTemplates.Templates;

namespace YouTubeStreamTemplatesCLI
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            try
            {
                var templateService = new TemplateService();
                SettingsService.Instance.Init(templateService).Wait();
                Console.WriteLine(templateService.Templates.Count);
                var template = templateService.Templates.First();
                templateService.SaveTemplate(new Template(template.Name)
                                             {
                                                 Id = Guid.NewGuid().ToString(),
                                                 Category = template.Category,
                                                 Description = template.Description,
                                                 EndTime = template.EndTime,
                                                 StartTime = template.StartTime,
                                                 Language = template.Language,
                                                 Tags = template.Tags,
                                                 ThumbnailsPath = template.ThumbnailsPath,
                                                 Title = template.Title
                                             });
            }
            catch (AggregateException e)
            {
                foreach (var innerException in e.InnerExceptions)
                {
                    Console.WriteLine(innerException.GetType() + ": " + innerException.Message);
                    Console.WriteLine(innerException.StackTrace);
                }
            }
        }
    }
}