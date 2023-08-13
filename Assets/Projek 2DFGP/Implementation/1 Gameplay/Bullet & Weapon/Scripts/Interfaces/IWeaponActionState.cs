namespace JT.FGP
{
    /// <summary>
    /// Contains weapon using state.
    /// </summary>
    public interface IWeaponActionState
    {
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
    }
}