using Krugames.LocalizationSystem.Editor.Package;
using UnityEditor;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.Styles {
    
    /// <summary>
    /// Provides LocalizationSystem-wide editor styles
    /// </summary>
    internal static class LocalizationEditorStyles {

        private const string GlobalStylePackagePath = "Editor/Styles/GlobalStyle.uss";
        
        public static readonly StyleSheet GlobalStyle;

        static LocalizationEditorStyles() {
            GlobalStyle = (StyleSheet)AssetDatabase.LoadAssetAtPath(
                PackageVariables.PackagePath + GlobalStylePackagePath, typeof(StyleSheet));
        }
    }
}