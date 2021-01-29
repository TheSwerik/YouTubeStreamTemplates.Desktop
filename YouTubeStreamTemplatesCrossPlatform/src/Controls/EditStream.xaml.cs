using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using YouTubeStreamTemplates.LiveStreaming;

namespace YouTubeStreamTemplatesCrossPlatform.Controls
{
    public class EditStream : UserControl
    {
        private readonly GenericComboBox<Category> _categoryComboBox;
        private readonly TextBox _descriptionTextBox;
        private readonly TagEditor _tagEditor;
        private readonly TextBox _titleTextBox;

        #region EventListener

        private void OnChanged(object? sender, RoutedEventArgs args) { Console.WriteLine("SOMETHING CHANGED"); }

        #endregion

        #region Methods

        private static void InvokeOnRender(Action action)
        {
            Dispatcher.UIThread.InvokeAsync(action, DispatcherPriority.Render);
        }

        private async Task FillValues()
        {
            var liveStream = await Service.LiveStreamService!.GetCurrentStream();
            Console.WriteLine(liveStream);
            _descriptionTextBox.Text = liveStream.Description;
            _categoryComboBox.SelectedItem = liveStream.Category;
            _tagEditor.RefreshTags(liveStream.Tags);
        }

        #endregion

        #region Init

        public EditStream()
        {
            _tagEditor = new TagEditor(); //TODO

            InitializeComponent();
            _categoryComboBox = this.Find<GenericComboBox<Category>>("CategoryComboBox");
            _titleTextBox = this.Find<TextBox>("TitleTextBox");
            _descriptionTextBox = this.Find<TextBox>("DescriptionTextBox");

            InvokeOnRender(async () => await Init());
        }

        private async Task Init()
        {
            //TODO REMOVE THIS:
            Service.LiveStreamService = await LiveStreamService.Init();
            //-------- Until here -------------------

            while (Service.LiveStreamService == null)
            {
                Console.WriteLine("Waiting for LiveStreamService to initialize...");
                await Task.Delay(25);
            }

            _categoryComboBox.SelectionChanged += OnChanged;
            _titleTextBox.TextInput += OnChanged;
            _descriptionTextBox.TextInput += OnChanged;

            _categoryComboBox.Items = Enum.GetValues(typeof(Category));
            await FillValues();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            Grid.SetRow(_tagEditor, 5);
            Grid.SetColumn(_tagEditor, 2);
            this.Find<Grid>("ContentGrid").Children.Add(_tagEditor);
        }

        #endregion
    }
}