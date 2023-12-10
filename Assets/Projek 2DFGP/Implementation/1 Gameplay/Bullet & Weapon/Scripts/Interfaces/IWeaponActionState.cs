namespace JT.FGP
{
    /// <summary>
    /// Contains weapon using state.
    /// </summary>
    public interface IWeaponActionState
    {
        /// <summary>
        /// Event called when instant use happen.
        /// </summary>
        event System.Action OnInstantUse;

        /// <summary>
        /// Who use this weapon?
        /// </summary>
        string Owner { get; set; }

        /// <summary>
        /// Current action state.
        /// </summary>
        bool IsInAction { get; }

        /// <summary>
        /// Set weapon to start action or aiming state.
        /// </summary>
        void StartAim();

        /// <summary>
        /// Set weapon to released state.
        /// </summary>
        void Release();

        /// <summary>
        /// Use it instantly with this method.
        /// </summary>
        void InstantUse();
    }
}