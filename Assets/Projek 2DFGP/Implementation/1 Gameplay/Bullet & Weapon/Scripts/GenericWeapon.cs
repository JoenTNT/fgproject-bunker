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
        private WeaponDefaultStatsSO _weaponStats = null;

        [Header("Runtime Properties")]
        [SerializeField]
        private string _targetElementID = string.Empty;

        [SerializeField]
        private string _weaponKeyword = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Weapon state for all weapon.
        /// </summary>
        public IWeaponActionState WeaponState => _weaponState;

        /// <summary>
        /// Weapon stats design information.
        /// </summary>
        public WeaponDefaultStatsSO WeaponStats => _weaponStats;

        /// <summary>
        /// Target data element ID.
        /// </summary>
        protected string TargetElementID => _targetElementID;

        /// <summary>
        /// This weapon keyword.
        /// </summary>
        public string WeaponKeyword => _weaponKeyword;

        #endregion

        #region Main

        /// <summary>
        /// To send info target element ID.
        /// </summary>
        /// <param name="elementID">Target element ID</param>
        public void SetTargetElementID(string elementID) => _targetElementID = elementID;

        #endregion
    }
}