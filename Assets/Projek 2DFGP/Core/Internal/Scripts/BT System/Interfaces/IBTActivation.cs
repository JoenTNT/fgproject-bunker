namespace JT
{
    /// <summary>
    /// Contains enable and disable state of action.
    /// </summary>
    public interface IBTActivation
    {
        /// <summary>
        /// Only running once after an object has been instantiated.
        /// </summary>
        void OnStartBehaviour();

        /// <summary>
        /// Run when behaviour is enabled.
        /// </summary>
        void OnEnableBehaviour();

        /// <summary>
        /// Run when behaviour is disabled.
        /// </summary>
        void OnDisableBehaviour();

        /// <summary>
        /// Only running once before an object has been destroyed.
        /// </summary>
        void OnEndBehaviour();
    }
}
