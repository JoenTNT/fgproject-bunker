using UnityEngine;

namespace JT
{
    /// <summary>
    /// Parameter Unity Object type.
    /// </summary>
    public class ParamUnityObject : Parameter<Object>
    {
        #region Parameter

        public override BakedParameter<Object> Bake()
        {
            return new BakedParamUnityObject(Value);
        }

        #endregion
    }
}
