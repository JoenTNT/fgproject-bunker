using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Physical short range weapon.
    /// </summary>
    public class MeleeWeapon : GenericWeapon, IPowerCharge, IInteruptable
    {
        #region Variables

        [Header("Weapon Requirements")]
        [SerializeField]
        private Transform _swingPivot = null;

        [SerializeField]
        private WeaponHitBox2DFunc _weaponHitBox = null;

        [SerializeField]
        private SpriteRenderer _meleeSprite = null;

        [SerializeField]
        private Collider2D _meleeCollider = null;

        [Header("Properties")]
        [SerializeField]
        private float _fullChargeInSecond = 0.5f;

        [SerializeField]
        private bool _isInteruptable = false;

        [SerializeField]
        private bool _disableOnStart = true;

        [Header("Optional")]
        private MW_AttackMetaSO _meta = null;

        [SerializeField]
        private SpriteRenderer _placeholderHint = null;

        // Runtime variable data.
        private float _currentDegreeSwing = 0f;
        private float _powerReleasePercent = 0f;
        private float _fullChargeIn = 0f;
        private bool _isAnticipate = false;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events.
            WeaponOwnerAdapter.OnInstantUse += ListenOnInstantUse;
        }

        private void OnDestroy()
        {
            // Unsubscribe events.
            WeaponOwnerAdapter.OnInstantUse -= ListenOnInstantUse;
        }

        private void Start()
        {
            // Close placeholder on start.
            if (_placeholderHint != null)
                _placeholderHint.enabled = false;

            // Deactivate melee on start.
            _meleeSprite.enabled = _meleeCollider.enabled = false;

            // Disable if needed on start.
            if (_disableOnStart) gameObject.SetActive(false);
        }

        private void Update()
        {
            // Check anticipation begin.
            if (WeaponOwnerAdapter.IsInAction && !_isAnticipate)
            {
                // Open placeholder.
                if (_placeholderHint != null)
                    _placeholderHint.enabled = true;

                // Reset full power charge.
                _fullChargeIn = _fullChargeInSecond;

                // Start anticipating.
                _isAnticipate = true;
            }

            // Check release action.
            else if (!WeaponOwnerAdapter.IsInAction && _isAnticipate)
            {
                // Stop anticipating and do an action.
                _isAnticipate = false;

                // Close placeholder.
                if (_placeholderHint != null)
                    _placeholderHint.enabled = false;

                //// Check if the routine not yet finish
                //// TODO: Do swing immediately if interupted.
                //if (_swingRoutine != null && !_isInteruptable) return;

                //// Create new routine or start over swing.
                //_swingRoutine = SwingRoutine();
                //StartCoroutine(_swingRoutine);
            }

            // Ignore the rest of the process if not anticipating.
            if (!_isAnticipate) return;

            // Charging power.
            OnCharge();
        }

        #endregion

        #region IPowerCharge

        public float PercentPower => !_isAnticipate ? 0f : _powerReleasePercent;

        public void OnCharge()
        {
            // Full charge tick.
            _fullChargeIn -= Time.deltaTime;

            // Calculate power percent.
            _powerReleasePercent = _fullChargeIn / _fullChargeInSecond;
        }

        #endregion

        #region IInteruptable

        public bool IsInteruptable
        {
            get => _isInteruptable;
            set => _isInteruptable = value;
        }

        #endregion

        #region Main

        private void ListenOnInstantUse()
        {
            //// Check if the routine not yet finish
            //// TODO: Do swing immediately if interupted.
            //if (_swingRoutine != null && !_isInteruptable) return;

            //// Always set owner of weapon if not yet assigned.
            //if (string.IsNullOrEmpty(_weaponHitBox.OwnerID) || _weaponHitBox.OwnerID != WeaponOwnerAdapter.Owner)
            //    _weaponHitBox.OwnerID = WeaponOwnerAdapter.Owner;

            //// Create new routine or start over swing.
            //_swingRoutine = SwingRoutine();
            //StartCoroutine(_swingRoutine);
        }

        #endregion
    }
}
