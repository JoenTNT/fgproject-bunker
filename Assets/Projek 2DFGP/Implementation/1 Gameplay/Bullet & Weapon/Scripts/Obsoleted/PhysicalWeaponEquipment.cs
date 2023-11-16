using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// A bag to hold weapons in physical inventory.
    /// </summary>
    [System.Obsolete]
    public class PhysicalWeaponEquipment : MonoBehaviour
    {
        #region Variables

        /// <summary>
        /// Standard weapon ID index.
        /// </summary>
        public const string WEAPON_ID_INDEX = "WP";

        [Header("Requirements")]
        [SerializeField]
        private EntityID _ownerID = null;

        [Header("Optional")]
        [SerializeField]
        private GenericWeapon[] _weaponHolds = new GenericWeapon[3];

        [Header("Game Events")]
        [SerializeField]
        private GameEventTwoString _onEquipWeaponCommand = null;

        [SerializeField]
        private GameEventTwoStringUnityObject _equipWeaponCallback = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events.
            _onEquipWeaponCommand.AddListener(ListenOnEquipWeaponCommand, this);
        }

        private void OnDestroy()
        {
            // Unsubscribe events.
            _onEquipWeaponCommand.RemoveListener(ListenOnEquipWeaponCommand, this);
        }

        private void Start()
        {
            // Invoke initial info for each weapon.
            for (int i = 0; i < _weaponHolds.Length; i++)
            {
                _equipWeaponCallback.Invoke(_ownerID.ID, $"{WEAPON_ID_INDEX}{i}", _weaponHolds[i], this);
                if (i != 0) _weaponHolds[i]?.gameObject.SetActive(false);
            }
        }

        #endregion

        #region Main

        private void ListenOnEquipWeaponCommand(string ownerID, string elementID)
        {
            // Check validation.
            if (_ownerID.ID != ownerID) return;

            // TEMP: Check fixed selected element.
            if (elementID == $"{WEAPON_ID_INDEX}1")
            {
                // Swap index element between 1 and zero.
                var temp = _weaponHolds[0];
                _weaponHolds[0] = _weaponHolds[1];
                _weaponHolds[1] = temp;

                // Set active weapon.
                _weaponHolds[0]?.gameObject.SetActive(true);
                _weaponHolds[1]?.gameObject.SetActive(false);

                // Send equipments information callback.
                _equipWeaponCallback.Invoke(_ownerID.ID, $"{WEAPON_ID_INDEX}0", _weaponHolds[0]);
                _equipWeaponCallback.Invoke(_ownerID.ID, $"{WEAPON_ID_INDEX}1", _weaponHolds[1]);
            }
            else if (elementID == $"{WEAPON_ID_INDEX}2")
            {
                // Swap index element between 2 and zero.
                var temp = _weaponHolds[0];
                _weaponHolds[0] = _weaponHolds[2];
                _weaponHolds[2] = temp;

                // Set active weapon.
                _weaponHolds[0]?.gameObject.SetActive(true);
                _weaponHolds[2]?.gameObject.SetActive(false);

                // Send equipments information callback.
                _equipWeaponCallback.Invoke(_ownerID.ID, $"{WEAPON_ID_INDEX}0", _weaponHolds[0]);
                _equipWeaponCallback.Invoke(_ownerID.ID, $"{WEAPON_ID_INDEX}2", _weaponHolds[2]);
            }
        }

        #endregion
    }
}
