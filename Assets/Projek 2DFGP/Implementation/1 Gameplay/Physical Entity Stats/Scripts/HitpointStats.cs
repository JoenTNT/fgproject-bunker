using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Physical hitpoint stats.
    /// </summary>
    public sealed class HitpointStats : MonoBehaviour, IHitPoint<float>, IDamagable
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private EntityID _entityID = null;

        [Tooltip("Kill command in form of component.")]
        [SerializeField]
        private Component _killCommand = null;

        [Header("Properties")]
        [SerializeField]
        private string _useSplatter = string.Empty;

        [SerializeField, Min(0f)]
        private float _maxHP = 100f;

        [SerializeField, Min(0f)]
        private float _hp = 0f;

        [SerializeField]
        private bool _isDamageable = true;

        [Header("Optional")]
        [SerializeField]
        private UI_HitpointBar _healthBarRef = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventTwoStringFloat _onEntityGotDamageBy = null;

        [SerializeField]
        private GameEventVector2Float _onEntityHitOnTheSpot = null;

        [SerializeField]
        private GameEventStringVector2Float _onSplatterCallback = null;
#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField]
        private bool _debug = false;
#endif
        // Runtime variable data.
        private IKill _killRef = null;
        private IBarValue<float> _bar = null;

        #endregion

        #region Properties

        /// <summary>
        /// This entity ID.
        /// </summary>
        public string EntityID => _entityID.ID;

        #endregion

        #region Mono

        private void Awake() => _killRef = (IKill)_killCommand;

        private void Start()
        {
            // Internal injection.
            if (_healthBarRef != null)
            {
                _bar = _healthBarRef;
                Reset();
            }
        }

        public void Reset()
        {
            // Reset HP stats.
            _hp = _maxHP;

            // Sync with UI if already exists.
            if (_bar != null)
            {
                _bar.MaxBarValue = _maxHP;
                _bar.BarValue = _hp;
            }
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            // Kill command component must implement IKill.
            if (_killCommand is not IKill) _killCommand = null;

            // Check max limit HP.
            if (_hp > _maxHP) _hp = _maxHP;

            // Runtime only validation.
            if (!Application.isPlaying) return;

            // Sync HP with UI if already exists.
            if (_bar != null)
            {
                _bar.MaxBarValue = _maxHP;
                _bar.BarValue = _hp;
            }
        }

        private void OnGUI()
        {
            // Check debugging mode.
            if (!_debug) return;

            // Take damage button for debugging only.
            GUIStyle style = GUI.skin.button;
            if (GUI.Button(new Rect(16f, 144f, 256f, 36f), $"{name} Take Damage by 20", style))
                TakeDamage(Vector2.right, 20f);
        }
#endif
        #endregion

        #region IHitPoint

        public float MaxHP
        {
            get => _maxHP;
            set
            {
                _maxHP = value;
                if (_hp > _maxHP) _hp = _maxHP;
                if (_bar != null)
                {
                    _bar.MaxBarValue = _maxHP;
                    _bar.BarValue = _hp;
                }
            }
        }

        public float CurrentHP => _hp;

        public bool IsHPFull => _hp >= _maxHP;

        public void Heal(float hp)
        {
            // Add HP.
            _hp += hp;

            // Check max limit HP.
            if (_hp > _maxHP) _hp = _maxHP;
        }

        #endregion

        #region IDamagable

        public bool IsDamagable
        {
            get => _isDamageable;
            set => _isDamageable = value;
        }

        #endregion

        #region Main

        /// <summary>
        /// Give damage to this entity.
        /// </summary>
        /// <param name="damage">Hitpoint damage</param>
        /// <param name="by">Who hit the entity?</param>
        public void TakeDamage(Vector2 hitFromDirection, float damage, string by = null)
        {
            // Take damage on this entity.
            _hp -= damage;
            if (_hp < 0f)
            {
                damage += _hp;
                _hp = 0f;
            }

            // Check if entity got killed.
            if (_hp <= 0f) _killRef.Kill();

            // Call events.
            _onEntityGotDamageBy.Invoke(_entityID.ID, by, damage);
            _onEntityHitOnTheSpot.Invoke(transform.position, damage);
            if (_bar != null) _bar.BarValue = _hp;

            // Check if not using splatter effect.
            if (string.IsNullOrEmpty(_useSplatter)) return;

            // Call splatter event.
            _onSplatterCallback.Invoke(_useSplatter, transform.position,
                Mathf.Atan2(-hitFromDirection.y, hitFromDirection.x) * Mathf.Rad2Deg);
        }

        internal void InjectBarRef(IBarValue<float> bar)
        {
            _bar = bar;
            Reset();
        }

        #endregion
    }
}
