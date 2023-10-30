using System.Collections;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Physical short range weapon.
    /// </summary>
    public class WeaponMelee : GenericWeapon, IPowerCharge, IInteruptable
    {
        #region enum

        /// <summary>
        /// Define swing type of melee weapon.
        /// </summary>
        [System.Flags]
        public enum SwingType : short
        {
            LeftToRight = 1,
            RightToLeft = 2,
            Stab = 4,
        }

        #endregion

        #region struct

        /// <summary>
        /// Infomation on how does the swinging works.
        /// </summary>
        [System.Serializable]
        private struct SwingMeta
        {
            public SwingType swingType;
            public SwingType firstMove;
            public int comboHit;
            public float degreeSwing;
            public float swingInterval;
            public bool randomSwing;
        }

        #endregion

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
        private SwingMeta _meta = new SwingMeta {
            swingType = SwingType.LeftToRight | SwingType.RightToLeft,
            firstMove = SwingType.RightToLeft,
            comboHit = 1,
            degreeSwing = 150f,
            swingInterval = 0.25f,
            randomSwing = false,
        };

        [SerializeField]
        private float _fullChargeInSecond = 0.5f;

        [SerializeField]
        private bool _isInteruptable = false;

        [Header("Optional")]
        [SerializeField]
        private SpriteRenderer _placeholderHint = null;

        // Runtime variable data.
        private SwingType _nextSwing = SwingType.LeftToRight;
        private IEnumerator _swingRoutine = null;
        private float _currentDegreeSwing = 0f;
        private float _powerReleasePercent = 0f;
        private float _fullChargeIn = 0f;
        private bool _isAnticipate = false;

        #endregion

        #region Properties

        /// <summary>
        /// Swing type meta.
        /// </summary>
        public SwingType SwingT
        {
            get => _meta.swingType;
            set => _meta.swingType = value;
        }

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
            // Set first move.
            _nextSwing = _meta.firstMove;

            // Close placeholder on start.
            if (_placeholderHint != null)
                _placeholderHint.enabled = false;

            // Deactivate melee on start.
            _meleeSprite.enabled = _meleeCollider.enabled = false;
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

                // Check if the routine not yet finish
                // TODO: Do swing immediately if interupted.
                if (_swingRoutine != null && !_isInteruptable) return;

                // Create new routine or start over swing.
                _swingRoutine = SwingRoutine();
                StartCoroutine(_swingRoutine);
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
            // Check if the routine not yet finish
            // TODO: Do swing immediately if interupted.
            if (_swingRoutine != null && !_isInteruptable) return;

            // Always set owner of weapon if not yet assigned.
            if (string.IsNullOrEmpty(_weaponHitBox.OwnerID) || _weaponHitBox.OwnerID != WeaponOwnerAdapter.Owner)
                _weaponHitBox.OwnerID = WeaponOwnerAdapter.Owner;

            // Create new routine or start over swing.
            _swingRoutine = SwingRoutine();
            StartCoroutine(_swingRoutine);
        }

        private IEnumerator SwingRoutine()
        {
            // Activate melee.
            _meleeSprite.enabled = _meleeCollider.enabled = true;

            // Randomize if needed.
            if (_meta.randomSwing)
            {
                // Randomize swing value.
                float randVal = Random.Range(1f, Mathf.Log((float)_meta.swingType, 2f));

                // Round result value.
                int roundVal = Mathf.RoundToInt(randVal) - 1;

                // Get result into swing type.
                _nextSwing = (SwingType)(int)(Mathf.Pow(2f, roundVal));
            }

            // Initialize starting values.
            float startDegree = -_meta.degreeSwing / 2f;
            float endDegree = -startDegree;
            float percentToEnd = 0f, tempSecToEnd = _meta.swingInterval;

            // Run swinging process.
            do
            {
                // Check swing type.
                switch (_nextSwing)
                {
                    // Swing from left to right.
                    case SwingType.LeftToRight:
                        _currentDegreeSwing = Mathf.Lerp(endDegree, startDegree, percentToEnd);
                        break;

                    // Swing from right to left.
                    case SwingType.RightToLeft:
                        _currentDegreeSwing = Mathf.Lerp(startDegree, endDegree, percentToEnd);
                        break;

                    default: // Stab
                        _currentDegreeSwing = 0f; // Just put it in front of the character.
                        break;
                }

                // Asign swing degree to pivot point.
                _swingPivot.localRotation = Quaternion.Euler(0f, 0f, _currentDegreeSwing);

                // Skip one frame.
                yield return null;

                // Tick the result.
                tempSecToEnd -= Time.deltaTime;
                percentToEnd = 1f - (tempSecToEnd / _meta.swingInterval);
            } while (tempSecToEnd > 0f);

            // Activate melee.
            _meleeSprite.enabled = _meleeCollider.enabled = false;

            // End of process.
            _swingRoutine = null;
        }

        #endregion
    }
}
