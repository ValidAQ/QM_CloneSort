using HarmonyLib;
using MGSC;
using System.Collections.Generic;
using UnityEngine;

namespace QM_CloneSort
{
    [HarmonyPatch]
    public static class CloneSortPatches
    {
        [HarmonyPatch(typeof(SelectMercenaryScreen), "OnEnable")]
        [HarmonyPrefix]
        private static void SelectMercenaryScreen_OnEnable_Prefix(SelectMercenaryScreen __instance)
        {
            Mercenaries mercenaries = Traverse.Create(__instance).Field<Mercenaries>("_mercenaries").Value;
            MercenaryOrdering.ApplySort(mercenaries);
        }

        [HarmonyPatch(typeof(SelectMercenaryScreen), "OnEnable")]
        [HarmonyPostfix]
        private static void SelectMercenaryScreen_OnEnable_Postfix(SelectMercenaryScreen __instance)
        {
            EnsureSortUi(__instance);
            EnsurePanelSortControls(__instance);
        }

        [HarmonyPatch(typeof(MercenariesScreen), "OnEnable")]
        [HarmonyPrefix]
        private static void MercenariesScreen_OnEnable_Prefix(MercenariesScreen __instance)
        {
            Mercenaries mercenaries = Traverse.Create(__instance).Field<Mercenaries>("_mercenaries").Value;
            MercenaryOrdering.ApplySort(mercenaries);
        }

        [HarmonyPatch(typeof(MercenariesScreen), "OnEnable")]
        [HarmonyPostfix]
        private static void MercenariesScreen_OnEnable_Postfix(MercenariesScreen __instance)
        {
            EnsureSortUi(__instance);
            EnsurePanelSortControls(__instance);
        }

        [HarmonyPatch(typeof(SelectMercenaryScreen), "PanelOnSelected")]
        [HarmonyPrefix]
        private static void SelectMercenaryScreen_PanelOnSelected_Prefix(string mercId)
        {
            CloneSortState.SetSelectedProfileId(mercId);
        }

        [HarmonyPatch(typeof(MercenariesScreen), "PanelOnIconSelected")]
        [HarmonyPrefix]
        private static void MercenariesScreen_PanelOnIconSelected_Prefix(Mercenary merc)
        {
            CloneSortState.SetSelectedProfileId(merc?.ProfileId);
        }

        private static void EnsureSortUi(SelectMercenaryScreen screen)
        {
            SelectMercenarySortUi sortUi = screen.GetComponent<SelectMercenarySortUi>();
            if (sortUi == null)
            {
                sortUi = screen.gameObject.AddComponent<SelectMercenarySortUi>();
            }

            CommonButton backButton = Traverse.Create(screen).Field<CommonButton>("_backButton").Value;
            sortUi.Initialize(screen, backButton);
        }

        private static void EnsureSortUi(MercenariesScreen screen)
        {
            MercenariesSortUi sortUi = screen.GetComponent<MercenariesSortUi>();
            if (sortUi == null)
            {
                sortUi = screen.gameObject.AddComponent<MercenariesSortUi>();
            }

            CommonButton backButton = Traverse.Create(screen).Field<CommonButton>("_backButton").Value;
            sortUi.Initialize(screen, backButton);
        }

        private static void EnsurePanelSortControls(SelectMercenaryScreen screen)
        {
            List<MercenaryPanel> panels = Traverse.Create(screen).Field<List<MercenaryPanel>>("_panels").Value;
            if (panels == null)
            {
                return;
            }

            SelectMercenaryMoveHandler moveHandler = new SelectMercenaryMoveHandler(screen);
            foreach (MercenaryPanel panel in panels)
            {
                if (panel == null)
                {
                    continue;
                }

                string profileId = MercenaryOrdering.GetPanelMercProfileId(panel);
                MercenaryPanelSortControls controls = panel.GetComponent<MercenaryPanelSortControls>();
                if (controls == null)
                {
                    controls = panel.gameObject.AddComponent<MercenaryPanelSortControls>();
                }

                controls.Initialize(profileId, moveHandler);
            }
        }

        private static void EnsurePanelSortControls(MercenariesScreen screen)
        {
            List<MercenaryPanel> panels = Traverse.Create(screen).Field<List<MercenaryPanel>>("_panels").Value;
            if (panels == null)
            {
                return;
            }

            MercenariesMoveHandler moveHandler = new MercenariesMoveHandler(screen);
            foreach (MercenaryPanel panel in panels)
            {
                if (panel == null)
                {
                    continue;
                }

                string profileId = MercenaryOrdering.GetPanelMercProfileId(panel);
                MercenaryPanelSortControls controls = panel.GetComponent<MercenaryPanelSortControls>();
                if (controls == null)
                {
                    controls = panel.gameObject.AddComponent<MercenaryPanelSortControls>();
                }

                controls.Initialize(profileId, moveHandler);
            }
        }

        private sealed class SelectMercenaryMoveHandler : MercenaryPanelSortControls.IMoveHandler
        {
            private readonly SelectMercenaryScreen _screen;

            public SelectMercenaryMoveHandler(SelectMercenaryScreen screen)
            {
                _screen = screen;
            }

            public bool TryMove(string profileId, int delta)
            {
                return ManualReorderActions.TryMoveByProfileId(_screen, profileId, delta);
            }
        }

        private sealed class MercenariesMoveHandler : MercenaryPanelSortControls.IMoveHandler
        {
            private readonly MercenariesScreen _screen;

            public MercenariesMoveHandler(MercenariesScreen screen)
            {
                _screen = screen;
            }

            public bool TryMove(string profileId, int delta)
            {
                return ManualReorderActions.TryMoveByProfileId(_screen, profileId, delta);
            }
        }
    }
}
