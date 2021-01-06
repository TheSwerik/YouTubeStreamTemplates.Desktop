namespace YouTubeStreamTemplates.Exceptions
{
    public class InvalidPathException : YouTubeStreamTemplateException
    {
        public InvalidPathException(string path) : base($"Path {path} is Invalid.") { }
    }
}