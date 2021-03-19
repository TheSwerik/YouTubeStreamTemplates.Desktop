using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace YouTubeStreamTemplatesCrossPlatform.Controls
{
    public class CheckBoxSearchResult : UserControl
    {
        private readonly Grid _grid;

        public CheckBoxSearchResult(string text) : this()
        {
            ((TextBlock) _grid.Children.First(c => c is TextBlock)).Text = text;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public CheckBoxSearchResult()
        {
            AvaloniaXamlLoader.Load(this);
            _grid = this.Find<Grid>("Grid");
        }

        private void InputElement_OnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            var checkbox = (CheckBox) _grid.Children.First(c => c is CheckBox);
            checkbox.IsChecked = !checkbox.IsChecked;
        }
    }
}