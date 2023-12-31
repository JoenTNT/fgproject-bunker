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
            #region Variables

            public string key;
            public GameObjectPool pool;

            #endregion
        }

        /// <summary>
        /// This manager has many requests.
        /// </summary>
        [System.Serializable]
        private struct PairOfRequest
        {
            #region Variables

            [SerializeField]
            private GameEventTwoString _requestPoolCallback;

            [SerializeField]
            private GameEventStringUnityObject _assignPoolCallback;

            #endregion

            #region Properties

            /// <summary>
            /// Event listener to request pool.
            /// </summary>
            public GameEventTwoString RequestPoolCallback => _requestPoolCallback;

            /// <summary>
            /// Event to send pool to the requester.
            /// </summary>
            public GameEventStringUnityObject AssignPoolCallback => _assignPoolCallback;

            #endregion
        }

        #endregion

        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private KeyObjectPool[] _registeredPools = null;

        [Header("Game Events")]
        [SerializeField]
        private PairOfRequest[] _requests = new PairOfRequest[0];

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

            // Subscribe events.
            for (int i = 0; i < _requests.Length; i++)
            {
                // Due to unidentified request index, then lambda method is implemented.
                int requestIndex = i;
                _requests[i].RequestPoolCallback.AddListener((sourceID, poolID) => {
                    // Call event.
                    _requests[requestIndex].AssignPoolCallback.Invoke(sourceID, _regDictPools[poolID]);
                });
            }
        }

        

        private void OnDestroy()
        {
            // Unsubscribe events
            for (int i = 0; i < _requests.Length; i++)
                _requests[i].RequestPoolCallback.RemoveAllListeners();
        }

        #endregion
    }
}

