using UnityEngine;

namespace JT
{
    /// <summary>
    /// Baked form parameter Vector 4D.
    /// </summary>
    public class BakedParamVector4 : BakedParameter<Vector4>
    {
        #region Constructor

        public BakedParamVector4(Vector4 value) : base(value) { }

        #endregion
    }
}
