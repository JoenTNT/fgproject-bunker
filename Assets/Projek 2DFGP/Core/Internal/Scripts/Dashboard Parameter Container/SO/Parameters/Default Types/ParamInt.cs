namespace JT
{
    /// <summary>
    /// Parameter integer type.
    /// </summary>
    public class ParamInt : Parameter<int>
    {
        #region Parameter

        public override BakedParameter<int> Bake()
        {
            return new BakedParamInt(Value);
        }

        #endregion
    }
}
