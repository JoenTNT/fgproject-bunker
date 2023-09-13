using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Weapon that can shoot bullets.
    /// </summary>
    public sealed class WeaponGun : GenericWeapon, IReloadCommand
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
        private Shooter2DFunc _shooterFunc = null;

        [SerializeField]
        private WeaponGunData _data = new();

        [Header("Properties")]
        [SerializeField]
        private string _bulletType = string.Empty;

        [SerializeField]
        private Meta _meta = new Meta { shotsPerAmmo = 1, firstShootDelay = 0f, roundsPerSeconds = 20f, };

        [Header("Optional")]
        [SerializeField]
        private GameObjectPool _bulletPool = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventTwoString _requestBulletPoolCallback = null;

        [SerializeField]
        private GameEventStringUnityObject _assignBulletPoolCallback = null;

        [SerializeField]
        private GameEventStringTwoInt _onAmmoDataChange = null;

        [SerializeField]
        private GameEventStringFloat _onReloadingDataChange = null;

        [SerializeField]
        private GameEventString _onReloadCommand = null;

        // Runtime variable data.
        private GameObject _bulletObj = null;
        private Bullet2DControl _bullet = null;
        private float _secondsBeforeShoot = 0f;
        private bool _isShooting = false;

        #endregion

        #region Properties

        /// <summary>
        /// Amount of current ammo in bag.
        /// </summary>
        public int AmmoLeftoverInBag => _data.AmmoInBag;

        /// <summary>
        /// Amount of current ammo in weapon.
        /// </summary>
        public int AmmoLeftover => _data.Ammo;

        #endregion

        #region Mono

        private void Awake()
        {
            // Set initialize values for ammo information.
            if (_meta.InitAmmoOnStart)
            {
                // Initialize ammo value.
                _data.Ammo = _data.MaxAmmo;
                _data.AmmoInBag = _data.MaxAmmoInBag;
            }

            // Subscribe events
            _assignBulletPoolCallback.AddListener(ListenAssignBulletPoolCallback);
            _onReloadCommand.AddListener(ListenOnReloadCommand);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _assignBulletPoolCallback.RemoveListener(ListenAssignBulletPoolCallback);
            _onReloadCommand.RemoveListener(ListenOnReloadCommand);
        }

        private void Start()
        {
            // Request a bullet type on start.
            _requestBulletPoolCallback.Invoke(WeaponState.OwnerOfState, _bulletType);

            // Run after 2 frame of the game.
            System.Collections.IEnumerator WaitAnotherEndFrames()
            {
                // End of frame.
                yield return new WaitForEndOfFrame();

                // Send information changes to UI.
                _onAmmoDataChange.Invoke(TargetElementID, _data.Ammo, _data.AmmoInBag);

                // Send reloading timer data.
                _onReloadingDataChange.Invoke(TargetElementID, _data.ReloadTimeLeft);
            }
            StartCoroutine(WaitAnotherEndFrames());
        }

        private void OnEnable()
        {
            // Check if weapon immediately need to be reloaded.
            if (_data.IsReloading && _data.MaxAmmo != -1)
            {
                // Check if there is still ammo leftovers, then dont reload it immediately.
                if (_data.Ammo > 0)
                {
                    _data.ReloadTimeLeft = 0f;
                    return;
                }

                // Reset reload timer.
                _data.ResetTimer();
            }
        }

        private void Update()
        {
            // Check shooting begin.
            if (WeaponState.IsInAction && !_isShooting)
            {
                _isShooting = true;
                _secondsBeforeShoot = _meta.firstShootDelay;

                // Run audio if exists.
                if (_data.AudioRuntime != null)
                    _data.AudioRuntime.OnPreRuntime();
                return;
            }

            // Check shooting end.
            else if (!WeaponState.IsInAction && _isShooting)
            {
                _isShooting = false;

                // Run audio if exists.
                if (_data.AudioRuntime != null && !_data.IsReloading)
                    _data.AudioRuntime.OnPostRuntime();
                return;
            }

            // Check if reloading and not infinite ammo.
            if (_data.IsReloading && _data.MaxAmmo != -1)
            {
                // Tick reload timer.
                _data.ReloadTimeLeft -= Time.deltaTime;

                // Check reloading is finish.
                if (!_data.IsReloading) Reload();

                // Send reloading timer data.
                _onReloadingDataChange.Invoke(TargetElementID, _data.ReloadTimeLeft);

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
                    _bulletObj = _bulletPool.GetObject();

                    // Cancel shoot if there are invalid information.
                    if (_bulletObj == null) return;
                    if (!_bulletObj.TryGetComponent(out _bullet)) return;

                    // Shoot command.
                    _bullet.OwnerID = WeaponState.OwnerOfState;
                    _shooterFunc.Shoot(_bullet);
                }

                // Run audio if exists.
                if (_data.AudioRuntime != null)
                    _data.AudioRuntime.OnRuntimeLoop();

                // Send information changes to UI.
                _data.Ammo--;
                _onAmmoDataChange.Invoke(TargetElementID, _data.Ammo, _data.AmmoInBag);

                // Reset shoot timer.
                _secondsBeforeShoot = 1f / _meta.roundsPerSeconds;
            }

            // Check ammo is not yet empty, then dont reload yet.
            if (_data.Ammo > 0) return;

            // Check ammo bag is empty, then do not reload.
            if (_data.AmmoInBag <= 0) return;

            // Reset reload timer.
            _data.ResetTimer();

            // Run audio if exists.
            if (_data.AudioRuntime != null)
                _data.AudioRuntime.OnPostRuntime();
        }

        #endregion

        #region IReloadCommand

        public void Reload()
        {
            // Immediately fill in the ammo.
            int tempReloadAmount = _data.MaxAmmo - _data.Ammo;

            // Check non-infinite ammo in bag.
            if (_data.MaxAmmoInBag != -1)
            {
                _data.AmmoInBag -= tempReloadAmount;

                // Out of ammo in bag.
                if (_data.AmmoInBag < 0)
                {
                    // Get all leftovers.
                    tempReloadAmount += _data.AmmoInBag;

                    // Make sure the bag is empty, not minus.
                    _data.AmmoInBag = 0;
                }
            }

            // Add to new ammo.
            _data.Ammo += tempReloadAmount;

            // Update UI Ammo.
            _onAmmoDataChange.Invoke(TargetElementID, _data.Ammo, _data.AmmoInBag);
        }

        public void SkipReload()
        {
            // Skip reload now.
            _data.ReloadTimeLeft = 0f;

            // Send reloading timer data.
            _onReloadingDataChange.Invoke(TargetElementID, _data.ReloadTimeLeft);

            // Instant Reload.
            Reload();
        }

        public void CancelReload()
        {
            // Skip reload now.
            _data.ReloadTimeLeft = 0f;

            // Send reloading timer data.
            _onReloadingDataChange.Invoke(TargetElementID, _data.ReloadTimeLeft);
        }

        #endregion

        #region Main

        private void ListenAssignBulletPoolCallback(string entityID, Object bulletPool)
        {
            // Validate information.
            if (entityID != WeaponState.OwnerOfState) return;
            if (bulletPool is not GameObjectPool) return;

            // Assign bullet pool.
            _bulletPool = (GameObjectPool)bulletPool;
        }

        private void ListenOnReloadCommand(string entityID)
        {
            // Validate information.
            if (entityID != WeaponState.OwnerOfState) return;

            // Start reloading.
            _data.ResetTimer();
        }

        #endregion
    }
}
