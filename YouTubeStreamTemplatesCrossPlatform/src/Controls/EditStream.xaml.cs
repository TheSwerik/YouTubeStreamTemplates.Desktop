using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using YouTubeStreamTemplates.LiveStreaming;
using YouTubeStreamTemplatesCrossPlatform.Entities;

namespace YouTubeStreamTemplatesCrossPlatform.Controls
{
    public class EditStream : UserControl, IDisposable
    {
        private readonly List<IDisposable> _subscriptions;
        private readonly TagEditor _tagEditor;
        private ObservableLiveStream SelectedLivestream { get; }
        private Subject<List<LiveStream>> LiveStreams { get; }

        #region Init

        public EditStream()
        {
            _subscriptions = new List<IDisposable>();
            SelectedLivestream = new ObservableLiveStream();
            LiveStreams = new Subject<List<LiveStream>>();
            _tagEditor = new TagEditor(SelectedLivestream);
            InitializeComponent();
            InitBindings();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            Grid.SetRow(_tagEditor, 5);
            Grid.SetColumn(_tagEditor, 2);
            this.Find<Grid>("ContentGrid").Children.Add(_tagEditor);
        }

        private void InitBindings()
        {
            var liveStreamComboBox = this.Find<ComboBox>("LiveStreamComboBox");
            _subscriptions.Add(liveStreamComboBox.Bind(ItemsControl.ItemsProperty, LiveStreams));

            var textBox = this.Find<TextBox>("DescriptionTextBox");
            _subscriptions.Add(textBox.Bind(TextBox.TextProperty, SelectedLivestream.Select(l => l.Description)));


            LiveStreams.OnNext(new List<LiveStream>
                               {
                                   TestStream()
                                   /*
                                    // TestData
                                    new() {Id = "1", Title = "Ass"},
                                    new() {Id = "2", Title = "Butt"},
                                    new() {Id = "3", Title = "Buttox"},
                                    new() {Id = "4", Title = "Arsch"},
                                    */
                               });
            liveStreamComboBox.SelectedIndex = 0;
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
            ReleaseUnmanagedResources();
            if (!disposing) return;
            _subscriptions.ForEach(s => s.Dispose());
            SelectedLivestream.Dispose();
            LiveStreams.Dispose();
        }

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        #endregion

        private LiveStream TestStream()
        {
            return new()
                   {
                       Id = "Abc123",
                       Title = "Summoner Master Solo",
                       Description = "Test TEst \n Test TEssadast\n\nsadasd",
                       Tags = new List<string> {"Lets Play", "Summoner", "Stream"},
                       Language = "German",
                       Category = Category.Gaming,
                       StartTime = DateTime.Today,
                       EndTime = DateTime.Today.AddDays(1)
                   };
        }

        #endregion

        #region EventListener

        private void LiveStreamDescription_OnFocusLost(object? sender, RoutedEventArgs e)
        {
            if (sender == null || SelectedLivestream.CurrentLiveStream == null) return;
            var descriptionTextBox = (TextBox) sender;
            SelectedLivestream.CurrentLiveStream.Description = descriptionTextBox.Text;
            SelectedLivestream.OnNext();
        }

        private void LiveStreamComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender == null) return;
            var liveStreamComboBox = (ComboBox) sender;
            SelectedLivestream.OnNext((LiveStream) liveStreamComboBox.SelectedItem);
        }

        private void CategoryComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender == null || SelectedLivestream.CurrentLiveStream == null) return;
            var categoryComboBox = (ComboBox) sender;
            SelectedLivestream.CurrentLiveStream.Category = (Category) categoryComboBox.SelectedItem;
            SelectedLivestream.OnNext();
        }

        #endregion
    }
}