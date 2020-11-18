using System;

namespace YouTubeStreamTemplates.LiveStream
{
    //TODO renmae
    public struct LiveStream : IComparable<LiveStream>
    {
        public DateTime StartDate { get; init; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int CompareTo(LiveStream other) { return StartDate.CompareTo(other.StartDate); }

    }
}