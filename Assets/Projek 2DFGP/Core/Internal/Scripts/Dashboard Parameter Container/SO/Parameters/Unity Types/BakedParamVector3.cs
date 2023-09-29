using UnityEngine;

namespace JT
{
    /// <summary>
    /// Baked form parameter Vector 3D.
    /// </summary>
    public class BakedParamVector3 : BakedParameter<Vector3>
    {
        #region Constructor

        public BakedParamVector3(Vector3 value) : base(value) { }

        #endregion
    }
}
