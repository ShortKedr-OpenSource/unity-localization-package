using System;
using System.IO;
using Krugames.LocalizationSystem.Translation.LanguageEncoding;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

//TODO move method locale creation|deletion and etc to LocaleLibrary, review this;
//TODO add undo
namespace Krugames.LocalizationSystem.Models.Utility.Editor {
    public static class LocaleLibraryUtility {

        private const string LocalesFolderPath = "Assets/Resources/Localization/Locales/";
        
        /// <summary>
        /// Add new Locale to Locale Library
        /// </summary>
        public static Locale AddLocaleToLibrary(SystemLanguage language) {
            
            if (Application.isPlaying) {
                Debug.LogError("Static Library can not be changed in runtime!");
                return null;
            }
            
            CreateLocalesFolderIfNotExists();

            Locale newLocale = ScriptableObject.CreateInstance<Locale>();
            newLocale.name = GenerateNameByLanguage(language);

            LocaleUtility.SetLanguage(newLocale, language);

            string assetPath = AssetDatabase.GenerateUniqueAssetPath(LocalesFolderPath + newLocale.name + ".asset");
            AssetDatabase.CreateAsset(newLocale, assetPath);
            AssetDatabase.Refresh();

            LocaleLibrary library = LocaleLibrary.Instance;
#pragma warning disable CS0618
            bool result;
            if (library.BaseLocale == null) result = library.SetBaseStaticLocale(newLocale);
            else result = library.AddStaticLocale(newLocale);
#pragma warning restore CS0618
            EditorUtility.SetDirty(library);

            if (!result) {
                AssetDatabase.DeleteAsset(assetPath);
                AssetDatabase.Refresh();
                Object.DestroyImmediate(newLocale);
                return null;
            }
            return newLocale;
        }

        public static bool RemoveLocaleFromLibrary(Locale locale) {
            
            if (Application.isPlaying) {
                Debug.LogError("Static Library can not be changed in runtime!");
                return false;
            }

            LocaleLibrary library = LocaleLibrary.Instance;
#pragma warning disable CS0618
            bool result = library.RemoveStaticLocale(locale);
#pragma warning restore CS0618
            if (result && AssetDatabase.IsMainAsset(locale)) {
                string path = AssetDatabase.GetAssetPath(locale);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.Refresh();
            }
            
            return result;
        }

        public static bool SortLocaleLibraryStaticLocales() {
#pragma warning disable CS0618
            return LocaleLibrary.Instance.SortStaticLocales();
#pragma warning restore CS0618
        }

        public static void RenameLocaleAssetToLanguageMatch(Locale locale) {
            string newName = GenerateNameByLanguage(locale.Language);
            if (locale.name == newName) return;
            string oldPath = AssetDatabase.GetAssetPath(locale);
            string newPath = (Path.GetDirectoryName(oldPath) + "\\" + newName + ".asset").Replace('\\', '/');
            string validatedPath = AssetDatabase.GenerateUniqueAssetPath(newPath);
            string validatedName = Path.GetFileName(validatedPath);
            AssetDatabase.RenameAsset(oldPath, validatedName);
            AssetDatabase.Refresh();
        }

        private static void CreateLocalesFolderIfNotExists() {
            if (AssetDatabase.IsValidFolder(LocalesFolderPath)) return;
            string[] pathParts = LocalesFolderPath.Split(new[]{'\\', '/'}, StringSplitOptions.RemoveEmptyEntries);
            string previousPath;
            string currentPath = pathParts[0];
            for (int i = 1; i < pathParts.Length; i++) {
                previousPath = currentPath;
                currentPath += "/" + pathParts[i];
                if (!AssetDatabase.IsValidFolder(currentPath)) {
                    AssetDatabase.CreateFolder(previousPath, pathParts[i]);
                }
            }
        }

        private static string GenerateNameByLanguage(SystemLanguage language) {
            return SystemLanguageEncoding.Iso639_1.GetCode(language) + "-locale";
        }

    }
}