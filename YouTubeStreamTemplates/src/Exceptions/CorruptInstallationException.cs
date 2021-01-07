namespace YouTubeStreamTemplates.Exceptions
{
    public class CorruptInstallationException : YouTubeStreamTemplateException
    {
        public CorruptInstallationException(string file) : base($"The File {file} is missing.") { }
    }
}