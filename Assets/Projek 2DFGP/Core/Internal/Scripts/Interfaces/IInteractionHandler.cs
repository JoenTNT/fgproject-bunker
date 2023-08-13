namespace JT
{
    /// <summary>
    /// Handles interaction local events.
    /// </summary>
    public interface IInteractionHandler
    {
        /// <summary>
        /// Runs before proceeding interaction.
        /// </summary>
        void OnInteractionStart();

        /// <summary>
        /// Runs after finishing interaction.
        /// </summary>
        void OnInteractionEnded();
    }
}
