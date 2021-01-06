using System;
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
                var settingsService = new SettingsService();
                var templateService = new TemplateService();
                settingsService.Init(templateService).Wait();
                // Console.WriteLine(templateService.Templates.Count);
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