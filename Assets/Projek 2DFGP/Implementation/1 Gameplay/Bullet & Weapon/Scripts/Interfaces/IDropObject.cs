namespace JT.FGP
{
    /// <summary>
    /// Handle drop any kind of object function.
    /// </summary>
    /// <typeparam name="T">Object type</typeparam>
    public interface IDropObject<T> where T : class
    {
        /// <summary>
        /// Drop target object method.
        /// </summary>
        /// <param name="obj">Object will be dropped</param>
        void DropObject(T obj);
    }
}
