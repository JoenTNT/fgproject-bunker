namespace JT
{
    /// <summary>
    /// Handle a custom wait routine.
    /// </summary>
    public interface IWaitUntilFinishHandler
    {
        /// <summary>
        /// Called when finish the process handler.
        /// </summary>
        event System.Action OnFinish;

        /// <summary>
        /// Is currently processing the handler.
        /// </summary>
        bool IsFinished { get; }
    }

    /// <summary>
    /// Handle a custom wait routine.
    /// </summary>
    /// <typeparam name="T">First parameter callback, Any type</typeparam>
    public interface IWaitUntilFinishHandler<T>
    {
        /// <summary>
        /// Called when finish the process handler.
        /// </summary>
        event System.Action<T> OnFinish;

        /// <summary>
        /// Is currently processing the handler.
        /// </summary>
        bool IsFinished { get; }
    }
}
