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

        [SerializeField]
        private PolygonCollider2D _polygonCollider = null;

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
        private bool _reshapeCollider = true;

        // Runtime variable data.
        //private Sprite _fovSprite = null;
        //private Rect _fovSpriteRect = Rect.zero;
        //private Texture2D _fovTexture = null;

        private RaycastHit2D[] _fovHits = new RaycastHit2D[1];
        private Vector2[] _tempPoints = null;
        private Vector2[] _tempOffsetedPoints = null;
        private ushort[] _fovTriangles = null;
        private int _fovHitCount = 0;

        #endregion

        #region Properties

        /// <summary>
        /// FOV wide angle.
        /// </summary>
        public float WideAngle
        {
            get => _wideAngle;
            set
            {
                // TODO: Reshape by angle
            }
        }

        /// <summary>
        /// FOV far reach distance.
        /// </summary>
        public float Distance
        {
            get => _distance;
            set
            {
                // TODO: Change FOV distance
            }
        }

        /// <summary>
        /// Set how many FOV triangle to render it.
        /// </summary>
        public int TriangleCount
        {
            get => _triangleCount;
            set
            {
                // TODO: Reinitalize FOV Resolution
            }
        }

        #endregion

        #region Mono

        private void Start() => Init();

        private void Update() => Reshape();
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
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

        private void Init()
        {
            int raycastCount = _triangleCount + 1;
            _tempPoints = new Vector2[raycastCount + 2];
            _tempOffsetedPoints = new Vector2[raycastCount + 2];
            _fovTriangles = new ushort[raycastCount * 3];
        }

        private void Reshape()
        {
            int raycastCount = _triangleCount + 1;
            Vector2 vertexOffset = new Vector2(0f, _distance / 2f);
            Vector2 currentWorldPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 currentLocalPos = new Vector2(transform.localPosition.x, transform.localPosition.y);
            float currentAngle = _wideAngle / 2f + transform.eulerAngles.z;
            float angleBetweenRaycasts = _wideAngle / _triangleCount;

            _tempPoints[0] = _tempPoints[raycastCount + 1] = Vector2.zero;
            _tempOffsetedPoints[0] = _tempOffsetedPoints[raycastCount + 1] = _tempPoints[0] + vertexOffset;

            for (int i = 0; i < raycastCount; i++)
            {
                // Create FOV cone shape
                Vector2 direction = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                    Mathf.Sin(currentAngle * Mathf.Deg2Rad));
                _fovHitCount = Physics2D.RaycastNonAlloc(currentWorldPos, direction,
                    _fovHits, _distance, _wallLayer);
                _tempPoints[i + 1] = _fovHitCount > 0 ? _fovHits[0].point - currentWorldPos
                    : currentLocalPos + direction * _distance;
                _tempOffsetedPoints[i + 1] = vertexOffset + _tempPoints[i + 1];

                // Triangle for renderer.
                int triangleIndex = i * 3;
                _fovTriangles[triangleIndex] = 0;
                _fovTriangles[triangleIndex + 1] = (ushort)(i + 2);
                _fovTriangles[triangleIndex + 2] = (ushort)(i + 1);
                currentAngle -= angleBetweenRaycasts;
            }

            if (_reshapeCollider)
            {
                _polygonCollider.transform.localEulerAngles = -transform.eulerAngles;
                _polygonCollider.points = _tempPoints;
            }

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
