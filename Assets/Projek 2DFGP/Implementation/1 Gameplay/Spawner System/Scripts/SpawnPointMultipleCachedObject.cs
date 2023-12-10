using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Point with multiple cache between spawn point and spawned object.
    /// </summary>
    public sealed class SpawnPointMultipleCachedObject : SpawnPointCachedObject,
        IMultipleCache<SpawnObjectCache>
    {
        #region Variables

        [Header("Properties")]
        [SerializeField, Min(1)]
        private int _limitCache = 1;

        // Runtime variable data.
        private SpawnObjectCache[] _objCaches = null;
        private int _emptySpotIndex = 0;

        #endregion

        #region Mono

        private void Awake()
        {
            // Create cache container.
            _objCaches = new SpawnObjectCache[_limitCache];
        }

        #endregion

        #region SpawnPointCachedObject

        public override bool IsCacheFull => _emptySpotIndex >= _limitCache;

        public override void CacheObject(SpawnObjectCache cache) => AddToCache(cache);

        #endregion

        #region Main

        public int FreeCacheAmount => _limitCache - _emptySpotIndex;

        public void AddToCache(SpawnObjectCache t)
        {
            _objCaches[_emptySpotIndex] = t;
            _emptySpotIndex++;
        }

        public void ReleaseFromCache(SpawnObjectCache t)
        {
            int len = _objCaches.Length;
            bool foundRelease = false;
            for (int i = 0; i < len; i++)
            {
                // After release procedure.
                if (foundRelease)
                {
                    _objCaches[i - 1] = _objCaches[i];
                    _objCaches[i] = null;
                    continue;
                }

                // Ignore unequal target.
                if (_objCaches[i] != t) continue;

                // Release cache.
                _objCaches[i] = null;
                foundRelease = true;
                _emptySpotIndex--;
            }
        }

        public void ClearCache()
        {
            _emptySpotIndex = 0;
            int len = _objCaches.Length;
            for (int i = 0; i < len; i++)
                _objCaches[i] = null;
        }

        #endregion
    }
}
