using HarmonyLib;
using MGSC;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

using QM_CloneSort.LocalizationSupport;

namespace QM_CloneSort
{
    public static class Plugin
    {
        public static string HarmonyId { get; } = "valid.QM_CloneSort";

        public static Logger Logger { get; } = new Logger();
        public static ConfigDirectories ConfigDirectories { get; } = new ConfigDirectories();
        public static ModConfig Config { get; private set; } = new ModConfig();

        private static void LoadLocalization()
        {
            LocalizationFileLoader.LoadFromEmbeddedJson(
                "QM_CloneSort.localization.json",
                Assembly.GetExecutingAssembly(),
                Logger.LogError);
        }

        [Hook(ModHookType.AfterConfigsLoaded)]
        public static void AfterConfig(IModContext context)
        {
            Directory.CreateDirectory(ConfigDirectories.ModPersistenceFolder);
            Config = ModConfig.LoadConfig(ConfigDirectories.ConfigPath);

#if MCM_PRESENT
            McmIntegration.RegisterIfPresent();
#endif

            try
            {
                LoadLocalization();
                new Harmony(HarmonyId).PatchAll(Assembly.GetExecutingAssembly());
                Logger.Log("Harmony patches applied.");
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to apply Harmony patches.");
                Logger.LogException(ex);
            }
        }
    }
}
