using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Ammunition counter information for any entity.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Ammo Info Preset",
        menuName = "FGP/Presets/Ammo Information")]
    public sealed class RW_AmmoInfoSO : ScriptableObject, ICreateRuntimeObject<RuntimeAmmoInfo>,
        ICopyValue<RuntimeAmmoInfo>
    {
        #region Variables

        [SerializeField]
        private RuntimeAmmoInfo _presetData = null;

        #endregion

        #region ICreateRuntimeObject

        public RuntimeAmmoInfo CreateRuntimeObject() => (RuntimeAmmoInfo)_presetData.Clone();

        #endregion

        #region ICopyValue

        public void CopyValue(RuntimeAmmoInfo target) => _presetData.CopyValue(target);

        #endregion
    }
}
