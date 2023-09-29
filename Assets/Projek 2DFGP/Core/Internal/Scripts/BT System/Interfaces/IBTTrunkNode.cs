namespace JT
{
    /// <summary>
    /// Node processor.
    /// </summary>
    public interface IBTTrunkNode
    {
        /// <summary>
        /// Current node index execution.
        /// </summary>
        int NodeIndex { get; }

        /// <summary>
        /// Get current process node at this trunk.
        /// </summary>
        /// <returns>Current process node</returns>
        BT_Execute GetCurrentProcess();

        /// <summary>
        /// Get current leaf process node at this trunk.
        /// </summary>
        /// <returns>Current leaf process node</returns>
        BT_Execute GetCurrentLeafProcess();
    }
}
