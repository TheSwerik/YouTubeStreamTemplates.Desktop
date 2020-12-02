using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using YouTubeStreamTemplates.LiveStreaming;

namespace YouTubeStreamTemplatesCrossPlatform.Controls
{
    public class EditStream : UserControl, IDisposable
    {
        private readonly List<IDisposable> _subscriptions;

        public EditStream()
        {
            InitializeComponent();
            _subscriptions = new List<IDisposable>();
            SelectedLivestream = new Subject<LiveStream>();
            LiveStreams = new Subject<List<LiveStream>>();
            InitBindings();
        }

        public Subject<LiveStream> SelectedLivestream { get; set; }
        public Subject<List<LiveStream>> LiveStreams { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void InitBindings()
        {
            var liveStreamComboBox = this.Find<ComboBox>("LiveStreamComboBox");
            _subscriptions.Add(liveStreamComboBox.Bind(ItemsControl.ItemsProperty, LiveStreams));

            var textBox = this.Find<TextBox>("DescriptionTextBox");
            _subscriptions.Add(textBox.Bind(TextBox.TextProperty, SelectedLivestream.Select(l => l.Title)));


            LiveStreams.OnNext(new List<LiveStream>
                               {
                                   new() {Id = "1", Title = "Ass"},
                                   new() {Id = "2", Title = "Butt"},
                                   new() {Id = "3", Title = "Buttox"},
                                   new() {Id = "4", Title = "Arsch"}
                               });
            liveStreamComboBox.SelectedIndex = 0;
        }

        private void InitializeComponent() { AvaloniaXamlLoader.Load(this); }

        private void LiveStreamComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender == null) return;
            var liveStreamComboBox = (ComboBox) sender;
            SelectedLivestream.OnNext((LiveStream) liveStreamComboBox.SelectedItem);
        }

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        private void Dispose(bool disposing)
        {
            Console.WriteLine("TEST");
            ReleaseUnmanagedResources();
            if (!disposing) return;
            _subscriptions.ForEach(s => s.Dispose());
            SelectedLivestream.Dispose();
            LiveStreams.Dispose();
        }
    }
}