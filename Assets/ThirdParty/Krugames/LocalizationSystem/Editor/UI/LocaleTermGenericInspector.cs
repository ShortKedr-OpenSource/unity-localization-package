using Krugames.LocalizationSystem.Models;
using UnityEditor;

namespace Krugames.LocalizationSystem.Editor.UI {
    [CustomEditor(typeof(LocaleTerm<>), true)]
    public class LocaleTermGenericInspector : UnityEditor.Editor {
        
        public override void OnInspectorGUI() {
            serializedObject.Update();
            SerializedProperty prop = serializedObject.GetIterator();
            if (prop.NextVisible(true)) {
                do {
                    if (prop.name == "m_Script") continue;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);
                }  while (prop.NextVisible(false));
            }
            serializedObject.ApplyModifiedProperties();
        }

    }
}