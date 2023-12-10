using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Flagged game object as a cache object.
    /// </summary>
    public sealed class SpawnObjectCache : MonoBehaviour
    {
        #region Variables

        [Header("Game Events")]
        [SerializeField]
        private GameEventUnityObject _onReleasePointCache = null;

        // Runtime variable data.
        private SpawnPointCachedObject _cachePoint = null;

        #endregion

        #region Mono

        private void OnDisable()
        {
            // Remove cache from point.
            switch (_cachePoint)
            {
                case SpawnPointSingleCachedObject single:
                    single.RemoveCache();
                    break;

                case SpawnPointMultipleCachedObject multiple:
                    multiple.ReleaseFromCache(this);
                    break;
            }

            // Call event for cache removal, only if exists.
            if (_cachePoint != null)
                _onReleasePointCache.Invoke(_cachePoint);
        }

        #endregion

        #region Main

        public void AssignSpawnPoint(SpawnPointCachedObject point)
        {
            _cachePoint = point;
            _cachePoint.CacheObject(this);
        }

        #endregion
    }
}
