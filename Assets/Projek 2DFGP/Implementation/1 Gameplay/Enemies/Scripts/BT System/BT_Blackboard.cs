using UnityEngine;

namespace JT
{
    /// <summary>
    /// Containing data of blackboards.
    /// </summary>
    public sealed class BT_Blackboard : MonoBehaviour, IBTBlackboard
    {
        #region Variables

        #endregion

        #region IBTBlackboard

        public T GetReference<T>(string paramKey) where T : Object
        {
            

            return default(T);
        }

        #endregion
    }

}
