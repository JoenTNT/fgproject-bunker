namespace JT
{
    /// <summary>
    /// All entities that can be interact with.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Does entity interactable.
        /// </summary>
        bool IsInteractable { get; }

        /// <summary>
        /// Interact with the entity.
        /// </summary>
        /// <returns>True if successfully proceed interaction</returns>
        bool Interact();
    }

    /// <summary>
    /// All entities that can be interact with.
    /// </summary>
    /// <typeparam name="T">The other entity that interact with this entity</typeparam>
    public interface IInteractable<T>
    {
        /// <summary>
        /// Does entity interactable.
        /// </summary>
        bool IsInteractable { get; }

        /// <summary>
        /// Interact with the entity.
        /// </summary>
        /// <param name="entity">Other entity input</param>
        /// <returns>True if successfully proceed interaction</returns>
        bool Interact(T entity);
    }
}
