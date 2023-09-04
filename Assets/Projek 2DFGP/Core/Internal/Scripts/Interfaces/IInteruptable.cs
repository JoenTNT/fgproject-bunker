namespace JT.FGP
{
    /// <summary>
    /// Anything that can be interrupt.
    /// </summary>
    public interface IInteruptable
    {
        /// <summary>
        /// Status that the action is interuptable.
        /// </summary>
        bool IsInteruptable { get; set; }
    }
}