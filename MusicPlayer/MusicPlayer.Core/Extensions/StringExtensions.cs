using System;
using System.Globalization;

namespace MusicPlayer.Core.Extensions
{
    public static class StringExtensions
    {
        public static T To<T>(this string source)
        {
            return (T)Convert.ChangeType(source, typeof(T), CultureInfo.InvariantCulture);
        }

        public static T? ToN<T>(this string source) where T : struct
        {
            if (!string.IsNullOrWhiteSpace(source))
            {
                try
                {
                    return (T)Convert.ChangeType(source, typeof(T), CultureInfo.InvariantCulture);
                }
                catch { }
            }

            return null;
        }
    }
}