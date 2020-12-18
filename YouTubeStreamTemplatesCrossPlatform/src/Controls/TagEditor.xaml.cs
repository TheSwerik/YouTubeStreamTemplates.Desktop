using System;
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
            InvokeOnRender(_inputTextBox.Focus);
        }

        private void InputTextBox_FinishWriting()
        {
            var text = _inputTextBox.Text;
            if (string.IsNullOrWhiteSpace(text) || SelectedLivestream.CurrentLiveStream == null) return;
            if (text.Length > 100) throw new ArgumentException("text is too long");
            if (string.Join(",", SelectedLivestream.CurrentLiveStream.Tags).Length + text.Length + 1 > 500)
                throw new ArgumentException("tags are too long");

            SelectedLivestream.CurrentLiveStream.Tags.Add(text);
            SelectedLivestream.OnNext();
            _inputTextBox.Text = "";
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
            SelectedLivestream = selectedLivestream;
            _subscription = SelectedLivestream.Subscribe(_ => RefreshTags());
        }

        private void RefreshTags()

        {
            var stream = SelectedLivestream.CurrentLiveStream;
            if (stream == null) return;

            var controls = _tagsPanel.Children;
            InvokeOnRender(() =>
                           {
                               controls.Clear();
                               controls.AddRange(stream.Tags.Select(tag => new TagCard(tag, SelectedLivestream)));
                               InvokeOnRender(ResizeInputBox);
                               controls.Add(_inputTextBox);
                           });
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

        #region Helper Methods

        private void ResizeInputBox()
        {
            var maxWidth = _tagsPanel.Bounds.Width;
            var allWithoutTextBox = _tagsPanel.Children.Where(c => c is not TextBox).ToList();
            var highestY = allWithoutTextBox.Max(c => c.Bounds.Y);
            var lineWidth = allWithoutTextBox.Where(c => Math.Abs(c.Bounds.Y - highestY) < 1).Sum(c => c.Bounds.Width);
            var desiredWidth = maxWidth - lineWidth;
            _inputTextBox.Width = desiredWidth < _inputTextBox.MinWidth ? maxWidth : desiredWidth;
        }

        private static void InvokeOnRender(Action action)
        {
            Dispatcher.UIThread.InvokeAsync(action, DispatcherPriority.Render);
        }

        #endregion
    }
}