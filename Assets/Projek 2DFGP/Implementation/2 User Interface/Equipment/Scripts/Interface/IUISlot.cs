namespace JT.FGP
{
    /// <summary>
    /// UI slot to put an item with it's amount.
    /// </summary>
    /// <typeparam name="T">The entity type should be filled with</typeparam>
    public interface IUISlot<T> where T : class
    {
        /// <summary>
        /// Usable or equipable item behaviour.
        /// </summary>
        /// <typeparam name="T">Type of item</typeparam>
        /// <returns>Item in this slot</returns>
        T GetItemInSlot();
    }
}