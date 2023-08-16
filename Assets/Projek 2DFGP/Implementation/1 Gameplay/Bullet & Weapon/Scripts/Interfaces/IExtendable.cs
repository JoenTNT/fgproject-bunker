namespace JT.FGP
{
    /// <summary>
    /// Extendable template.
    /// </summary>
    public interface IExtendable<T> where T : notnull
    {
        /// <summary>
        /// Current size of template.
        /// </summary>
        T CurrentSize { get; }

        /// <summary>
        /// Extend size.
        /// </summary>
        /// <param name="size"></param>
        void Extend(T size);
    }
}

