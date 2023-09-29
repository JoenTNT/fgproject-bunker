using UnityEngine;

namespace JT
{
    /// <summary>
    /// Parameter Sprite Renderer type.
    /// </summary>
    public class ParamSpriteRenderer : Parameter<SpriteRenderer>
    {
        #region Parameter

        public override BakedParameter<SpriteRenderer> Bake()
        {
            return new BakedParamSpriteRenderer(Value);
        }

        #endregion
    }
}
