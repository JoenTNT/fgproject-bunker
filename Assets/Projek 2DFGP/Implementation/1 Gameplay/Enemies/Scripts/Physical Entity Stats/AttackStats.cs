using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Attack stats of entity.
    /// </summary>
    public sealed class AttackStats : MonoBehaviour, IAttackPoint<float>
    {
        #region Variables

        [Header("Properties")]
        [SerializeField, Min(0f)]
        private float _attackPoint = 10f;

        #endregion

        #region IAttackPoint

        public float AttackPointDamage
        {
            get => _attackPoint;
            set => _attackPoint = value;
        }

        #endregion
    }
}
