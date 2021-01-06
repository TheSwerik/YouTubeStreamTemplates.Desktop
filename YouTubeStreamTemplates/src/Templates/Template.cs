using YouTubeStreamTemplates.LiveStreaming;

namespace YouTubeStreamTemplates.Templates
{
    public class Template : LiveStream
    {
        public Template(string name)
        {
            Name = name;
            ThumbnailsPath = "";
        }

        public string Name { get; set; }
        public string ThumbnailsPath { get; set; }
    }
}