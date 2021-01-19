using Avalonia.Controls;

namespace YouTubeStreamTemplatesCrossPlatform.Controls
{
    public class GenericComboBox<T> : ComboBox
    {
        public new T SelectedItem
        {
            get => (T) base.SelectedItem;
            set => base.SelectedItem = value;
        }
    }
}