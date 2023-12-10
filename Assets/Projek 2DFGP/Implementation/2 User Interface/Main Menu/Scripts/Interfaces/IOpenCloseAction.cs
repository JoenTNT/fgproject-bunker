namespace JT.FGP
{
    /// <summary>
    /// Any behaviour that can be open and close.
    /// </summary>
    public interface IOpenCloseAction
    {
        /// <summary>
        /// Does the current thing opened?
        /// </summary>
        bool IsOpened { get; }

        /// <summary>
        /// Open action command.
        /// </summary>
        void Open();

        /// <summary>
        /// Close action command.
        /// </summary>
        void Close();
    }
}
