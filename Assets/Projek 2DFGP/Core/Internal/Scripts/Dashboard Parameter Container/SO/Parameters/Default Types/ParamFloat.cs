namespace JT
{
    /// <summary>
    /// Parameter floating number type.
    /// </summary>
    public class ParamFloat : Parameter<float>
    {
        #region Parameter

        public override BakedParameter<float> Bake()
        {
            return new BakedParamFloat(Value);
        }

        #endregion
    }
}
