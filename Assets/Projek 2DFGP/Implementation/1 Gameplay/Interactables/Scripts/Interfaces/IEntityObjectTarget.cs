namespace JT.FGP
{
    /// <summary>
    /// Handle setting target of something to entity.
    /// </summary>
    /// <typeparam name="T">Type of object target</typeparam>
    public interface IEntityObjectTarget<T> where T : class
    {
        /// <summary>
        /// Target of entity object.
        /// </summary>
        T EntityObjectTarget { get; set; }
    }
}
