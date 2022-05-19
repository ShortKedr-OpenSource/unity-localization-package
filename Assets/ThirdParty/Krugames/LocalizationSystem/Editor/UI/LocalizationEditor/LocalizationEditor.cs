using Krugames.LocalizationSystem.Editor.Package;
using Krugames.LocalizationSystem.Models;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    
    /// <summary>
    /// Localization Package main editor.
    /// </summary>
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

        public LocalizationEditor() {
        }
        
        public void CreateGUI() {
            VisualElement root = rootVisualElement;
            
            string uxmlPath = PackageVariables.PackagePath + "Editor/UI/LocalizationEditor/LocalizationEditor.uxml";
            string ussPath = PackageVariables.PackagePath + "Editor/UI/LocalizationEditor/LocalizationEditor.uss";

            VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(ussPath);

            if (visualTreeAsset != null) visualTreeAsset.CloneTree(rootVisualElement);
            else Debug.LogError($"{nameof(LocalizationEditor)} uxml schema asset not found!");

            if (styleSheet != null) rootVisualElement.styleSheets.Add(styleSheet);
            else Debug.LogError($"{nameof(LocalizationEditor)} uss style sheet asset not found!");
            
            AssignVisualTreeComponents();
        }

        private void AssignVisualTreeComponents() {
            /*LocaleLibrary library = LocaleLibrary.Instance;
            SerializedProperty property = new SerializedObject(library).FindProperty("baseLocale");
            rootVisualElement.Q<PropertyField>("BaseLocaleField").BindProperty(property);*/
        }
        
    }
}