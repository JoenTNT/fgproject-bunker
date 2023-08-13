using UnityEngine;

namespace JT.FGP.Dialogue
{
    /// <summary>
    /// Dialogue choice type.
    /// </summary>
    [System.Serializable]
    public struct WrappedChoices : ISingleText
    {
        #region Variable

        // TODO: Create multiple choice

        [Header("Contents")]
        [TextArea(3, 10)]
        private string _text;

        #endregion

        #region ISingleText

        public string Text => _text;

        #endregion
    }
}
