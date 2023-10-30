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
        /// Current amount of ammo in bag.
        /// </summary>
        int AmmoInBag { get; }

        /// <summary>
        /// Max ammo for single round.
        /// </summary>
        int MaxAmmo { get; }

        /// <summary>
        /// Current ammo in weapon.
        /// </summary>
        int Ammo { get; }

        /// <summary>
        /// Add ammo into bag.
        /// </summary>
        /// <param name="amount">Amount of added ammo</param>
        void AddAmmoToBag(int amount);

        /// <summary>
        /// Use ammo and decrese the counter.
        /// </summary>
        /// <param name="amount">Amount of ammo used</param>
        /// <returns>Actual ammo amount used</returns>
        int UseAmmoInBarrel(int amount);

        /// <summary>
        /// Transfer ammo from bag to barrel until full.
        /// </summary>
        void TransferAmmo();

        /// <summary>
        /// Reset ammo in gauge back to max ammo.
        /// </summary>
        void ResetAmmo();

        /// <summary>
        /// Reset
        /// </summary>
        void ResetAmmoBag();
    }
}