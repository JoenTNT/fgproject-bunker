namespace JT.FGP
{
    /// <summary>
    /// Anything that is collectable.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public interface ICollectable<T> where T : class
    {
        /// <summary>
        /// Collect an item.
        /// </summary>
        /// <returns>Collectible item</returns>
        T Collect();
    }
}
