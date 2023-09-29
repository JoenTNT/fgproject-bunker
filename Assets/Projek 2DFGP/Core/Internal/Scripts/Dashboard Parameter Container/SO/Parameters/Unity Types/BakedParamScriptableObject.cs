using UnityEngine;

namespace JT
{
    /// <summary>
    /// Baked form parameter Scriptable Object.
    /// </summary>
    public class BakedParamScriptableObject : BakedParameter<ScriptableObject>
    {
        #region Constructor

        public BakedParamScriptableObject(ScriptableObject value) : base(value) { }

        #endregion
    }
}
