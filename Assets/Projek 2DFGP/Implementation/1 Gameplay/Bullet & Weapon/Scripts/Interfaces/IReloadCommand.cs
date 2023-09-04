namespace JT.FGP
{
    /// <summary>
    /// Contains reloading command control.
    /// </summary>
    public interface IReloadCommand
    {
        /// <summary>
        /// Reload function.
        /// </summary>
        void Reload();

        /// <summary>
        /// If currently reloading, then reload instantly.
        /// </summary>
        void SkipReload();

        /// <summary>
        /// If currently reloading, then cancel reloading process.
        /// </summary>
        void CancelReload();
    }
}

