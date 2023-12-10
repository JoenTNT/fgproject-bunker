using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Point with single cache between spawn point and spawned object.
    /// </summary>
    public sealed class SpawnPointSingleCachedObject : SpawnPointCachedObject,
        ISingleCache<SpawnObjectCache>
    {
        #region Variables

        // Runtime variable data.
        private SpawnObjectCache _cache = null;

        #endregion

        #region SpawnPointCachedObject

        public override bool IsCacheFull => _cache != null;

        public override void CacheObject(SpawnObjectCache cache) => AssignCache(cache);

        #endregion

        #region ISingleCache

        public bool HasCache => _cache != null;

        public void AssignCache(SpawnObjectCache t) => _cache = t;

        public void RemoveCache() => _cache = null;

        #endregion
    }
}