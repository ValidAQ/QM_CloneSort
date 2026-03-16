using MGSC;
using HarmonyLib;
using UnityEngine;

namespace QM_CloneSort
{
    public abstract class SortUiBase : MonoBehaviour
    {
        private const float SortButtonOffset = 240f;

        private MonoBehaviour _screen;
        private CommonButton _sortButton;

        protected void InitializeBase(MonoBehaviour screen, CommonButton backButtonTemplate, string buttonName)
        {
            _screen = screen;

            if (_sortButton == null && backButtonTemplate != null)
            {
                _sortButton = Object.Instantiate(backButtonTemplate, backButtonTemplate.transform.parent);
                _sortButton.name = buttonName;
                ClearHotkey(_sortButton);
                _sortButton.OnClick += SortButtonOnClick;
                _sortButton.ChangeLabel(string.Empty);
                _sortButton.SetInteractable(true);
                RepositionNearBackButton(_sortButton, backButtonTemplate, SortButtonOffset);
            }

            RefreshCaption();
            if (_sortButton != null)
            {
                _sortButton.gameObject.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            if (_sortButton != null)
            {
                _sortButton.OnClick -= SortButtonOnClick;
            }
        }

        private void SortButtonOnClick(CommonButton button, int clickCount)
        {
            CloneSortState.CycleSortMode();
            RefreshCaption();
            _screen?.SendMessage("OnEnable", SendMessageOptions.DontRequireReceiver);
        }

        private void RefreshCaption()
        {
            if (_sortButton == null)
            {
                return;
            }

            _sortButton.InitCaption("Sort: " + CloneSortState.SortMode);
        }

        private static void ClearHotkey(CommonButton button)
        {
            HotkeyButton hotkeyButton = button as HotkeyButton;
            if (hotkeyButton == null) return;
            GameKeyPanel panel = Traverse.Create(hotkeyButton).Field<GameKeyPanel>("_gameKeyPanel").Value;
            if (panel != null) panel.gameObject.SetActive(false);
            Traverse.Create(hotkeyButton).Field("_keyId").SetValue(string.Empty);
        }

        private static void RepositionNearBackButton(CommonButton sortButton, CommonButton backButton, float horizontalOffset)
        {
            RectTransform sortRect = sortButton.transform as RectTransform;
            RectTransform backRect = backButton.transform as RectTransform;
            if (sortRect == null || backRect == null)
            {
                return;
            }

            sortRect.anchorMin = backRect.anchorMin;
            sortRect.anchorMax = backRect.anchorMax;
            sortRect.pivot = backRect.pivot;
            sortRect.anchoredPosition = backRect.anchoredPosition + new Vector2(horizontalOffset, 0f);
            sortRect.sizeDelta = backRect.sizeDelta;
            sortRect.localScale = backRect.localScale;
        }
    }
}
