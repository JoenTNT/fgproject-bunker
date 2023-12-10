using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Type of ranged weapon item.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Ranged Weapon Item",
        menuName = "FGP/Item/Ranged Weapon Item")]
    public sealed class RangedWeaponItemSO : ItemSO
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private RW_AmmoInfoSO _ammoInfoPreset = null;

        [Header("RW Properties")]
        [SerializeField]
        private string _ammoType = string.Empty;

        [SerializeField]
        private RW_Mechanic _mechanic = new RW_Mechanic();

        [SerializeField]
        private RW_Reloader _reloader = new RW_Reloader();

        [Header("Optional")]
        [SerializeField]
        private RW_Audio _audio = new RW_Audio();

        #endregion

        #region Properties

        /// <summary>
        /// Ammunition info preset.
        /// </summary>
        public RW_AmmoInfoSO AmmoInfoPreset => _ammoInfoPreset;

        /// <summary>
        /// Ranged weapon shooting mechanic information.
        /// </summary>
        public RW_Mechanic Mechanic => _mechanic;

        /// <summary>
        /// Reloading ranged weapon preset.
        /// </summary>
        public RW_Reloader Reloader => _reloader;

        /// <summary>
        /// To play ranged weapon audio SFX.
        /// </summary>
        public RW_Audio Audio => _audio;

        /// <summary>
        /// Ammunition type used for ranged weapon.
        /// </summary>
        public string AmmoType => _ammoType;

        #endregion
    }
}