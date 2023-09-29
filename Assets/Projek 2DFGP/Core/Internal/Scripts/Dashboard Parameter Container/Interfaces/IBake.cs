namespace JT
{
    /// <summary>
    /// Bake into something lightweight.
    /// </summary>
    public interface IBake
    {
        /// <summary>
        /// Bake into other form then delete the original form.
        /// </summary>
        object Bake();
    }

    /// <summary>
    /// Bake into something lightweight.
    /// </summary>
    /// <typeparam name="T">Baked type of original form</typeparam>
    public interface IBake<T>
    {
        /// <summary>
        /// Bake into other form then delete the original form.
        /// </summary>
        T Bake();
    }
}
