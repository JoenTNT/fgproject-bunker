namespace JT
{
    /// <summary>
    /// Lock and unlock anything.
    /// </summary>
    public interface ILocker
    {
        /// <summary>
        /// Is the thing locked.
        /// </summary>
        bool IsLocked { get; }

        /// <summary>
        /// Lock things.
        /// </summary>
        void Lock();

        /// <summary>
        /// Unlock things.
        /// </summary>
        void Unlock();
    }
}
