namespace JT.FGP
{
    /// <summary>
    /// Any behaviour with time delay before doing action.
    /// </summary>
    /// <typeparam name="T">Delay seconds type</typeparam>
    public interface IDelayAction<T> where T : notnull
    {
        /// <summary>
        /// Initial delay second.
        /// </summary>
        T InitialDelaySecond { set; get; }

        /// <summary>
        /// Runtime delay second.
        /// </summary>
        T SecondBeforeAction { get; }

        /// <summary>
        /// Call for reset delay.
        /// Only works when delay time is running.
        /// </summary>
        void ResetDelay();
    }
}
