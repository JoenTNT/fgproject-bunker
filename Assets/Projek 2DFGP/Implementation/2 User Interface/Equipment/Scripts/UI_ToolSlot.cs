using JT.GameEvents;
using UnityEngine;
using UnityEngine.UI;

namespace JT.FGP
{
    /// <summary>
    /// Equipment tool slot.
    /// </summary>
    public sealed class UI_ToolSlot : MonoBehaviour, IEntityID<string>
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private UI_ImageChanger _iconChanger = null;

        [SerializeField]
        private Image _fillBarImage = null;

        [Header("Properties")]
        [SerializeField]
        private string _targetEntityID = string.Empty;

        [SerializeField]
        private string _toolSlotID = string.Empty;

        [Header("Game Events")]
        [SerializeField]
        private GameEventStringUnityObject _injectUIInfoCallback = null;

        #endregion

        #region Properties

        /// <summary>
        /// This tool slot ID to connect with owner's physical slot.
        /// </summary>
        public string ToolSlotID => _toolSlotID;

        #endregion

        #region Mono

        private void Start() => _injectUIInfoCallback.Invoke(_targetEntityID, this);

        #endregion

        #region IEntityID

        public string EntityID
        {
            get => _targetEntityID;
            set => _targetEntityID = value;
        }

        #endregion

        #region Main

        /// <summary>
        /// Set UI tool slot item icon.
        /// </summary>
        /// <param name="s">Item icon</param>
        public void SetIcon(Sprite s) => _iconChanger.ChangeImage(s);

        /// <summary>
        /// To set filler percentage.
        /// </summary>
        /// <param name="p">Equal or between 0 and 1</param>
        public void SetFillPercentage(float p) => _fillBarImage.fillAmount = p;

        #endregion
    }
}
