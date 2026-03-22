using System;
using System.IO;
using Newtonsoft.Json;

namespace QM_CloneSort
{
    public enum SortingModeOptions
    {
        /// <summary>Button toggles between Unlocked (UpButton visible) and Locked (order fixed, UpButton hidden).</summary>
        ManualOnly,
        /// <summary>Button cycles through Manual and all enabled name/rank sort modes.</summary>
        ManualAndAuto
    }

    public class ModConfig
    {
        public SortingModeOptions SortingMode { get; set; } = SortingModeOptions.ManualAndAuto;
        public bool EnableNameSort { get; set; } = true;
        public bool EnableRankSort { get; set; } = true;

        public static ModConfig LoadConfig(string configPath)
        {
            var settings = new JsonSerializerSettings { Formatting = Formatting.Indented };

            if (File.Exists(configPath))
            {
                try
                {
                    string sourceJson = File.ReadAllText(configPath);
                    var config = JsonConvert.DeserializeObject<ModConfig>(sourceJson, settings);

                    string upgraded = JsonConvert.SerializeObject(config, settings);
                    if (upgraded != sourceJson)
                    {
                        Plugin.Logger.Log("Config updated with new default fields.");
                        File.WriteAllText(configPath, upgraded);
                    }

                    return config;
                }
                catch (Exception ex)
                {
                    Plugin.Logger.LogError("Error reading config — using defaults.");
                    Plugin.Logger.LogException(ex);
                    return new ModConfig();
                }
            }
            else
            {
                var config = new ModConfig();
                File.WriteAllText(configPath, JsonConvert.SerializeObject(config, settings));
                return config;
            }
        }

        public void Save(string configPath)
        {
            try
            {
                var settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
                File.WriteAllText(configPath, JsonConvert.SerializeObject(this, settings));
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogException(ex);
            }
        }
    }
}
