using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using YouTubeStreamTemplates.LiveStream;

namespace YouTubeStreamTemplates
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            try
            {
                new Program().Run().Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions) Console.WriteLine("ERROR: " + e.Message + "\n" + e.StackTrace);
            }
        }

        private async Task Run()
        {
            using var service = await CreateYouTubeService();
            var request = service.LiveBroadcasts.List("snippet");
            request.Mine = true;
            var response = await request.ExecuteAsync();

            if (response.Items == null) return;
            var streams = new List<LiveStream.LiveStream>();
            foreach (var stream in response.Items)
            {
                streams.Add(new LiveStream.LiveStream
                            {
                                StartDate = DateTime.Parse(stream.Snippet.ScheduledStartTime),
                                Title = stream.Snippet.Title
                            });
                Console.WriteLine(stream.Snippet.ScheduledStartTime + " - " + stream.Snippet.Title);
            }

            Console.WriteLine();
            streams.Sort(LiveStreamComparer.ByDateAsc);
            foreach (var stream in streams) Console.WriteLine(stream.StartDate + "\t" + stream.Title);

            Console.WriteLine();
            streams.Sort(LiveStreamComparer.ByDateDesc);
            foreach (var stream in streams) Console.WriteLine(stream.StartDate + "\t" + stream.Title);

            Console.WriteLine();
            streams.Sort(LiveStreamComparer.ByTitleDesc);
            foreach (var stream in streams) Console.WriteLine(stream.StartDate + "\t" + stream.Title);

            Console.WriteLine();
            streams.Sort(LiveStreamComparer.ByTitleAsc);
            foreach (var stream in streams) Console.WriteLine(stream.StartDate + "\t" + stream.Title);
        }

        private async Task<UserCredential> GetCredentials()
        {
            await using var stream = new FileStream(@"..\..\..\..\client_id.json", FileMode.Open, FileAccess.Read);
            return await GoogleWebAuthorizationBroker.AuthorizeAsync(
                       GoogleClientSecrets.Load(stream).Secrets,
                       new[]
                       {
                           // YouTubeService.Scope.Youtube,
                           // YouTubeService.Scope.Youtubepartner,
                           YouTubeService.Scope.YoutubeReadonly
                           // YouTubeService.Scope.YoutubeUpload,
                           // YouTubeService.Scope.YoutubeForceSsl,
                           // YouTubeService.Scope.YoutubepartnerChannelAudit,
                           // YouTubeService.Scope.YoutubeChannelMembershipsCreator,
                       },
                       "Swerik", //TODO why does my username have to be here
                       CancellationToken.None,
                       new FileDataStore("YouTube.ListMyVideos"));
        }

        private async Task<YouTubeService> CreateYouTubeService()
        {
            return new(new BaseClientService.Initializer
                       {
                           HttpClientInitializer = await GetCredentials(),
                           ApplicationName = "YouTube Sample",
                           ApiKey = await File.ReadAllTextAsync(@"..\..\..\..\apikey.txt")
                       });
        }
    }
}