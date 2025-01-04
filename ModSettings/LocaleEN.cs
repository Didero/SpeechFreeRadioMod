using Colossal;
using Game.Input;
using Game.Settings;
using Game.UI.Widgets;
using System.Collections.Generic;

namespace SpeechFreeRadio.ModSettings
{
    public class LocaleEN : IDictionarySource
    {
        private readonly ModSettings m_Setting;
        public LocaleEN(ModSettings setting)
        {
            m_Setting = setting;
        }
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), ModAssemblyInfo.Title },
                { m_Setting.GetOptionTabLocaleID(ModSettings.kSection), "Main" },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.ToggleAllowWeather)), "Allow Weather Reports" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.ToggleAllowWeather)), "If enabled, weather reports will still be played" },
            };
        }

        public void Unload()
        {

        }
    }

}
