using System;
using System.Collections.Specialized;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using YouTubeStreamTemplatesCrossPlatform.Entities;

namespace YouTubeStreamTemplatesCrossPlatform.Controls
{
    public class TagEditor : UserControl, IDisposable
    {
        private readonly TextBox _inputTextBox;
        private readonly IDisposable _subscription = null!;
        private readonly WrapPanel _tagsPanel;
        private ObservableLiveStream SelectedLivestream { get; } = null!;

        #region EventListener

        private void InputTextBox_OnLostFocus(object? sender, RoutedEventArgs e) { InputTextBox_FinishWriting(); }

        private void InputTextBox_OnKeyUp(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) InputTextBox_FinishWriting();
        }

        private void InputTextBox_FinishWriting()
        {
            var text = _inputTextBox.Text;
            if (string.IsNullOrWhiteSpace(text)) return;

            SelectedLivestream.CurrentLiveStream?.Tags.Add(text);
            SelectedLivestream.OnNext();
            _inputTextBox.Text = "";
        }

        private void OnTagsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
                                            {
                                                var maxWidth = _tagsPanel.Bounds.Width;
                                                var allWithoutTextBox = _tagsPanel.Children
                                                    .Where(c => c is not TextBox)
                                                    .ToList();
                                                var highestY = allWithoutTextBox.Max(c => c.Bounds.Y);
                                                var lineWidth = allWithoutTextBox
                                                                .Where(c => Math.Abs(c.Bounds.Y - highestY) < 1)
                                                                .Sum(c => c.Bounds.Width);
                                                var desiredWidth = maxWidth - lineWidth;
                                                return _inputTextBox.Width = desiredWidth < _inputTextBox.MinWidth
                                                                                 ? maxWidth
                                                                                 : desiredWidth;
                                            },
                                            DispatcherPriority.Render);
        }

        #endregion

        #region Init

        // ReSharper disable once MemberCanBePrivate.Global
        public TagEditor()
        {
            AvaloniaXamlLoader.Load(this);
            _tagsPanel = this.Find<WrapPanel>("TagsPanel")!;
            _inputTextBox = this.Find<TextBox>("InputTextBox")!;
        }

        public TagEditor(ObservableLiveStream selectedLivestream) : this()
        {
            _tagsPanel.Children.CollectionChanged += OnTagsChanged;
            SelectedLivestream = selectedLivestream;
            _subscription = SelectedLivestream.Subscribe(_ => RefreshTags());
        }

        private void RefreshTags()

        {
            var stream = SelectedLivestream.CurrentLiveStream;
            if (stream == null) return;

            var children = _tagsPanel.Children;
            // Remove Listener so that it doesn't get called way too often completely unnecessarily
            Dispatcher.UIThread.InvokeAsync(() =>
                                            {
                                                children.CollectionChanged -= OnTagsChanged;
                                                children.Clear();
                                                children.AddRange(stream.Tags.Select(t => new TagCard(t, children)));
                                                children.CollectionChanged += OnTagsChanged;
                                                children.Add(_inputTextBox);
                                            }, DispatcherPriority.Render);
        }

        #region Dispose

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

        #endregion
    }
}