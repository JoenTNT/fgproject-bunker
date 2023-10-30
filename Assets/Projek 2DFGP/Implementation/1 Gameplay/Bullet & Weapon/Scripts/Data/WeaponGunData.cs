using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle data for weapon gun.
    /// </summary>
    [System.Serializable]
    public sealed class WeaponGunData : IWeaponGunInfo, IRequiredInitialize
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private Shooter2DFunc _shooterFunc = null;

        [Header("Properties")]
        [SerializeField]
        private string _bulletType = string.Empty;

        [Header("Optional")]
        [SerializeField]
        private AmmoInfoPreset _ammoInfoPreset = null;

        [SerializeField]
        private ReloadTimeInfoPreset _reloadTimeInfo = null;

        [SerializeField]
        private Audio_RuntimeLogic _audio = null;

        [SerializeField]
        private GameObjectPool _bulletPool = null;

        // Runtime variable data.
        private RuntimeAmmoInfo _runtimeAmmoInfo = null;
        private RuntimeReloadInfo _runtimeReloadInfo = null;
        private bool _isInit = false;

        #endregion

        #region Properties

        /// <summary>
        /// Gun audio handler.
        /// </summary>
        public IRuntimeHandler AudioRuntime => _audio;

        /// <summary>
        /// Function to shoot an ammo.
        /// </summary>
        public IShootCommand<Bullet2DControl> ShooterFunc => _shooterFunc;

        /// <summary>
        /// Bullet pool reference.
        /// </summary>
        public GameObjectPool BulletPool
        {
            get => _bulletPool;
            internal set => _bulletPool = value;
        }

        /// <summary>
        /// The weapon is using the bullet type.
        /// </summary>
        public string BulletType => _bulletType;

        #endregion

        #region IWeaponGunInfo

        public IAmmo AmmoInfo => _runtimeAmmoInfo;

        public IReload ReloadInfo => _runtimeReloadInfo;

        #endregion

        #region IRequiredInitialize

        public bool IsInitialized => _isInit;

        public void Initialize()
        {
            // Ignore if already initialized.
            if (_isInit) return;

            // Create all runtime objects.
            _runtimeAmmoInfo = _ammoInfoPreset?.CreateRuntimeObject();
            _runtimeReloadInfo = _reloadTimeInfo?.CreateRuntimeObject();
            _isInit = true;
        }

        #endregion
    }
}