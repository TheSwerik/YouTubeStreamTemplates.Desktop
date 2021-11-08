using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace YouTubeStreamTemplatesCrossPlatform.Controls
{
    public class TagEditor : UserControl
    {
        private readonly TextBox _inputTextBox;
        private readonly bool _isReadOnly;
        private readonly WrapPanel _tagsPanel;

        public List<TagCard> TagCards => _tagsPanel.Children.OfType<TagCard>().ToList();
        public HashSet<string> Tags { get; set; }
        public event EventHandler OnChanged = null!;

        #region EventListener

        private void InputTextBox_OnLostFocus(object? sender, RoutedEventArgs e) { InputTextBox_FinishWriting(); }

        private void InputTextBox_OnKeyUp(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter or Key.OemComma) InputTextBox_FinishWriting();
            InvokeOnRender(_inputTextBox.Focus);
        }

        public void Remove(TagCard tagCard)
        {
            Tags.Remove(tagCard.Text);
            OnChanged.Invoke(this, new EventArgs());
            RefreshTags();
        }

        private void InputTextBox_FinishWriting(string? text = null)
        {
            if (_inputTextBox.Text == null) return;
            text ??= _inputTextBox.Text.Replace(",", "");
            if (string.IsNullOrWhiteSpace(text)) return;
            if (text.Length > 100) throw new ArgumentException("text is too long");
            if (string.Join(",", Tags).Length + text.Length >= 500) throw new ArgumentException("tags are too long");
            Tags.Add(text);
            _inputTextBox.Text = "";
            OnChanged.Invoke(this, new EventArgs());
            RefreshTags();
        }

        #endregion

        #region Init

        // ReSharper disable once MemberCanBePrivate.Global
        public TagEditor()
        {
            AvaloniaXamlLoader.Load(this);
            _tagsPanel = this.Find<WrapPanel>("TagsPanel")!;
            _inputTextBox = this.Find<TextBox>("InputTextBox")!;
            Tags = new HashSet<string>();
        }

        public TagEditor(bool isReadOnly = false) : this()
        {
            _isReadOnly = isReadOnly;
            _inputTextBox.IsVisible = !isReadOnly;
        }

        public void RefreshTags()
        {
            var tags = Tags.ToHashSet();
            InvokeOnRender(() =>
                           {
                               var controls = _tagsPanel.Children;
                               controls.Clear();
                               controls.AddRange(tags.Select(tag => new TagCard(this, tag, _isReadOnly)));
                               InvokeOnRender(ResizeInputBox);
                               controls.Add(_inputTextBox);
                           });
        }

        public void RefreshTags(IEnumerable<string> tags)
        {
            Tags = tags.ToHashSet();
            RefreshTags();
        }

        #endregion

        #region Helper Methods

        private void ResizeInputBox()
        {
            var maxWidth = _tagsPanel.Bounds.Width;
            var allTags = _tagsPanel.Children.Where(c => c is not TextBox).ToList();
            var highestY = allTags.Max(c => c.Bounds.Y);
            var lineWidth = allTags.Where(c => Math.Abs(c.Bounds.Y - highestY) < 1)
                                   .Sum(c => c.Bounds.Width + c.Margin.Left + c.Margin.Right);
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