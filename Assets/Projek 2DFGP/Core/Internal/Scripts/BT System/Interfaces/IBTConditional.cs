namespace JT
{
    /// <summary>
    /// To check if the behaviour condition is valid before running the action.
    /// </summary>
    public interface IBTConditional
    {
        /// <summary>
        /// Handle check validation of behaviour tree.
        /// </summary>
        /// <returns>Condition is valid</returns>
        BT_State CalcStateCondition();
    }
}

