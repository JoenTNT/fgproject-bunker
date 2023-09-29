namespace JT
{
    /// <summary>
    /// Parameter short type.
    /// </summary>
    public class ParamShort : Parameter<short>
    {
        #region Parameter

        public override BakedParameter<short> Bake()
        {
            return new BakedParamShort(Value);
        }

        #endregion
    }
}
