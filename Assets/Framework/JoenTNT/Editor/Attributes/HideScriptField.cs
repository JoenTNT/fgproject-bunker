using UnityEditor;
using UnityEngine;

namespace JT.Editors
{
    [CustomPropertyDrawer(typeof(HideScriptFieldAttribute))]
    public class HideScriptField : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue != null)
            {
                MonoScript script = MonoScript.FromMonoBehaviour((MonoBehaviour)property.objectReferenceValue);
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.ObjectField(position, label, script, typeof(MonoScript), false);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}

