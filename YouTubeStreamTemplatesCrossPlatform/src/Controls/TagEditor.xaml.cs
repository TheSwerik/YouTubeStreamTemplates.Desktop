using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using YouTubeStreamTemplatesCrossPlatform.Entities;

namespace YouTubeStreamTemplatesCrossPlatform.Controls
{
    public class TagEditor : UserControl, IDisposable
    {
        private readonly IDisposable _subscription = null!;
        private readonly WrapPanel _tagsPanel;
        private readonly ObservableLiveStream SelectedLivestream { get; } = null!;

        #region EventListener

        private void InputTextBox_OnKeyUp(object? sender, KeyEventArgs e) { Console.WriteLine(e.Key); }

        #endregion

        #region Init

        // ReSharper disable once MemberCanBePrivate.Global
        public TagEditor()
        {
            AvaloniaXamlLoader.Load(this);
            _tagsPanel = this.Find<WrapPanel>("TagsPanel")!;
        }

        public TagEditor(ObservableLiveStream selectedLivestream) : this()
        {
            _tagsPanel.Children.CollectionChanged += (a, b) => OnTagsChanged();
            SelectedLivestream = selectedLivestream;

            if (SelectedLivestream.CurrentLiveStream == null) return;

            var textBox = _tagsPanel.Children.First();
            _tagsPanel.Children.Clear();
            foreach (var tag in SelectedLivestream.CurrentLiveStream.Tags)
                _tagsPanel.Children.Add(new TagCard(tag, _tagsPanel.Children));
            _tagsPanel.Children.Add(textBox);
        }

        private void OnTagsChanged()
        {
            Console.WriteLine(_tagsPanel.Bounds.Width + "\t" + _tagsPanel.Children.Last().Bounds.X + " + " +
                              _tagsPanel.Children.Last().Bounds.Width);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            Console.WriteLine("TEST");
            if (!disposing) return;
            _subscription.Dispose();
        }

        #endregion
    }
}