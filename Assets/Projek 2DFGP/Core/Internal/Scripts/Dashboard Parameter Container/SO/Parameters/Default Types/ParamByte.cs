namespace JT
{
    /// <summary>
    /// Parameter byte type.
    /// </summary>
    public class ParamByte : Parameter<byte>
    {
        #region Parameter

        public override BakedParameter<byte> Bake()
        {
            return new BakedParamByte(Value);
        }

        #endregion
    }
}
