namespace JT.FGP
{
    /// <summary>
    /// Handle opening dialogue when interacting with this object.
    /// </summary>
    public sealed class OpenDialogueInteract : InteractableComponent
    {
        #region InteractableComponent

        public override InteractionType Type => InteractionType.Talk;

        public override bool Interact(string entity)
        {
            return true;
        }

        #endregion
    }
}

