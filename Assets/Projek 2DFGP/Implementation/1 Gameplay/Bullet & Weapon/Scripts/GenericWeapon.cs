using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// For all kinds of weapon behaviour.
    /// </summary>
    [RequireComponent(typeof(WeaponActionState))]
    public abstract class GenericWeapon : MonoBehaviour
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private WeaponActionState _weaponState = null;

        [SerializeField]
        private WeaponStatsSO _weaponStats = null;

        #endregion

        #region Properties

        /// <summary>
        /// Weapon state for all weapon.
        /// </summary>
        public IWeaponActionState WeaponState => _weaponState;

        /// <summary>
        /// Weapon stats design information.
        /// </summary>
        public WeaponStatsSO WeaponStats => _weaponStats;

        #endregion

        #region Main

        /// <summary>
        /// Set send info target element ID.
        /// </summary>
        /// <param name="elementID">Target element ID</param>
        public abstract void SetTargetElementID(string elementID);

        #endregion
    }
}