namespace JT
{
    /// <summary>
    /// Parameter boolean type.
    /// </summary>
    public class ParamBool : Parameter<bool>
    {
        #region Parameter

        public override BakedParameter<bool> Bake()
        {
            return new BakedParamBool(Value);
        }

        #endregion
    }
}
