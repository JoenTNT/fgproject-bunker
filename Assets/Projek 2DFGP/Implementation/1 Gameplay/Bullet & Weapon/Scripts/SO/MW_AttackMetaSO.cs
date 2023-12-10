using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Infomation on how does the swinging works.
    /// </summary>
    public abstract class MW_AttackMetaSO : ScriptableObject, IRuntimeMotion
    {
        #region Variables

        [Header("Base Properties")]
        [SerializeField]
        private AnimationCurve _runtimeMotion = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        #endregion

        #region IRuntimeTransform

        public AnimationCurve RuntimeMotion => _runtimeMotion;

        #endregion
    }
}