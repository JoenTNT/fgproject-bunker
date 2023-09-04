using JT.GameEvents;
using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEngine.InputSystem.OnScreen;

namespace JT.FGP
{
    /// <summary>
    /// The joystick main controller.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UI_JoystickControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        #region Variable

        [SerializeField]
        private UI_JoystickControlData _data = new UI_JoystickControlData();

        [Header("Game Events")]
        [SerializeField]
        private GameEventStringVector2 _joystickInputCallback = null;

        // Temp variable data
        private RectTransform _rectTransform = null;
        //private Vector2 _touchDownPoint = Vector2.zero;
        private Vector2 _joystickPosition2D = Vector2.zero; 
        private Vector2 _dragPosition = Vector2.zero;

        #endregion

        #region Properties

        /// <summary>
        /// Rect transform for this UI element.
        /// </summary>
        public RectTransform rectTransform => _rectTransform;

        #endregion

        #region Mono

        private void Awake() => _rectTransform = (RectTransform)transform;

        private void Start()
        {
            // Dependency injection to joystick commands.
            var commands = GetComponents<UI_JoystickCommand>();

            // Assign private data to all commands.
            for (int i = 0; i < commands.Length; i++)
                commands[i].InjectData(_data);
        }

        #endregion

        #region IPointerDownHandler

        public void OnPointerDown(PointerEventData eventData)
        {
#if UNITY_EDITOR
            //Debug.Log($"Touch Down: {eventData.position}");
#endif
            //_rectTransform.position = _touchDownPoint = eventData.position;
            //_data.JoystickHandle.position = eventData.position;
            _joystickPosition2D = _data.JoystickHandle.position;
            if (_data.OnJoystickDownSend) SendInputCallback();
            //SendInputCallback();
            //_data.InnerJoystick.OnPointerDown(eventData);

            // Set joystick pressed status.
            _data.IsJoystickPressed = true;
        }

        #endregion

        #region IPointerUpHandler

        public void OnPointerUp(PointerEventData eventData)
        {
#if UNITY_EDITOR
            //Debug.Log("Joystick Pointer Up");
#endif
            _data.JoystickHandle.anchoredPosition = Vector3.zero;
            if (_data.OnJoystickUpSend)
                _joystickInputCallback.Invoke(_data.TargetID, Vector2.zero);

            // Clear data when joystick released.
            _data.MagnitudePercentage = _data.JoystickMagnitude = 0f;
            _data.JoystickDirection = Vector2.zero;

            //_data.InnerJoystick.OnPointerUp(eventData);

            // Set joystick pressed status.
            _data.IsJoystickPressed = false;
        }

        #endregion

        #region IDragHandler

        public void OnDrag(PointerEventData eventData)
        {
#if UNITY_EDITOR
            //Debug.Log($"Dragged: {eventData.position}");
#endif
            _dragPosition = eventData.position;
            SendInputCallback();
            //_data.InnerJoystick.OnDrag(eventData);
        }

        #endregion

        #region Main

        private void SendInputCallback()
        {
#if UNITY_EDITOR
            //Debug.Log($"{_dragPosition} - {_joystickPosition2D}");
#endif
            // Calculate magnitude
            _data.JoystickDirection = (_dragPosition - _joystickPosition2D).normalized;
            _data.JoystickMagnitude = (_dragPosition - _joystickPosition2D).magnitude;

            // Handle position is out of the limit length
            if (_data.JoystickMagnitude > _data.MaxHandleLength)
                _data.JoystickMagnitude = _data.MaxHandleLength;

            // Assign handle position
            _data.MagnitudePercentage = _data.JoystickMagnitude / _data.MaxHandleLength;
            _data.JoystickHandle.position = _joystickPosition2D + (_data.JoystickDirection * _data.JoystickMagnitude);

            // Don't run the input callback
            if (_data.MagnitudePercentage < _data.DeadZoneLength)
                return;

            // Send input callback
#if UNITY_EDITOR
            //Debug.Log($"Direction: {_handleDirection}; Percent: {_handlePercent}");
#endif
            _joystickInputCallback.Invoke(_data.TargetID, _data.JoystickDirection * _data.MagnitudePercentage);
        }

        #endregion
    }

    /// <summary>
    /// Handle data for UI Joystick control.
    /// </summary>
    [System.Serializable]
    internal class UI_JoystickControlData : ITargetID<string>
    {
        #region Variable

        [Header("Requirements")]
        [SerializeField]
        private string _targetID = null;

        [SerializeField]
        private RectTransform _joystickHandle = null;

        [Header("Properties")]
        [SerializeField, Min(0)]
        private float _maxHandleLength = 100f;

        [SerializeField, Min(0)]
        private float _deadZoneLength = 0.1f;

        [SerializeField]
        private bool _onJoystickDownSend = false;

        [SerializeField]
        private bool _onJoystickUpSend = false;

        //[SerializeField]
        //private OnScreenStick _innerJoystick;

        // Runtime variable data.
        private Vector2 _joystickDirection = Vector2.zero;
        private float _joystickMagnitude = 0f;
        private float _magnitudePercentage = 0f;
        private bool _isJoystickPressed = false;

        #endregion

        #region Properties

        /// <summary>
        /// Inner handle of the joystick.
        /// </summary>
        public RectTransform JoystickHandle => _joystickHandle;

        /// <summary>
        /// Maximum magnitude length the handle moved from pivot point.
        /// </summary>
        public float MaxHandleLength => _maxHandleLength;

        /// <summary>
        /// The percent of magnitude which handle must be moved to validate the input.
        /// </summary>
        public float DeadZoneLength => _deadZoneLength;

        /// <summary>
        /// When joystick press down, then send.
        /// </summary>
        public bool OnJoystickDownSend => _onJoystickDownSend;

        /// <summary>
        /// When joystick release up, then send.
        /// </summary>
        public bool OnJoystickUpSend => _onJoystickUpSend;

        //public OnScreenStick InnerJoystick => _innerJoystick;

        /// <summary>
        /// Current joystick dragging direction.
        /// </summary>
        public Vector2 JoystickDirection
        {
            get => _joystickDirection;
            internal set => _joystickDirection = value;
        }

        /// <summary>
        /// Current joystick on UI position magnitude.
        /// </summary>
        public float JoystickMagnitude
        {
            get => _joystickMagnitude;
            internal set => _joystickMagnitude = value;
        }

        /// <summary>
        /// Current joystick magnitude value in percentage.
        /// Value always equal between 0 to 1.
        /// </summary>
        public float MagnitudePercentage
        {
            get => _magnitudePercentage;
            internal set => _magnitudePercentage = value;
        }

        /// <summary>
        /// The state when joystick is currently being pressed.
        /// </summary>
        public bool IsJoystickPressed
        {
            get => _isJoystickPressed;
            internal set => _isJoystickPressed = value;
        }

        #endregion

        #region ITargetID

        public string TargetID
        {
            get => _targetID;
            set => _targetID = value;
        }

        #endregion
    }
}
