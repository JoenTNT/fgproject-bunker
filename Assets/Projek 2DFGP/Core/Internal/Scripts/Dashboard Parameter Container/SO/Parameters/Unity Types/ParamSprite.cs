using UnityEngine;

namespace JT
{
    /// <summary>
    /// Parameter Sprite type.
    /// </summary>
    public class ParamSprite : Parameter<Sprite>
    {
        #region Parameter

        public override BakedParameter<Sprite> Bake()
        {
            return new BakedParamSprite(Value);
        }

        #endregion
    }
}
