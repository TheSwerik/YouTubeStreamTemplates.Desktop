using YouTubeStreamTemplates.LiveStreaming;
using YouTubeStreamTemplates.Templates;

namespace YouTubeStreamTemplatesCrossPlatform
{
    public static class Service
    {
        public static LiveStreamService? LiveStreamService { get; set; }
        public static TemplateService? TemplateService { get; set; }
    }
}