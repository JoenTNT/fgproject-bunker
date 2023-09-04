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
        float SecondsReload { get; }

        /// <summary>
        /// Is it currently reloading?
        /// </summary>
        bool IsReloading { get; }
    }
}
