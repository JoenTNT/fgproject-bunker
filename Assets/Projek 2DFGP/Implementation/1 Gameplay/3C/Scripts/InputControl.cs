using JT.GameEvents;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JT.FGP
{
    /// <summary>
    /// Handles all kinds of input.
    /// </summary>
    public class InputControl : MonoBehaviour
    {
        #region Variable

        [Header("Properties")]
        [SerializeField]
        private InputControlData _data = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventStringVector2 _onMoveInput = null;

        [SerializeField]
        private GameEventStringVector2 _onLookPositionInput = null;

        [SerializeField]
        private GameEventString _onInteractCommand = null;

        [SerializeField]
        private GameEventBool _lockControllerCallback = null;

        // Temp variable data
        private Vector2 _moveDir = Vector2.zero;
        private Vector2 _lookAtPos = Vector2.zero;
        private bool _inputDisabled = false;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events
            _lockControllerCallback.AddListener(ListenLockControllerCallback);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _lockControllerCallback.RemoveListener(ListenLockControllerCallback);
        }

        private void Start() => _inputDisabled = _data.IsInputDisableOnStart;

        #endregion

        #region Main

        private void ListenLockControllerCallback(bool isLock)
        {
            if (_data.IsInputDisableOnStart) return;

            _inputDisabled = isLock;
        }

        private void OnMovement(InputValue val)
        {
            if (_inputDisabled) return;

            _moveDir = val.Get<Vector2>();
            _onMoveInput.Invoke(_data.TargetID, _moveDir);
        }

        private void OnMouseLook(InputValue val)
        {
            if (_inputDisabled) return;

            _lookAtPos = val.Get<Vector2>();
            _onLookPositionInput.Invoke(_data.TargetID, Camera.main.ScreenToWorldPoint(_lookAtPos));
        }

        private void OnInteract(InputValue val)
        {
            if (_inputDisabled) return;
#if UNITY_EDITOR
            Debug.Log("Interact Input Send");
#endif
            _onInteractCommand.Invoke(_data.TargetID);
        }

        #endregion
    }

    /// <summary>
    /// Handle data for input control.
    /// </summary>
    [System.Serializable]
    internal class InputControlData : ITargetID<string>
    {
        #region Variable

        [SerializeField]
        private string _targetID = string.Empty;

        [SerializeField]
        private bool _isInputDisableOnStart = false;

        #endregion

        #region Properties

        /// <summary>
        /// To disable input control.
        /// </summary>
        public bool IsInputDisableOnStart => _isInputDisableOnStart;

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
