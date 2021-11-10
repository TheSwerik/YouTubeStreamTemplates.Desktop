using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using YouTubeStreamTemplates.LiveStream;

namespace YouTubeStreamTemplatesCrossPlatform.Windows
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            Instance = this;
            AvaloniaXamlLoader.Load(this);
            Closing += Dispose;
        }

        public static MainWindow Instance { get; private set; } = null!;

        private void Dispose(object? sender, CancelEventArgs e) { StreamService.Instance.Dispose(); }
    }
}