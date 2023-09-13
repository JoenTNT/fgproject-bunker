namespace JT.FGP
{
    /// <summary>
    /// Handle when runtime is hapenning in this object.
    /// </summary>
    public interface IRuntimeHandler
    {
        /// <summary>
        /// Called before runtime happens, just before loop run.
        /// </summary>
        void OnPreRuntime();

        /// <summary>
        /// Called every frame of runtime.
        /// </summary>
        void OnRuntimeLoop();

        /// <summary>
        /// Called after the loop ended.
        /// </summary>
        void OnPostRuntime();
    }

    /// <summary>
    /// Handle when runtime is hapenning in this object.
    /// </summary>
    /// <typeparam name="T">Value invoker data type</typeparam>
    public interface IRuntimeHandler<T>
    {
        /// <summary>
        /// Called before runtime happens, just before loop run.
        /// </summary>
        /// <param name="invokerValue">Input value.</param>
        void OnPreRuntime(T invokerValue);

        /// <summary>
        /// Called every frame of runtime.
        /// </summary>
        /// <param name="invokerValue">Input value.</param>
        void OnRuntimeLoop(T invokerValue);

        /// <summary>
        /// Called after the loop ended.
        /// </summary>
        /// <param name="invokerValue">Input value</param>
        void OnPostRuntime(T invokerValue);
    }
}