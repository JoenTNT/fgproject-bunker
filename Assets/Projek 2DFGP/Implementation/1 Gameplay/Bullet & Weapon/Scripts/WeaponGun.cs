using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Weapon that can shoot bullets.
    /// </summary>
    public sealed class WeaponGun : GenericWeapon
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private GameObjectPoolFactory _bulletFactory = null;

        [Header("Properties")]
        [SerializeField]
        private float firstShootDelay = 0f;

        [SerializeField]
        private float _roundsPerSeconds = 20f;

        // Runtime variable data.
        private float _secondsBeforeShoot = 0f;
        private bool _isShooting = false;

        #endregion

        #region Mono

        private void Update()
        {
            if (WeaponState.IsInAction && !_isShooting)
            {
                _isShooting = true;
                _secondsBeforeShoot = firstShootDelay;
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

        #region Mono

        public void Shoot()
        {
            Debug.Log("Shoot!");
        }

        #endregion
    }
}
