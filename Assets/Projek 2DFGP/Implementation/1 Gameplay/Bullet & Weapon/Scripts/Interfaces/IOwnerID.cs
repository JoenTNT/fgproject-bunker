namespace JT.FGP
{
    /// <summary>
    /// Owner ID of this entity.
    /// </summary>
    /// <typeparam name="T">ID data type</typeparam>
    public interface IOwnerID<T>
    {
        /// <summary>
        /// Owner ID.
        /// </summary>
        T OwnerID { get; set; }
    }
}
