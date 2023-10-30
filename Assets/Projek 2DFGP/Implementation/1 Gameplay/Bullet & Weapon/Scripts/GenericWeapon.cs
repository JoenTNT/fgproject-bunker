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

        //[SerializeField]
        //private WeaponDefaultStatsSO _weaponStats = null;

        [Header("Runtime Properties")]
        [SerializeField]
        private string _targetInfoID = string.Empty;

        [SerializeField]
        private string _weaponKeyword = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Weapon state for all weapon.
        /// </summary>
        public WeaponOwnerAdapter WeaponOwnerAdapter => _weaponOwnerAdapter;

        ///// <summary>
        ///// Weapon stats design information.
        ///// </summary>
        //public WeaponDefaultStatsSO WeaponStats => _weaponStats;

        /// <summary>
        /// Target data element ID.
        /// </summary>
        protected string TargetInfoID => _targetInfoID;

        /// <summary>
        /// This weapon keyword.
        /// </summary>
        public string WeaponKeyword => _weaponKeyword;

        #endregion

        #region Main

        /// <summary>
        /// To send info target ID.
        /// </summary>
        /// <param name="infoID">Target info ID</param>
        public void SetTargetInfoID(string infoID) => _targetInfoID = infoID;

        #endregion
    }
}