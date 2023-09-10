namespace JT.FGP
{
    /// <summary>
    /// Duplicate scriptable object interface.
    /// </summary>
    /// <typeparam name="T">Specific type of duplication</typeparam>
    public interface IDuplicateSO<T>
    {
        /// <summary>
        /// Duplicate this object.
        /// </summary>
        /// <returns>Duplicated object</returns>
        T Duplicate();
    }
}
