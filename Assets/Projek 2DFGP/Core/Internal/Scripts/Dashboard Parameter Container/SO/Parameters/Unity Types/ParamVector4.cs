using UnityEngine;

namespace JT
{
    /// <summary>
    /// Parameter Vector 4D type.
    /// </summary>
    public class ParamVector4 : Parameter<Vector4>
    {
        #region Parameter

        public override BakedParameter<Vector4> Bake()
        {
            return new BakedParamVector4(Value);
        }

        #endregion
    }
}
