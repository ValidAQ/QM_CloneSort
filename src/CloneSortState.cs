using UnityEngine;

namespace QM_CloneSort
{
    public static class CloneSortState
    {
        public static SortMode SortMode { get; private set; } = SortMode.Manual;

        public static string SelectedProfileId { get; private set; } = string.Empty;

        public static SortMode CycleSortMode()
        {
            int next = ((int)SortMode + 1) % 4;
            SortMode = (SortMode)next;
            Debug.Log($"[QM_CloneSort] Sort mode: {SortMode}");
            return SortMode;
        }

        public static void SetSortMode(SortMode mode)
        {
            SortMode = mode;
            Debug.Log($"[QM_CloneSort] Sort mode: {SortMode}");
        }

        public static void SetSelectedProfileId(string profileId)
        {
            SelectedProfileId = profileId ?? string.Empty;
        }
    }
}
