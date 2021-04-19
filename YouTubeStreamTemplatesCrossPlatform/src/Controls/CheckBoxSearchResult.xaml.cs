using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using YouTubeStreamTemplates.LiveStreaming;

namespace YouTubeStreamTemplatesCrossPlatform.Controls
{
    public class CheckBoxSearchResult : UserControl
    {
        private readonly CheckBox _checkBox;
        private readonly Grid _grid;
        public readonly string Text;

        public CheckBoxSearchResult(string text) : this()
        {
            ((TextBlock) _grid.Children.First(c => c is TextBlock)).Text = text;
            Text = text;
        }

        public CheckBoxSearchResult(Playlist playlist) : this(playlist.Title) { Playlist = playlist; }

        // ReSharper disable once MemberCanBePrivate.Global
        public CheckBoxSearchResult()
        {
            AvaloniaXamlLoader.Load(this);
            _grid = this.Find<Grid>("Grid");
            _checkBox = this.Find<CheckBox>("CheckBox");
        }

        private Playlist Playlist { get; }
        public bool IsChecked => _checkBox.IsChecked ?? false;

        private void InputElement_OnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            var checkbox = (CheckBox) _grid.Children.First(c => c is CheckBox);
            checkbox.IsChecked = !checkbox.IsChecked;
            // TODO raise event again and add/remove playlist from template
        }
    }
}