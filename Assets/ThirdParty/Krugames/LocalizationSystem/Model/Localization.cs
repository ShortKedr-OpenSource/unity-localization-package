using System;
using Krugames.LocalizationSystem.Models.Dynamic;
using Krugames.LocalizationSystem.Models.Interfaces;
using Krugames.LocalizationSystem.Models.Structs;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Krugames.LocalizationSystem.Models {
    public static class Localization {

        public static SystemLanguage CurrentUsedLanguage => throw new NotImplementedException();

        public static Locale[] StaticLocales => throw new NotImplementedException();
        
        public static DynamicLocale[] DynamicLocales => throw new NotImplementedException();
        
        public static ILocale[] AllLocales => throw new NotImplementedException();
        
        /// <summary>
        /// Dynamically add new locale to localization, during runtime.
        /// Very useful if you want to add locales dynamically via Cloud Content Delivery for example,
        /// or if dynamic approach is more suitable for your particular project
        /// </summary>
        public static void AddDynamicLocale(DynamicLocale dynamicLocale) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                Debug.LogError("Dynamic Library Update can be performed only in runtime!");
                return;
            }
#endif
            //TODO implement
        }
        
        public static void SetLanguage(SystemLanguage language) {
            //TODO implement
        }

        public static void AddLanguageUpdateCallback(Action callback) {
            //TODO implement
        }

        public static void RemoveLanguageUpdateCallback(Action callback) {
            //TODO implement
        }

        public static object GetTermValue(string term) {
            throw new NotImplementedException();
        }

        public static TTermValueType GetTermValue<TTermValueType>(string term) {
            throw new NotImplementedException();
        }

        public static object GetTermValueFromLanguage(string term, SystemLanguage language) {
            throw new NotImplementedException();
        }
        
        public static TTermValueType GetTermValueFromLanguage<TTermValueType>(string term, SystemLanguage language) {
            throw new NotImplementedException();
        }

        public static void UnloadUnusedResources() {
            throw new NotImplementedException();
            //TODO clear unused ScriptableObject Resources, such as unreferenced LocaleTerms, Locales;
        }

        public static TermStructureInfo[] GetBaseTermLayout() {
            throw new NotImplementedException();
        }
    }
}