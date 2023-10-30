using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Weapon that can shoot bullets.
    /// </summary>
    public sealed class WeaponGun : GenericWeapon, IReloadCommand, IWeaponGunInfo
    {
        #region structs

        /// <summary>
        /// Contains meta data for weapon gun type.
        /// </summary>
        [System.Serializable]
        private struct Meta
        {
            #region Variables

            [Tooltip("In data used one ammo, this is the physical bullets per ammo used.")]
            public float shotsPerAmmo;
            public float firstShootDelay;
            public float roundsPerSeconds;

            [SerializeField]
            private bool _initAmmoOnStart;

            #endregion

            #region Properties

            /// <summary>
            /// Does the weapon must be loaded on start?
            /// </summary>
            public bool InitAmmoOnStart => _initAmmoOnStart;

            #endregion
        }

        #endregion

        #region Variables

        [Header("Weapon Requirements")]
        [SerializeField]
        private WeaponGunData _data = new();

        [SerializeField]
        private Meta _meta = new Meta {
            shotsPerAmmo = 1, 
            firstShootDelay = 0f,
            roundsPerSeconds = 20f,
        };

        [Header("Game Events")]
        [SerializeField]
        private GameEventString _onReloadCommand = null;

        //[SerializeField]
        //private GameEventTwoString _requestBulletPoolCallback = null;

        //[SerializeField]
        //private GameEventStringUnityObject _assignBulletPoolCallback = null;

        // Runtime variable data.
        private GameObject _bulletObj = null;
        private Bullet2DControl _bullet = null;
        private float _secondsBeforeShoot = 0f;
        private bool _isShooting = false;

        #endregion

        #region Mono

        private void Awake()
        {
            // Create runtime data.
            _data.Initialize();

            // Set initialize values for ammo information.
            if (_meta.InitAmmoOnStart)
            {
                _data.AmmoInfo.ResetAmmoBag();
                _data.AmmoInfo.ResetAmmo();
            }

            // Subscribe events
            //_assignBulletPoolCallback.AddListener(ListenAssignBulletPoolCallback, this);
            _onReloadCommand.AddListener(ListenOnReloadCommand, this);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            //_assignBulletPoolCallback.RemoveListener(ListenAssignBulletPoolCallback, this);
            _onReloadCommand.RemoveListener(ListenOnReloadCommand, this);
        }

        private void Start()
        {
            // Assign bullet pool.
            _data.BulletPool = GameObjectPoolManager.Instance.GetPool(_data.BulletType);

            //// Request a bullet type on start.
            //_requestBulletPoolCallback.Invoke(WeaponOwnerAdapter.Owner, _data.BulletType, this);
        }

        private void OnEnable()
        {
            // Check if weapon immediately need to be reloaded.
            if (_data.ReloadInfo.IsReloading && _data.AmmoInfo.MaxAmmo != -1)
            {
                // Check if there is still ammo leftovers, then dont reload it immediately.
                if (_data.AmmoInfo.Ammo > 0)
                {
                    CancelReload();
                    return;
                }

                // Reset reload timer.
                _data.ReloadInfo.ResetReloadTime();
            }
        }

        private void Update()
        {
            // Check shooting begin.
            if (WeaponOwnerAdapter.IsInAction && !_isShooting)
            {
                _isShooting = true;
                _secondsBeforeShoot = _meta.firstShootDelay;

                // Run audio if exists.
                if (_data.AudioRuntime != null)
                    _data.AudioRuntime.OnPreRuntime();
                return;
            }

            // Check shooting end.
            else if (!WeaponOwnerAdapter.IsInAction && _isShooting)
            {
                _isShooting = false;

                // Run audio if exists.
                if (_data.AudioRuntime != null && !_data.ReloadInfo.IsReloading)
                    _data.AudioRuntime.OnPostRuntime();
                return;
            }

            // Check if reloading and not infinite ammo.
            if (_data.ReloadInfo.IsReloading && _data.AmmoInfo.MaxAmmo != -1)
            {
                // Tick reload timer.
                _data.ReloadInfo.OnReloading();

                // Check reloading is finish.
                if (!_data.ReloadInfo.IsReloading) Reload();

                // Do not run the shooting method while reloading.
                return;
            }

            // Check shooting control.
            if (!_isShooting) return;

            // Tick shoot seconds.
            _secondsBeforeShoot -= Time.deltaTime;

            // Check if shoot command must called.
            if (_secondsBeforeShoot <= 0f)
            {
                // Release shots per ammo.
                for (int i = 0; i < _meta.shotsPerAmmo; i++)
                {
                    // Get bullet object from pool.
                    _bulletObj = _data.BulletPool.GetObject();

                    // Cancel shoot if there are invalid information.
                    if (_bulletObj == null) return;
                    if (!_bulletObj.TryGetComponent(out _bullet)) return;

                    // Shoot command.
                    _bullet.OwnerID = WeaponOwnerAdapter.Owner;
                    _data.ShooterFunc.Shoot(_bullet);
                }

                // Run audio if exists.
                if (_data.AudioRuntime != null)
                    _data.AudioRuntime.OnRuntimeLoop();

                // Send information changes to UI.
                _data.AmmoInfo.UseAmmoInBarrel(1);

                // Reset shoot timer.
                _secondsBeforeShoot = 1f / _meta.roundsPerSeconds;
            }

            // Check ammo is not yet empty, then dont reload yet.
            if (_data.AmmoInfo.Ammo > 0) return;

            // Check ammo bag is empty, then do not reload.
            if (_data.AmmoInfo.AmmoInBag <= 0) return;

            // Reset reload timer.
            _data.ReloadInfo.ResetReloadTime();

            // Run audio if exists.
            if (_data.AudioRuntime != null)
                _data.AudioRuntime.OnPostRuntime();
        }

        #endregion

        #region IReloadCommand

        public void Reload() => _data.AmmoInfo.TransferAmmo();

        public void SkipReload()
        {
            // Skip reload immediately.
            _data.ReloadInfo.SkipReload();

            // Instant Reload.
            Reload();
        }

        public void CancelReload() => _data.ReloadInfo.SkipReload();

        #endregion

        #region IWeaponGunInfo

        public IAmmo AmmoInfo => _data.AmmoInfo;

        public IReload ReloadInfo => _data.ReloadInfo;

        #endregion

        #region Main

        //private void ListenAssignBulletPoolCallback(string entityID, Object bulletPool)
        //{
        //    // Validate information.
        //    if (entityID != WeaponOwnerAdapter.Owner) return;
        //    if (bulletPool is not GameObjectPool) return;

        //    // Assign bullet pool.
        //    _data.BulletPool = (GameObjectPool)bulletPool;
        //}

        private void ListenOnReloadCommand(string entityID)
        {
            // Validate information.
            if (entityID != WeaponOwnerAdapter.Owner) return;

            // Start reloading.
            _data.ReloadInfo.ResetReloadTime();
        }

        #endregion
    }
}
