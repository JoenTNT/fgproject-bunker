using System.Collections.Generic;
namespace JT
{
    /// <summary>
    /// Baked object class implementation.
    /// This is a sample that contains one parameter, you can follow the template of DPC Architecture.
    /// </summary>
    public class SampleBaked : IBakedObject
    {
        #region Variables

        // Runtime variable data.
        private BakedParamInt _sampleInt1 = null;

        #endregion

        #region Properties

        /// <summary>
        /// Sample integer one.
        /// </summary>
        public int SampleInt1
        {
            get => _sampleInt1.Value;
            set => _sampleInt1.Value = value;
        }

        #endregion

        #region IBakeDashboardTo

        public void Bake(IReadOnlyDictionary<string, BakedParameter> parameters)
        {
            _sampleInt1 = (BakedParamInt)parameters["Sample Int"];
        }

        #endregion
    }
}
