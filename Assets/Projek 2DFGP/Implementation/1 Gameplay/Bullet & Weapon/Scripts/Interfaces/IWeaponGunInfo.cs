namespace JT.FGP
{
    /// <summary>
    /// Contains weapon information.
    /// </summary>
    public interface IWeaponGunInfo
    {
        /// <summary>
        /// Ammunition counter information.
        /// </summary>
        IAmmo AmmoInfo { get; }

        /// <summary>
        /// Reloading time info.
        /// </summary>
        IReload ReloadInfo { get; }
    }
}
