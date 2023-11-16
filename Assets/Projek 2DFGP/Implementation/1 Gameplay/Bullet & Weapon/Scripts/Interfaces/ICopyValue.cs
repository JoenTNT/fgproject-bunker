namespace JT.FGP
{
    /// <summary>
    /// To copy information from source object to target object.
    /// </summary>
    /// <typeparam name="T">The same type of object that implement this interface</typeparam>
    public interface ICopyValue<T> where T : class
    {
        /// <summary>
        /// Copy the whole value method.
        /// </summary>
        /// <param name="target">Target object</param>
        void CopyValue(T target);
    }
}
