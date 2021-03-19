using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using YouTubeStreamTemplates.LiveStreaming;
using YouTubeStreamTemplatesCrossPlatform.Windows;

namespace YouTubeStreamTemplatesCrossPlatform.Controls
{
    public class AutoCompleteSelectComboBox : UserControl
    {
        private readonly Grid _comboBoxGrid;
        private readonly Popup _resultPopup;
        private readonly TextBox _searchInputBox;
        private readonly StackPanel _searchResultPanel;

        private readonly Stopwatch _stopwatch;
        private readonly Grid _textGrid;

        public AutoCompleteSelectComboBox()
        {
            AvaloniaXamlLoader.Load(this);
            _textGrid = this.Find<Grid>("TextGrid");
            _comboBoxGrid = this.Find<Grid>("ComboBoxGrid");
            _searchInputBox = this.Find<TextBox>("SearchInputBox");
            _resultPopup = this.Find<Popup>("ResultPopup");
            _searchResultPanel = this.Find<StackPanel>("ResultStackPanel");
            _stopwatch = new Stopwatch();
            Items = new List<Playlist>();
            OnLostFocus(null, null);
            MainWindow.Instance.PositionChanged += OnWindowPositionChanged;
        }

        public IEnumerable Items { get; set; }

        private void OnClick(object? sender, PointerPressedEventArgs e)
        {
            _textGrid.IsEnabled = false;
            _textGrid.IsVisible = false;
            _comboBoxGrid.IsEnabled = true;
            _comboBoxGrid.IsVisible = true;
            _searchInputBox.Focus();
            var a = new Popup();
        }

        private void OnLostFocus(object? sender, RoutedEventArgs e)
        {
            if (_comboBoxGrid.IsFocused || _comboBoxGrid.Children.Any(c => c.IsFocused)) return;
            var controlList = _searchResultPanel.Children.ToList();
            while (controlList.Any())
                if (controlList.Any(c => c.IsPointerOver)) return;
                else controlList = controlList.Where(c => c is Panel).SelectMany(c => ((Panel) c).Children).ToList();

            _textGrid.IsEnabled = true;
            _textGrid.IsVisible = true;
            _comboBoxGrid.IsEnabled = false;
            _comboBoxGrid.IsVisible = false;
            _resultPopup.Close();
        }

        private void SearchResult_OnClick(object? sender, PointerPressedEventArgs e)
        {
            Console.WriteLine(sender + " clicked");
            e.Handled = true;
        }

        private void SearchInputBox_OnTextInput(object? sender, KeyEventArgs keyEventArgs)
        {
            if (!_resultPopup.IsOpen) _resultPopup.Open();
        }

        private async void OnWindowPositionChanged(object? sender, PixelPointEventArgs e)
        {
            if (!_resultPopup.IsOpen) return;
            var isRunning = _stopwatch.IsRunning;
            _stopwatch.Restart();
            if (!isRunning) await ChangePopupPosition();
        }

        private async Task ChangePopupPosition()
        {
            while (_stopwatch.ElapsedMilliseconds < 500) await Task.Delay(50);
            _resultPopup.Close();
            _resultPopup.Open();
            _stopwatch.Reset();
        }

        private void InputElement_OnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            var grid = (Grid) sender;
            var checkbox = (CheckBox) grid.Children.First(c => c is CheckBox);
            checkbox.IsChecked = !checkbox.IsChecked;
        }
    }
}