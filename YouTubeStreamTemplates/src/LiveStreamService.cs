using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using YouTubeStreamTemplates.Exceptions;
using YouTubeStreamTemplates.LiveStreaming;
using LiveStream = YouTubeStreamTemplates.LiveStreaming.LiveStream;

namespace YouTubeStreamTemplates
{
    public class LiveStreamService
    {
        private YouTubeService? _youTubeService;

        public async Task<LiveStream> GetCurrentStream()
        {
            if (_youTubeService == null) await Init();
            var request = _youTubeService!.LiveBroadcasts.List("id,snippet,contentDetails,status");
            request.BroadcastType = LiveBroadcastsResource.ListRequest.BroadcastTypeEnum.All;
            request.BroadcastStatus = LiveBroadcastsResource.ListRequest.BroadcastStatusEnum.Active;
            // request.Mine = true;

            var response = await request.ExecuteAsync();
            Console.WriteLine(response.Items?.Count);
            if (response.Items == null || response.Items.Count <= 0) throw new NoCurrentStreamException();
            if (response.Items.Count == 1) return response.Items[0].ToLiveStream();

            // Get the latest Stream if there is more than one:
            var streams = response.Items.Select(s => s.ToLiveStream()).ToList();
            streams.Sort(LiveStreamComparer.ByDateDesc);
            streams.ForEach(s => Console.WriteLine(s.Title));
            return streams[0];
        }

        public async Task UpdateStream()
        {
            var liveBroadcast = await GetCurrentStream();
            var video = new Video
                        {
                            Id = liveBroadcast.Id,
                            Snippet = new VideoSnippet
                                      {
                                          Title = "test",
                                          Description = liveBroadcast.Description,
                                          CategoryId = liveBroadcast.Category,
                                          Thumbnails = liveBroadcast.Thumbnails,
                                          Tags = liveBroadcast.Tags,
                                          DefaultLanguage = liveBroadcast.Language,
                                          DefaultAudioLanguage = liveBroadcast.Language
                                      },
                            Localizations = liveBroadcast.Localizations,
                            LiveStreamingDetails = new VideoLiveStreamingDetails
                                                   {
                                                       ScheduledStartTime =
                                                           DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                                                       ScheduledEndTime = DateTime.UtcNow.AddDays(1)
                                                           .ToString(CultureInfo.InvariantCulture)
                                                   }
                        };
            if (_youTubeService == null) await Init();
            // var request = _youTubeService!.Videos.Update(video, "snippet");
            // var response = await request.ExecuteAsync();

            // if (response == null) return;
            // Console.WriteLine(response.Snippet.Title + " - " + response.Snippet.Title);
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

        ~LiveStreamService() { _youTubeService?.Dispose(); }

        public async Task Init()
        {
            _youTubeService ??= await CreateYouTubeService(YouTubeService.Scope.YoutubeReadonly,
                                                           YouTubeService.Scope.YoutubeForceSsl);
        }
    }
}