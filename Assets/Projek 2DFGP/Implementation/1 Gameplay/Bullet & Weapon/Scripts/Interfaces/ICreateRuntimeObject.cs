namespace JT.FGP
{
    /// <summary>
    /// Method to create runtime object, used for presets.
    /// </summary>
    /// <typeparam name="T">Create object type</typeparam>
    public interface ICreateRuntimeObject<T> where T : class
    {
        /// <summary>
        /// Handle create runtime object method.
        /// </summary>
        /// <returns>Runtime object.</returns>
        T CreateRuntimeObject();
    }
}
