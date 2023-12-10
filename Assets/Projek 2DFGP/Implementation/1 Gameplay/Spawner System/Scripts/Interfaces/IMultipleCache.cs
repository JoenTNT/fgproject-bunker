namespace JT.FGP
{
    /// <summary>
    /// A room to put multiple cache.
    /// </summary>
    /// <typeparam name="T">Any type of cache</typeparam>
    public interface IMultipleCache<T>
    {
        /// <summary>
        /// Amount of cache free room.
        /// </summary>
        int FreeCacheAmount { get; }

        /// <summary>
        /// To check if cache is full.
        /// </summary>
        bool IsCacheFull { get; }

        /// <summary>
        /// Assign cache into empty room.
        /// </summary>
        /// <param name="t">Target cache</param>
        void AddToCache(T t);

        /// <summary>
        /// Release specific cache from the room.
        /// </summary>
        /// <param name="t">Target cache</param>
        void ReleaseFromCache(T t);

        /// <summary>
        /// Empty the cache room.
        /// </summary>
        void ClearCache();
    }
}
