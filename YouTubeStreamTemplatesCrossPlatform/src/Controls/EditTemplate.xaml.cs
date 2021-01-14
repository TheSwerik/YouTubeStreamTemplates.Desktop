using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using YouTubeStreamTemplates.LiveStreaming;
using YouTubeStreamTemplates.Templates;
using YouTubeStreamTemplatesCrossPlatform.Entities;

namespace YouTubeStreamTemplatesCrossPlatform.Controls
{
    public class EditTemplate : UserControl, IDisposable
    {
        private readonly List<IDisposable> _subscriptions;
        private readonly TagEditor _tagEditor;
        private ObservableLiveStream SelectedTemplate { get; }
        private Subject<List<Template>> Templates { get; }

        private static void InvokeOnRender(Action action)
        {
            Dispatcher.UIThread.InvokeAsync(action, DispatcherPriority.Render);
        }

        #region Init

        public EditTemplate()
        {
            _subscriptions = new List<IDisposable>();
            SelectedTemplate = new ObservableLiveStream();
            Templates = new Subject<List<Template>>();
            _tagEditor = new TagEditor(SelectedTemplate);
            InitializeComponent();
            InitBindings();
        }

        public EditTemplate(bool isStream) : this()
        {
            InvokeOnRender(async () =>
                           {
                               while (Service.LiveStreamService == null || Service.TemplateService == null)
                                   await Task.Delay(25);
                               Templates.OnNext(Service.TemplateService.Templates);
                               this.Find<ComboBox>("TemplateComboBox").SelectedIndex = 0;
                               this.Find<ComboBox>("CategoryComboBox").SelectedItem =
                                   SelectedTemplate.CurrentLiveStream.Category;
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
            _subscriptions.Add(liveStreamComboBox.Bind(ItemsControl.ItemsProperty, Templates));

            var textBox = this.Find<TextBox>("DescriptionTextBox");
            _subscriptions.Add(textBox.Bind(TextBox.TextProperty, SelectedTemplate.Select(l => l.Description)));

            var categoryStreamComboBox = this.Find<ComboBox>("CategoryComboBox");
            categoryStreamComboBox.Items = Enum.GetValues(typeof(Category));
            _subscriptions.Add(SelectedTemplate.Subscribe(s => categoryStreamComboBox.SelectedItem = s.Category));
            categoryStreamComboBox.SelectionChanged += (s, e) =>
                                                       {
                                                           SelectedTemplate.CurrentLiveStream.Category =
                                                               (Category) categoryStreamComboBox.SelectedItem;
                                                           SelectedTemplate.OnNext();
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
            SelectedTemplate.Dispose();
            Templates.Dispose();
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
            SelectedTemplate.CurrentLiveStream.Description = descriptionTextBox.Text;
            SelectedTemplate.OnNext();
        }

        private void LiveStreamComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender == null) return;
            var liveStreamComboBox = (ComboBox) sender;
            SelectedTemplate.OnNext((LiveStream) liveStreamComboBox.SelectedItem);
        }

        private void CategoryComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender == null) return;
            var categoryComboBox = (ComboBox) sender;
            SelectedTemplate.CurrentLiveStream.Category = (Category) categoryComboBox.SelectedItem;
            SelectedTemplate.OnNext();
        }

        #endregion
    }
}