namespace JT.FGP
{
    /// <summary>
    /// Handle reload function.
    /// </summary>
    public interface IReload
    {
        /// <summary>
        /// Initial seconds reload.
        /// </summary>
        float InitialReloadSecond { get; }

        /// <summary>
        /// Get current reload time second before reloading.
        /// </summary>
        float CurrentReloadSecond { get; }

        /// <summary>
        /// Is it currently reloading?
        /// </summary>
        bool IsReloading { get; }

        /// <summary>
        /// Runtime reloading update.
        /// </summary>
        void OnReloading();

        /// <summary>
        /// Skip reload time.
        /// Only works when reload time is running.
        /// </summary>
        void SkipReload();

        /// <summary>
        /// Reset reload time back to initial second.
        /// </summary>
        void ResetReloadTime();
    }
}
