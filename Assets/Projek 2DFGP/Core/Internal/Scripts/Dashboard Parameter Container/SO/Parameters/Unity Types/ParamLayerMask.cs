using UnityEngine;

namespace JT
{
    /// <summary>
    /// Parameter layer mask type.
    /// </summary>
    public class ParamLayerMask : Parameter<LayerMask>
    {
        #region Parameter

        public override BakedParameter<LayerMask> Bake()
        {
            return new BakedParamLayerMask(Value);
        }

        #endregion
    }
}
