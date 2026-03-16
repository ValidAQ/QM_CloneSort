using HarmonyLib;
using MGSC;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QM_CloneSort
{
    internal sealed class MoveUpIcon : Navigable, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private Image _icon;
        private Image _background;
        private Image _selectionBorder;
        private bool _interactable = true;

        private static Sprite _upArrowSprite;

        public override bool IsInteractable => _interactable;

        public event System.Action OnClicked = delegate { };

        private void Awake()
        {
            MercenaryBackpackIcon source = GetComponent<MercenaryBackpackIcon>();
            if (source != null)
            {
                _icon = Traverse.Create(source).Field<Image>("_icon").Value;
                _background = Traverse.Create(source).Field<Image>("_background").Value;
                _selectionBorder = Traverse.Create(source).Field<Image>("_selectionBorder").Value;

                Sprite normalBgSprite = Traverse.Create(source).Field<Sprite>("_normalBgSprite").Value;
                if (_background != null && normalBgSprite != null)
                {
                    _background.sprite = normalBgSprite;
                }
            }

            if (_icon != null)
            {
                _icon.sprite = GetUpArrowSprite();
            }

            if (_selectionBorder != null)
            {
                _selectionBorder.gameObject.SetActive(false);
            }
        }

        public void SetInteractable(bool interactable)
        {
            _interactable = interactable;
        }

        public override void Select()
        {
            OnPointerEnter(null);
        }

        public override void Diselect()
        {
            OnPointerExit(null);
        }

        public override void EvaluateConfirm()
        {
            OnPointerClick(null);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_interactable)
            {
                return;
            }

            this.OnClicked();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_selectionBorder != null)
            {
                _selectionBorder.gameObject.SetActive(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_selectionBorder != null)
            {
                _selectionBorder.gameObject.SetActive(false);
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();
            OnPointerExit(null);
        }

        private static Sprite GetUpArrowSprite()
        {
            if (_upArrowSprite == null)
                _upArrowSprite = CreateUpArrowSprite();
            return _upArrowSprite;
        }

        private static Sprite CreateUpArrowSprite()
        {
            var asm = Assembly.GetExecutingAssembly();
            using (Stream stream = asm.GetManifestResourceStream("QM_CloneSort.merkStatus_UpIcon.png"))
            using (var reader = new BinaryReader(stream))
            {
                byte[] data = reader.ReadBytes((int)stream.Length);
                Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                tex.filterMode = FilterMode.Point;
                tex.wrapMode = TextureWrapMode.Clamp;
                ImageConversion.LoadImage(tex, data, false);
                return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            }
        }
    }
}
