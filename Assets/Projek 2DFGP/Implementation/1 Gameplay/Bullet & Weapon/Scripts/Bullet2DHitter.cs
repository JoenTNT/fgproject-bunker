#if FMOD
using FMODUnity;
#endif
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Generic bullet behaviour for 2D game.
    /// </summary>
    [RequireComponent(typeof(Bullet2DControl))]
    public sealed class Bullet2DHitter : MonoBehaviour, IDamagable, IRequiredReset
    {
        #region events

        // TODO: Call bounce bullet when hit something.
        /// <summary>
        /// Event called when bullet is bouncing target.
        /// </summary>
        //public event System.Action BounceCallback;

        #endregion

        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private Bullet2DControl _controller = null;

        [SerializeField]
        private AttackStats _attackStats = null;

        [Header("Properties")]
        [SerializeField]
        private LayerMask _targetHit = ~0;

        [Tooltip("This will ignore the bullet to hit the target with tag.")]
        [SerializeField]
        private string[] _ignoreTags = new string[0];

        // TODO: When hitting this layer target, then destroy bullet immediately.
        //[SerializeField]
        //private LayerMask _selfDestroyTargetHit = ~0;

        //[SerializeField]
        //private CollisionDetectionMode2D _collisionDetection = CollisionDetectionMode2D.Discrete;

        [SerializeField]
        private float _collisionRadius = 0.2f;

        [SerializeField]
        private int _maxTargetHit = 1;

        [SerializeField]
        private int _maxOverlapHit = 1;

        [SerializeField]
        private bool _doNotDamage = false;
#if FMOD
        [Header("Optional")]
        [SerializeField]
        private StudioEventEmitter _hitEmitter = null;
#endif
        // Runtime variable data.
        private HitpointStats _tempHP = null;
        private RaycastHit2D[] _hitCast = null;
        private int _targetHitCountLeft = 0;
        private int _hitCount = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Has done hitting all the targets.
        /// </summary>
        public bool IsDone => _targetHitCountLeft <= 0;

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

        /// <summary>
        /// Handle detect any hit in front while bullet is moving by velocity.
        /// </summary>
        /// <param name="originPoint">Origin bullet position before moving to next point</param>
        /// <param name="tickVelocity">Velocity of bullet</param>
        public void OnDetectHit(Vector2 originPoint, Vector2 tickVelocity)
        {
            // Disable after the amount of overlap hit.
            if (IsDone)
            {
                // While playing the sound, don't disable yet.
                if (_hitEmitter != null && _hitEmitter.IsPlaying())
                {
                    // Disable sprite only instead.
                    if (_controller.SpriteEnabled)
                        _controller.SpriteEnabled = false;
                    return;
                }

                // Finally disable bullet.
                gameObject.SetActive(false);
                return;
            }

            // Enable sprite if not yet enabled.
            if (!_controller.SpriteEnabled) _controller.SpriteEnabled = true;

            // Find end point.
            Vector2 endPoint = originPoint + tickVelocity;
#if UNITY_EDITOR
            //Debug.DrawLine(originPoint, endPoint, Color.yellow, 2.0f);
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
            _hitCount = Physics2D.CircleCastNonAlloc(originPoint, _collisionRadius, tickVelocity,
                _hitCast, distance, _targetHit);

            // Check hit count, if not hit then abort the process.
            if (_hitCount <= 0) return;

            // Check all hits.
            GameObject obj;
            for (int i = 0; i < _hitCount; i++)
            {
                // Get game object.
                obj = _hitCast[i].collider.gameObject;

                // Ignore target with tags.
                bool isIgnored = false;
                for (int j = 0; j < _ignoreTags.Length; j++)
                {
                    if (obj.CompareTag(_ignoreTags[j]))
                    {
                        isIgnored = true;
                        break;
                    }
                }
                if (isIgnored) continue;
#if UNITY_EDITOR
                //Debug.Log($"Entity that got Hit is {obj}", obj);
#endif
                // TODO: How to hit the wall.
                // Check if entity or obstacle has HP stats when hit.
                if (obj.TryGetComponent(out _tempHP))
                {
                    // Ignore self hit.
                    if (_tempHP.EntityID == _controller.OwnerID) continue;

                    // Give attack damage at other entity.
                    _tempHP.TakeDamage(tickVelocity, _attackStats.AttackPointDamage, _controller.OwnerID);
#if FMOD
                    // Run sound if exists.
                    // TODO: Change sound parameter when hit something else.
                    if (_hitEmitter != null)
                        _hitEmitter.Play();
#endif
                }

                // Countdown target hit left.
                _targetHitCountLeft--;

                // Check for last target hit.
                if (_targetHitCountLeft <= 0) break;
            }
        }

        #endregion
    }
}
