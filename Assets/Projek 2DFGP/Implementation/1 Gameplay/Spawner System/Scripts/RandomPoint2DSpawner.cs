using JT.GameEvents;
using System.Collections.Generic;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// This spawner will spawn in random registered point positions.
    /// </summary>
    public sealed class RandomPoint2DSpawner : BaseSpawner
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private SpawnPointCachedObject[] _points = new SpawnPointCachedObject[0];

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

        [Header("Game Events")]
        [SerializeField]
        private GameEventUnityObject _onReleasePointCache = null;

        // Runtime variable data.
        private List<SpawnPointCachedObject> _emptyPoints = null;
        private List<SpawnPointCachedObject> _filledPoints = null;
        private SpawnObjectCache _tempCacheObj = null;
        private IHasRenderer<SpriteRenderer> _renderer = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Generate containers.
            _emptyPoints = new List<SpawnPointCachedObject>();
            _filledPoints = new List<SpawnPointCachedObject>();

            // Insert all points into empty container.
            int pc = _points.Length;
            for (int i = 0; i < pc; i++)
            {
                // Ignore unregistered point.
                if (_points[i] == null) continue;
                _emptyPoints.Add(_points[i]);
            }

            // Subscribe events.
            _onReleasePointCache.AddListener(ListenOnReleasePointCache);
        }

        private void OnDestroy()
        {
            // Unsubscribe events.
            _onReleasePointCache.RemoveListener(ListenOnReleasePointCache);
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
            // Check if there's no more empty point cache, then ignore spawn.
            if (_emptyPoints.Count == 0) return;

            // Get random empty point.
            int choosenIndex = Random.Range(0, _emptyPoints.Count);
            GameObject obj = _objectPool.GetObject();
            if (!obj.TryGetComponent(out _tempCacheObj)) return;

            // Cache an object.
            _tempCacheObj.AssignSpawnPoint(_emptyPoints[choosenIndex]);

            // Set random position of object in world.
            Vector2 spawnPos = _emptyPoints[choosenIndex].GetRandomCirclePositionByRadius();
            Transform objTransform = _tempCacheObj.transform;
            objTransform.position = spawnPos;

            // Check random rotation or follow the spawner as default rotation.
            if (_randomRotation)
            {
                float zDegree = Random.Range(-180f, 180f + Mathf.Epsilon);
                objTransform.rotation = Quaternion.Euler(0, 0, zDegree);
            }
            else objTransform.rotation = _emptyPoints[choosenIndex].transform.rotation;

            // Check for random flip.
            if (_randomFlip && objTransform.TryGetComponent(out _renderer))
            {
                _renderer.Renderer.flipX = Random.Range(0, 2) == 0;
                _renderer.Renderer.flipY = Random.Range(0, 2) == 0;
            }

            // Move to filled point if it is full.
            if (_emptyPoints[choosenIndex].IsCacheFull)
            {
                _filledPoints.Add(_emptyPoints[choosenIndex]);
                _emptyPoints.RemoveAt(choosenIndex);
            }
        }

        #endregion

        #region Main

        private void ListenOnReleasePointCache(Object point)
        {
            // Validate information.
            if (point is not SpawnPointCachedObject) return;

            // Find the point in filled point.
            int len = _filledPoints.Count;
            int targetID = point.GetInstanceID();
            for (int i = 0; i < len; i++)
            {
                // Ignore unmatch point.
                if (_filledPoints[i].GetInstanceID() != targetID) continue;

                // Send filled back to empty list.
                _emptyPoints.Add(_filledPoints[i]);
                _filledPoints.RemoveAt(i);
                break;
            }
        }

        #endregion
    }
}
