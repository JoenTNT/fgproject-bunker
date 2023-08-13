using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handles scene management in game.
    /// </summary>
    public class GameSceneManager : MonoBehaviour
    {
        #region Variable

        private static GameSceneManager s_instance = null;

        #endregion

        #region Properties

        public static GameSceneManager Instance => s_instance;

        #endregion

        #region Mono

        // TODO: Singleton

        #endregion
    }
}
