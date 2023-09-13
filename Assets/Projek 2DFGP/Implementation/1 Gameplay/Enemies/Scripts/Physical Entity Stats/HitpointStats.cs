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

        [SerializeField]
        private KillCommand _killCommand = null;

        [Header("Properties")]
        [SerializeField]
        private string _useSplatter = string.Empty;

        [SerializeField, Min(0f)]
        private float _maxHP = 100f;

        [SerializeField, Min(0f)]
        private float _hp = 0f;

        [SerializeField]
        private bool _isDamageable = true;

        [Header("Game Events")]
        [SerializeField]
        private GameEventTwoStringFloat _onEntityGotDamageBy = null;

        [SerializeField]
        private GameEventVector2Float _onEntityHitOnTheSpot = null;

        [SerializeField]
        private GameEventStringVector2Float _onSplatterCallback = null;
        
        #endregion

        #region Properties

        /// <summary>
        /// This entity ID.
        /// </summary>
        public string EntityID => _entityID.ID;

        #endregion

        #region Mono
#if UNITY_EDITOR
        private void OnValidate()
        {
            // Check max limit HP.
            if (_hp > _maxHP)
                _hp = _maxHP;
        }
#endif
        #endregion

        #region IHitPoint

        public float MaxHP => _maxHP;

        public float CurrentHP => _hp;

        public void Heal(float hp)
        {
            // Add HP.
            _hp += hp;

            // Check max limit HP.
            if (_hp > _maxHP)
                _hp = _maxHP;
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
            if (_hp <= 0f)
                _killCommand.Kill();

            // Call events.
            _onEntityGotDamageBy.Invoke(_entityID.ID, by, damage);
            _onEntityHitOnTheSpot.Invoke(transform.position, damage);

            // Check if not using splatter effect.
            if (string.IsNullOrEmpty(_useSplatter)) return;

            // Call splatter event.
            _onSplatterCallback.Invoke(_useSplatter, transform.position,
                Mathf.Atan2(-hitFromDirection.y, hitFromDirection.x) * Mathf.Rad2Deg);
        }

        #endregion
    }
}
