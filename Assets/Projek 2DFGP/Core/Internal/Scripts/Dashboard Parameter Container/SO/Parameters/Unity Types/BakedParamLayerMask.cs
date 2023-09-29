using UnityEngine;

namespace JT
{
    /// <summary>
    /// Baked form parameter Layer Mask.
    /// </summary>
    public class BakedParamLayerMask : BakedParameter<LayerMask>
    {
        #region Constructor

        public BakedParamLayerMask(LayerMask value) : base(value) { }

        #endregion
    }
}
