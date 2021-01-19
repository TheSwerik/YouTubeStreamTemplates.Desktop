using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using YouTubeStreamTemplates.Exceptions;
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

        private static void InvokeOnRender(Action action)
        {
            Dispatcher.UIThread.InvokeAsync(action, DispatcherPriority.Render);
        }

        #region Init

        public EditStream()
        {
            _subscriptions = new List<IDisposable>();
            SelectedLivestream = new ObservableLiveStream();
            LiveStreams = new Subject<List<LiveStream>>();
            _tagEditor = new TagEditor(SelectedLivestream.CurrentLiveStream.Tags);
            InitializeComponent();
            InitBindings();
        }

        public EditStream(bool isStream) : this()
        {
            InvokeOnRender(async () =>
                           {
                               while (Service.LiveStreamService == null) await Task.Delay(25);

                               try
                               {
                                   // LiveStreams.OnNext(isStream
                                   //                        ? Service.TemplateService.Templates
                                   //                        : new List<LiveStream>
                                   //                          {await Service.LiveStreamService.GetCurrentStream()});
                                   LiveStreams.OnNext(new List<LiveStream>
                                                      {await Service.LiveStreamService.GetCurrentStream()});
                               }
                               catch (NoCurrentStreamException) //TODO
                               {
                               }

                               this.Find<ComboBox>("LiveStreamComboBox").SelectedIndex = 0;
                               this.Find<ComboBox>("CategoryComboBox").SelectedItem =
                                   SelectedLivestream.CurrentLiveStream.Category;
                           });
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

            var categoryStreamComboBox = this.Find<ComboBox>("CategoryComboBox");
            categoryStreamComboBox.Items = Enum.GetValues(typeof(Category));
            _subscriptions.Add(SelectedLivestream.Subscribe(s => categoryStreamComboBox.SelectedItem = s.Category));
            categoryStreamComboBox.SelectionChanged += (s, e) =>
                                                       {
                                                           SelectedLivestream.CurrentLiveStream.Category =
                                                               (Category) categoryStreamComboBox.SelectedItem;
                                                           SelectedLivestream.OnNext();
                                                       };
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

        #endregion

        #region EventListener

        private void LiveStreamDescription_OnFocusLost(object? sender, RoutedEventArgs e)
        {
            if (sender == null) return;
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
            if (sender == null) return;
            var categoryComboBox = (ComboBox) sender;
            SelectedLivestream.CurrentLiveStream.Category = (Category) categoryComboBox.SelectedItem;
            SelectedLivestream.OnNext();
        }

        #endregion
    }
}