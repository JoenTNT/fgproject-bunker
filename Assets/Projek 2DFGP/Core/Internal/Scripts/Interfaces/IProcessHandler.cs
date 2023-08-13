namespace JT
{
    /// <summary>
    /// Handle process handler.
    /// </summary>
    public interface IProcessHandler
    {
        /// <summary>
        /// Is currently processing the handler.
        /// </summary>
        bool IsProcessRunning { get; }

        /// <summary>
        /// Is the current process is breakable.
        /// </summary>
        bool ProcessInterruptable { get; set; }

        /// <summary>
        /// Before process method running.
        /// </summary>
        void OnStartProcess();

        /// <summary>
        /// Process method.
        /// </summary>
        void OnProcess();

        /// <summary>
        /// When process done before labeled finish.
        /// </summary>
        void OnEndProcess();
    }

    /// <summary>
    /// Handle process handler.
    /// </summary>
    /// <typeparam name="T">First parameter placeholder, Any type</typeparam>
    public interface IProcessHandler<T>
    {
        /// <summary>
        /// Is currently processing the handler.
        /// </summary>
        bool IsProcessing { get; }

        /// <summary>
        /// Is the current process is breakable.
        /// </summary>
        bool ProcessInterruptable { get; set; }

        /// <summary>
        /// Before process method running.
        /// </summary>
        /// <param name="p1">First placeholder</param>
        void OnStartProcess(T p1);

        /// <summary>
        /// Process method.
        /// </summary>
        /// <param name="p1">First placeholder</param>
        void OnProcess(T p1);

        /// <summary>
        /// When process done before labeled finish.
        /// </summary>
        /// <param name="p1">First placeholder</param>
        void OnEndProcess(T p1);
    }
}
