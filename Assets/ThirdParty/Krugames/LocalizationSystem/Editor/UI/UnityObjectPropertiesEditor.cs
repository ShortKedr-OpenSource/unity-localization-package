using UnityEditor;
using UnityEngine;

//TODO GameObject support

namespace Krugames.LocalizationSystem.Editor.UI {
    /// <summary>
    /// Shows default unity object editor.
    /// Similar to inspector view.
    /// Can not display GameObject default inspector view.
    /// </summary>
    public class UnityObjectPropertiesEditor : EditorWindow{
        
        private Object _unityObject;
        private UnityEditor.Editor _objectEditor;
        private bool _allowDrawing = false;

        public static UnityObjectPropertiesEditor Create(Object unityObject) {
            if (unityObject == null) return null;
            UnityObjectPropertiesEditor editor = EditorWindow.GetWindow<UnityObjectPropertiesEditor>();
            editor._unityObject = unityObject;
            editor.titleContent = new GUIContent(unityObject.name, AssetDatabase.GetCachedIcon(AssetDatabase.GetAssetPath(unityObject)));
            editor.minSize = new Vector2(300, 200);
            editor.Initialize();
            return editor;
        }

        private void Initialize() {
            _objectEditor = UnityEditor.Editor.CreateEditor(_unityObject);
            if (_objectEditor != null) _allowDrawing = true;
        }

        private void OnGUI() {
            if (!_allowDrawing) return;
            _objectEditor.DrawHeader();
            EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
            _objectEditor.DrawDefaultInspector();
            EditorGUILayout.EndVertical();
        }
    }
}