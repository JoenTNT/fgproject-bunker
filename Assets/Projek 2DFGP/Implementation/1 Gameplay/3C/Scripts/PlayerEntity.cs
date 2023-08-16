using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Player entity runtime main data.
    /// </summary>
    [RequireComponent(typeof(EntityID))]
    public class PlayerEntity : MonoBehaviour, IInteractable
    {
        #region Variable

        [Header("Properties")]
        [SerializeField]
        private PlayerEntityData _data = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventStringInt _openInteractionCommandCallback = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventString _onActionCommandBegin = null;

        [SerializeField]
        private GameEventString _onActionCommandEnded = null;

        // Temporary variable data
        private GameObject _nearestInteractionTarget = null;
        private InteractionMark _nearestInteractionMark = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Assign weapon ownership on awake.
            if (_data.Weapon != null) _data.Weapon = _data.Weapon;

            // Subscribe events
            _onActionCommandBegin.AddListener(ListenOnActionCommandBegin);
            _onActionCommandEnded.AddListener(ListenOnActionCommandEnded);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _onActionCommandBegin.RemoveListener(ListenOnActionCommandBegin);
            _onActionCommandEnded.RemoveListener(ListenOnActionCommandEnded);
        }

        private void Update()
        {
            // Handle interaction control
            _nearestInteractionTarget = _data.AreaOfInteraction.GetNearestObject(transform.position);

            if (_nearestInteractionTarget != null)
            {
                if (_nearestInteractionMark?.gameObject == _nearestInteractionTarget) return;
                if (!_nearestInteractionTarget.TryGetComponent(out _nearestInteractionMark)) return;
#if UNITY_EDITOR
                Debug.Log("Enable Interaction Command Interface.");
#endif
                _openInteractionCommandCallback.Invoke(_data.ID, (int)_nearestInteractionMark.DisplayTypeInteraction);
            }
            else
            {
                if (_nearestInteractionMark == null) return;
#if UNITY_EDITOR
                Debug.Log("Disable Interaction Command Interface.");
#endif
                _openInteractionCommandCallback.Invoke(_data.ID, (int)InteractionType.None);
                _nearestInteractionMark = null;
            }
        }

        #endregion

        #region IInteractable

        public bool Interact()
        {
            if (_nearestInteractionMark == null) return false;
            if (_nearestInteractionMark.IsLocked) return false;
#if UNITY_EDITOR
            Debug.Log($"Interact with {_nearestInteractionMark}");
#endif
            return _nearestInteractionMark.Interact(_data.ID);
        }

        #endregion

        #region Main

        private void ListenOnActionCommandBegin(string id)
        {
            // Check if this entity is not the target ID, then abort process.
            if (_data.ID != id) return;

            // Set weapon state to start aim.
            _data.Weapon.StartAim();
        }

        private void ListenOnActionCommandEnded(string id)
        {
            // Check if this entity is not the target ID, then abort process.
            if (_data.ID != id) return;

            // Set weapon state to release.
            _data.Weapon.Release();
        }

        #endregion
    }

    /// <summary>
    /// Handle data for player entity.
    /// </summary>
    [System.Serializable]
    internal class PlayerEntityData
    {
        #region Variable

        [Header("Requirements")]
        [SerializeField]
        private EntityID _entityID = null;

        [SerializeField]
        private InsideAreaObjectCollector2D _areaOfInteraction = null;

        [Header("Optional")]
        [SerializeField]
        private WeaponActionState _weaponOnHand = null;

        #endregion

        #region Properties

        /// <summary>
        /// Entity identifier.
        /// </summary>
        public string ID => _entityID.ID;

        /// <summary>
        /// Player interaction area.
        /// </summary>
        public InsideAreaObjectCollector2D AreaOfInteraction => _areaOfInteraction;

        /// <summary>
        /// Weapon on player's hand.
        /// </summary>
        public WeaponActionState Weapon
        {
            get => _weaponOnHand;
            set
            {
                // Set weapon ownership.
                _weaponOnHand.Owner = _entityID;
                _weaponOnHand = value;
            }
        }

        #endregion
    }
}
