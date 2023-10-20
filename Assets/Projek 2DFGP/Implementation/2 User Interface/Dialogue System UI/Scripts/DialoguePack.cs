using System.Collections.Generic;
using UnityEngine;

namespace JT.FGP.Dialogue
{
    /// <summary>
    /// One pack of dialogue 
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Dialogue Pack",
        menuName = "FGP/Dialogue/Dialogue Pack")]
    public class DialoguePack : ScriptableObject
    {
        #region Variable

        [SerializeField]
        private DialogueSequence[] _registeredSequences = new DialogueSequence[0];

        #endregion

        #region Properties

        /// <summary>
        /// Get dialogue sequence by name.
        /// </summary>
        /// <param name="sequenceName"></param>
        /// <returns></returns>
        public DialogueSequence this[string sequenceName]
        {
            get
            {
                for (int i = 0; i < _registeredSequences.Length; i++)
                {
                    if (_registeredSequences[i] == null) continue;
                    if (_registeredSequences[i].name != sequenceName) continue;

                    return _registeredSequences[i];
                }

                return null;
            }
        }

        #endregion
    }
}