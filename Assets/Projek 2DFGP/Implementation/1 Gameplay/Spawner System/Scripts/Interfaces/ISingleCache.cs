namespace JT.FGP
{
    /// <summary>
    /// A room to put single cache.
    /// </summary>
    /// <typeparam name="T">Any type of cache</typeparam>
    public interface ISingleCache<T>
    {
        /// <summary>
        /// To check if there's cache assigned.
        /// </summary>
        bool HasCache { get; }

        /// <summary>
        /// Assign or replace cache.
        /// </summary>
        /// <param name="t">Target cache</param>
        void AssignCache(T t);

        /// <summary>
        /// Release cache into empty room.
        /// </summary>
        void RemoveCache();
    }
}

