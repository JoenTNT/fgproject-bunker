namespace JT.FGP
{
    /// <summary>
    /// For stats only if things can give critical.
    /// </summary>
    /// <typeparam name="T">Critical hit point data type</typeparam>
    public interface ICriticalHit<T> where T : notnull
    {
        /// <summary>
        /// Chance of getting critical.
        /// Usually from 0% to 100% (0 to 1).
        /// </summary>
        float CriticalChance { get; }

        /// <summary>
        /// When critical happens, then multiply the value by.
        /// </summary>
        float CriticalMultiplier { get; }

        /// <summary>
        /// Let the entity calculate the critical.
        /// This may or may not change the value if not hitting the chance.
        /// </summary>
        /// <param name="targetValue">Value that will be multiply by critical hit</param>
        /// <returns></returns>
        T GetCriticalValue(T targetValue);
    }
}

