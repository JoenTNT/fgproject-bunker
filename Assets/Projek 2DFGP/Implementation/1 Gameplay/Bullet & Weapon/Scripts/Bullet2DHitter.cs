using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Generic bullet behaviour for 2D game.
    /// </summary>
    [RequireComponent(typeof(Bullet2DControl))]
    public sealed class Bullet2DHitter : MonoBehaviour, IDamagable, IRequiredReset
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private Bullet2DControl _controller = null;

        [SerializeField]
        private AttackStats _attackStats = null;

        [Header("Properties")]
        [SerializeField]
        private LayerMask _targetHit = ~0;

        [SerializeField]
        private CollisionDetectionMode2D _collisionDetection = CollisionDetectionMode2D.Discrete;

        [SerializeField]
        private float _collisionRadius = 0.2f;

        [SerializeField]
        private int _maxTargetHit = 1;

        [SerializeField]
        private int _maxOverlapHit = 1;

        [SerializeField]
        private bool _doNotDamage = false;

        // Runtime variable data.
        private HitpointStats _tempHP = null;
        private RaycastHit2D[] _hitCast = null;
        private int _targetHitCountLeft = 0;
        private int _hitCount = 0;

        #endregion

        #region Mono

        private void Awake()
        {
            // Initialize max cast hit.
            _hitCast = new RaycastHit2D[_maxOverlapHit];
        }

        #region IRequiredReset

        public void Reset()
        {
            // Reset overlap hit counter.
            _targetHitCountLeft = _maxTargetHit;
        }

        #endregion
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(255, 165, 0);
            Gizmos.DrawWireSphere(transform.position, _collisionRadius);
            Gizmos.color = Color.white;
        }
#endif
        #endregion

        #region IDamagable

        public bool IsDamagable
        {
            get => _doNotDamage;
            set => _doNotDamage = value;
        }

        #endregion

        #region Main

        public void OnDetectHit(Vector2 originPoint, Vector2 hitDir, float castLength)
        {
            // Find end point.
            Vector2 endPoint = originPoint + (hitDir * castLength);
#if UNITY_EDITOR
            //Debug.Log($"Origin: {originPoint}; End Point: {endPoint}; Cast Length: {castLength}; Direction: {hitDir}");
            //float r = Random.Range(100, 256);
            //Random.InitState(System.DateTime.Now.Millisecond);
            //float g = Random.Range(100, 255);
            //Random.InitState(System.DateTime.Now.Millisecond);
            //float b = Random.Range(100, 255);
            //Debug.DrawLine(originPoint, endPoint, new Color(r, g, b), 3f, false);
            //UnityEditor.EditorApplication.isPaused = true;
            //Vector2 leftDir = new Vector2(-hitDir.x, hitDir.y), rightDir = new Vector2(hitDir.x, -hitDir.y);
            //Debug.DrawRay(originPoint, leftDir * _collisionRadius, Color.cyan, 3f, false);
            //Debug.DrawRay(originPoint, rightDir * _collisionRadius, Color.cyan, 3f, false);
#endif
            // Start casting.
            float distance = (endPoint - originPoint).magnitude;
            _hitCount = Physics2D.CircleCastNonAlloc(originPoint, _collisionRadius, hitDir.normalized,
                _hitCast, distance, _targetHit);

            // Check hit count, if not hit then abort the process.
            if (_hitCount <= 0) return;

            // Check all hits.
            for (int i = 0; i < _hitCount; i++)
            {
                // TODO: How to hit the wall.
                // Check if entity or obstacle has HP stats when hit.
                if (_hitCast[i].collider.TryGetComponent(out _tempHP))
                {
#if UNITY_EDITOR
                    Debug.Log($"Entity that got Hit is {_hitCast[i].collider.transform.parent.name}");
#endif
                    // Check if bullet hit the player self, then ignore it.
                    if (_tempHP.EntityID == _controller.ShooterID)
                        continue;

                    // Give attack damage at other entity.
                    _tempHP.TakeDamage(hitDir, _attackStats.AttackPointDamage, _controller.ShooterID);
                }

                // Countdown target hit left.
                _targetHitCountLeft--;

                // Check for last target hit.
                if (_targetHitCountLeft <= 0) break;
            }

            // Disable after the amount of overlap hit.
            if (_targetHitCountLeft <= 0)
                gameObject.SetActive(false);
        }

        #endregion
    }
}
