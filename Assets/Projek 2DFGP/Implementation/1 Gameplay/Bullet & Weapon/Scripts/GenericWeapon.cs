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

        #endregion

        #region Properties

        /// <summary>
        /// Weapon state for all weapon.
        /// </summary>
        public IWeaponActionState WeaponState => _weaponState;

        #endregion
    }
}