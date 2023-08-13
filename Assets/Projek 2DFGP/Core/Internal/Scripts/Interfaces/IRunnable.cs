namespace JT
{
    /// <summary>
    /// Anything runnable.
    /// </summary>
    public interface IRunnableCommand
    {
        /// <summary>
        /// Start command.
        /// </summary>
        void StartRun();

        /// <summary>
        /// Stop command.
        /// </summary>
        void StopRun();
    }
}
