using JT.GameEvents;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using SM = UnityEngine.SceneManagement.SceneManager;

namespace JT.FGP
{
    /// <summary>
    /// Handles scene management in game.
    /// </summary>
    public class GameSceneManager : MonoBehaviour
    {
        #region Variable

        // Singleton instance.
        private static GameSceneManager s_instance = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventNoParam _onStartLoadingScreen = null;

        [SerializeField]
        private GameEventNoParam _onEndLoadingScreen = null;

        [SerializeField]
        private GameEventString _onSetLoadingScreenText = null;

        [SerializeField]
        private GameEventFloat _onSetLoadingValuePercent = null;

        // Runtime variable data.
        private IEnumerator _currentRoutine = null;

        #endregion

        #region Properties

        /// <summary>
        /// Game scene manager instance.
        /// </summary>
        public static GameSceneManager Instance => s_instance;

        #endregion

        #region Mono

        private void Awake()
        {
            // Check singleton object.
            if (s_instance != null)
            {
                // Destroy extra singleton object.
                Destroy(gameObject);
                return;
            }

            // Assign singleton and don't destroy on load.
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            // Ignore destroyed duplicated object.
            if (s_instance != this) return;

            // Release cache memory.
            s_instance = null;
        }

        #endregion

        #region Main

        /// <summary>
        /// Close all scene and load a new single scene.
        /// </summary>
        /// <param name="sceneName">Scene name</param>
        public void LoadSingleScene(string sceneName)
        {
            IEnumerator Routine(string sceneName)
            {
                // Loading Status started.
                _onStartLoadingScreen.Invoke();
                _onSetLoadingScreenText.Invoke("Loading...");

                // Loading Process.
                AsyncOperation op = SM.LoadSceneAsync(sceneName);
                while (!op.isDone)
                {
                    // Calculate loading scene value.
                    float progVal = op.progress / 0.9f; // Progress is done always stop at 0.9
                    _onSetLoadingValuePercent.Invoke(progVal);

                    // Tick routine.
                    yield return null;
                }

                // Loading Status ended.
                _onEndLoadingScreen.Invoke();
                _currentRoutine = null;
            }

            // Make sure loading routine cannot be interrupted.
            if (_currentRoutine != null) return;

            // Start loading new scene.
            _currentRoutine = Routine(sceneName);
            StartCoroutine(_currentRoutine);
        }

        /// <summary>
        /// Add a new scene to current opened scenes.
        /// </summary>
        /// <param name="sceneName">Scene name</param>
        public void LoadAdditiveScene(string sceneName)
        {
            IEnumerator Routine(string sceneName)
            {
                yield return null;
            }

            // Make sure loading routine cannot be interrupted.
            if (_currentRoutine != null) return;

            // Start loading new scene.
            _currentRoutine = Routine(sceneName);
            StartCoroutine(_currentRoutine);
        }

        #endregion

        #region Statics

        /// <summary>
        /// Check scene loaded by name.
        /// </summary>
        /// <param name="sceneName">Scene name</param>
        /// <returns>Is scene loaded?</returns>
        public static bool IsSceneLoaded(string sceneName)
        {
            Scene s = SM.GetSceneByName(sceneName);
            return s.isLoaded;
        }

        /// <summary>
        /// Check scene loaded by build index.
        /// </summary>
        /// <param name="scene">Scene build index</param>
        /// <returns>Is scene loaded?</returns>
        public static bool IsSceneLoaded(int sceneIndex)
        {
            Scene s = SM.GetSceneByBuildIndex(sceneIndex);
            return s.isLoaded;
        }

        #endregion
    }
}
