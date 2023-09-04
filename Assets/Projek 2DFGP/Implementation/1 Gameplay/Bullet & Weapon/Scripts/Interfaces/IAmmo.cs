namespace JT.FGP
{
    /// <summary>
    /// Ammo information interface for weapon relation.
    /// </summary>
    public interface IAmmo
    {
        /// <summary>
        /// Max backup ammo that can be stored.
        /// </summary>
        int MaxAmmoInBag { get; }

        /// <summary>
        /// Max ammo for single round.
        /// </summary>
        int MaxAmmo { get; }

        /// <summary>
        /// Current ammo in weapon.
        /// </summary>
        int Ammo { get; }
    }
}