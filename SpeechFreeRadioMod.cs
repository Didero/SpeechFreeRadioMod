using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using HarmonyLib;
using SpeechFreeRadio.ModSettings;
using System.Linq;


namespace SpeechFreeRadio
{
    public class SpeechFreeRadioMod : IMod
    {
        internal static readonly ILog logger = LogManager.GetLogger(ModAssemblyInfo.Name);
        internal static ModSettings.ModSettings m_Setting = null;
        private Harmony harmony;

        public void OnLoad(UpdateSystem updateSystem)
        {
            logger.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                logger.Info($"Current mod asset at {asset.path}");

            m_Setting = new ModSettings.ModSettings(this);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));
            AssetDatabase.global.LoadSettings(nameof(SpeechFreeRadio), m_Setting, new ModSettings.ModSettings(this));

            harmony = new Harmony($"{nameof(SpeechFreeRadio)}.{nameof(SpeechFreeRadioMod)}");
            harmony.PatchAll(typeof(SpeechFreeRadioMod).Assembly);
            var patchedMethods = harmony.GetPatchedMethods();
            logger.Info($"Plugin SpeechFreeRadio patched {patchedMethods.Count()} methods:");
            foreach (var patchedMethod in patchedMethods)
            {
                logger.Info($" Patched method: {patchedMethod.Module.Name}:{patchedMethod.Name}");
            }
        }

        public void OnDispose()
        {
            logger.Info(nameof(OnDispose));
            if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }

            harmony.UnpatchAll($"{nameof(SpeechFreeRadio)}.{nameof(SpeechFreeRadioMod)}");
        }
    }
}
