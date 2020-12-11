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
                                     CategoryId = liveStream.Category.ToString(),
                                     Thumbnails = liveStream.Thumbnails,
                                     Tags = liveStream.Tags,
                                     DefaultLanguage = liveStream.Language,
                                     DefaultAudioLanguage = liveStream.Language
                                 },
                       Localizations = liveStream.Localizations,
                       LiveStreamingDetails = new VideoLiveStreamingDetails
                                              {
                                                  ScheduledStartTime = liveStream.StartTime.ToUniversalTime()
                                                      .ToString(CultureInfo.InvariantCulture),
                                                  ScheduledEndTime = liveStream.EndTime.ToUniversalTime()
                                                                               .ToString(CultureInfo.InvariantCulture)
                                              }
                   };
        }
    }
}