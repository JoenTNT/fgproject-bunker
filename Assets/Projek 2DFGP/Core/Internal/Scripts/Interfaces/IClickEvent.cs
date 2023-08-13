using UnityEngine.Events;

namespace JT
{
    /// <summary>
    /// Any object that has click event.
    /// </summary>
    public interface IClickEvent
    {
        /// <summary>
        /// On click event.
        /// </summary>
        UnityEvent OnClick { get; }
    }
}
