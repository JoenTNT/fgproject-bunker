namespace JT.FGP
{
    /// <summary>
    /// Containing hitpoint data.
    /// </summary>
    /// <typeparam name="T">Hitpoint value type</typeparam>
    public interface IHitPoint<T> where T : notnull
    {
        /// <summary>
        /// Max hitpoint for this entity.
        /// </summary>
        T MaxHP { get; }

        /// <summary>
        /// Current hitpoint this entity has.
        /// </summary>
        T CurrentHP { get; }

        /// <summary>
        /// Give hitpoint heal to this entity.
        /// </summary>
        /// <param name="hp">Hitpoint heal</param>
        void Heal(T hp);
    }
}