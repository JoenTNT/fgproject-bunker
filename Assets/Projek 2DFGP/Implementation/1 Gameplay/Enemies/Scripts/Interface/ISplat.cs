namespace JT.FGP
{
    /// <summary>
    /// Has splat method.
    /// </summary>
    public interface ISplat
    {
        /// <summary>
        /// One and only splat method.
        /// </summary>
        void Splat();
    }

    /// <summary>
    /// Has splat method.
    /// </summary>
    /// <typeparam name="T">Splat direction data type</typeparam>
    public interface ISplat<T> where T : notnull
    {
        /// <summary>
        /// One and only splat method.
        /// </summary>
        /// <param name="direction">Splat direction, you can set to zero if no direction.</param>
        void Splat(T direction);
    }
}

