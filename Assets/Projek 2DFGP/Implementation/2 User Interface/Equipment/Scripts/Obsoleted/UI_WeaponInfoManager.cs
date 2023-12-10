using JT.GameEvents;
using UnityEngine;
using UnityEngine.UI;

namespace JT.FGP
{
    /// <summary>
    /// Handle managing weapon information.
    /// </summary>
    [System.Obsolete]
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
        private Image _iconPlaceholder = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventString _onChangeWeaponCommand = null;

        [SerializeField]
        private GameEventTwoString _onEquipWeaponCommand = null;

        //[SerializeField]
        //private GameEventTwoStringUnityObject _equipWeaponCallback = null;

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
            //_equipWeaponCallback.AddListener(ListenEquipWeaponCallback);
        }

        private void OnDestroy()
        {
            // Unsubscribe events.
            _onChangeWeaponCommand.RemoveListener(ListenOnChangeWeaponCommand);
            //_equipWeaponCallback.RemoveListener(ListenEquipWeaponCallback);
        }

        #endregion

        #region Main

        private void ListenOnChangeWeaponCommand(string targetID)
        {
            // Forward to the physical weapon equipment.
            _onEquipWeaponCommand.Invoke(targetID, _elementID.ID);
        }

        //private void ListenEquipWeaponCallback(string ownerID, string elementID, Object weapon)
        //{
        //    // Check validation.
        //    if (elementID != _elementID.ID) return;
        //    if (weapon != null && weapon is not GenericWeapon) return;

        //    // Set target element for weapon.
        //    _weaponRef = (GenericWeapon)weapon;
        //    _weaponRef?.SetTargetInfoID(_elementID.ID);

        //    // Set icon if exists.
        //    if (_iconPlaceholder != null)
        //    {
        //        // Set sprite icon.
        //        Sprite s = _weaponRef == null ? null : _assetPack.GetIcon(_weaponRef.WeaponKeyword);
        //        _iconPlaceholder.sprite = s;

        //        // Set active icon object.
        //        _iconPlaceholder.gameObject.SetActive(s != null);
        //    }
        //}

        #endregion
    }
}
