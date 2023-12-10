using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle checking hit box when a weapon hit any target.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public sealed class WeaponHitBox2DFunc : MonoBehaviour, IEntityID<string>
    {
        #region enum

        /// <summary>
        /// When will the weapon hit target?
        /// </summary>
        [System.Flags]
        private enum HitMode { None = 0, OnEnter = 1, OnStay = 2, OnExit = 4, }

        #endregion

        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private AttackStats _attackStats = null;

        // Runtime variable data.
        private HitpointStats _tempHP = null;
        private string _hitterID = string.Empty;

        #endregion

        #region Mono

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Check hitting a hitpoint of entity.
            if (collision.TryGetComponent(out _tempHP))
            {
                // Ignore self harm.
                if (_hitterID == _tempHP.EntityID)
                    return;

                // Give damage to target.
                _tempHP.TakeDamage(transform.right, _attackStats.AttackPointDamage, _hitterID);
            }
        }

        #endregion

        #region IOwnerID

        /// <summary>
        /// The hitter ID.
        /// </summary>
        public string EntityID
        {
            get => _hitterID;
            set => _hitterID = value;
        }

        #endregion
    }
}

