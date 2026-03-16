using MGSC;
using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace QM_CloneSort
{
    public static class MercenaryOrdering
    {
        // Profile IDs in the user's chosen manual order; null until first use.
        private static List<string> _manualOrder;

        public static void ApplySort(Mercenaries mercenaries)
        {
            if (mercenaries == null || mercenaries.Values == null || mercenaries.Values.Count <= 1)
            {
                return;
            }

            if (CloneSortState.SortMode == SortMode.Manual)
            {
                EnsureManualOrder(mercenaries);
                ApplyManualOrder(mercenaries);
                return;
            }

            List<Mercenary> sorted;
            switch (CloneSortState.SortMode)
            {
                case SortMode.NameAsc:
                    sorted = mercenaries.Values
                        .OrderBy(GetDisplayName, StringComparer.CurrentCultureIgnoreCase)
                        .ToList();
                    break;

                case SortMode.NameDesc:
                    sorted = mercenaries.Values
                        .OrderByDescending(GetDisplayName, StringComparer.CurrentCultureIgnoreCase)
                        .ToList();
                    break;

                case SortMode.RankDesc:
                    sorted = mercenaries.Values
                        .OrderByDescending(GetRankLevel)
                        .ThenBy(GetDisplayName, StringComparer.CurrentCultureIgnoreCase)
                        .ToList();
                    break;

                default:
                    return;
            }

            mercenaries.Values.Clear();
            mercenaries.Values.AddRange(sorted);
        }

        public static bool MoveByProfileId(Mercenaries mercenaries, string profileId, int delta)
        {
            if (mercenaries == null || mercenaries.Values == null || mercenaries.Values.Count <= 1 || string.IsNullOrEmpty(profileId))
            {
                return false;
            }

            int index = mercenaries.Values.FindIndex(m => m != null && m.ProfileId == profileId);
            if (index < 0)
            {
                return false;
            }

            int target = index + delta;
            if (target < 0 || target >= mercenaries.Values.Count)
            {
                return false;
            }

            Mercenary current = mercenaries.Values[index];
            mercenaries.Values[index] = mercenaries.Values[target];
            mercenaries.Values[target] = current;

            _manualOrder = mercenaries.Values
                .Where(m => m != null)
                .Select(m => m.ProfileId)
                .ToList();
            ManualOrderStore.Save(_manualOrder);
            return true;
        }

        /// <summary>
        /// Initialises _manualOrder from disk on first call, falling back to the current
        /// mercenaries order so we don't lose the game's own initial ordering.
        /// </summary>
        private static void EnsureManualOrder(Mercenaries mercenaries)
        {
            if (_manualOrder != null)
                return;

            _manualOrder = ManualOrderStore.Load();
            if (_manualOrder == null)
            {
                _manualOrder = mercenaries.Values
                    .Where(m => m != null)
                    .Select(m => m.ProfileId)
                    .ToList();
                ManualOrderStore.Save(_manualOrder);
            }
        }

        /// <summary>
        /// Reorders mercenaries.Values to match _manualOrder.
        /// Mercs absent from the saved list are appended at the end.
        /// Stale IDs (mercs no longer in the roster) are silently dropped.
        /// </summary>
        private static void ApplyManualOrder(Mercenaries mercenaries)
        {
            var mercById = mercenaries.Values
                .Where(m => m != null)
                .ToDictionary(m => m.ProfileId);

            var ordered = new List<Mercenary>(_manualOrder.Count);
            foreach (string id in _manualOrder)
                if (mercById.TryGetValue(id, out Mercenary m))
                    ordered.Add(m);

            // Append any mercs not yet tracked (e.g. newly hired)
            var seen = new HashSet<string>(_manualOrder);
            foreach (Mercenary m in mercenaries.Values)
                if (m != null && !seen.Contains(m.ProfileId))
                    ordered.Add(m);

            // Keep _manualOrder in sync with the actual roster
            _manualOrder = ordered.Select(m => m.ProfileId).ToList();

            mercenaries.Values.Clear();
            mercenaries.Values.AddRange(ordered);
        }

        private static string GetDisplayName(Mercenary mercenary)
        {
            if (mercenary == null)
            {
                return string.Empty;
            }

            string key = "spec." + mercenary.ProfileId + ".name";
            string localized = Localization.Get(key);
            if (string.IsNullOrEmpty(localized) || localized == key)
            {
                return mercenary.ProfileId ?? string.Empty;
            }

            return localized;
        }

        private static int GetRankLevel(Mercenary mercenary)
        {
            if (mercenary?.CreatureData?.RankPerk == null)
                return 0;

            ParseHelper.GetGradeByPerkId(mercenary.CreatureData.RankPerk.PerkId, out int level, out _, out _);
            return level;
        }

        public static string GetPanelMercProfileId(MercenaryPanel panel)
        {
            if (panel == null)
            {
                return string.Empty;
            }

            Mercenary mercenary = Traverse.Create(panel).Field<Mercenary>("_mercenary").Value;
            return mercenary?.ProfileId ?? string.Empty;
        }
    }
}
