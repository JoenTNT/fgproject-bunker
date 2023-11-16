using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Weapon that can shoot bullets.
    /// </summary>
    public sealed class RangedWeapon : GenericWeapon, IRequiredInitialize
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private Shooter2DFunc _shooterFunc = null;

        [Header("Properties")]
        [SerializeField]
        private bool _disableOnStart = true;

        [Header("Optional")]
        [SerializeField]
        private RangedWeaponItemSO _weaponItemPreset = null;

        [SerializeField]
        private AudioSource _audioSource = null;

        [SerializeField]
        private GameObjectPool _ammoPool = null;

        // Runtime variable data.
        private GameObject _ammoObj = null;
        private PhysicalAmmo2DControl _ammo = null;
        private RuntimeAmmoInfo _ammoInfoRef = null;
        private RW_Mechanic _mechanic;
        private RW_Audio _audio;
        private RW_Reloader _reloader;
        private float _secondsBeforeShoot = 0f;
        private bool _isInit = false;
        private bool _isShooting = false;

        #endregion

        #region Mono

        private void Start()
        {
            // Initialize on start.
            Initialize();

            // Disable if needed on start.
            if (_disableOnStart) gameObject.SetActive(false);
        }

        private void Update()
        {
            // Check shooting begin.
            if (WeaponOwnerAdapter.IsInAction && !_isShooting)
            {
                _isShooting = true;
                _secondsBeforeShoot = _mechanic.FirstShotDelay;

                // Run audio if exists.
                _audio.PlayBeforeShotClip(_audioSource);
                return;
            }
            // Check shooting end.
            else if (!WeaponOwnerAdapter.IsInAction && _isShooting)
            {
                _isShooting = false;
                return;
            }

            // Check shooting control.
            if (!_isShooting) return;

            // Tick shoot seconds.
            _secondsBeforeShoot -= Time.deltaTime;

            // Check if shoot command must called.
            if (_secondsBeforeShoot > 0f) return;

            // Release shots per ammo.
            int pspa = _mechanic.PhysicalShotsPerAmmo;
            for (int i = 0; i < pspa; i++)
            {
                // Get bullet object from pool.
                _ammoObj = _ammoPool.GetObject();

                // Cancel shoot if there are invalid information.
                if (_ammoObj == null) return;
                if (!_ammoObj.TryGetComponent(out _ammo)) return;

                // Shoot command.
                _ammo.EntityID = WeaponOwnerAdapter.Owner;
                _shooterFunc.Shoot(_ammo);
            }

            // Run audio if exists.
            _audio.PlayOnShotClip(_audioSource);

            // Reset shoot timer.
            _secondsBeforeShoot = 1f / _mechanic.RoundPerSecond;
        }

        #endregion

        #region IRequiredInitialize

        public bool IsInitialized => _isInit;

        public void Initialize()
        {
            // Ignore if already initialized.
            if (_isInit) return;

            // Set initial weapon data if preset assigned on awake.
            if (_weaponItemPreset != null)
            {
                // Then change used bullet type.
                _ammoPool = GameObjectPoolManager.Instance.GetGameObjPool(_weaponItemPreset.AmmoType);

                // Access temporary data.
                _mechanic = _weaponItemPreset.Mechanic;
                _audio = _weaponItemPreset.Audio;
                _reloader = _weaponItemPreset.Reloader;

                // Assign components properties.
                _shooterFunc.ShootForce = _mechanic.ShootAmmoForce;
                _shooterFunc.AccuracyDegree = _mechanic.AccuracyDegree;
            }

            // Set status to initialized.
            _isInit = true;
        }

        #endregion

        #region Main

        /// <summary>
        /// Change weapon behaviour, even thought it's the same objek to reduce memory.
        /// Must be run after awake when ammunition pool has been initialized else where.
        /// </summary>
        /// <param name="weaponItem">Collected weapon item modifier</param>
        /// <param name="ammoInfo">Leftover ammunition reference, if not null then there's leftover</param>
        internal void ChangeWeaponBehaviour(RangedWeaponItemSO weaponItem, ref RuntimeAmmoInfo ammoInfo)
        {
            // Set weapon item preset.
            _weaponItemPreset = weaponItem;

            // Check leftover, if not then create one.
            if (ammoInfo == null && weaponItem != null)
                ammoInfo = _ammoInfoRef = weaponItem.AmmoInfoPreset.CreateRuntimeObject();
            else if (ammoInfo != null) // Set reference.
                _ammoInfoRef = ammoInfo;

            // Then reinitialize informations.
            _isInit = false;
            Initialize();
        }

        #endregion
    }
}
