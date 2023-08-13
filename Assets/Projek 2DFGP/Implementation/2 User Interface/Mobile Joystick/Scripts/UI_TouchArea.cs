using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JT.FGP
{
    /// <summary>
    /// Callback handler for touch area of the UI.
    /// </summary>
    [RequireComponent(typeof(RectTransform), typeof(Image))]
    public class UI_TouchArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        #region Variable

        // Temp variable data
        private Transform _tempChild = null;
        private RectTransform _tempRectChild = null;
        private Transform[] _childOrder = null;
        private Vector3[] _originPosChildren = null;
        private IPointerDownHandler _touchDownHandler = null;
        private IPointerUpHandler _touchUpHandler = null;
        private IDragHandler _dragHandler = null;

        #endregion

        #region Properties

        public RectTransform rectTransform => (RectTransform)transform;

        #endregion

        #region Mono

        private void Start() => InitChildData();

        #endregion

        #region IPointerDownHandler

        public void OnPointerDown(PointerEventData eventData)
        {
#if UNITY_EDITOR
            //Debug.Log("Touch Area Down");
#endif
            SendEventToChild(TouchPhase.Began, eventData);
        }

        #endregion

        #region IPointerUpHandler

        public void OnPointerUp(PointerEventData eventData)
        {
#if UNITY_EDITOR
            //Debug.Log("Touch Area Up");
#endif
            SendEventToChild(TouchPhase.Ended, eventData);
        }

        #endregion

        #region IDragHandler

        public void OnDrag(PointerEventData eventData)
        {
#if UNITY_EDITOR
            //Debug.Log("Touch Area Drag");
#endif
            SendEventToChild(TouchPhase.Moved, eventData);
        }

        #endregion

        #region Main

        private void InitChildData()
        {
            _childOrder = new Transform[transform.childCount];
            _originPosChildren = new Vector3[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                _childOrder[i] = _tempChild = transform.GetChild(i);
                _originPosChildren[i] = _tempChild.position;
            }
        }

        private void SendEventToChild(TouchPhase phase, PointerEventData eventData)
        {
            if (transform.childCount != _originPosChildren.Length)
                InitChildData();

            for (int i = 0; i < transform.childCount; i++)
            {
                _tempChild = transform.GetChild(i);

                if (_tempChild != _childOrder[i])
                {
                    InitChildData();
                    SendEventToChild(phase, eventData);
                    return;
                }

                switch (phase)
                {
                    case TouchPhase.Began:
                        if (!_tempChild.TryGetComponent(out _touchDownHandler)) return;
                        Vector2 pos = eventData.position;
                        if (_tempChild is RectTransform)
                        {
                            _tempRectChild = (RectTransform)_tempChild;
                            pos -= _tempRectChild.rect.size / 2f;
                        }
                        _tempChild.position = pos;
                        _touchDownHandler.OnPointerDown(eventData);
                        break;

                    case TouchPhase.Ended:
                        if (!_tempChild.TryGetComponent(out _touchUpHandler)) return;
                        _tempChild.position = _originPosChildren[i];
                        _touchUpHandler.OnPointerUp(eventData);
                        break;

                    case TouchPhase.Moved:
                        if (!_tempChild.TryGetComponent(out _dragHandler)) return;
                        _dragHandler.OnDrag(eventData);
                        break;
                }
            }
        }

        #endregion
    }
}
