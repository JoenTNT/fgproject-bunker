using System.Collections.Generic;
using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle creating game object pool in runtime.
    /// </summary>
    public sealed class GameObjectPoolManager : MonoBehaviour
    {
        #region struct

        /// <summary>
        /// Each pool has key object.
        /// </summary>
        [System.Serializable]
        private struct KeyObjectPool
        {
            public string key;
            public GameObjectPool pool;
        }

        #endregion

        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private KeyObjectPool[] _registeredPools = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventTwoString _requestPoolCallback = null;

        [SerializeField]
        private GameEventStringUnityObject _assignPoolCallback = null;

        // Runtime variable data.
        private Dictionary<string, GameObjectPool> _regDictPools = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Initialize dictionary of key pools.
            _regDictPools = new Dictionary<string, GameObjectPool>();
            for (int i = 0; i < _registeredPools.Length; i++)
            {
                // Ignore unregistered pool.
                if (_registeredPools[i].pool == null) continue;

                // Register pool.
                _regDictPools[_registeredPools[i].key] = _registeredPools[i].pool;
            }

            // Subscribe events
            _requestPoolCallback.AddListener(ListenRequestPollCallback);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _requestPoolCallback.RemoveListener(ListenRequestPollCallback);
        }

        #endregion

        #region Main

        private void ListenRequestPollCallback(string sourceId, string poolId)
        {
            // Send pool reference.
            _assignPoolCallback.Invoke(sourceId, _regDictPools[poolId]);
        }

        #endregion
    }
}

