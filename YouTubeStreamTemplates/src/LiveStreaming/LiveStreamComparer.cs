using System;
using System.Collections.Generic;
using static YouTubeStreamTemplates.LiveStreaming.LiveStreamSortMode;

namespace YouTubeStreamTemplates.LiveStreaming
{
    public class LiveStreamComparer : IComparer<LiveStream>
    {
        public static readonly LiveStreamComparer ByDateDesc = new(DateDesc);
        public static readonly LiveStreamComparer ByDateAsc = new(DateAsc);
        public static readonly LiveStreamComparer ByTitleDesc = new(TitleDesc);
        public static readonly LiveStreamComparer ByTitleAsc = new(TitleAsc);
        private readonly LiveStreamSortMode _sortMode;

        private LiveStreamComparer(LiveStreamSortMode sortMode) { _sortMode = sortMode; }

        public int Compare(LiveStream? x, LiveStream? y)
        {
            if (y == null) return 1;
            if (x == null) return -1;
            return _sortMode switch
                   {
                       DateDesc => -x.StartTime.CompareTo(y.StartTime),
                       DateAsc => x.StartTime.CompareTo(y.StartTime),
                       TitleDesc => -string.Compare(x.Title, y.Title, StringComparison.InvariantCulture),
                       TitleAsc => string.Compare(x.Title, y.Title, StringComparison.InvariantCulture),
                       _ => throw new ArgumentOutOfRangeException(nameof(_sortMode), _sortMode + "", "wrong")
                   };
        }
    }
}