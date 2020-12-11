using System;
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
        public ObservableLiveStream SelectedLivestream { get; set; } = null!;

        #region EventListener

        private void InputTextBox_OnKeyUp(object? sender, KeyEventArgs e) { throw new NotImplementedException(); }

        #endregion

        #region Init

        public TagEditor(ObservableLiveStream selectedLivestream) : this()
        {
            SelectedLivestream = selectedLivestream;
            //TODO
            // _subscription  = _tagsPanel.Bind(ItemsControl.ItemsProperty, SelectedLivestream.Select(l => l.Tags));
        }

        public TagEditor()
        {
            AvaloniaXamlLoader.Load(this);
            _tagsPanel = this.Find<WrapPanel>("TagsPanel")!;
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