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
        private GameEventString _onMoveInputBegin = null;

        [SerializeField]
        private GameEventStringVector2 _onMoveInput = null;

        [SerializeField]
        private GameEventString _onMoveInputRelease = null;

        [SerializeField]
        private GameEventString _onLookInputBegin = null;

        [SerializeField]
        private GameEventStringVector2 _onLookPositionInput = null;

        [SerializeField]
        private GameEventStringVector2 _onLookDirectionInput = null;

        [SerializeField]
        private GameEventString _onLookInputRelease = null;

        [SerializeField]
        private GameEventString _onInteractCommand = null;

        [SerializeField]
        private GameEventString _onActionCommandBegin = null;

        [SerializeField]
        private GameEventString _onActionCommandEnded = null;

        [SerializeField]
        private GameEventTransform _setFollowTargetCallback = null;

        // Runtime variable data.
        private InsideAreaObjectCollector2D _tempSightRef = null;
        private IDirectionBaseMovement2D _dirBaseMove = null;
        private DampingRotation2DFunc _tempSightRot = null;
        private PlayerEntity _entityTarget = null;
        private PlayerEntityData _entityData = null;
        private GameObject _nearestTarget = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Get player entity component immediately.
            TryGetComponent(out _entityTarget);

            // Subscribe events
            _onMoveInputBegin.AddListener(ListenOnMoveInputBegin);
            _onMoveInput.AddListener(ListenOnMoveInput);
            _onMoveInputRelease.AddListener(ListenOnMoveInputRelease);
            _onLookInputBegin.AddListener(ListenOnLookInputBegin);
            _onLookPositionInput.AddListener(ListenOnLookPositionInput);
            _onLookDirectionInput.AddListener(ListenOnLookDirectionInput);
            _onLookInputRelease.AddListener(ListenOnLookInputRelease);
            _onInteractCommand.AddListener(ListenOnInteractCommand);
            _onActionCommandBegin.AddListener(ListenOnActionCommandBegin);
            _onActionCommandEnded.AddListener(ListenOnActionCommandEnded);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _onMoveInputBegin.RemoveListener(ListenOnMoveInputBegin);
            _onMoveInput.RemoveListener(ListenOnMoveInput);
            _onMoveInputRelease.RemoveListener(ListenOnMoveInputRelease);
            _onLookInputBegin.RemoveListener(ListenOnLookInputBegin);
            _onLookPositionInput.RemoveListener(ListenOnLookPositionInput);
            _onLookDirectionInput.RemoveListener(ListenOnLookDirectionInput);
            _onLookInputRelease.RemoveListener(ListenOnLookInputRelease);
            _onInteractCommand.RemoveListener(ListenOnInteractCommand);
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
            // Put temporary references.
            _tempSightRef = _entityData.AreaOfWeaponSight;
            _tempSightRot = _data.RotateFunc;

            // Handle movement
            if (_data.IsMoving)
            {
                // Do movement update.
                _data.MoveFunc.OnMove();

                // Check rotation is not being controlled, then control by movement.
                if (!_data.IsLookingAround)
                {
                    _tempSightRot.SetTargetLookDirection(_data.MoveFunc.Velocity);
                    _tempSightRot.OnRotate();
                }
            }

            // Using weapon to auto aim.
            if (_data.IsUsingWeapon && _tempSightRef.HasObject)
            {
                _nearestTarget = _tempSightRef.GetNearestObject(transform.position);
                _tempSightRot.SetInstantLookAtPosition(_nearestTarget.transform.position);
                return;
            }
        }

        #endregion

        #region Main

        private void ListenOnMoveInputBegin(string id)
        {
            // Validate ID.
            if (_entityData.ID != id) return;

            // Set state.
            _data.IsMoving = true;
        }

        private void ListenOnMoveInput(string id, Vector2 moveDir)
        {
            // Validate ID.
            if (_entityData.ID != id) return;

            // Do direction base movement.
            if (_data.MoveFunc is IDirectionBaseMovement2D)
                ((IDirectionBaseMovement2D)_data.MoveFunc).SetMoveDirection(moveDir);
        }

        private void ListenOnMoveInputRelease(string id)
        {
            // Validate ID.
            if (_entityData.ID != id) return;

            // Stop velocity and set state to released.
            if (_data.MoveFunc is IDirectionBaseMovement2D)
                ((IDirectionBaseMovement2D)_data.MoveFunc).SetMoveDirection(Vector2.zero);
            _data.MoveFunc.OnMove();
            _data.IsMoving = false;
        }

        private void ListenOnLookInputBegin(string id)
        {
            // Validate ID.
            if (_entityData.ID != id) return;

            // Set state.
            _data.IsLookingAround = true;
        }

        // For mouse controller, look position is the mouse cursor.
        private void ListenOnLookPositionInput(string id, Vector2 lookPos)
        {
            // Validate ID.
            if (_entityData.ID != id) return;

            _data.RotateFunc.SetInstantLookAtPosition(lookPos);
        }

        // For mobile joystick controller, look direction is joystick drag direction.
        private void ListenOnLookDirectionInput(string id, Vector2 lookDir)
        {
            // Validate ID.
            if (_entityData.ID != id) return;

            // Set looking around status.
            _data.RotateFunc.SetInstantLookDirection(lookDir);
        }

        private void ListenOnLookInputRelease(string id)
        {
            // Validate ID.
            if (_entityData.ID != id) return;

            // Set state.
            _data.IsLookingAround = false;
        }

        private void ListenOnInteractCommand(string id)
        {
            // Validate ID.
            if (_entityData.ID != id) return;

            // Do Interact.
            _entityTarget.Interact();
        }

        private void ListenOnActionCommandBegin(string id)
        {
            // Check if this entity is not the target ID, then abort process.
            if (_entityData.ID != id) return;

            // Set weapon state to start aim.
            _entityData.Weapon?.StartAim();
            _data.IsUsingWeapon = true;
        }

        private void ListenOnActionCommandEnded(string id)
        {
            // Check if this entity is not the target ID, then abort process.
            if (_entityData.ID != id) return;

            // Set weapon state to release.
            _entityData.Weapon?.Release();
            _data.IsUsingWeapon = false;
        }

        internal void InjectEntityData(PlayerEntityData entityData) => _entityData = entityData;

        #endregion
    }
}
