using Krugames.LocalizationSystem.Models;
using UnityEditor;

namespace Krugames.LocalizationSystem.Editor {
    [CustomEditor(typeof(LocaleTerm<>), true)]
    public class LocaleTermGenericInspector : UnityEditor.Editor {

        private SerializedProperty _termProp;
        private SerializedProperty _smartValueProp;
        
        private void OnEnable() {
            _termProp = serializedObject.FindProperty("term");
            _smartValueProp = serializedObject.FindProperty("smartValue");
        }

        public override void OnInspectorGUI() {
            EditorGUILayout.PropertyField(_termProp);
            EditorGUILayout.PropertyField(_smartValueProp);
            serializedObject.ApplyModifiedProperties();
        }
    }
}