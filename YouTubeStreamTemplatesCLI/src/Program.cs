using System;
using YouTubeStreamTemplates;

namespace YouTubeStreamTemplatesCLI
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            try
            {
                var service = new LiveStreamService();
                service.UpdateStream().Wait();
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