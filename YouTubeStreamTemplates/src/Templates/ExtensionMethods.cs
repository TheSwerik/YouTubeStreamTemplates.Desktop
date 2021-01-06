using System;
using System.Collections.Generic;
using System.Linq;

namespace YouTubeStreamTemplates.Templates
{
    public static class ExtensionMethods
    {
        public static string GetValue(this IEnumerable<string> lines, string valueName)
        {
            var line = lines.FirstOrDefault(l => l.Trim().StartsWith(valueName + ":"));
            if (line == null) throw new Exception("THERE IS NO " + valueName);
            return line.Substring(valueName.Length + 2).Trim();
        }
    }
}