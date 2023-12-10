namespace JT.FGP
{
    /// <summary>
    /// To randomize selection.
    /// </summary>
    public interface IRandomizeSelection
    {
        /// <summary>
        /// Self internal random selection.
        /// </summary>
        void SelectRandom();
    }

    /// <summary>
    /// To get randomize selection.
    /// </summary>
    /// <typeparam name="T">Random value or object type</typeparam>
    public interface IRandomizeSelection<T>
    {
        /// <summary>
        /// Get random value or object selection.
        /// </summary>
        T SelectRandom();
    }
}
