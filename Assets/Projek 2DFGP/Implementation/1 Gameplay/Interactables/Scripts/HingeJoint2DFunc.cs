using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Making hinge joint 2D.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class HingeJoint2DFunc : MonoBehaviour
    {
        #region Variable

        [Header("Properties")]
        [SerializeField, Min(0.001f)]
        private float _mass = 1f;

        [SerializeField]
        private Vector2 _anchorOffset = Vector2.zero;

        [SerializeField, Min(0f)]
        private float _limitDegree = 120f;

        // Runtime variable data.
        private ContactPoint2D _tempCP;
        private Vector2 _connectedAnchor = Vector2.zero;

        #endregion

        #region Mono

        private void Start()
        {
            // Set connected anchor.
            Vector2 jointPos = transform.position;
            _connectedAnchor = jointPos + _anchorOffset;
        }

        private void LateUpdate()
        {
            // Always set connected anchor.
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Rigidbody2D rb;
            Vector2 velocity, normalVel, hitPointPos, forceDir, torqueDir;
            float torque;

            Vector2 jointPos = transform.position;
            Vector2 worldAnchor = jointPos + _anchorOffset;
            for (int i = 0; i < collision.contactCount; i++)
            {
                rb = collision.rigidbody;
                _tempCP = collision.GetContact(i);
                velocity = _tempCP.relativeVelocity;
                hitPointPos = _tempCP.point;
                Debug.DrawRay(hitPointPos, velocity, Color.green, 3f);
                Debug.DrawRay(hitPointPos, _tempCP.normal, Color.cyan, 3f);
                normalVel = velocity * _tempCP.normal;
                forceDir = rb.mass * (normalVel / Time.deltaTime);
                torqueDir = forceDir * (worldAnchor - hitPointPos);
                torque = Mathf.Atan2(torqueDir.y, torqueDir.x);
                Debug.Log($"ForceD: {forceDir}; TorqueD: {torqueDir}; Torque: {torque}");
            }
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            // Limit degree must not be greater than 360.
            if (_limitDegree > 359.99f)
                _limitDegree = 359.99f;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Vector2 currentPos = transform.position;
            Gizmos.DrawWireSphere(currentPos + _anchorOffset, 1f);

            Vector2 defaultDir = transform.right;
            float halfDegree = _limitDegree / 2f;
            Debug.DrawRay(transform.position, RotateDirByDegree(defaultDir, halfDegree), Color.cyan);
            Debug.DrawRay(transform.position, RotateDirByDegree(defaultDir, -halfDegree), Color.cyan);
            Gizmos.color = Color.white;
        }
#endif
        #endregion

        #region Statics

        private static Vector2 RotateDirByDegree(Vector2 dir, float degree)
        {
            float radian = degree * Mathf.Deg2Rad;
            return new Vector2(dir.x * Mathf.Cos(radian) - dir.y * Mathf.Sin(radian),
                dir.x * Mathf.Sin(radian) + dir.y * Mathf.Cos(radian));
        }

        #endregion
    }
}
