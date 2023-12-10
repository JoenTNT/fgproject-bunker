using UnityEngine;
using UnityEngine.UI;

namespace JT.FGP
{
    /// <summary>
    /// Shows health bar of the entity.
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class UI_HitpointBar : MonoBehaviour, IBarValue<float>
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private Slider _healthBar = null;

        #endregion

        #region IBarValue

        public float MaxBarValue
        {
            get => _healthBar.maxValue;
            set => _healthBar.maxValue = value;
        }

        public float BarValue
        { 
            get => _healthBar.value;
            set => _healthBar.value = value;
        }
        
        #endregion
    }
}
