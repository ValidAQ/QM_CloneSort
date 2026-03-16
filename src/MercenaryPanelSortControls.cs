using MGSC;
using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QM_CloneSort
{
    public class MercenaryPanelSortControls : MonoBehaviour
    {
        private const float RankShiftRight = 24f;

        private MoveUpIcon _upIcon;
        private string _profileId;
        private IMoveHandler _moveHandler;
        private RectTransform _rankUnblockedRect;
        private RectTransform _rankBlockedRect;
        private RectTransform _mercNameRect;
        private RectTransform _mercClassRect;
        private Vector2 _rankUnblockedOriginalPos;
        private Vector2 _rankBlockedOriginalPos;
        private Vector2 _mercNameOriginalPos;
        private Vector2 _mercClassOriginalPos;
        private bool _rankPositionCached;

        public void Initialize(string profileId, IMoveHandler moveHandler)
        {
            _profileId = profileId ?? string.Empty;
            _moveHandler = moveHandler;

            if (_upIcon == null)
            {
                CreateMoveUpIcon();
            }

            bool manual = CloneSortState.SortMode == SortMode.Manual;
            if (_upIcon != null)
            {
                _upIcon.gameObject.SetActive(manual);
                _upIcon.SetInteractable(manual);
            }

            ApplyRankShift(manual);
        }

        private void OnDestroy()
        {
            if (_upIcon != null)
            {
                _upIcon.OnClicked -= UpButtonOnClicked;
            }
        }

        private void UpButtonOnClicked()
        {
            _moveHandler?.TryMove(_profileId, -1);
        }

        private void CreateMoveUpIcon()
        {
            MercenaryPanel panel = GetComponent<MercenaryPanel>();
            if (panel == null)
            {
                return;
            }

            MercenaryBackpackIcon backpackIcon = Traverse.Create(panel).Field<MercenaryBackpackIcon>("_backpackIcon").Value;
            RectTransform backpackRect = backpackIcon?.transform as RectTransform;
            if (backpackIcon == null || backpackRect == null)
            {
                return;
            }

            GameObject rankUnblocked = Traverse.Create(panel).Field<GameObject>("_rankUnblocked").Value;
            GameObject rankBlocked = Traverse.Create(panel).Field<GameObject>("_rankBlocked").Value;
            _rankUnblockedRect = rankUnblocked?.transform as RectTransform;
            _rankBlockedRect = rankBlocked?.transform as RectTransform;

            // _mercName/_mercClass are TextMeshProUGUI (not Text), use untyped access
            _mercNameRect = (Traverse.Create(panel).Field("_mercName").GetValue() as Component)?.transform as RectTransform;
            _mercClassRect = (Traverse.Create(panel).Field("_mercClass").GetValue() as Component)?.transform as RectTransform;

            CacheOriginalRankPositions();

            RectTransform panelRect = panel.transform as RectTransform;
            if (panelRect == null)
            {
                return;
            }

            MercenaryBackpackIcon clone = Object.Instantiate(backpackIcon, panelRect);
            clone.name = "QM_CloneSort_MoveUpIcon";

            RectTransform cloneRect = clone.transform as RectTransform;
            if (cloneRect != null)
            {
                // Anchor to left-center of panel; place centered within the RankShiftRight gap
                cloneRect.anchorMin = new Vector2(0f, 0.5f);
                cloneRect.anchorMax = new Vector2(0f, 0.5f);
                cloneRect.pivot = new Vector2(0.5f, 0.5f);
                cloneRect.sizeDelta = backpackRect.sizeDelta;
                cloneRect.localScale = backpackRect.localScale;
                cloneRect.anchoredPosition = new Vector2(RankShiftRight / 2f, 0f);
                cloneRect.SetAsLastSibling();
            }

            MoveUpIcon moveUpIcon = clone.gameObject.AddComponent<MoveUpIcon>();
            moveUpIcon.gameObject.SetActive(true);
            moveUpIcon.OnClicked += UpButtonOnClicked;

            Object.Destroy(clone);
            _upIcon = moveUpIcon;
        }

        private void LateUpdate()
        {
            bool manual = CloneSortState.SortMode == SortMode.Manual;
            if (_upIcon != null && _upIcon.gameObject.activeSelf != manual)
            {
                _upIcon.gameObject.SetActive(manual);
                _upIcon.SetInteractable(manual);
            }

            ApplyRankShift(manual);
        }

        private void CacheOriginalRankPositions()
        {
            if (_rankPositionCached)
            {
                return;
            }

            _rankUnblockedOriginalPos = _rankUnblockedRect != null ? _rankUnblockedRect.anchoredPosition : Vector2.zero;
            _rankBlockedOriginalPos = _rankBlockedRect != null ? _rankBlockedRect.anchoredPosition : Vector2.zero;
            _mercNameOriginalPos = _mercNameRect != null ? _mercNameRect.anchoredPosition : Vector2.zero;
            _mercClassOriginalPos = _mercClassRect != null ? _mercClassRect.anchoredPosition : Vector2.zero;
            _rankPositionCached = true;
        }

        private void ApplyRankShift(bool shifted)
        {
            if (!_rankPositionCached)
            {
                return;
            }

            Vector2 offset = shifted ? new Vector2(RankShiftRight, 0f) : Vector2.zero;

            if (_rankUnblockedRect != null)
            {
                _rankUnblockedRect.anchoredPosition = _rankUnblockedOriginalPos + offset;
            }
            if (_rankBlockedRect != null)
            {
                _rankBlockedRect.anchoredPosition = _rankBlockedOriginalPos + offset;
            }
            if (_mercNameRect != null)
            {
                _mercNameRect.anchoredPosition = _mercNameOriginalPos + offset;
            }
            if (_mercClassRect != null)
            {
                _mercClassRect.anchoredPosition = _mercClassOriginalPos + offset;
            }
        }

        public interface IMoveHandler
        {
            bool TryMove(string profileId, int delta);
        }
    }
}
