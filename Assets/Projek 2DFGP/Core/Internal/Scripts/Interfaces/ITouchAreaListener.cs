using UnityEngine;
using UnityEngine.EventSystems;

namespace JT.FGP
{
    /// <summary>
    /// Touch area event listeners.
    /// </summary>
    public interface ITouchAreaListener
    {
        /// <summary>
        /// When touch press inside the area.
        /// </summary>
        void OnTouchAreaDown(PointerEventData eventData);

        /// <summary>
        /// When touch is being drag.
        /// </summary>
        void OnTouchDrag(PointerEventData eventData);

        /// <summary>
        /// When touch has been released the area.
        /// </summary>
        void OnTouchAreaUp(PointerEventData eventData);
    }
}

