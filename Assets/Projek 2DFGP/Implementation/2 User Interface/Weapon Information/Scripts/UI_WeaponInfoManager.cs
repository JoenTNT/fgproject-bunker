using JT.GameEvents;
using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField]
        private WeaponAssetSO _assetPack = null;

        [Header("Optional")]
        [SerializeField]
        private UI_WeaponGunInfo _weaponGunInfo = null;

        [SerializeField]
        private UI_WeaponReloadTimerInfo _reloadTimer = null;

        [SerializeField]
        private Image _iconPlaceholder = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventString _onChangeWeaponCommand = null;

        [SerializeField]
        private GameEventTwoString _onEquipWeaponCommand = null;

        [SerializeField]
        private GameEventStringTwoInt _onAmmoDataChange = null;

        [SerializeField]
        private GameEventTwoStringUnityObject _equipWeaponCallback = null;

        [SerializeField]
        private GameEventStringFloat _onReloadingDataChange = null;

        // Runtime variable data.
        private GenericWeapon _weaponRef = null;

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
            _onAmmoDataChange.AddListener(ListenOnAmmoDataChange);
            _equipWeaponCallback.AddListener(ListenEquipWeaponCallback);
            _onReloadingDataChange.AddListener(ListenOnReloadingDataChange);
        }

        private void OnDestroy()
        {
            // Unsubscribe events.
            _onChangeWeaponCommand.RemoveListener(ListenOnChangeWeaponCommand);
            _onAmmoDataChange.RemoveListener(ListenOnAmmoDataChange);
            _equipWeaponCallback.RemoveListener(ListenEquipWeaponCallback);
            _onReloadingDataChange.RemoveListener(ListenOnReloadingDataChange);
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

        private void ListenOnAmmoDataChange(string elementID, int ammo, int ammoInBag)
        {
            // Check element ID matches, if not then abort process.
            if (_elementID.ID != elementID) return;

            // Set information.
            _weaponGunInfo.SetInfo(ammo, ammoInBag);
        }

        private void ListenEquipWeaponCallback(string ownerID, string elementID, Object weapon)
        {
            // Check validation.
            if (elementID != _elementID.ID) return;
            if (weapon is not GenericWeapon && weapon != null) return;

            // Set target element for weapon.
            _weaponRef = (GenericWeapon)weapon;
            _weaponRef?.SetTargetElementID(_elementID.ID);

            // Set icon if exists.
            if (_iconPlaceholder != null)
            {
                // Set sprite icon.
                _iconPlaceholder.sprite = _weaponRef == null ? null
                    : _assetPack.GetIcon(_weaponRef.WeaponKeyword);

                // Set active icon object.
                _iconPlaceholder.gameObject.SetActive(_iconPlaceholder.sprite != null);
            }

            // Open weapon informations.
            _weaponGunInfo.enabled = _weaponRef is WeaponGun;
            if (_weaponGunInfo.enabled)
            {
                // Set information immediately.
                var tempGunInfo = (WeaponGun)_weaponRef;
                _weaponGunInfo.SetInfo(tempGunInfo.AmmoLeftover, tempGunInfo.AmmoLeftoverInBag);
            }

            // TEMP: Always disable everytime changing weapon.
            if (_reloadTimer != null)
                _reloadTimer.gameObject.SetActive(false);
        }

        private void ListenOnReloadingDataChange(string elementID, float reloadTimer)
        {
            // Check element ID matches, if not then abort process.
            if (_elementID.ID != elementID) return;

            // Change display timer.
            _reloadTimer?.SetDisplayTimer(reloadTimer);
        }

        #endregion
    }
}
