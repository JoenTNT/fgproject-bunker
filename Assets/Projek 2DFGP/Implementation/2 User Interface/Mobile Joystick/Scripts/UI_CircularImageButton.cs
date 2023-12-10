using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace JT.FGP
{
    /// <summary>
    /// Button that is valid only in radius range.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class UI_CircularImageButton : MonoBehaviour, IPointerClickHandler,
        IPointerDownHandler, IPointerUpHandler, IClickEvent
    {
        #region Variables

        [Header("Game Events")]
        [SerializeField]
        private UnityEvent _onClick = new UnityEvent();

        [SerializeField]
        private UnityEvent _onPressDown = new UnityEvent();

        [SerializeField]
        private UnityEvent _onPressUp = new UnityEvent();
#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField]
        private bool _debug = false;
#endif
        // Runtime variable data.
        private RectTransform _rectTransform = null;
        private bool _isPressing = false;

        #endregion

        #region Properties

        public RectTransform rectTransform => _rectTransform;

        #endregion

        #region Mono

        private void Awake()
        {
            // Convert to rect transform.
            _rectTransform = (RectTransform)transform;
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!_debug) return;

            _rectTransform = (RectTransform)transform;
            Vector2 centerPos = (Vector2)transform.position + _rectTransform.rect.center;
            Vector2 size = _rectTransform.sizeDelta;

            Debug.DrawRay(centerPos, new Vector2(-size.x / 2f, 0f), Color.green);
            Debug.DrawRay(centerPos, new Vector2(size.x / 2f, 0f), Color.green);
            Debug.DrawRay(centerPos, new Vector2(0f, -size.y / 2f), Color.green);
            Debug.DrawRay(centerPos, new Vector2(0f, size.y / 2f), Color.green);
        }
#endif
        #endregion

        #region IClickEvent

        public UnityEvent OnClick => _onClick;

        #endregion

        #region IPointerClickHandler

        public void OnPointerClick(PointerEventData eventData)
        {
            // Validate press position.
            if (!IsValid(eventData.position)) return;

            // Call event.
            _onClick?.Invoke();
        }

        #endregion

        #region IPointerDownHandler

        public void OnPointerDown(PointerEventData eventData)
        {
            // Validate press position.
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

        #region Main

        private bool IsValid(Vector2 pressPos)
        {
            // Calculate normal transform position.
            Vector2 centerPos = (Vector2)transform.position + _rectTransform.rect.center;
            Vector2 halfSize = _rectTransform.sizeDelta / 2f;

            // Check position inside circular area.
            pressPos -= centerPos;
            float pressRad = Mathf.Sqrt(pressPos.x * pressPos.x + pressPos.y * pressPos.y);
            float allowedRad = Mathf.Sqrt(halfSize.x * halfSize.x + halfSize.y * halfSize.y);

            // Validate press position.
            if (pressRad > allowedRad) return false;
            return true;
        }

        #endregion
    }
}
