using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YouTubeStreamTemplates
{
    public class SettingsService
    {
        private const string DefaultPath = @"res/Default.cfg";
        private readonly Dictionary<Settings, string> _defaultSettings;
        public Dictionary<Settings, string> Settings { get; init; }

        #region Initialisation

        public SettingsService()
        {
            Settings = new Dictionary<Settings, string>();
            _defaultSettings = new Dictionary<Settings, string>();

            if (!File.Exists(DefaultPath)) throw new Exception("ERROR DEFAULT IS MISSING"); //TODO make an Exception
            AddAllSettings(_defaultSettings, DefaultPath);
            if (!File.Exists(_defaultSettings[YouTubeStreamTemplates.Settings.SavePath]))
                File.Copy(DefaultPath, _defaultSettings[YouTubeStreamTemplates.Settings.SavePath]);
            AddAllSettings(Settings, _defaultSettings[YouTubeStreamTemplates.Settings.SavePath]);
        }

        private void AddAllSettings(IDictionary<Settings, string> settings, string path)
        {
            var lines = File.ReadLines(path).Where(l => l.Contains('=')).Select(l => l.Split('=')).ToArray();
            var settingNames = Enum.GetValues<Settings>();

            if (path.Equals(DefaultPath) &&
                (lines.Length != settingNames.Length ||
                 settingNames.Any(n => lines.All(l => !l[0].Trim().Equals(n.ToString())))))
                throw new Exception("ERROR DEFAULT IS BROKEN"); //TODO make an Exception

            foreach (var setting in settingNames)
            {
                var value = lines.SingleOrDefault(line => line[0].Trim().Equals(setting.ToString()));
                if (value != null && value.Length == 2) settings.Add(setting, value[1]);
                else settings.Add(setting, _defaultSettings[setting]);
            }
        }

        #endregion

        #region Public Methods

        #endregion
    }
}