using System.Collections.Generic;
using System.Linq;

namespace YouTubeStreamTemplates.LiveStreaming
{
    public struct Playlist
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<string> VideoIds { get; set; }

        public Playlist(string id, string title, IEnumerable<string> videoIds)
        {
            Id = id;
            Title = title;
            VideoIds = videoIds.ToList();
        }

        public override string ToString() { return $"{{{Id}, {Title}}}"; }
    }
}