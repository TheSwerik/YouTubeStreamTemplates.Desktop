using System;
using System.Globalization;
using System.Reflection;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace YouTubeStreamTemplatesCrossPlatform
{
    /// <summary>
    ///     <para>
    ///         Converts a string path to a bitmap asset.
    ///     </para>
    ///     <para>
    ///         The asset must be in the same assembly as the program. If it isn't,
    ///         specify "avares://<assemblynamehere>/" in front of the path to the asset.
    ///     </para>
    /// </summary>
    public class BitmapAssetValueConverter : IValueConverter
    {
        public static BitmapAssetValueConverter Instance = new();

        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case null:
                    return null;
                case string rawUri when targetType == typeof(IBitmap):
                {
                    if (Assembly.GetEntryAssembly() == null) return null;
                    var uri = rawUri.StartsWith("avares://")
                                  ? new Uri(rawUri)
                                  : new Uri($"avares://{Assembly.GetEntryAssembly()!.GetName().Name}{rawUri}");

                    var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                    return new Bitmap(assets.Open(uri));
                }
                default:
                    throw new NotSupportedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}