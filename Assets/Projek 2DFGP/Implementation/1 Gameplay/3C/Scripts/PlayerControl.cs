using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Player component attached on character which can be controlled.
    /// </summary>
    [RequireComponent(typeof(PlayerEntity))]
    public class PlayerControl : MonoBehaviour
    {
        #region Variable

        [SerializeField]
        private PlayerControlData _data = new PlayerControlData();

        [Header("Game Events")]
        [SerializeField]
        private GameEventStringVector2 _onMoveInput = null;

        [SerializeField]
        private GameEventStringVector2 _onLookPositionInput = null;

        [SerializeField]
        private GameEventStringVector2 _onLookDirectionInput = null;

        [SerializeField]
        private GameEventString _onInteractCommand = null;

        [SerializeField]
        private GameEventTransform _setFollowTargetCallback = null;

        // Temporary variable data.
        private Vector2 _moveDir = Vector2.zero;
        private float _tempBeforeROM = 0f;
        private bool _isLookingAround = false;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events
            _onMoveInput.AddListener(ListenOnMoveInput);
            _onLookPositionInput.AddListener(ListenOnLookPositionInput);
            _onLookDirectionInput.AddListener(ListenOnLookDirectionInput);
            _onInteractCommand.AddListener(ListenOnInteractCommand);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _onMoveInput.RemoveListener(ListenOnMoveInput);
            _onLookPositionInput.RemoveListener(ListenOnLookPositionInput);
            _onLookDirectionInput.RemoveListener(ListenOnLookDirectionInput);
            _onInteractCommand.RemoveListener(ListenOnInteractCommand);
        }

        private void Start()
        {
            // TEMPORARY: Calibrate camera to main character
            _setFollowTargetCallback.Invoke(_data.FocusPoint);
        }

        private void Update()
        {
#if UNITY_EDITOR
            //Debug.Log($"On Update: {_moveDir}");
#endif
            // Handle movement
            _data.MoveFunc.Move(_moveDir);

            // Delaye rotate on move.
            if (_tempBeforeROM > 0f)
            {
                // Countdown time to zero.
                _tempBeforeROM -= Time.deltaTime;

                // Check time is up, if true then deactivate look control handler.
                if (_tempBeforeROM <= 0f)
                    _isLookingAround = false;
            }

            // TEMPORARY: Ignore Look Rotation at Zero Move Direction
            // TODO: Fix to Remove this Issue.
            // Check if currently not looking around action.
            if (!_isLookingAround && _moveDir != Vector2.zero)
                _data.LookFunc.LookAtDirection(_moveDir);
        }

        #endregion

        #region Main

        private void ListenOnMoveInput(string id, Vector2 moveDir)
        {
#if UNITY_EDITOR
            //Debug.Log($"{_data.ID} != {id} is {_data.ID != id}");
#endif
            if (_data.ID != id) return;

            _moveDir = moveDir;
#if UNITY_EDITOR
            //Debug.Log($"Assign Move Direction: {_moveDir}");
#endif
        }

        // For mouse controller, look position is the mouse cursor.
        private void ListenOnLookPositionInput(string id, Vector2 lookPos)
        {
            if (_data.ID != id) return;

            _data.LookFunc.LookAtPosition(lookPos);
        }

        // For mobile joystick controller, look direction is joystick drag direction.
        private void ListenOnLookDirectionInput(string id, Vector2 lookDir)
        {
            if (_data.ID != id) return;

            // Set looking around status.
            _isLookingAround = true;
            _tempBeforeROM = _data.SecondBeforeRotateOnMove;

            _data.LookFunc.LookAtDirection(lookDir);
        }

        private void ListenOnInteractCommand(string id)
        {
            if (_data.ID != id) return;

            _data.Entity.Interact();
        }

        #endregion
    }

    /// <summary>
    /// Handle data for player control.
    /// </summary>
    [System.Serializable]
    internal class PlayerControlData
    {
        #region Variable

        [Header("Requirements")]
        [SerializeField]
        private EntityID _entityID = null;

        [SerializeField]
        private PlayerEntity _playerEntity = null;

        [SerializeField]
        private Topview2DMovementFunc _moveFunc = null;

        [SerializeField]
        private Topview2DLookAtFunc _lookFunc = null;

        [SerializeField]
        private Transform _focusPoint = null;

        [Header("Properties")]
        [SerializeField]
        private float secondsBeforeRotateOnMove = 3f;

        #endregion

        #region Properties

        /// <summary>
        /// Entity ID.
        /// </summary>
        public string ID => _entityID.ID;

        /// <summary>
        /// Runtime entity component.
        /// </summary>
        public PlayerEntity Entity => _playerEntity;

        /// <summary>
        /// Move function.
        /// </summary>
        public IMovement2D MoveFunc => _moveFunc;

        /// <summary>
        /// Look function.
        /// </summary>
        public Topview2DLookAtFunc LookFunc => _lookFunc;

        /// <summary>
        /// Shot focus the camera at this point.
        /// </summary>
        public Transform FocusPoint => _focusPoint;

        /// <summary>
        /// Seconds delay to handle rotation using movement direction.
        /// </summary>
        public float SecondBeforeRotateOnMove => secondsBeforeRotateOnMove;

        #endregion
    }
}
