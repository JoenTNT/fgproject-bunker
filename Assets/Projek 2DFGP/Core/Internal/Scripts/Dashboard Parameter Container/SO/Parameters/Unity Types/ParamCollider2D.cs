using UnityEngine;

namespace JT
{
    /// <summary>
    /// Parameter Collider 2D type.
    /// </summary>
    public class ParamCollider2D : Parameter<Collider2D>
    {
        #region Parameter

        public override BakedParameter<Collider2D> Bake()
        {
            return new BakedParamCollider2D(Value);
        }

        #endregion
    }
}
