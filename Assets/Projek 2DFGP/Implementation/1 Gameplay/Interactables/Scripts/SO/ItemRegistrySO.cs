using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// To register all Items into one single preset.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Item Registry",
        menuName = "FGP/Item/Item Registry", order = 0)]
    public sealed class ItemRegistrySO : ScriptableObject, IRandomizeSelection<ItemSO>
    {
        #region Variables

        [SerializeField]
        private ItemSO[] _registeredItems = new ItemSO[0];

        // Runtime variable data.
        private System.Random _rand = null;
        private int _randomIndex = 0;

        #endregion

        #region IRandomizeSelection

        public ItemSO SelectRandom()
        {
            if (_rand == null)
                _rand = new System.Random();

            _randomIndex = _rand.Next(0, _registeredItems.Length);
            return _registeredItems[_randomIndex];
        }

        #endregion
    }
}
