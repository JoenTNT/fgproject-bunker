using System.Collections.Generic;

namespace JT
{
    /// <summary>
    /// Bake data to lightweight type.
    /// </summary>
    public interface IBakedObject
    {
        /// <summary>
        /// Bake all data into conventional type.
        /// </summary>
        /// <param name="parameters">Registered parameters from dashboard.</param>
        /// <returns>Baked data</returns>
        public void Bake(IReadOnlyDictionary<string, BakedParameter> parameters);
    }
}
