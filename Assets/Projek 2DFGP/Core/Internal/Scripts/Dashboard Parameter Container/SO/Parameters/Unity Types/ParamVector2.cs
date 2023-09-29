using UnityEngine;

namespace JT
{
    /// <summary>
    /// Parameter Vector 2D type.
    /// </summary>
    public class ParamVector2 : Parameter<Vector2>
    {
        #region Parameter

        public override BakedParameter<Vector2> Bake()
        {
            return new BakedParamVector2(Value);
        }

        #endregion
    }
}
