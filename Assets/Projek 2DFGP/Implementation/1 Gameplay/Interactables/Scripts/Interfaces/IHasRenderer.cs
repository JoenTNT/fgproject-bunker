namespace JT.FGP
{
    /// <summary>
    /// An object that has renderer.
    /// </summary>
    /// <typeparam name="T">Renderer type</typeparam>
    public interface IHasRenderer<T> where T : class
    {
        /// <summary>
        /// To check if renderer assigned.
        /// </summary>
        bool HasRenderer { get; }

        /// <summary>
        /// Get renderer reference.
        /// </summary>
        T Renderer { get; }
    }
}
