using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Generic command for joystick controller.
    /// </summary>
    [RequireComponent(typeof(UI_JoystickControl))]
    public abstract class UI_JoystickCommand : MonoBehaviour
    {
        #region Variables

        // Temporary variable data.
        private UI_JoystickControlData _dataRef = null;

        #endregion

        #region Properties

        /// <summary>
        /// Joystick data reference.
        /// </summary>
        internal UI_JoystickControlData JoystickData => _dataRef;

        #endregion

        #region Main

        /// <summary>
        /// Inject data by source.
        /// </summary>
        /// <param name="data">Data reference</param>
        internal void InjectData(UI_JoystickControlData data) => _dataRef = data;

        #endregion
    }
}

