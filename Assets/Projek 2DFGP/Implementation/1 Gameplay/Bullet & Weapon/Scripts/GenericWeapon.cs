using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// For all kinds of weapon behaviour.
    /// </summary>
    [RequireComponent(typeof(WeaponOwnerAdapter))]
    public abstract class GenericWeapon : MonoBehaviour
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private WeaponOwnerAdapter _weaponOwnerAdapter = null;

        #endregion

        #region Properties

        /// <summary>
        /// Weapon state for all weapon.
        /// </summary>
        public WeaponOwnerAdapter WeaponOwnerAdapter => _weaponOwnerAdapter;

        #endregion
    }
}