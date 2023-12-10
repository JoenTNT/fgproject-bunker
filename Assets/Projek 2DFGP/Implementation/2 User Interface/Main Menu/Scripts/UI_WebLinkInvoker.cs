using UnityEngine;
using UnityEngine.EventSystems;

namespace JT.FGP
{
    /// <summary>
    /// Used to open a web link when clicked.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UI_WebLinkInvoker : MonoBehaviour, IPointerClickHandler
    {
        #region Variables

        [Header("Properties")]
        [SerializeField]
        private string _link = string.Empty;

        #endregion

        #region IPointerClickHandler

        public void OnPointerClick(PointerEventData eventData)
        {
            Application.OpenURL(_link);
        }

        #endregion
    }
}
