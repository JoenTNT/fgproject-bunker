namespace JT
{
    /// <summary>
    /// Parameter array of boolean type.
    /// </summary>
    public class ParamArrayBool : Parameter<bool[]>
    {
        #region Parameter

        public override BakedParameter<bool[]> Bake()
        {
            return new BakedParamArrayBool(Value);
        }

        #endregion
    }
}
