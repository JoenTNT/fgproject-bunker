using UnityEngine;

namespace JT.FGP
{
    [CreateAssetMenu(
        fileName = "New MW Slash Attack Meta",
        menuName = "FGP/Weapon/Meta/Melee Slash Attack Meta")]
    public sealed class MW_SlashAttackMetaSO : MW_AttackMetaSO
    {
        #region Variables

        [Header("Properties")]
        [SerializeField]
        private float _frontOffsetDegree = 0f;

        [Tooltip("Randomize which starting degree the melee placed on start after offseting it.")]
        [SerializeField, Min(0f)]
        private float _randomStartDegreeRange = 0f;

        [SerializeField, Min(0f)]
        private float _farSwingDegree = 0f;

        [SerializeField]
        private bool _affectLocalOnly = false;

        [Tooltip("Swings left to right, or else right to left.")]
        [SerializeField]
        private bool _isLeftHanded = false;

        #endregion

        #region Properties

        /// <summary>
        /// Does the runtime affect local transform, if it's false then world transform.
        /// </summary>
        public bool AffectLocalOnly => _affectLocalOnly;

        #endregion
    }
}
