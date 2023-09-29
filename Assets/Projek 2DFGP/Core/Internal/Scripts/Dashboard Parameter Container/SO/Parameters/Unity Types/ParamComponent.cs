using UnityEngine;

namespace JT
{
    /// <summary>
    /// Parameter Component type.
    /// </summary>
    public class ParamComponent : Parameter<Component>
    {
        #region Parameter

        public override BakedParameter<Component> Bake()
        {
            return new BakedParamComponent(Value);
        }

        #endregion
    }
}
