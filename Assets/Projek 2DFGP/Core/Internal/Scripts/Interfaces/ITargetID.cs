namespace JT
{
    /// <summary>
    /// Target ID data property.
    /// </summary>
    /// <typeparam name="T">ID Data Type</typeparam>
    public interface ITargetID<T>
    {
        /// <summary>
        /// Target ID data value.
        /// </summary>
        T TargetID { get; set; }
    }
}
