using System;
using YouTubeStreamTemplates.Templates;

namespace YouTubeStreamTemplatesCLI
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            try
            {
                var service = new TemplateService();
                var result = service.LoadTemplate("Test.tlpt").Result;
                Console.WriteLine(result);
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