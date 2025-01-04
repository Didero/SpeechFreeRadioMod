using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;

namespace SpeechFreeRadio.ModSettings
{
    [FileLocation($"ModsSettings\\{nameof(SpeechFreeRadio)}\\settings")]
    [SettingsUIGroupOrder(kToggleGroup)]
    public class ModSettings : ModSetting
    {
        public const string kSection = "Main";
        public const string kToggleGroup = "Toggle";

        public ModSettings(IMod mod) : base(mod)
        {
        }

        [SettingsUISection(kSection, kToggleGroup)]
        public bool ToggleAllowWeather { get; set; } = false;

        public override void SetDefaults()
        {
            ToggleAllowWeather = false;
        }
    }
}
