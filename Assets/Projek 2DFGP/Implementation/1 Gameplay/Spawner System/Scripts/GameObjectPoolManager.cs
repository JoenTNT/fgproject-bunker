using System.Collections.Generic;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle creating game object pool in runtime.
    /// </summary>
    public sealed class GameObjectPoolManager : MonoBehaviour, IRequiredInitialize
    {
        #region struct

        /// <summary>
        /// Each pool has key object.
        /// </summary>
        [System.Serializable]
        private struct KeyObjectPool
        {
            #region Variables

            public string key;
            public GameObjectPool pool;

            #endregion
        }

        #endregion

        #region Variables

        // Singleton behaviour.
        private static GameObjectPoolManager s_instance = null;

        [Header("Requirements")]
        [SerializeField]
        private KeyObjectPool[] _registeredPools = null;

        // Runtime variable data.
        private Dictionary<string, GameObjectPool> _regGODictPools = null;
        private Dictionary<string, Queue<object>> _regNonMonoPools = null;
        private bool _isInit = false;

        #endregion

        #region Properties

        /// <summary>
        /// Singleton manager instance.
        /// </summary>
        public static GameObjectPoolManager Instance => s_instance;

        #endregion

        #region Mono

        private void Awake()
        {
            // Check singleton already exists.
            if (s_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            // Assign singleton.
            s_instance = this;

            // Initialize.
            Initialize();
        }

        private void OnDestroy()
        {
            // Release cache.
            if (s_instance == this)
                s_instance = null;
        }

        #endregion

        #region IRequiredInitialize

        public bool IsInitialized => _isInit;

        public void Initialize()
        {
            // Check has been initialized, then ignore.
            if (_isInit) return;

            // Initialize dictionary of key pools.
            _regGODictPools = new Dictionary<string, GameObjectPool>();
            for (int i = 0; i < _registeredPools.Length; i++)
            {
                // Ignore unregistered pool.
                if (_registeredPools[i].pool == null) continue;

                // Register pool.
                _regGODictPools[_registeredPools[i].key] = _registeredPools[i].pool;
            }
            _regNonMonoPools = new();
            _isInit = true;
        }

        #endregion

        #region Main

        /// <summary>
        /// Get game object pool by key.
        /// </summary>
        /// <param name="key">Pool target key</param>
        /// <returns>Game object pool.</returns>
        public GameObjectPool GetGameObjPool(string key) => _regGODictPools[key];

        /// <summary>
        /// Get non monobehaviour object pool by key.
        /// </summary>
        /// <param name="key">Pool target key</param>
        /// <returns>Non Mono object pool.</returns>
        public Queue<object> GetNonMonoPool(string key)
        {
            if (!_regNonMonoPools.ContainsKey(key))
                _regNonMonoPools[key] = new Queue<object>();
            return _regNonMonoPools[key];
        }

        #endregion
    }
}

