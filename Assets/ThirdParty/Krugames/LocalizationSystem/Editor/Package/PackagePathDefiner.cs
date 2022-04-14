using System.Text;
using UnityEditor;
using UnityEngine;

namespace Krugames.LocalizationSystem.Editor.Package {
    /// <summary>
    /// Calculates Localization package root folder "LocalizationSystem".
    /// Calculation made via MonoScript and position of this script file in project
    /// </summary>
    internal class PackagePathDefiner : ScriptableObject {
        public static string GetPackagePath() {
            PackagePathDefiner dummy = CreateInstance<PackagePathDefiner>();
            string fullDummyScriptPath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(dummy));
            string[] pathParts = fullDummyScriptPath.Split('/');
            StringBuilder packagePathBuilder = new StringBuilder();
            for (int i = 0; i < pathParts.Length - 3; i++) packagePathBuilder.Append(pathParts[i]+"/");
            Resources.UnloadUnusedAssets();
            return packagePathBuilder.ToString();
        }
    }
}