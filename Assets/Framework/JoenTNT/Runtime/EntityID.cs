#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace JT
{
    /// <summary>
    /// Handling owner ID for entity.
    /// </summary>
    public class EntityID : MonoBehaviour
    {
        #region enum

        /// <summary>
        /// Enum for assign owner ID option.
        /// </summary>
        private enum Mode { Name = 0, InstanceID = 1, Manual = 2, }

        #endregion

        #region Variable

        /// <summary>
        /// Mode to assign owner ID.
        /// </summary>
        [SerializeField]
        private Mode _mode = Mode.Name;

        /// <summary>
        /// ID for this owner.
        /// </summary>
        [SerializeField]
        private string _id = string.Empty;

        // Temp variable data
        private bool _isInit = false;

        #endregion

        #region Properties

        /// <summary>
        /// Entity's ID value.
        /// </summary>
        public string ID
        {
            get
            {
                Init();
                return _id;
            }
            set => _id = value;
        }

        #endregion

        #region Mono

        private void Awake() => Init();

        #endregion

        #region Main

        private void Init()
        {
            if (_isInit) return;

            switch (_mode)
            {
                case Mode.InstanceID:
                    _id = gameObject.GetInstanceID().ToString();
                    break;

                case Mode.Name:
                    _id = gameObject.name;
                    break;
            }

            _isInit = true;
        }

        #endregion
    }
}