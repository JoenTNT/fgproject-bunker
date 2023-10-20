using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JT.FGP
{
    /// <summary>
    /// Button that ignores transparent part of sprite.
    /// </summary>
    [RequireComponent(typeof(RectTransform), typeof(Image))]
    public class UI_NonAlphaImageButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler,
        IPointerUpHandler, IClickEvent
    {
        #region Variable

        [Header("Requirements")]
        [SerializeField]
        private Image _imageGraphic = null;

        [Header("Game Events")]
        [SerializeField]
        private UnityEvent _onClick = new UnityEvent();

        [SerializeField]
        private UnityEvent _onPressDown = new UnityEvent();

        [SerializeField]
        private UnityEvent _onPressUp = new UnityEvent();

        // Temporary variable
        private Vector2 _ratioRectSprite = Vector2.zero;
        private Vector2 _pointOnSpriteRect = Vector2.zero;
        private bool _isPressing = false;

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
            if (!IsValid(eventData.position)) return;
            
            // Call event.
            _onClick?.Invoke();
        }

        #endregion

        #region IPointerDownHandler

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_imageGraphic.sprite == null) return;
            if (!IsValid(eventData.position)) return;

            // Call event.
            _isPressing = true;
            _onPressDown?.Invoke();
        }

        #endregion

        #region IPointerUpHandler

        public void OnPointerUp(PointerEventData eventData)
        {
            // Check pressing status.
            if (!_isPressing) return;

            // Reset status and call event.
            _isPressing = false;
            _onPressUp?.Invoke();
        }

        #endregion

        #region IClickEvent

        /// <summary>
        /// Called when the button clicked.
        /// </summary>
        public UnityEvent OnClick => _onClick;

        #endregion

        #region Main

        private bool IsValid(Vector2 pressPos)
        {
            Vector2 rectPos = rectTransform.position;
            Vector2 pointerOnRect = rectPos - pressPos;
            Rect spriteRect = _imageGraphic.sprite.rect;

            _ratioRectSprite.x = spriteRect.width / rectTransform.sizeDelta.x;
            _ratioRectSprite.y = spriteRect.height / rectTransform.sizeDelta.y;

            // TODO: GC Allocation, Change this Logic
            _pointOnSpriteRect.x = (rectTransform.sizeDelta.x - pointerOnRect.x) * _ratioRectSprite.x;
            _pointOnSpriteRect.y = -pointerOnRect.y * _ratioRectSprite.y;

            if (!spriteRect.Contains(_pointOnSpriteRect)) return false;

            // TODO: GC Allocation, Change this Logic
            Color pixelColor = _imageGraphic.sprite.texture.GetPixel(
                Mathf.FloorToInt(_pointOnSpriteRect.x - spriteRect.x),
                Mathf.FloorToInt(_pointOnSpriteRect.y - spriteRect.y));

            if (pixelColor.a <= 0f) return false;

            // Complete validation.
            return true;
        }

        #endregion
    }
}