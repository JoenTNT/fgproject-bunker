namespace JT.FGP
{
    /// <summary>
    /// Container data for parameters.
    /// </summary>
    public interface IParameterContainerData
    {
        /// <summary>
        /// Amount of keys registered in container.
        /// </summary>
        int CountKeys { get; }

        /// <summary>
        /// Check if the key is exists.
        /// </summary>
        /// <param name="key">Desired key</param>
        /// <returns>True if found in the registry, else then false</returns>
        bool KeyExists(string key);

        /// <summary>
        /// Set value at target parameter.
        /// </summary>
        /// <typeparam name="T">Parameter type.</typeparam>
        /// <param name="key">Key of parameter.</param>
        /// <param name="value">Value of parameter.</param>
        void SetParamValue<T>(string key, T value);

        /// <summary>
        /// Get value at targer parameter.
        /// </summary>
        /// <typeparam name="T">Parameter type.</typeparam>
        /// <param name="key">Key of parameter.</param>
        /// <returns>Value of parameter.</returns>
        IParameterData<T> GetParamValue<T>(string key);
    }
}