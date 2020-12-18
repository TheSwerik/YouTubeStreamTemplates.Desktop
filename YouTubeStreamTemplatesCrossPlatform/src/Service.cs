using YouTubeStreamTemplates;

namespace YouTubeStreamTemplatesCrossPlatform
{
    public static class Service
    {
        public static SettingsService? SettingsService { get; set; }
        public static LiveStreamService? LiveStreamService { get; set; }
        public static TemplateService? TemplateService { get; set; }
    }
}