namespace YouTubeStreamTemplates.Exceptions
{
    public class NoCurrentStreamException : YouTubeStreamTemplateException
    {
        public NoCurrentStreamException() : base("There is no currently active LiveStream.") { }
    }
}