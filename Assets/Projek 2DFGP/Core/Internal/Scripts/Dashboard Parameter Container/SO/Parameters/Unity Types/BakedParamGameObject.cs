using UnityEngine;

namespace JT
{
    /// <summary>
    /// Baked form parameter Game Object.
    /// </summary>
    public class BakedParamGameObject : BakedParameter<GameObject>
    {
        #region Constructor

        public BakedParamGameObject(GameObject value) : base(value) { }

        #endregion
    }
}
