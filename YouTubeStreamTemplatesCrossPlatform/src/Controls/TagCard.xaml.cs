using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using YouTubeStreamTemplatesCrossPlatform.Entities;

namespace YouTubeStreamTemplatesCrossPlatform.Controls
{
    public class TagCard : UserControl
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public TagCard() { AvaloniaXamlLoader.Load(this); }

        public TagCard(string text, ObservableLiveStream livestream) : this()
        {
            this.Find<Button>("CloseButton").Click += (s, e) =>
                                                      {
                                                          livestream.CurrentLiveStream?.Tags.Remove(text);
                                                          livestream.OnNext();
                                                      };
            this.Find<TextBlock>("TextBlock").Text = text;
        }
    }
}