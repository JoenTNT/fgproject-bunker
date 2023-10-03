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

        [SerializeField]
        private GameEventString _onActionCommandInstantCallback = null;

        [SerializeField]
        private GameEventString _onActionCommandBegin = null;

        [SerializeField]
        private GameEventString _onActionCommandEnded = null;

        [SerializeField]
        private GameEventTwoStringUnityObject _equipWeaponCallback = null;

        // Runtime variable data
        private GameObject _nearestInteractionTarget = null;
        private InteractionMark _nearestInteractionMark = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events
            _onActionCommandInstantCallback.AddListener(ListenOnActionCommandInstantCallback);
            _onActionCommandBegin.AddListener(ListenOnActionCommandBegin);
            _onActionCommandEnded.AddListener(ListenOnActionCommandEnded);
            _equipWeaponCallback.AddListener(ListeonEquipWeaponCallback);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _onActionCommandInstantCallback.RemoveListener(ListenOnActionCommandInstantCallback);
            _onActionCommandBegin.RemoveListener(ListenOnActionCommandBegin);
            _onActionCommandEnded.RemoveListener(ListenOnActionCommandEnded);
            _equipWeaponCallback.RemoveListener(ListeonEquipWeaponCallback);
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
                //Debug.Log("[DEBUG] Enable Interaction Command Interface.");
#endif
                _openInteractionCommandCallback.Invoke(_data.ID, (int)_nearestInteractionMark.DisplayTypeInteraction);
            }
            else
            {
                if (_nearestInteractionMark == null) return;
#if UNITY_EDITOR
                //Debug.Log("[DEBUG] Disable Interaction Command Interface.");
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

        private void ListenOnActionCommandInstantCallback(string id)
        {
            // Check if this entity is not the target ID, then abort process.
            if (_data.ID != id) return;

            // Instant call action.
            _data.Weapon?.InstantUse();
        }

        private void ListenOnActionCommandBegin(string id)
        {
            // Check if this entity is not the target ID, then abort process.
            if (_data.ID != id) return;

            // Set weapon state to start aim.
            _data.Weapon?.StartAim();
        }

        private void ListenOnActionCommandEnded(string id)
        {
            // Check if this entity is not the target ID, then abort process.
            if (_data.ID != id) return;

            // Set weapon state to release.
            _data.Weapon?.Release();
        }

        private void ListeonEquipWeaponCallback(string id, string elementID, Object weapon)
        {
            // Check if this entity is not the target ID, then abort process.
            if (_data.ID != id) return;
            if (elementID != $"{PhysicalWeaponEquipment.WEAPON_ID_INDEX}0") return;

            // Check the weapon object.
            if (weapon is not GenericWeapon)
            {
                // Remove weapon if equipped.
                _data.Weapon = null;
                return;
            }

            // Proceed dependency injection.
            _data.Weapon = ((GenericWeapon)weapon).WeaponState;
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

        // Runtime variable data.
        private IWeaponActionState _weaponOnHand = null;

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
        /// This can be null because player may not equip any weapon.
        /// </summary>
        public IWeaponActionState Weapon
        {
            get => _weaponOnHand;
            set
            {
                // Unset old one.
                if (_weaponOnHand != null)
                    _weaponOnHand.OwnerOfState = string.Empty;

                // Set weapon ownership if not null.
                if (value != null)
                    value.OwnerOfState = _entityID.ID;
                _weaponOnHand = value;
            }
        }

        #endregion
    }
}
