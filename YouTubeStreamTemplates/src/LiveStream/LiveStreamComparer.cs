using System;
using System.Collections.Generic;
using static YouTubeStreamTemplates.LiveStream.LiveStreamSortMode;

namespace YouTubeStreamTemplates.LiveStream
{
    public class LiveStreamComparer : IComparer<LiveStream>
    {
        public static readonly LiveStreamComparer ByDateDesc = new(DateDesc);
        public static readonly LiveStreamComparer ByDateAsc = new(DateAsc);
        public static readonly LiveStreamComparer ByTitleDesc = new(TitleDesc);
        public static readonly LiveStreamComparer ByTitleAsc = new(TitleAsc);
        private readonly LiveStreamSortMode _sortMode;

        public LiveStreamComparer(LiveStreamSortMode sortMode) { _sortMode = sortMode; }

        public int Compare(LiveStream x, LiveStream y)
        {
            return _sortMode switch
                   {
                       DateDesc => -x.StartDate.CompareTo(y.StartDate),
                       DateAsc => x.StartDate.CompareTo(y.StartDate),
                       TitleDesc => -string.Compare(x.Title, y.Title, StringComparison.InvariantCulture),
                       TitleAsc => string.Compare(x.Title, y.Title, StringComparison.InvariantCulture),
                       _ => throw new ArgumentOutOfRangeException(nameof(_sortMode), _sortMode + "", "wrong")
                   };
        }
    }
}