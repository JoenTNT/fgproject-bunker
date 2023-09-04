using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle managing weapon information.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class UI_WeaponInfoManager : MonoBehaviour
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private EntityID _elementID = null;

        [Header("Optional")]
        [SerializeField]
        private UI_WeaponGunInfo _weaponGunInfo = null;

        [SerializeField]
        private UI_WeaponReloadTimer _reloadTimer = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventString _onChangeWeaponCommand = null;

        [SerializeField]
        private GameEventTwoString _onEquipWeaponCommand = null;

        [SerializeField]
        private GameEventTwoStringUnityObject _equipWeaponCallback = null;

        #endregion

        #region Properties

        /// <summary>
        /// Element ID info.
        /// </summary>
        public string ElementID => _elementID.ID;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events.
            _onChangeWeaponCommand.AddListener(ListenOnChangeWeaponCommand);
            _equipWeaponCallback.AddListener(ListenEquipWeaponCallback);
        }

        private void OnDestroy()
        {
            // Unsubscribe events.
            _onChangeWeaponCommand.RemoveListener(ListenOnChangeWeaponCommand);
            _equipWeaponCallback.RemoveListener(ListenEquipWeaponCallback);
        }

        #endregion

        #region Main

        private void ListenOnChangeWeaponCommand(string targetID)
        {
#if UNITY_EDITOR
            Debug.Log($"[DEBUG] {targetID} => {_elementID.ID}");
#endif
            // Forward to the physical weapon equipment.
            _onEquipWeaponCommand.Invoke(targetID, _elementID.ID);
        }

        private void ListenEquipWeaponCallback(string ownerID, string elementID, Object weapon)
        {
            // Check validation.
            if (elementID != _elementID.ID) return;

            // Set target element for weapon.
            if (weapon is GenericWeapon)
                ((GenericWeapon)weapon).SetTargetElementID(_elementID.ID);

            // Open weapon informations.
            _weaponGunInfo.enabled = weapon is WeaponGun;
            if (_weaponGunInfo.enabled)
            {
                // Set information immediately.
                var tempGunInfo = (WeaponGun)weapon;
                _weaponGunInfo.SetInfo(tempGunInfo.AmmoLeftover, tempGunInfo.AmmoLeftoverInBag);
            }
        }

        #endregion
    }
}
