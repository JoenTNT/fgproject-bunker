using UnityEngine;

namespace JT
{
    /// <summary>
    /// Parameter Transform type.
    /// </summary>
    public class ParamTransform : Parameter<Transform>
    {
        #region Parameter

        public override BakedParameter<Transform> Bake()
        {
            return new BakedParamTransform(Value);
        }

        #endregion
    }
}
