namespace JT.FGP
{
    /// <summary>
    /// Used for any kind of bar value.
    /// </summary>
    /// <typeparam name="T">Bar value type</typeparam>
    public interface IBarValue<T> where T : notnull
    {
        /// <summary>
        /// Maximum bar value.
        /// </summary>
        T MaxBarValue { get; set; }

        /// <summary>
        /// Current value in bar.
        /// </summary>
        T BarValue { get; set; }
    }
}
