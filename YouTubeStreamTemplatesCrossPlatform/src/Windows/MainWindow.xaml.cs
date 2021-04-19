using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using YouTubeStreamTemplates.LiveStreaming;
using YouTubeStreamTemplates.Settings;

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

        private void Dispose(object? sender, CancelEventArgs e) { LiveStreamService.Instance.Dispose(); }

        private void InitAPI()
        {
            Task.Run(LiveStreamService.Init).Wait();
            Task.Run(SettingsService.Init).Wait();
        }
    }
}