using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using YouTubeStreamTemplates.LiveStream;

namespace YouTubeStreamTemplates
{
    public class Program
    {
        private YouTubeService _youTubeService;

        [STAThread]
        public static void Main()
        {
            try
            {
                var program = new Program();
                // program.ListStreams().Wait();
                program.UpdateStream().Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions) Console.WriteLine("ERROR: " + e.Message + "\n" + e.StackTrace);
            }
        }

        private async Task<LiveBroadcast> ListStreams()
        {
            await SetYouTubeService();
            var request = _youTubeService.LiveBroadcasts.List("snippet");
            request.Mine = true;
            var response = await request.ExecuteAsync();

            if (response.Items == null) return null;
            var streams = new List<LiveStream.LiveStream>();
            foreach (var stream in response.Items)
            {
                streams.Add(new LiveStream.LiveStream
                            {
                                Title = stream.Snippet.Title,
                                Description = stream.Snippet.Description,
                                Thumbnail = stream.Snippet.Thumbnails.Standard,
                                StartDate = DateTime.Parse(stream.Snippet.ScheduledStartTime),
                                LiveBroadcast = stream
                            });
                Console.WriteLine(stream.Snippet.ScheduledStartTime + " - " + stream.Snippet.Title);
            }

            Console.WriteLine();
            streams.Sort(LiveStreamComparer.ByDateDesc);
            foreach (var stream in streams)
                Console.WriteLine(stream.StartDate + "\t" + stream.Title + "\t" + stream.Thumbnail?.Url);
            return streams[0].LiveBroadcast;
        }

        private async Task UpdateStream()
        {
            var liveBroadcast = await ListStreams();
            liveBroadcast.Snippet.Title = "test";
            await SetYouTubeService();
            var request = _youTubeService.LiveBroadcasts.Update(liveBroadcast, "snippet");
            var response = await request.ExecuteAsync();

            if (response == null) return;
            Console.WriteLine(response.Snippet.ScheduledStartTime + " - " + response.Snippet.Title);
        }

        private async Task<UserCredential> GetCredentials(IEnumerable<string> scopes)
        {
            await using var stream = new FileStream(@"..\..\..\..\client_id.json", FileMode.Open, FileAccess.Read);
            return await GoogleWebAuthorizationBroker.AuthorizeAsync(
                       GoogleClientSecrets.Load(stream).Secrets,
                       scopes,
                       "user",
                       CancellationToken.None,
                       new FileDataStore("YouTube.Test2"));
        }

        private async Task<YouTubeService> CreateYouTubeService(params string[] scopes)
        {
            return new(new BaseClientService.Initializer
                       {
                           HttpClientInitializer = await GetCredentials(scopes),
                           ApplicationName = "YouTube Sample",
                           ApiKey = await File.ReadAllTextAsync(@"..\..\..\..\apikey.txt")
                       });
        }

        ~Program() { _youTubeService?.Dispose(); }

        private async Task SetYouTubeService()
        {
            _youTubeService ??= await CreateYouTubeService(YouTubeService.Scope.YoutubeReadonly,
                                                           YouTubeService.Scope.YoutubeForceSsl);
        }
    }
}