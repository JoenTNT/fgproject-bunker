using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Physical switch interface.
    /// </summary>
    public interface IPhysicalSwitch
    {
        /// <summary>
        /// Is switch turned on state.
        /// </summary>
        bool IsTurnedOn { get; }

        /// <summary>
        /// Make things turn on.
        /// </summary>
        void TurnOn();

        /// <summary>
        /// Make things turn off.
        /// </summary>
        void TurnOff();
    }
}
