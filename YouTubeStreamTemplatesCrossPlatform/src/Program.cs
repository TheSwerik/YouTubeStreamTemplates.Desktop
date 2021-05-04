﻿using System;
using System.IO;
using Avalonia;

namespace YouTubeStreamTemplatesCrossPlatform
{
    internal static class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break. 
        public static void Main(string[] args)
        {
            try
            {
                BuildAvaloniaApp()
                    .StartWithClassicDesktopLifetime(args);
            }
            catch (Exception e)
            {
                File.WriteAllText($"error logs {DateTime.Now.ToFileTime()}.txt", e.Message + "\n" + e.StackTrace);
                throw;
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        private static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                             .UsePlatformDetect()
                             .LogToTrace();
        }
    }
}