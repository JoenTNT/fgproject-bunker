using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle sending action command using joystick controller.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UI_CallJoystickActionCommand : UI_JoystickCommand
    {
        #region Variables

        [Header("Properties")]
        [SerializeField, Min(0f)]
        private float _actionPressedBeforeSeconds = 0.15f;

        [SerializeField, Min(0f)]
        private float _activeOnPercentBiggerThan = 0.65f;

        [SerializeField]
        private bool _releaseToEndAction = false;

        [Header("Game Events")]
        [SerializeField]
        private GameEventString _onJoystickPressed = null;

        [SerializeField]
        private GameEventString _onJoystickActionBegin = null;

        [SerializeField]
        private GameEventString _onJoystickActionEnded = null;

        // Runtime variable data.
        private float _tempSecPress = 0f;
        private bool _isPressed = false;
        private bool _isCalled = false;

        #endregion

        #region Mono

        private void Update()
        {
            // Pressed seconds.
            if (_isPressed) _tempSecPress += Time.deltaTime;

            // Check joystick pressed begin from the source.
            if (JoystickData.IsJoystickPressed && !_isPressed)
            {
                // Reset time press.
                _tempSecPress = 0f;

                // Pressed status.
                _isPressed = true;
            }

            // Check joystick pressed end from the source.
            else if (!JoystickData.IsJoystickPressed && _isPressed)
            {
                // Check event called.
                if (_tempSecPress <= _actionPressedBeforeSeconds)
                {
                    // Call event.
                    _onJoystickPressed.Invoke(JoystickData.TargetID);
                }

                // Change press released status.
                _isPressed = false;
            }

            // Check if joystick must be moved first before call.
            bool needToBeActive = IsCheckerValid(JoystickData.MagnitudePercentage);

            // Check if it is need to be active and yet haven't active.
            if (needToBeActive && !_isCalled)
            {
                // Set status has been called.
                _isCalled = true;

                // Call for active event.
                _onJoystickActionBegin.Invoke(JoystickData.TargetID);
            }

            // Check if it is need to be inactive and yet disabled.
            else if (!needToBeActive && _isCalled)
            {
                // Set status has been called.
                _isCalled = false;

                // Check release to end action checker, then wait until the joystick has been released.
                if (_releaseToEndAction && JoystickData.MagnitudePercentage != 0f) return;

                // Call for inactive event.
                _onJoystickActionEnded.Invoke(JoystickData.TargetID);
            }

            if (_releaseToEndAction && !_isCalled && JoystickData.MagnitudePercentage == 0f)
                // Call for inactive event.
                _onJoystickActionEnded.Invoke(JoystickData.TargetID);
        }

        #endregion

        #region Main

        /// <summary>
        /// To check if the joystick action command should be active.
        /// </summary>
        /// <param name="magnitudePercentage">Magnitude percentage of joystick position</param>
        /// <returns>True if should be active.</returns>
        private bool IsCheckerValid(float magnitudePercentage) => magnitudePercentage > _activeOnPercentBiggerThan;

        #endregion
    }
}
