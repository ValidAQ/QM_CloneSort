using System.Collections.Generic;

namespace QM_CloneSort
{
    public static class CloneSortState
    {
        public static SortMode SortMode { get; private set; } = SortMode.Manual;

        /// <summary>
        /// When true (ManualOnly config mode), manual order is maintained but the UpButton is hidden.
        /// </summary>
        public static bool IsManualLocked { get; private set; } = false;

        public static string SelectedProfileId { get; private set; } = string.Empty;

        public static SortMode CycleSortMode()
        {
            ModConfig config = Plugin.Config;

            if (config.SortingMode == SortingModeOptions.ManualOnly)
            {
                IsManualLocked = !IsManualLocked;
                SortMode = SortMode.Manual;
                Plugin.Logger.Log($"Sort mode: Manual (locked={IsManualLocked})");
                return SortMode;
            }

            // ManualAndNameRank: build list of available modes
            var modes = new List<SortMode> { SortMode.Manual };
            if (config.EnableNameSort)
            {
                modes.Add(SortMode.NameAsc);
                modes.Add(SortMode.NameDesc);
            }
            if (config.EnableRankSort)
            {
                modes.Add(SortMode.RankDesc);
            }

            int current = modes.IndexOf(SortMode);
            SortMode = modes[(current + 1) % modes.Count];
            IsManualLocked = false;
            Plugin.Logger.Log($"Sort mode: {SortMode}");
            return SortMode;
        }

        public static void SetSortMode(SortMode mode)
        {
            SortMode = mode;
            IsManualLocked = false;
            Plugin.Logger.Log($"Sort mode: {SortMode}");
        }

        /// <summary>
        /// Resets to Manual if the current sort mode is no longer available under the current config.
        /// Called after the player saves config in MCM.
        /// </summary>
        public static void ResetToValidMode()
        {
            ModConfig config = Plugin.Config;

            if (config.SortingMode == SortingModeOptions.ManualOnly)
            {
                SortMode = SortMode.Manual;
                return;
            }

            // ManualAndNameRank mode
            IsManualLocked = false;
            if ((SortMode == SortMode.NameAsc || SortMode == SortMode.NameDesc) && !config.EnableNameSort)
                SortMode = SortMode.Manual;
            else if (SortMode == SortMode.RankDesc && !config.EnableRankSort)
                SortMode = SortMode.Manual;
        }

        public static void SetSelectedProfileId(string profileId)
        {
            SelectedProfileId = profileId ?? string.Empty;
        }
    }
}
