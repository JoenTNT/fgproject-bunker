using UnityEngine;

namespace JT
{
    /// <summary>
    /// Baked form parameter Transform.
    /// </summary>
    public class BakedParamTransform : BakedParameter<Transform>
    {
        #region Constructor

        public BakedParamTransform(Transform value) : base(value) { }

        #endregion
    }
}
