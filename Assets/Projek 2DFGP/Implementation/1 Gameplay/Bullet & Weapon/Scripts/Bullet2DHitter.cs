using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Generic bullet behaviour for 2D game.
    /// </summary>
    [RequireComponent(typeof(Bullet2DControl))]
    public sealed class Bullet2DHitter : MonoBehaviour
    {
        #region Variables

        [Header("Properties")]
        [SerializeField]
        private LayerMask _targetHit = ~0;

        [SerializeField]
        private float _collisionRadius = 0.2f;

        [SerializeField]
        private int _maxOverlapHit = 1;

        // Runtime variable data.
        private RaycastHit2D[] _hitCast = null;
        private int _hitCount = 0;

        #endregion

        #region Mono

        private void Awake()
        {
            // Initialize max cast hit.
            _hitCast = new RaycastHit2D[_maxOverlapHit];
        }

        #endregion

        #region Main

        public void OnDetectHit(Vector2 originPoint, Vector2 hitDir, float castLength)
        {
            // Find end point.
            Vector2 endPoint = originPoint + (hitDir * castLength);
#if UNITY_EDITOR
            Debug.DrawLine(originPoint, endPoint, Color.cyan, 3f);
            //Gizmos.color = Color.cyan;
            //Gizmos.DrawWireSphere(originPoint, _collisionRadius);
            //Gizmos.color = Color.white;
#endif
            // Start casting.
            float distance = (endPoint - originPoint).magnitude;
            _hitCount = Physics2D.CircleCastNonAlloc(originPoint, _collisionRadius, hitDir.normalized,
                _hitCast, distance, _targetHit);

            // Check hit count.
            if (_hitCount > 0)
            {
                // TODO: hit the target.
            }
        }

        #endregion
    }
}
