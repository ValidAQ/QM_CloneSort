using HarmonyLib;
using MGSC;
using UnityEngine;
using UnityEngine.UI;

namespace QM_CloneSort
{
    public static class ManualReorderActions
    {
        public static bool TryMoveByProfileId(SelectMercenaryScreen screen, string profileId, int delta)
        {
            if (screen == null || delta == 0 || CloneSortState.SortMode != SortMode.Manual)
            {
                return false;
            }

            ScrollRect scrollRect = Traverse.Create(screen).Field<ScrollRect>("_scrollRect").Value;
            Vector2 scrollPos = scrollRect != null ? scrollRect.normalizedPosition : Vector2.zero;

            Mercenaries mercenaries = Traverse.Create(screen).Field<Mercenaries>("_mercenaries").Value;
            bool result = TryMoveAndRefresh(mercenaries, profileId, delta, screen.gameObject);

            if (result && scrollRect != null)
                scrollRect.normalizedPosition = scrollPos;

            return result;
        }

        public static bool TryMoveByProfileId(MercenariesScreen screen, string profileId, int delta)
        {
            if (screen == null || delta == 0 || CloneSortState.SortMode != SortMode.Manual)
            {
                return false;
            }

            ScrollRect scrollRect = Traverse.Create(screen).Field<ScrollRect>("_scrollRect").Value;
            Vector2 scrollPos = scrollRect != null ? scrollRect.normalizedPosition : Vector2.zero;

            Mercenaries mercenaries = Traverse.Create(screen).Field<Mercenaries>("_mercenaries").Value;
            bool result = TryMoveAndRefresh(mercenaries, profileId, delta, screen.gameObject);

            if (result && scrollRect != null)
                scrollRect.normalizedPosition = scrollPos;

            return result;
        }

        public static bool TryMoveSelected(SelectMercenaryScreen screen, int delta)
        {
            if (screen == null || delta == 0 || CloneSortState.SortMode != SortMode.Manual)
            {
                return false;
            }

            ScrollRect scrollRect = Traverse.Create(screen).Field<ScrollRect>("_scrollRect").Value;
            Vector2 scrollPos = scrollRect != null ? scrollRect.normalizedPosition : Vector2.zero;

            Mercenaries mercenaries = Traverse.Create(screen).Field<Mercenaries>("_mercenaries").Value;
            bool result = TryMoveAndRefresh(mercenaries, CloneSortState.SelectedProfileId, delta, screen.gameObject);

            if (result && scrollRect != null)
                scrollRect.normalizedPosition = scrollPos;

            return result;
        }

        public static bool TryMoveSelected(MercenariesScreen screen, int delta)
        {
            if (screen == null || delta == 0 || CloneSortState.SortMode != SortMode.Manual)
            {
                return false;
            }

            ScrollRect scrollRect = Traverse.Create(screen).Field<ScrollRect>("_scrollRect").Value;
            Vector2 scrollPos = scrollRect != null ? scrollRect.normalizedPosition : Vector2.zero;

            Mercenaries mercenaries = Traverse.Create(screen).Field<Mercenaries>("_mercenaries").Value;
            bool result = TryMoveAndRefresh(mercenaries, CloneSortState.SelectedProfileId, delta, screen.gameObject);

            if (result && scrollRect != null)
                scrollRect.normalizedPosition = scrollPos;

            return result;
        }

        private static bool TryMoveAndRefresh(Mercenaries mercenaries, string selectedProfileId, int delta, GameObject screenObject)
        {
            if (mercenaries == null || mercenaries.Values == null || mercenaries.Values.Count <= 1)
            {
                return false;
            }

            string profileId = selectedProfileId;
            if (string.IsNullOrEmpty(profileId))
            {
                profileId = mercenaries.Values[0]?.ProfileId;
                CloneSortState.SetSelectedProfileId(profileId);
            }

            bool moved = MercenaryOrdering.MoveByProfileId(mercenaries, profileId, delta);
            if (!moved)
            {
                return false;
            }

            screenObject.SendMessage("OnEnable", SendMessageOptions.DontRequireReceiver);
            return true;
        }
    }
}