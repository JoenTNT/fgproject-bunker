namespace JT.FGP
{
    /// <summary>
    /// Single object identity.
    /// </summary>
    public interface IObjectID<T> where T : notnull
    {
        /// <summary>
        /// Object identifier.
        /// </summary>
        T ObjectID { get; }
    }
}
