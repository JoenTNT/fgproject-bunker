namespace JT
{
    /// <summary>
    /// Parameter double type.
    /// </summary>
    public class ParamDouble : Parameter<double>
    {
        #region Parameter

        public override BakedParameter<double> Bake()
        {
            return new BakedParamDouble(Value);
        }

        #endregion
    }
}
