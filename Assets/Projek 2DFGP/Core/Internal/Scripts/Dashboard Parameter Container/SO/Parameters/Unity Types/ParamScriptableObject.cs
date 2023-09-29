using UnityEngine;

namespace JT
{
    /// <summary>
    /// Parameter Scriptable Object type.
    /// </summary>
    public class ParamScriptableObject : Parameter<ScriptableObject>
    {
        #region Parameter

        public override BakedParameter<ScriptableObject> Bake()
        {
            return new BakedParamScriptableObject(Value);
        }

        #endregion
    }
}
