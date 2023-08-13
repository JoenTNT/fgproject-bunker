using UnityEngine;

namespace JT.FGP
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class UI_Inventory : MonoBehaviour
    {
        #region Properties

        public RectTransform rectTransform => (RectTransform)transform;

        #endregion
    }
}

