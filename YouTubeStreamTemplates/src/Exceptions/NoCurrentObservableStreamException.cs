namespace YouTubeStreamTemplates.Exceptions
{
    public class NoCurrentObservableStreamException : YouTubeStreamTemplateException
    {
        public NoCurrentObservableStreamException() : base(
            "There is no current LiveStream in this ObservableLiveStream.")
        {
        }
    }
}