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
        private GameEventTwoStringInt _notifyInteractionHintCallback = null;

        [SerializeField]
        private GameEventTwoStringUnityObject _equipWeaponCallback = null;

        // Runtime variable data
        private GameObject _nearestDetectedInteractable = null;
        private InteractableComponent _nearestInteractable = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Inject data to controller.
            if (TryGetComponent(out PlayerControl controller))
                controller.InjectEntityData(_data);

            // Subscribe events
            _equipWeaponCallback.AddListener(ListeonEquipWeaponCallback);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _equipWeaponCallback.RemoveListener(ListeonEquipWeaponCallback);
        }

        private void Update()
        {
            // Handle interaction control
            _nearestDetectedInteractable = _data.AreaOfInteraction.GetNearestObject(transform.position);

            if (_nearestDetectedInteractable != null)
            {
                if (_nearestInteractable?.gameObject == _nearestDetectedInteractable) return;
                if (!_nearestDetectedInteractable.TryGetComponent(out _nearestInteractable)) return;

                _notifyInteractionHintCallback.Invoke(_data.ID, _nearestInteractable.UniqueID,
                    (int)_nearestInteractable.Type);
            }
            else
            {
                if (_nearestInteractable == null) return;

                _notifyInteractionHintCallback.Invoke(_data.ID, null, (int)InteractionType.None);
                _nearestInteractable = null;
            }
        }

        #endregion

        #region IInteractable

        public bool Interact()
        {
            if (_nearestInteractable == null) return false;
#if UNITY_EDITOR
            Debug.Log($"[DEBUG] Interact with {_nearestInteractable}");
#endif
            return _nearestInteractable.Interact(_data.ID);
        }

        #endregion

        #region Main

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
}
