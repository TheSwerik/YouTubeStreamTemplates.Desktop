using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using YouTubeStreamTemplates;

namespace YouTubeStreamTemplatesCrossPlatform.Windows
{
    public class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            Console.WriteLine(Directory.GetCurrentDirectory());
        }

        private void InitializeComponent() { AvaloniaXamlLoader.Load(this); }

        private void Login_OnPress(object? sender, PointerPressedEventArgs pointerPressedEventArgs)
        {
            var img = (Image) sender!;
            img.Opacity = .75;
        }

        private async void Login_OnClick(object? sender, PointerReleasedEventArgs e)
        {
            var img = (Image) sender!;
            img.Opacity = 1;
            var service = LiveStreamService.Init().Result;
            Console.WriteLine("LOGIN");
        }
    }
}