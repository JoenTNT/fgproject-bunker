namespace JT.FGP
{
    /// <summary>
    /// To spawn a thing.
    /// </summary>
    public interface ISpawner
    {
        /// <summary>
        /// Handle spawn method.
        /// </summary>
        void Spawn();
    }

    /// <summary>
    /// To spawn a thing.
    /// </summary>
    /// <typeparam name="T">Any type that represent spawned thing</typeparam>
    public interface ISpawner<T>
    {
        /// <summary>
        /// Handle spawn method.
        /// </summary>
        /// <returns>A thing that has been spawned</returns>
        T Spawn();
    }
}
