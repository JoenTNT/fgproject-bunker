using UnityEngine;

namespace JT
{
    /// <summary>
    /// Baked form parameter array of Vector 2D.
    /// </summary>
    public class BakedParamArrayVector2 : BakedParameter<Vector2[]>
    {
        #region Constructor

        public BakedParamArrayVector2(Vector2[] value) : base(value) { }

        #endregion
    }
}
