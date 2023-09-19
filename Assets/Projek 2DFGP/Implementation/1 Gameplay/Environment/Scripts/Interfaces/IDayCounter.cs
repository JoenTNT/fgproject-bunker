namespace JT.FGP
{
    /// <summary>
    /// Handle counting days.
    /// </summary>
    /// <typeparam name="T">Type of day counter</typeparam>
    public interface IDayCounter<T> where T : notnull
    {
        /// <summary>
        /// Current day.
        /// </summary>
        T DayCount { get; }

        /// <summary>
        /// To reset day count back to default value.
        /// </summary>
        void ResetDayCount();
    }
}
