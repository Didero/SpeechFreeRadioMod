using Colossal.IO.AssetDatabase;
using Game.Audio.Radio;
using Game.Triggers;
using HarmonyLib;
using System.Linq;
using Unity.Entities;

namespace SpeechFreeRadio
{
    class HarmonyPatches
    {
        private static readonly AudioAsset[] emptyAudioAssetArray = System.Array.Empty<AudioAsset>();

        private static void skipClips(Radio.RuntimeSegment segment, bool shouldFlushEvents)
        {
            SpeechFreeRadioMod.logger.Info($"Skipping {segment.type} clips");
            segment.clips = emptyAudioAssetArray;
            if (shouldFlushEvents)
            {
                RadioTagSystem existingSystemManaged = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<RadioTagSystem>();
                existingSystemManaged.FlushEvents(segment.type);
            }
        }

        [HarmonyPatch(typeof(Radio), "GetCommercialClips")]
        class Patch_GetCommercialClips
        {
            static bool Prefix(Radio.RuntimeSegment segment)
            {
                skipClips(segment, false);
                return false;
            }
        }

        [HarmonyPatch(typeof(Radio), "GetNewsClips")]
        class Patch_GetNewsClips
        {
            static bool Prefix(Radio.RuntimeSegment segment)
            {
                skipClips(segment, true);
                return false;
            }
        }

        [HarmonyPatch(typeof(Radio), "GetPSAClips")]
        class Patch_GetPsaClips
        {
            static bool Prefix(Radio.RuntimeSegment segment)
            {
                skipClips(segment, true);
                return false;
            }
        }

        [HarmonyPatch(typeof(Radio), "GetTalkShowClips")]
        class Patch_GetTalkShowClips
        {
            static bool Prefix(Radio.RuntimeSegment segment)
            {
                skipClips(segment, false);
                return false;
            }
        }

        [HarmonyPatch(typeof(Radio), "GetWeatherClips")]
        class Patch_GetWeatherClips
        {
            static bool Prefix(Radio.RuntimeSegment segment)
            {
                if (SpeechFreeRadioMod.m_Setting.ToggleAllowWeather)
                {
                    SpeechFreeRadioMod.logger.Info("Weather reports enabled, so allowing Weather clips");
                    return true;
                } else {
                    skipClips(segment, true);
                    return false;
                }
            }
        }
        
        [HarmonyPatch(typeof(Radio), "GetPlaylistClips")]
        class Patch_GetPlaylistClips
        {
            static bool Prefix(Radio.RuntimeSegment segment)
            {
                // 'PlaylistClips' should just be music, but Second Moon Radio has some talking in there,
                // so skip everything that isn't music
                foreach(string tag in segment.tags)
                {
                    if (tag.StartsWith("type:"))
                    {
                        if (tag == "type:Music")
                        {
                            // This is music, so allow it normally
                            return true;
                        } else {
                            skipClips(segment, false);
                            return false;
                        }
                    }
                }
                // We shouldn't reach here, since there should always be a 'type' tag, but handle it anyway
                string tagsString = string.Join("; ", segment.tags);
                SpeechFreeRadioMod.logger.Info($"Playlist segment didn't have a 'type' tag, allowing it. {segment.tags.Count()} tags in segment: {tagsString}");
                return true;
            }
        }
    }
}
