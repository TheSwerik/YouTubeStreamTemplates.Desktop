using System;
using Google.Apis.YouTube.v3.Data;

namespace YouTubeStreamTemplates.LiveStream
{
    //TODO rename
    public class LiveStream : IComparable<LiveStream>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Thumbnail Thumbnail { get; set; }
        public string[] Playlists { get; set; }
        public string[] Tags { get; set; }
        public string Language { get; set; }
        public string Category { get; set; }
        public string Game { get; set; }
        public DateTime StartDate { get; init; }
        public LiveBroadcast LiveBroadcast { get; init; }

        public int CompareTo(LiveStream other) { return StartDate.CompareTo(other.StartDate); }
    }
}