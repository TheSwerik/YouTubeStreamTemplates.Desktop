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
                service.Init().Wait();
                service.UpdateStream().Wait();
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}