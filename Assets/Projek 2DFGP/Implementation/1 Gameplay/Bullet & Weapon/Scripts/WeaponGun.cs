using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Weapon that can shoot bullets.
    /// </summary>
    public sealed class WeaponGun : GenericWeapon, IShootCommand
    {
        #region Variables

        // Requirements.
        [SerializeField]
        private Transform _shootPoint = null;

        [Header("Properties")]
        [SerializeField]
        private string _bulletType = string.Empty;

        [SerializeField]
        private float _shootForce = 100f; // Unit per second.

        [SerializeField]
        private float _firstShootDelay = 0f;

        [SerializeField]
        private float _roundsPerSeconds = 20f;

        [SerializeField]
        private bool _initBulletOnStart = true;

        [Header("Optional")]
        [SerializeField]
        private GameObjectPool _bulletPool = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventTwoString _requestBulletPoolCallback = null;

        [SerializeField]
        private GameEventStringUnityObject _assignBulletPoolCallback = null;

        // Runtime variable data.
        private GameObject _bulletObj = null;
        private Bullet2DControl _bullet = null;
        private float _secondsBeforeShoot = 0f;
        private bool _isShooting = false;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events
            _assignBulletPoolCallback.AddListener(ListenAssignBulletPoolCallback);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _assignBulletPoolCallback.RemoveListener(ListenAssignBulletPoolCallback);
        }

        private void Start()
        {
            // Request a bullet type on start.
            if (_initBulletOnStart)
                _requestBulletPoolCallback.Invoke(HolderID, _bulletType);
        }

        private void Update()
        {
            if (WeaponState.IsInAction && !_isShooting)
            {
                _isShooting = true;
                _secondsBeforeShoot = _firstShootDelay;
                return;
            }
            else if (!WeaponState.IsInAction && _isShooting)
            {
                _isShooting = false;
                return;
            }

            if (_isShooting)
            {
                _secondsBeforeShoot -= Time.deltaTime;

                if (_secondsBeforeShoot <= 0f)
                {
                    Shoot();

                    _secondsBeforeShoot = 1f / _roundsPerSeconds;
                }
            }
        }

        #endregion

        #region IShootCommand

        public void Shoot()
        {
#if UNITY_EDITOR
            Debug.Log("Shoot!");
#endif
            // Get bullet object from pool.
            _bulletObj = _bulletPool.GetObject();

            // Cancel shoot if there are invalid information.
            if (_bulletObj == null) return;
            if (!_bulletObj.TryGetComponent(out _bullet)) return;

            // Calculate facing forward of the weapon and then shoot the bullet.
            _bullet.transform.position = _shootPoint.position;
            _bullet.Shoot(transform.right, _shootForce);
        }

        #endregion

        #region Main

        private void ListenAssignBulletPoolCallback(string entityID, Object bulletPool)
        {
            // Validate information.
            if (entityID != HolderID) return;
            if (bulletPool is not GameObjectPool) return;

            // Assign bullet pool.
            _bulletPool = (GameObjectPool)bulletPool;
        }

        #endregion
    }
}
