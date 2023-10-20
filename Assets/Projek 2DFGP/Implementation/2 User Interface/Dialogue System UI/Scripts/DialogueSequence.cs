using System.Collections.Generic;
using UnityEngine;

namespace JT.FGP.Dialogue
{
    /// <summary>
    /// One conversation sequence stored in one asset.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Dialogue Sequence",
        menuName = "FGP/Dialogue/Dialogue Sequence")]
    public class DialogueSequence : ScriptableObject
    {
        #region Variable

        [SerializeField]
        private List<WrappedSentence> _sentences = new();

        #endregion

        #region Properties

        /// <summary>
        /// How many sentences in one sequence.
        /// </summary>
        public int SentenceCount => _sentences.Count;

        #endregion

        #region Main

        public ISingleText GetContent(int index)
        {
            return _sentences[index];
        }

        #endregion
    }
}
