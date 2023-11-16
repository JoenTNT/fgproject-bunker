namespace JT.FGP
{
    /// <summary>
    /// To consume consumable command.
    /// </summary>
    public interface IConsumeCommand
    {
        /// <summary>
        /// Consume the consumable method.
        /// </summary>
        /// <param name="c">A thing that is consumable</param>
        void Consume(IConsumable c);
    }
}