using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Eye sight view of entity.
    /// </summary>
    public class PolygonalFOVDetection2DFunc : MonoBehaviour
    {
        #region Variable

        //[SerializeField]
        //private SpriteRenderer _renderer = null;

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
        [Min(2)]
        private int _triangleCount = 2;

        //[SerializeField]
        //private bool _renderFOVSprite = true;

        [SerializeField]
        private bool _reshapeAtRuntime = true;
#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField]
        private bool _debug = false;
#endif
        // Runtime variable data.
        //private Sprite _fovSprite = null;
        //private Rect _fovSpriteRect = Rect.zero;
        //private Texture2D _fovTexture = null;

        private RaycastHit2D[] _fovHits = new RaycastHit2D[1];
        private Vector2[] _tempPoints = null;
        private ushort[] _fovTriangles = null;
        private int _fovHitCount = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Layer that blocks casting FOV.
        /// Only works when reshape at runtime is activated.
        /// </summary>
        public LayerMask WallLayer => _wallLayer;

        /// <summary>
        /// FOV wide angle.
        /// </summary>
        public float WideAngle => _wideAngle;

        /// <summary>
        /// FOV far reach distance.
        /// </summary>
        public float Distance => _distance;

        /// <summary>
        /// Set how many FOV triangle to render it.
        /// </summary>
        public int TriangleCount => _triangleCount;

        #endregion

        #region Mono

        private void Start() => Init();

        private void Update()
        {
            // Check if polygon need to be reshaped.
            if (_reshapeAtRuntime) Reshape();
        }

        #endregion

        #region Main

        private void Init()
        {
            int raycastCount = _triangleCount + 1;
            _tempPoints = new Vector2[raycastCount + 2];
            _fovTriangles = new ushort[raycastCount * 3];

            // Reshape once after init.
            Reshape();
        }

        private void Reshape()
        {
            if (_triangleCount + 3 != _tempPoints.Length) Init();

            int raycastCount = _triangleCount + 1;
            Vector2 worldPos = transform.position;
            Vector2 localPos = transform.localPosition;
            float localAngle = _wideAngle / 2f;
            float worldAngle = localAngle + transform.eulerAngles.z;
            float angleBetweenRaycasts = _wideAngle / _triangleCount;
            float degToRad = Mathf.Deg2Rad;

            // Start and End Point are the same.
            _tempPoints[0] = _tempPoints[raycastCount + 1] = Vector2.zero;

            for (int i = 0; i < raycastCount; i++)
            {
                // Create FOV cone shape
                Vector2 localDir = new Vector2(Mathf.Cos(localAngle * degToRad), Mathf.Sin(localAngle * degToRad));
                Vector2 worldDir = new Vector2(Mathf.Cos(worldAngle * degToRad), Mathf.Sin(worldAngle * degToRad));
                _fovHitCount = Physics2D.RaycastNonAlloc(worldPos, worldDir, _fovHits, _distance, _wallLayer);
                _tempPoints[i + 1] = localPos + localDir * (_fovHitCount > 0 ? _fovHits[0].distance : _distance);
#if UNITY_EDITOR
                // Check debug mode.
                if (_debug)
                {
                    Debug.DrawLine(worldPos, worldPos + _tempPoints[i + 1], Color.yellow);
                }
#endif
                // Triangle for renderer.
                int triangleIndex = i * 3;
                _fovTriangles[triangleIndex] = 0;
                _fovTriangles[triangleIndex + 1] = (ushort)(i + 2);
                _fovTriangles[triangleIndex + 2] = (ushort)(i + 1);
                localAngle -= angleBetweenRaycasts;
                worldAngle -= angleBetweenRaycasts;
            }

            // Reshape polygonal collider.
            _polygonCollider.points = _tempPoints;

            //if (!_renderFOVSprite) return;

            //int pixelDistance = (int)Mathf.Ceil(_distance);
            //_fovTexture = new Texture2D(pixelDistance, pixelDistance);

            //for (int x = 0; x < pixelDistance; x++)
            //    for (int y = 0; y < pixelDistance; y++)
            //        _fovTexture.SetPixel(x, y, Color.white);

            //_fovTexture.filterMode = FilterMode.Point;
            //_fovTexture.Apply();
            //_fovSpriteRect = new Rect(0f, 0f, _distance, _distance);
            //_fovSprite = Sprite.Create(_fovTexture, _fovSpriteRect, new Vector2(0f, 0.5f), 1f);
            //_fovSprite.name = "FOV";
            //_fovSprite.OverrideGeometry(_tempOffsetedPoints, _fovTriangles);
            //_renderer.sprite = _fovSprite;
        }

        #endregion
    }
}
