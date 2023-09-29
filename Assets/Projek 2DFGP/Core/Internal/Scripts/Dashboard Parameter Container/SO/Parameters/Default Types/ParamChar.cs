namespace JT
{
    /// <summary>
    /// Parameter character type.
    /// </summary>
    public class ParamChar : Parameter<char>
    {
        #region Parameter

        public override BakedParameter<char> Bake()
        {
            return new BakedParamChar(Value);
        }

        #endregion
    }
}
