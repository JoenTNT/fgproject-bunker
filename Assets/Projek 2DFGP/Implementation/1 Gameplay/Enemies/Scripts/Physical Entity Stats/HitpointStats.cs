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

        [Header("Properties")]
        [SerializeField, Min(1f)]
        private float _maxHP = 100f;

        [SerializeField]
        private bool _isDamageable = true;

        [Header("Optional")]
        [SerializeField]
        private Splatter2DFXControl _splatter = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventTwoStringFloat _onEntityGotDamageBy = null;

        // Runtime variable data.
        private float _hp = 0f;

        #endregion

        #region Properties

        /// <summary>
        /// This entity ID.
        /// </summary>
        public string EntityID => _entityID.ID;

        #endregion

        #region IHitPoint

        public float MaxHP => _maxHP;

        public float CurrentHP => _hp;

        public void Heal(float hp)
        {
            // TODO: Heal.
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
            // TODO: Hit me baby one more time.
            Debug.Log($"Hit from Direction {hitFromDirection}; With Damage {damage}; By {by}");

            // Emit splatter effect if exists.
            if (_splatter != null)
                _splatter.Splat(Mathf.Atan2(-hitFromDirection.y, hitFromDirection.x) * Mathf.Rad2Deg);
        }

        #endregion
    }
}
