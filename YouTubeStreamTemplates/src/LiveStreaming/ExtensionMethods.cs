using System;
using System.Globalization;
using Google.Apis.YouTube.v3.Data;

namespace YouTubeStreamTemplates.LiveStreaming
{
    public static class ExtensionMethods
    {
        public static LiveStream ToLiveStream(this LiveBroadcast liveBroadcast)
        {
            return new()
                   {
                       Id = liveBroadcast.Id,
                       Title = liveBroadcast.Snippet.Title,
                       Description = liveBroadcast.Snippet.Description,
                       Thumbnails = liveBroadcast.Snippet.Thumbnails,
                       StartTime = DateTime.Parse(liveBroadcast.Snippet.ActualStartTime),
                       EndTime = DateTime.Parse(liveBroadcast.Snippet.ActualEndTime)
                   };
        }

        public static Video ToVideo(this LiveStream liveStream)
        {
            return new()
                   {
                       Id = liveStream.Id,
                       Snippet = new VideoSnippet
                                 {
                                     Title = liveStream.Title,
                                     Description = liveStream.Description,
                                     CategoryId = liveStream.Category,
                                     Thumbnails = liveStream.Thumbnails,
                                     Tags = liveStream.Tags,
                                     DefaultLanguage = liveStream.Language,
                                     DefaultAudioLanguage = liveStream.Language
                                 },
                       Localizations = liveStream.Localizations,
                       LiveStreamingDetails = new VideoLiveStreamingDetails
                                              {
                                                  ScheduledStartTime =
                                                      DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                                                  ScheduledEndTime = DateTime.UtcNow.AddDays(1)
                                                                             .ToString(CultureInfo.InvariantCulture)
                                              }
                   };
        }
    }
}