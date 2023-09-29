namespace JT
{
    /// <summary>
    /// Behaviour tree process holder.
    /// </summary>
    public interface IBTProcessHolder
    {
        /// <summary>
        /// Is current execution is beinng halt?
        /// </summary>
        bool IsExecutionHold { get; }

        /// <summary>
        /// Release execution holder.
        /// </summary>
        void ReleaseHolder();
    }
}
