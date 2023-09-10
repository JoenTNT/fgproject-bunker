namespace JT.FGP
{
    /// <summary>
    /// Single entity shoot command.
    /// </summary>
    public interface IShootCommand
    {
        /// <summary>
        /// Shoot method.
        /// </summary>
        void Shoot();
    }

    /// <summary>
    /// Single entity shoot command.
    /// </summary>
    /// <typeparam name="T">Type of ammo.</typeparam>
    public interface IShootCommand<T>
    {
        /// <summary>
        /// Shoot an ammo.
        /// </summary>
        /// <param name="ammo">Using ammo</param>
        void Shoot(T ammo);
    }
}
