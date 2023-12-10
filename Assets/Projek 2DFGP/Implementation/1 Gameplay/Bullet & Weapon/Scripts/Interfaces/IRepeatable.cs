namespace JT.FGP
{
    /// <summary>
    /// Any kind of action or function can be repeat.
    /// </summary>
    public interface IRepeatable
    {
        /// <summary>
        /// Action many times need to be repeated.
        /// </summary>
        int RepeatAmount { get; }

        /// <summary>
        /// Wait time in second before next repeat.
        /// </summary>
        float RepeatDelay { get; }
    }
}
