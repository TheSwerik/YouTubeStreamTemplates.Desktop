using System;

namespace YouTubeStreamTemplates.Exceptions
{
    public class YouTubeStreamTemplateException : Exception
    {
        public YouTubeStreamTemplateException(string? message) : base(message) { }
    }
}