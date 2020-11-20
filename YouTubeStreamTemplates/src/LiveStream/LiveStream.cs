using System;
using System.Collections.Generic;
using System.Globalization;
using Google.Apis.YouTube.v3.Data;

namespace YouTubeStreamTemplates.LiveStream
{
    //TODO rename
    public class LiveStream : IComparable<LiveStream>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ThumbnailDetails Thumbnails { get; set; }
        public string[] Playlists { get; set; }
        public string[] Tags { get; set; }
        public string Language { get; set; }
        public Dictionary<string, VideoLocalization> Localizations { get; set; }
        public string Category { get; set; }
        public string Game { get; set; }
        public DateTime StartDate { get; init; }

        public int CompareTo(LiveStream other) { return StartDate.CompareTo(other.StartDate); }

        public LiveBroadcast ToLiveBroadcast()
        {
            return new()
                   {
                       Id = Id,
                       Snippet = new LiveBroadcastSnippet
                                 {
                                     Title = Title,
                                     Description = Description,
                                     Thumbnails = Thumbnails,
                                     ScheduledStartTime = StartDate.ToUniversalTime()
                                                                   .ToString(CultureInfo.InvariantCulture)
                                 }
// Kind = 
                   };
        }
    }
}