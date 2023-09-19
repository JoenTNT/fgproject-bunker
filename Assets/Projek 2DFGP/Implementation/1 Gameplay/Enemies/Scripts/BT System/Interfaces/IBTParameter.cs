namespace JT
{
    /// <summary>
    /// Parameter interface access.
    /// </summary>
    /// <typeparam name="T">Data type parameter</typeparam>
    [System.Obsolete]
    public interface IBTParameter<T> where T : UnityEngine.Object
    {
        /// <summary>
        /// Parameter keyword access.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Parameter value.
        /// </summary>
        T Value { get; }
    }
}

