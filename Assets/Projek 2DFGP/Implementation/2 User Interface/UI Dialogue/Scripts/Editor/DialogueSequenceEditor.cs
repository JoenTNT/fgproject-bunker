using UnityEditor;

namespace JT.FGP.Dialogue
{
    [CustomEditor(typeof(DialogueSequence))]
    internal class DialogueSequenceEditor : Editor
    {
        #region Editor

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Create each Dialogue sentences one by one.\n" +
                "You can use delay between the string, for example\"<delay s='5'>\"", MessageType.Info);
            base.OnInspectorGUI();
        }

        #endregion
    }
}
