using HarmonyLib;
using MGSC;
using System;
using System.Reflection;
using UnityEngine;

namespace QM_CloneSort
{
    public static class Plugin
    {
        public static string HarmonyId { get; } = "valid.QM_CloneSort";

        [Hook(ModHookType.AfterConfigsLoaded)]
        public static void AfterConfig(IModContext context)
        {
            try
            {
                new Harmony(HarmonyId).PatchAll(Assembly.GetExecutingAssembly());
                Debug.Log("[QM_CloneSort] Harmony patches applied");
            }
            catch (Exception ex)
            {
                Debug.LogError("[QM_CloneSort] Failed to apply Harmony patches");
                Debug.LogException(ex);
            }
        }
    }
}
