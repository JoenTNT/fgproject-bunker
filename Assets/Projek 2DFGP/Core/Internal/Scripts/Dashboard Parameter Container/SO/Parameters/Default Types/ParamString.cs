namespace JT
{
    /// <summary>
    /// Parameter string type.
    /// </summary>
    public class ParamString : Parameter<string>
    {
        #region Parameter

        public override BakedParameter<string> Bake()
        {
            return new BakedParamString(Value);
        }

        #endregion
    }
}
