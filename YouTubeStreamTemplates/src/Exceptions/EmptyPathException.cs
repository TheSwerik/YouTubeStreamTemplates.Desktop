namespace YouTubeStreamTemplates.Exceptions
{
    public class EmptyPathException : YouTubeStreamTemplateException
    {
        public EmptyPathException() : base("Path is Empty.") { }
    }
}