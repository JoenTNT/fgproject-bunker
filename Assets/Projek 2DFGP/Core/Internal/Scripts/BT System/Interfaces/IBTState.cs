namespace JT
{
    /// <summary>
    /// State of node.
    /// </summary>
    public interface IBTState
    {
        /// <summary>
        /// Current state check.
        /// </summary>
        BT_State State { get; }
    }
}
