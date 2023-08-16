namespace JT.FGP
{
    /// <summary>
    /// Single entity shoot command using direction.
    /// </summary>
    public interface IShootDirectionCommand<T> where T : notnull
    {
        /// <summary>
        /// Shoot command with direction.
        /// </summary>
        /// <param name="direction">Shoot direction</param>
        /// <param name="speed">Shoot speed</param>
        void Shoot(T direction, float speed);
    }
}

