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
using YouTubeStreamTemplates.LiveStream;

namespace YouTubeStreamTemplates
{
    public class LiveStreamService
    {
        private YouTubeService _youTubeService;

        public async Task<List<LiveStream.LiveStream>> GetAllStreams()
        {
            var request = _youTubeService.LiveBroadcasts.List("snippet");
            request.Mine = true;
            var response = await request.ExecuteAsync();

            if (response.Items == null) return null;
            var streams = response.Items
                                  .Select(stream => new LiveStream.LiveStream
                                                    {
                                                        Id = stream.Id,
                                                        Title = stream.Snippet.Title,
                                                        Description = stream.Snippet.Description,
                                                        Thumbnails = stream.Snippet.Thumbnails,
                                                        StartDate = DateTime.Parse(stream.Snippet.ScheduledStartTime)
                                                    })
                                  .ToList();
            streams.Sort(LiveStreamComparer.ByDateDesc);
            return streams;
        }

        public async Task<LiveStream.LiveStream> GetLatestStream() { return (await GetAllStreams())[0]; }

        public async Task UpdateStream()
        {
            var liveBroadcast = await GetLatestStream();
            if (liveBroadcast.Tags != null) Console.WriteLine(string.Join(", ", liveBroadcast.Tags));
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
            // var request = _youTubeService.Videos.Update(video, "snippet");
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