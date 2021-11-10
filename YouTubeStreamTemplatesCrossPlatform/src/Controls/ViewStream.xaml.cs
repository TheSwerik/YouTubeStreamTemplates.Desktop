using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using NLog;
using YouTubeStreamTemplates.Helpers;
using YouTubeStreamTemplates.LiveStream;
using YouTubeStreamTemplatesCrossPlatform.Exceptions;

namespace YouTubeStreamTemplatesCrossPlatform.Controls
{
    public class ViewStream : UserControl
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly TextBlock _categoryTextBlock;
        private readonly Grid _contentGrid;
        private readonly TextBlock _descriptionTextBlock;
        private readonly Grid _noStreamGrid;
        private readonly TagEditor _tagEditor;
        private readonly Image _thumbnail;

        private readonly TextBlock _titleTextBlock;

        private void ThumbnailImage_OnClick(object? sender, PointerReleasedEventArgs e)
        {
            if (!StreamService.IsInitialized || StreamService.Instance.CurrentLiveStream == null) return;
            Process.Start(new ProcessStartInfo
                          {
                              UseShellExecute = true,
                              FileName = "https://www.youtube.com/watch?v=" +
                                         StreamService.Instance.CurrentLiveStream.Id
                          })?.Dispose();
        }

        #region Init

        public ViewStream()
        {
            _tagEditor = new TagEditor(true);

            InitializeComponent();
            _categoryTextBlock = this.Find<TextBlock>("CategoryTextBlock");
            _titleTextBlock = this.Find<TextBlock>("TitleTextBlock");
            _thumbnail = this.Find<Image>("ThumbnailImage");
            _descriptionTextBlock = this.Find<TextBlock>("DescriptionTextBlock");
            _contentGrid = this.Find<Grid>("ContentGrid");
            _noStreamGrid = this.Find<Grid>("NoStreamGrid");
            _contentGrid.Children.Add(_tagEditor);

            InvokeOnRender(async () => await Init());
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            Grid.SetRow(_tagEditor, 10);
            Grid.SetColumn(_tagEditor, 1);
        }

        private async Task Init()
        {
            while (!StreamService.IsInitialized)
            {
                Logger.Debug("Waiting for StreamService to initialize...");
                await Task.Delay(100);
            }

            await Task.Run(() => CheckForStream());
        }

        #endregion

        #region Methods

        private static void InvokeOnRender(Action action)
        {
            Dispatcher.UIThread.InvokeAsync(action, DispatcherPriority.Render);
        }

        private async Task CheckForStream(int delay = 1000)
        {
            if (StreamService.Instance == null) throw new ServiceNotInitializedException(typeof(StreamService));
            await foreach (var stream in StreamService.Instance.CheckForStream(delay))
                if (stream == null) InvokeOnRender(ClearValues);
                else InvokeOnRender(() => FillValues(stream));
        }

        private async void FillValues(Stream stream)
        {
            // Logger.Debug($"Fill Values with:\n{stream}");
            _titleTextBlock.Text = stream.Title;
            _descriptionTextBlock.Text = stream.Description;
            _categoryTextBlock.Text = StreamService.Instance
                                                   .Category
                                                   .First(kp => kp.Key.Equals(stream.Category))
                                                   .Value;
            _tagEditor.Tags = stream.Tags.ToHashSet();
            _tagEditor.RefreshTags();
            _contentGrid.IsVisible = !(_noStreamGrid.IsVisible = false);
            _thumbnail.Source = new Bitmap(await ImageHelper.GetLiveStreamImagePathAsync(stream.Id));
        }

        private void ClearValues()
        {
            _contentGrid.IsVisible = !(_noStreamGrid.IsVisible = true);
            _titleTextBlock.Text = "";
            _descriptionTextBlock.Text = "";
            _categoryTextBlock.Text = "";
            _tagEditor.Tags = new HashSet<string>();
            _tagEditor.RefreshTags();
        }

        #endregion
    }
}