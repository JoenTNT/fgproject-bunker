namespace JT
{
    /// <summary>
    /// Parameter array of floating number type.
    /// </summary>
    public class ParamArrayFloat : Parameter<float[]>
    {
        #region Parameter

        public override BakedParameter<float[]> Bake()
        {
            return new BakedParamArrayFloat(Value);
        }

        #endregion
    }
}
