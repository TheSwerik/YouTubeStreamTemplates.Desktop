using System;
using System.Collections.Specialized;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
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

        private void InputTextBox_OnKeyUp(object? sender, KeyEventArgs e) { Console.WriteLine(e.Key); }

        private void OnTagsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(() => _inputTextBox.Width = _tagsPanel.Bounds.Width -
                                                                        _tagsPanel.Children.Where(c => c is not TextBox)
                                                                            .Sum(c => c.Bounds.Width));
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
            _subscription = SelectedLivestream.Subscribe(s => RefreshTags());
        }

        private void RefreshTags()
        {
            if (SelectedLivestream.CurrentLiveStream == null) return;

            var tagControls = _tagsPanel.Children;
            tagControls.AddRange(
                SelectedLivestream.CurrentLiveStream.Tags.Select(tag => new TagCard(tag, tagControls)));

            tagControls.Remove(_inputTextBox);
            tagControls.Add(_inputTextBox);
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