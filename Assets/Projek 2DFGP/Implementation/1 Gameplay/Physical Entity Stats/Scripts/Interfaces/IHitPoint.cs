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
        T MaxHP { get; set; }

        /// <summary>
        /// Current hitpoint this entity has.
        /// </summary>
        T CurrentHP { get; }

        /// <summary>
        /// To check if Hitpoint is full.
        /// </summary>
        bool IsHPFull { get; }

        /// <summary>
        /// Give hitpoint heal to this entity.
        /// </summary>
        /// <param name="hp">Hitpoint heal</param>
        void Heal(T hp);
    }
}