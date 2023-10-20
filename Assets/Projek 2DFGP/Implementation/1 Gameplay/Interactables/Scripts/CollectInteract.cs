namespace JT.FGP
{
    /// <summary>
    /// Handle taking item when interact.
    /// </summary>
    public sealed class CollectInteract : InteractableComponent
    {
        #region Variable

        #endregion

        #region InteractableComponent

        public override InteractionType Type => InteractionType.PickUpItem;

        public override bool Interact(string entity)
        {
            return true;
        }

        #endregion
    }
}
