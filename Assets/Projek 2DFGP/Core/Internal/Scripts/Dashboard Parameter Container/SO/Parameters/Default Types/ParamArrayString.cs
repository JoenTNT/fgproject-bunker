namespace JT
{
    /// <summary>
    /// Parameter array of string type.
    /// </summary>
    public class ParamArrayString : Parameter<string[]>
    {
        #region Parameter

        public override BakedParameter<string[]> Bake()
        {
            return new BakedParamArrayString(Value);
        }

        #endregion
    }
}
