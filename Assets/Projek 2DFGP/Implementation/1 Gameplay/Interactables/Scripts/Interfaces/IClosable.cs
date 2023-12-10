namespace JT.FGP
{
    /// <summary>
    /// Handle closable entity behaviour.
    /// </summary>
    public interface IClosable
    {
        /// <summary>
        /// Is it closable.
        /// </summary>
        bool IsClosable { get; }

        /// <summary>
        /// Close command.
        /// </summary>
        void Close();
    }
}
