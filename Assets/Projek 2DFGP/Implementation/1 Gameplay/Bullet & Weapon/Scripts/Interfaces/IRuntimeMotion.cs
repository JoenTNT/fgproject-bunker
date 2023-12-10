using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle runtime update using motion animation.
    /// </summary>
    public interface IRuntimeMotion
    {
        /// <summary>
        /// From start to end physical attack motion.
        /// </summary>
        AnimationCurve RuntimeMotion { get; }
    }
}
