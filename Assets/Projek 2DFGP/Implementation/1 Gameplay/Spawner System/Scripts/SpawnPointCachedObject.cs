using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// General point cache information.
    /// </summary>
    public abstract class SpawnPointCachedObject : MonoBehaviour
    {
        #region Variables

        [Header("Properties")]
        [SerializeField, Min(0f)]
        private float _pointRadius = 0f;
#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField]
        private bool _debug = false;
#endif
        #endregion

        #region Properties

        /// <summary>
        /// To check if point room full with cache.
        /// </summary>
        public abstract bool IsCacheFull { get; }

        #endregion
#if UNITY_EDITOR
        #region Mono

        private void OnDrawGizmosSelected()
        {
            if (!_debug) return;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _pointRadius);
            Gizmos.color = Color.white;
        }

        #endregion
#endif
        #region Main

        /// <summary>
        /// Cache a game object into this point.
        /// </summary>
        /// <param name="cache">Target cache.</param>
        public abstract void CacheObject(SpawnObjectCache cache);

        /// <summary>
        /// To get position by point position, extending by point radius area.
        /// </summary>
        public Vector2 GetRandomCirclePositionByRadius()
        {
            Vector2 pos = transform.position;
            return pos + (Random.insideUnitCircle * _pointRadius);
        }

        #endregion
    }
}
