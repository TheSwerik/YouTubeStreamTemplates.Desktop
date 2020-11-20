namespace YouTubeStreamTemplates.Exceptions
{
    public class CouldNotCreateServiceException : YouTubeStreamTemplateException
    {
        public CouldNotCreateServiceException() : base("Could not create YouTubeService.") { }
    }
}