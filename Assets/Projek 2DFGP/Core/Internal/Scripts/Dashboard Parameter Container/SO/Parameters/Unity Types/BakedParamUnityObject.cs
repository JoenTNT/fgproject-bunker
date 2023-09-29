using UnityEngine;

namespace JT
{
    /// <summary>
    /// Baked form parameter Unity Object.
    /// </summary>
    public class BakedParamUnityObject : BakedParameter<Object>
    {
        #region Constructor

        public BakedParamUnityObject(Object value) : base(value) { }

        #endregion
    }
}
