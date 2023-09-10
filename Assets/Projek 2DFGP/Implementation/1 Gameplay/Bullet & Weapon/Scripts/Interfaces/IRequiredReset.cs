namespace JT.FGP
{
    /// <summary>
    /// Object that has exclusive reset method.
    /// </summary>
    public interface IRequiredReset
    {
        /// <summary>
        /// Required reset method.
        /// Can be used as MonoBehaviour Reset() method.
        /// </summary>
        void Reset();
    }
}
