namespace JT.FGP
{
    /// <summary>
    /// To swap values between 2 identical objects.
    /// </summary>
    /// <typeparam name="T">Object type that implement this interface</typeparam>
    public interface ISwapValue<T> where T : class
    {
        /// <summary>
        /// Swap values between 2 objects method.
        /// </summary>
        /// <param name="target">Target object</param>
        void SwapValue(T target);
    }
}