using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle data for weapon gun.
    /// </summary>
    [System.Serializable]
    public sealed class WeaponGunData : IAmmo, IReload
    {
        #region Variables

        [Header("Properties")]
        [SerializeField, Min(-1)]
        private int _maxAmmoInBag = -1; // Minus one means infinite.

        [SerializeField, Min(-1)]
        private int _maxAmmo = -1; // Minus one means infinite.

        [SerializeField, Min(0f)]
        private float _reloadTime = 3f;

        [Header("Optional")]
        [SerializeField]
        private Audio_RuntimeLogic _audio = null;

        // Runtime variable data.
        private int _ammoInBag = 0;
        private int _ammoLeft = 0;
        private float _reloadTimeLeft = 0f;

        #endregion

        #region Properties

        /// <summary>
        /// Gun audio handler.
        /// </summary>
        public IRuntimeHandler AudioRuntime => _audio;

        /// <summary>
        /// Amount of runtime current ammo in bag.
        /// </summary>
        public int AmmoInBag
        {
            get => _ammoInBag;
            set => _ammoInBag = value;
        }

        /// <summary>
        /// Current reloading time in seconds.
        /// </summary>
        public float ReloadTimeLeft
        {
            get => _reloadTimeLeft;
            set => _reloadTimeLeft = value;
        }

        #endregion

        #region IAmmo

        public int MaxAmmoInBag => _maxAmmoInBag;

        public int MaxAmmo => _maxAmmo;

        public int Ammo
        {
            get => _ammoLeft;
            set => _ammoLeft = value;
        }

        #endregion

        #region IReload

        public float SecondsReload
        {
            get => _reloadTime;
            set => _reloadTime = value;
        }

        public bool IsReloading => _reloadTimeLeft > 0f;

        #endregion

        #region Main

        /// <summary>
        /// Reset timer for reload.
        /// </summary>
        internal void ResetTimer()
        {
            // Reset reload time.
            _reloadTimeLeft = _reloadTime;
        }

        #endregion
    }
}