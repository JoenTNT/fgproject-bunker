using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace JT.FGP
{
    /* ISSUE: There is no RaycastCommand for RaycastHit2D yet */
    /// <summary>
    /// Eye sight view of entity using Job System.
    /// </summary>
    public class PolygonalFOV2DJobFunc : MonoBehaviour
    {
        #region structs

        /// <summary>
        /// Handle calculate directions job.
        /// </summary>
        private struct CalculateDirectionsJob : IJobParallelFor
        {
            public NativeArray<RaycastCommand> castCommands;
            public NativeArray<Vector2> castDirs;

            [Unity.Collections.ReadOnly]
            public LayerMask wallMask;

            [Unity.Collections.ReadOnly]
            public Vector2 fromPos;
            
            public float currentAngle;

            [Unity.Collections.ReadOnly]
            public float substractAngle;

            [Unity.Collections.ReadOnly]
            public float distance;

            public void Execute(int index)
            {
                castDirs[index] = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                    Mathf.Sin(currentAngle * Mathf.Deg2Rad));

                currentAngle -= substractAngle;

                castCommands[index] = new RaycastCommand(fromPos, castDirs[index], new QueryParameters() {
                    layerMask = wallMask, }, distance: distance);
            }
        }

        #endregion

        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private PolygonCollider2D _polygonCollider = null;

        [Header("Properties")]
        [SerializeField]
        private LayerMask _wallLayer = ~0;

        [SerializeField]
        [Min(0f)]
        private float _wideAngle = 45f;

        [SerializeField]
        [Min(0f)]
        private float _distance = 10f;

        [SerializeField]
        [Min(1)]
        private int _triangleCount = 2;

        [SerializeField]
        [Min(1)]
        private int _maxCastHit = 1;
#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField]
        private bool _debug = true;
#endif
        // Runtime variable data.
        private NativeArray<RaycastHit> _hits;
        private JobHandle _jobDependency;
        private JobHandle _job;

        #endregion

        #region Mono

        private void Update() => Reshape();
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            // Check debug mode, only for editor mode.
            if (!_debug || Application.isPlaying) return;

            int raycastCount = _triangleCount + 1;
            Vector2 currentWorldPos = new Vector2(transform.position.x, transform.position.y);
            float currentAngle = _wideAngle / 2f + transform.eulerAngles.z;
            float angleBetweenRaycasts = _wideAngle / _triangleCount;
            for (int i = 0; i < raycastCount; i++)
            {
                Vector2 direction = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                    Mathf.Sin(currentAngle * Mathf.Deg2Rad));
                Debug.DrawRay(currentWorldPos, direction * _distance, Color.yellow);
                currentAngle -= angleBetweenRaycasts;
            }
        }
#endif
        #endregion

        #region Main

        private void Reshape()
        {
            // Initalize native collections.
            int rayCount = _triangleCount + 1;
            _hits = new NativeArray<RaycastHit>(_maxCastHit * rayCount, Allocator.TempJob);

            // Define starting declaration with Job.
            var calcJob = new CalculateDirectionsJob()
            {
                castCommands = new NativeArray<RaycastCommand>(rayCount, Allocator.TempJob),
                castDirs = new NativeArray<Vector2>(rayCount, Allocator.TempJob),
                wallMask = _wallLayer,
                fromPos = transform.position,
                currentAngle = _wideAngle / 2f + transform.eulerAngles.z,
                substractAngle = _wideAngle / _triangleCount,
                distance = _distance,
            };

            // Do calculation job first.
            _jobDependency = calcJob.Schedule(rayCount, 250);
            _jobDependency.Complete();

            // Running job.
            _job = RaycastCommand.ScheduleBatch(calcJob.castCommands, _hits, rayCount, _maxCastHit, _jobDependency);
            _job.Complete();

            int hitCount = _hits.Length;
            RaycastHit hit;
            for (int i = 0; i < hitCount; i++)
            {
                hit = _hits[i];
                if (hit.collider == null) continue;
                Debug.Log(hit.collider);
            }
        }

        #endregion
    }
}
