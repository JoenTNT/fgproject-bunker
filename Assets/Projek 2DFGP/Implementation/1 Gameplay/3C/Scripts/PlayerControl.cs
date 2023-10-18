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
        private GameEventString _onReleaseLookAround = null;

        [SerializeField]
        private GameEventString _onInteractCommand = null;

        [SerializeField]
        private GameEventTransform _setFollowTargetCallback = null;

        [SerializeField]
        private GameEventString _onActionCommandBegin = null;

        [SerializeField]
        private GameEventString _onActionCommandEnded = null;

        // Runtime variable data.
        private PlayerEntity _entityTarget = null;
        private PlayerEntityData _entityData = null;
        private GameObject _nearestTarget = null;
        private bool _isUsingWeapon = false;

        #endregion

        #region Mono

        private void Awake()
        {
            // Get player entity component immediately.
            TryGetComponent(out _entityTarget);

            // Subscribe events
            _onMoveInput.AddListener(ListenOnMoveInput);
            _onLookPositionInput.AddListener(ListenOnLookPositionInput);
            _onLookDirectionInput.AddListener(ListenOnLookDirectionInput);
            _onInteractCommand.AddListener(ListenOnInteractCommand);
            _onReleaseLookAround.AddListener(ListenOnReleaseLookAround);
            _onActionCommandBegin.AddListener(ListenOnActionCommandBegin);
            _onActionCommandEnded.AddListener(ListenOnActionCommandEnded);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _onMoveInput.RemoveListener(ListenOnMoveInput);
            _onLookPositionInput.RemoveListener(ListenOnLookPositionInput);
            _onLookDirectionInput.RemoveListener(ListenOnLookDirectionInput);
            _onInteractCommand.RemoveListener(ListenOnInteractCommand);
            _onReleaseLookAround.RemoveListener(ListenOnReleaseLookAround);
            _onActionCommandBegin.RemoveListener(ListenOnActionCommandBegin);
            _onActionCommandEnded.RemoveListener(ListenOnActionCommandEnded);
        }

        private void Start()
        {
            // TEMPORARY: Calibrate camera to main character
            _setFollowTargetCallback.Invoke(_data.CameraFocusPoint);
        }

        private void Update()
        {
            // Handle movement
            _data.MoveFunc.Move();

            // Using weapon to auto aim.
            if (_isUsingWeapon && _entityData.AreaOfWeaponSight.HasObject)
            {
                _nearestTarget = _entityData.AreaOfWeaponSight.GetNearestObject(transform.position);
                _data.LookFunc.LookAtPosition(_nearestTarget.transform.position);
                return;
            }

            // TEMPORARY: Ignore Look Rotation at Zero Move Direction
            // TODO: Fix to Remove this Issue.
            // Check if currently not looking around action.
            if (_data.MoveFunc.Direction != Vector2.zero)
                _data.LookFunc.LookAtDirection(_data.MoveFunc.Direction);
        }

        #endregion

        #region Main

        private void ListenOnMoveInput(string id, Vector2 moveDir)
        {
#if UNITY_EDITOR
            //Debug.Log($"{_data.ID} != {id} is {_data.ID != id}");
#endif
            if (_entityData.ID != id) return;

            _data.MoveFunc.Direction = moveDir;
#if UNITY_EDITOR
            //Debug.Log($"Assign Move Direction: {_moveDir}");
#endif
        }

        // For mouse controller, look position is the mouse cursor.
        private void ListenOnLookPositionInput(string id, Vector2 lookPos)
        {
            // Validate ID.
            if (_entityData.ID != id) return;

            _data.LookFunc.LookAtPosition(lookPos);
        }

        // For mobile joystick controller, look direction is joystick drag direction.
        private void ListenOnLookDirectionInput(string id, Vector2 lookDir)
        {
            // Validate ID.
            if (_entityData.ID != id) return;

            // Set looking around status.
            _data.LookFunc.LookAtDirection(lookDir);
        }

        private void ListenOnInteractCommand(string id)
        {
            // Validate ID.
            if (_entityData.ID != id) return;

            _entityTarget.Interact();
        }

        private void ListenOnReleaseLookAround(string id)
        {
            // Validate ID.
            if (_entityData.ID != id) return;

            // Special Case, set move to look direction for runtime update.
            _data.MoveFunc.Direction = Vector2.zero;
            _data.LookFunc.LookAtDirection(_data.LookFunc.LookDirection);
        }

        private void ListenOnActionCommandBegin(string id)
        {
            // Check if this entity is not the target ID, then abort process.
            if (_entityData.ID != id) return;

            // Set weapon state to start aim.
            _entityData.Weapon?.StartAim();
            _isUsingWeapon = true;
        }

        private void ListenOnActionCommandEnded(string id)
        {
            // Check if this entity is not the target ID, then abort process.
            if (_entityData.ID != id) return;

            // Set weapon state to release.
            _entityData.Weapon?.Release();
            _isUsingWeapon = false;
        }

        internal void InjectEntityData(PlayerEntityData entityData) => _entityData = entityData;

        #endregion
    }
}
