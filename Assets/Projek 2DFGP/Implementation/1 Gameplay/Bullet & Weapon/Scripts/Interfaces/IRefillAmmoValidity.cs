namespace JT.FGP
{
    /// <summary>
    /// Handle checking refilling ammo.
    /// </summary>
    public interface IRefillAmmoValidity
    {
        /// <summary>
        /// Method to refill ammo.
        /// </summary>
        /// <param name="refillAmount">Amount of ammo</param>
        /// <returns>Valid refilling</returns>
        bool RefillAmmo(int refillAmount);
    }

    /// <summary>
    /// Handle checking refilling ammo.
    /// </summary>
    /// <typeparam name="T">Type of refill ammo</typeparam>
    public interface IRefillAmmoValidity<T>
    {
        /// <summary>
        /// Method to refill ammo.
        /// </summary>
        /// <param name="refillAmount">Ammo type</param>
        /// <returns>Valid refilling</returns>
        bool RefillAmmo(T refillAmount);
    }
}
