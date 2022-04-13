using System.Text;
using UnityEditor;
using UnityEngine;

namespace Krugames.LocalizationSystem.Editor.Package {
    /// <summary>
    /// Provides global Localization package variables
    /// </summary>
    internal static class PackageVariables {

        public static readonly string PackagePath;

        static PackageVariables() {
            
            ScriptableDummy dummy = ScriptableObject.CreateInstance<ScriptableDummy>();
            string fullDummyScriptPath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(dummy));
            string[] pathParts = fullDummyScriptPath.Split('/');
            StringBuilder packagePathBuilder = new StringBuilder();
            for (int i = 0; i < pathParts.Length - 3; i++) packagePathBuilder.Append(pathParts[i]+"/");
            PackagePath = packagePathBuilder.ToString();
            Resources.UnloadUnusedAssets();
            
        }
    }
}