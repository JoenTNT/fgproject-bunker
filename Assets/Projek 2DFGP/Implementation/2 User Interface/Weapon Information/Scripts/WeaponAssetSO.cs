using System.Collections.Generic;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Asset combination for weapon in one place.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Weapon Asset",
        menuName = "FGP/Arsenal/Weapon Asset")]
    public sealed class WeaponAssetSO : ScriptableObject
    {
        #region structs

        /// <summary>
        /// Data structure assets for target weapon.
        /// </summary>
        [System.Serializable]
        private struct Structure
        {
            #region Variables

            [SerializeField]
            private string _weaponKey;

            [SerializeField]
            private Sprite _weaponIcon;

            #endregion

            #region Properties

            /// <summary>
            /// Unique key of the weapon.
            /// </summary>
            public string Key => _weaponKey;

            /// <summary>
            /// Display icon on the User Interface.
            /// </summary>
            public Sprite Icon => _weaponIcon;

            #endregion
        }

        #endregion

        #region Variables

        [Header("Properties")]
        [SerializeField, Tooltip("Registered Data, must be unique.")]
        private Structure[] _data = new Structure[0];

        // Runtime variable data.
        private Dictionary<string, Structure> _dictData = new();

        #endregion

        #region SO

        private void OnEnable() => InitRegistry();

        #endregion

        #region Main

        private void InitRegistry()
        {
            // Create dictionary if not yet created.
            if (_dictData == null)
                _dictData = new Dictionary<string, Structure>();

            // Register all data structures.
            for (int i = 0; i < _data.Length; i++)
            {
                // Skip empty key structure.
                if (string.IsNullOrEmpty(_data[i].Key))
                    continue;

                // Add a structure to dictionary.
                _dictData[_data[i].Key] = _data[i];
            }
        }

        [ContextMenu("Sort Data Ascending")]
        private void SortAscending()
        {

        }

        [ContextMenu("Sort Data Descending")]
        private void SortDescending()
        {

        }

        /// <summary>
        /// Get sprite icon for User interface.
        /// </summary>
        /// <param name="key"></param>
        public Sprite GetIcon(string key)
        {
            // Check empty asset, then init the dictionary.
            if (_dictData == null || _dictData.Count == 0)
                InitRegistry();

            // Search and return if exists.
            return _dictData.ContainsKey(key) ? _dictData[key].Icon : null;
        }

        #endregion
    }
}
