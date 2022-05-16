using UnityEditor;
using UnityEngine;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    public class LocalizationEditor : EditorWindow {

        private static readonly Vector2 MinSize = new Vector2(800, 500);
        private static readonly Vector2 DefaultSize = new Vector2(800, 500);
        
        [MenuItem("Window/Krugames/Localization")]
        public static void Open() {
            LocalizationEditor editorWindow = EditorWindow.GetWindow<LocalizationEditor>("Localization", true);
            editorWindow.minSize = MinSize;
            editorWindow.position = new Rect(editorWindow.position.position, DefaultSize);
            editorWindow.Show();
        }
        
        //TODO implement
    }
}