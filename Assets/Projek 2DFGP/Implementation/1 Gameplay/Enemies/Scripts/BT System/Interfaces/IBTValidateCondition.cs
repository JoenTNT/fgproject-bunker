namespace JT
{
    /// <summary>
    /// To check if the behaviour condition is valid before running the action.
    /// </summary>
    public interface IBTValidateCondition
    {
        /// <summary>
        /// Handle check validation of behaviour tree.
        /// </summary>
        /// <returns>Condition is valid</returns>
        bool IsConditionValid();
    }
}

