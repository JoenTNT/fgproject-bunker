namespace JT.FGP
{
    /// <summary>
    /// Any item behaviour that is consumable.
    /// </summary>
    public interface IConsumable
    {
        /// <summary>
        /// Is this thing consumable one time only.
        /// </summary>
        bool IsOneTimeOnly { get; }
    }

    /// <summary>
    /// Any item behaviour that is consumable.
    /// </summary>
    /// <typeparam name="T">Consumable value type</typeparam>
    public interface IConsumable<T> : IConsumable
    {
        /// <summary>
        /// How much gain when consumed.
        /// </summary>
        T ConsumerGain { get; }
    }
}

