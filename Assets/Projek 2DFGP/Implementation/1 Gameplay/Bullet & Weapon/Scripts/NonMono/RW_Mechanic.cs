using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Contains meta data for weapon gun type.
    /// </summary>
    [System.Serializable]
    public struct RW_Mechanic
    {
        #region Variables

        [Header("Properties")]
        [Tooltip("In data used one ammo, this is the physical bullets per ammo used.")]
        [SerializeField, Min(0)]
        private int _physicalShotsPerAmmo;

        [SerializeField, Min(0)]
        private float _firstShotDelay;

        [SerializeField, Min(1)]
        private float _roundPerSecond;

        [SerializeField]
        private float _shootAmmoForce;

        [SerializeField, Min(0f)]
        private float _accuracyDegree;

        [SerializeField]
        private bool _initAmmoOnStart;

        #endregion

        #region Properties

        /// <summary>
        /// Amount of ammo used in one shot.
        /// </summary>
        public int PhysicalShotsPerAmmo => _physicalShotsPerAmmo;

        /// <summary>
        /// Wait time before ranged weapon first shot.
        /// </summary>
        public float FirstShotDelay => _firstShotDelay;

        /// <summary>
        /// Wait time before next shot, this must be run in loop update.
        /// </summary>
        public float RoundPerSecond => _roundPerSecond;

        /// <summary>
        /// Physical ammo shoot push power/force.
        /// </summary>
        public float ShootAmmoForce => _shootAmmoForce;

        /// <summary>
        /// The higher value the more unaccurate shots.
        /// </summary>
        public float AccuracyDegree => _accuracyDegree;

        /// <summary>
        /// Does the weapon must be loaded on start?
        /// </summary>
        public bool InitAmmoOnStart => _initAmmoOnStart;

        #endregion
    }
}