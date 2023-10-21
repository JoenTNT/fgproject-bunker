namespace JT.FGP
{
    /// <summary>
    /// For objects that required to be initialized on start.
    /// </summary>
    public interface IRequiredInitialize
    {
        /// <summary>
        /// Status if object has been initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Run this method on awake or start.
        /// </summary>
        void Initialize();
    }
}
