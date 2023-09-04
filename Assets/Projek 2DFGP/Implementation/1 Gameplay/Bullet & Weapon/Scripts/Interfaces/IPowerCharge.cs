namespace JT.FGP
{
    /// <summary>
    /// Handle power charge on specific object behaviour.
    /// </summary>
    public interface IPowerCharge
    {
        /// <summary>
        /// Filling percent of the power that has been charged.
        /// </summary>
        float PercentPower { get; }

        /// <summary>
        /// Handle charging method.
        /// Run this via update.
        /// </summary>
        void OnCharge();
    }
}
