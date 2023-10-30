using UnityEngine;

namespace JT
{
    /// <summary>
    /// Parameter array of Vector 2D type.
    /// </summary>
    public class ParamArrayVector2 : Parameter<Vector2[]>
    {
        #region Parameter

        public override BakedParameter<Vector2[]> Bake()
        {
            return new BakedParamArrayVector2(Value);
        }

        #endregion
    }
}
