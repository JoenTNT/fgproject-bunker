using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle on press event to change, swap, or use the item in tool slot.
    /// </summary>
    [RequireComponent(typeof(UI_ToolSlot))]
    public class UI_OnPressToolSlot : MonoBehaviour
    {
        #region Variables

        [Header("Game Events")]
        [SerializeField]
        private GameEventTwoString _onToolSlotCommand = null;

        // Runtime variable data.
        private UI_ToolSlot _toolSlotRef = null;

        #endregion

        #region Mono

        private void Awake() => TryGetComponent(out _toolSlotRef);

        #endregion

        #region Main

        /// <summary>
        /// Use UI button event to call this event.
        /// </summary>
        public void OnPress() => _onToolSlotCommand.Invoke(
            _toolSlotRef.EntityID, _toolSlotRef.ToolSlotID);

        #endregion
    }
}

