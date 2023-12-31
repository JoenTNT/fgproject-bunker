using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Finite state machine system control.
    /// </summary>
    public class FSMSO_Control : MonoBehaviour
    {
        #region structs

        /// <summary>
        /// Pair of key and value of status.
        /// </summary>
        [System.Serializable]
        private struct StatusStructure
        {
            public string k; // Key
            public int v; // Value
        }

        #endregion
    }
}

