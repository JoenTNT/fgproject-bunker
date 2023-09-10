using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle object pooling for game object.
    /// </summary>
    public sealed class GameObjectPool : MonoBehaviour, IObjectPoolGetter<GameObject>, IExtendable<int>
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private GameObject _prefab = null;

        [Header("Properties")]
        [SerializeField, Min(1)]
        private int _initialAmount = 10;

        [SerializeField]
        private int _extendIfEmpty = 5;

        [SerializeField]
        private bool _extendable = false;

        [Tooltip("If the pool object is all active, " +
            "then grab the next active one even thought the process ain't finish yet.\n" +
            "This option is ignored if extendable is active.")]
        [SerializeField]
        private bool _interuptWhenEmpty = false;

        [SerializeField]
        private bool _disableObjsOnStart = true;

        [Header("Optional")]
        [SerializeField]
        private Transform _parentOfObject = null;

        // Runtime variable data.
        private GameObject[] _pool = null;
        private int _latestObjectIndex = -1;

        #endregion

        #region Mono

        private void Start()
        {
            // Initialize fixed pool size.
            _pool = new GameObject[_initialAmount];

            // Generate all objects into the pool.
            for (int i = 0; i < _initialAmount; i++)
            {
                _pool[i] = Instantiate(_prefab);
                if (_disableObjsOnStart)
                    _pool[i].SetActive(false);
                if (_parentOfObject != null)
                    _pool[i].transform.SetParent(_parentOfObject);
            }
        }

        #endregion

        #region IUnityObjectPooler

        public GameObject GetObject()
        {
            // Store last index for further checking.
            int lastIndex = _latestObjectIndex;

            // Check for any inactive object, circle back to the current index.
            do
            {
                // Increase index.
                _latestObjectIndex++;

                // Check if the index is out of range, then back to zero.
                if (_latestObjectIndex >= _pool.Length) _latestObjectIndex = 0;

                // Check if object disabled, then use the object.
                if (!_pool[_latestObjectIndex].activeSelf) break;

            } while (_latestObjectIndex == lastIndex);

            // Check if looped back to current index and current object is active.
            if (lastIndex == _latestObjectIndex && _pool[_latestObjectIndex].activeSelf)
            {
                // Check extendable pool.
                if (!_extendable)
                {
                    if (_interuptWhenEmpty)
                    {
                        // Get next object index.
                        _latestObjectIndex++;
                        if (_latestObjectIndex >= _pool.Length) _latestObjectIndex = 0;
                        return _pool[_latestObjectIndex];
                    }

                    return null; // No object in pool.
                }

                // Extend the object pool.
                Extend(_extendIfEmpty);
                _latestObjectIndex = _pool.Length - 1;
            }

            // Auto active and return object out from the pool.
            _pool[_latestObjectIndex].gameObject.SetActive(_pool[_latestObjectIndex]);
            return _pool[_latestObjectIndex];
        }

        #endregion

        #region IExtendable

        public int CurrentSize => _pool.Length;

        public void Extend(int size)
        {
            // Create new array with extended size.
            GameObject[] newPoolWithSize = new GameObject[_pool.Length + size];

            // Copy all content to new array.
            for (int i = 0; i < _pool.Length; i++)
                newPoolWithSize[i] = _pool[i];

            // Add new objects into the pool.
            for (int i = _pool.Length; i < newPoolWithSize.Length; i++)
            {
                newPoolWithSize[i] = Instantiate(_prefab);
                newPoolWithSize[i].SetActive(false);
            }

            // Replace with new pool.
            _pool = newPoolWithSize;
        }

        #endregion
    }
}
