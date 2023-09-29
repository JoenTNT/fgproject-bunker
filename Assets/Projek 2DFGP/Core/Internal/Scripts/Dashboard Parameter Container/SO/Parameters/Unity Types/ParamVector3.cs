using UnityEngine;

namespace JT
{
    /// <summary>
    /// Parameter Vector 3D type.
    /// </summary>
    public class ParamVector3 : Parameter<Vector3>
    {
        #region Parameter

        public override BakedParameter<Vector3> Bake()
        {
            return new BakedParamVector3(Value);
        }

        #endregion
    }
}
