using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace YouTubeStreamTemplatesCrossPlatform.Controls
{
    public class TagCard : UserControl
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public TagCard() { AvaloniaXamlLoader.Load(this); }

        public TagCard(string text, Avalonia.Controls.Controls parentControlList) : this()
        {
            this.Find<Button>("CloseButton").Click += (s, e) => parentControlList.Remove(this);
            this.Find<TextBlock>("TextBlock").Text = text;
        }
    }
}