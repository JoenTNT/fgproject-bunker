using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// This spawner will spawn in random area.
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed class RandomRectArea2DSpawner : BaseSpawner
    {
        #region Variables

        [Header("Properties")]
        [SerializeField]
        private string _poolKey = string.Empty;

        [SerializeField]
        private bool _randomRotation = false;

        [SerializeField]
        private bool _randomFlip = false;

        [SerializeField]
        private bool _getPoolOnStart = false;

        [Header("Optional")]
        [SerializeField]
        private GameObjectPool _objectPool = null;

        // Runtime variable data.
        private BoxCollider2D _area2D = null;
        private GameObject _spawnedObj = null;
        private IHasRenderer<SpriteRenderer> _renderer = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Get required component.
            TryGetComponent(out _area2D);
        }

        private void Start()
        {
            // Retrieve pool reference.
            if (_getPoolOnStart || _objectPool == null)
                _objectPool = GameObjectPoolManager.Instance.GetGameObjPool(_poolKey);
        }

        #endregion

        #region BaseSpawner

        public override void Spawn()
        {
            // Retrieve object from pool.
            _spawnedObj = _objectPool.GetObject();
            Transform objTransform = _spawnedObj.transform;

            // Set random position of object in world.
            Vector2 spawnPos = RandomInsideArea(_area2D);
            objTransform.position = spawnPos;

            // Check random rotation or follow the spawner as default rotation.
            if (_randomRotation)
            {
                float zDegree = Random.Range(-180f, 180f + Mathf.Epsilon);
                objTransform.rotation = Quaternion.Euler(0, 0, zDegree);
            }
            else objTransform.rotation = transform.rotation;

            // Check for random flip.
            if (_randomFlip && objTransform.TryGetComponent(out _renderer))
            {
                _renderer.Renderer.flipX = Random.Range(0, 2) == 0;
                _renderer.Renderer.flipY= Random.Range(0, 2) == 0;
            }
        }

        #endregion

        #region Statics

        /// <summary>
        /// To randomize which position inside area.
        /// </summary>
        /// <param name="area">Target area</param>
        /// <returns>Randomized position inside the target area</returns>
        private static Vector2 RandomInsideArea(BoxCollider2D area)
        {
            Bounds areaBound = area.bounds;
            Vector2 min = areaBound.min, max = areaBound.max;
            float xPos = Random.Range(min.x, max.x + Mathf.Epsilon);
            float yPos = Random.Range(min.y, max.y + Mathf.Epsilon);
            return new Vector2(xPos, yPos);
        }

        #endregion
    }
}
