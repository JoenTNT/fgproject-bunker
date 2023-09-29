using UnityEngine;

namespace JT
{
    /// <summary>
    /// Parameter Game Object type.
    /// </summary>
    public class ParamGameObject : Parameter<GameObject>
    {
        #region Parameter

        public override BakedParameter<GameObject> Bake()
        {
            return new BakedParamGameObject(Value);
        }

        #endregion
    }
}
