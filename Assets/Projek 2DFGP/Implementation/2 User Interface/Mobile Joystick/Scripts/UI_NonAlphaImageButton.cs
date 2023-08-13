using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JT.FGP
{
    [RequireComponent(typeof(RectTransform), typeof(Image))]
    public class UI_NonAlphaImageButton : MonoBehaviour, IPointerClickHandler, IClickEvent
    {
        #region Variable

        [SerializeField]
        private Image _imageGraphic = null;

        [SerializeField]
        private UnityEvent _onClick = new UnityEvent();

        // Temporary variable
        private Vector2 _ratioRectSprite = Vector2.zero;
        private Vector2 _pointOnSpriteRect = Vector2.zero;

        #endregion

        #region Properties

        /// <summary>
        /// Transform component in RectTransform.
        /// </summary>
        public RectTransform rectTransform => (RectTransform)transform;

        #endregion

        #region IPointerClickHandler

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_imageGraphic.sprite == null) return;

            Vector2 rectPos = rectTransform.position;
            Vector2 pointerOnRect = rectPos - eventData.position;
            Rect spriteRect = _imageGraphic.sprite.rect;

            _ratioRectSprite.x = spriteRect.width / rectTransform.sizeDelta.x;
            _ratioRectSprite.y = spriteRect.height / rectTransform.sizeDelta.y;

            // TODO: GC Allocation, Change this Logic
            _pointOnSpriteRect.x = (rectTransform.sizeDelta.x - pointerOnRect.x) * _ratioRectSprite.x;
            _pointOnSpriteRect.y = -pointerOnRect.y * _ratioRectSprite.y;
#if UNITY_EDITOR
            //Debug.Log($"Pointer on Sprite Rect: {_pointOnSpriteRect}");
#endif
            if (!spriteRect.Contains(_pointOnSpriteRect)) return;

            // TODO: GC Allocation, Change this Logic
            Color pixelColor = _imageGraphic.sprite.texture.GetPixel(
                Mathf.FloorToInt(_pointOnSpriteRect.x - spriteRect.x),
                Mathf.FloorToInt(_pointOnSpriteRect.y - spriteRect.y));
#if UNITY_EDITOR
            //Debug.Log($"Color On Pixel is: {pixelColor}");
#endif
            if (pixelColor.a <= 0f) return;
#if UNITY_EDITOR
            //Debug.Log("Clicked!");
#endif
            _onClick?.Invoke();
        }

        #endregion

        #region IClickEvent

        /// <summary>
        /// Called when the button clicked.
        /// </summary>
        public UnityEvent OnClick => _onClick;

        #endregion
    }
}