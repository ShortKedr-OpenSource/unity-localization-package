using Krugames.LocalizationSystem.Editor.Package;
using UnityEditor;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.Styles {
    
    /// <summary>
    /// Provides LocalizationSystem-wide editor styles
    /// </summary>
    internal static class LocalizationEditorStyles {

        private const string GlobalStylePackagePath = "Editor/Styles/GlobalStyle.uss";
        private const string LocalizationEditorStylePackagePath = "Editor/UI/LocalizationEditor/LocalizationEditor.uss";
        
        
        public static readonly StyleSheet GlobalStyle;
        public static readonly StyleSheet LocalizationEditorStyle;

        static LocalizationEditorStyles() {
            GlobalStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                PackageVariables.PackagePath + GlobalStylePackagePath);

            LocalizationEditorStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                PackageVariables.PackagePath + LocalizationEditorStylePackagePath);
        }
        
    }
}