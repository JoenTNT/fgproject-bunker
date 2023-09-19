namespace JT
{
    /// <summary>
    /// Run the behaviour action.
    /// </summary>
    public interface IBTRuntimeAction
    {
        /// <summary>
        /// Runs after transition to this action.
        /// </summary>
        void OnBeforeAction();

        /// <summary>
        /// Running per frame action.
        /// </summary>
        void OnTickAction();

        /// <summary>
        /// Runs before transition to other action.
        /// </summary>
        void OnAfterAction();
    }
}
