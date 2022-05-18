using System;
using Krugames.LocalizationSystem.Common.Extensions;
using Krugames.LocalizationSystem.Models.Structs;
using Krugames.LocalizationSystem.Common.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Krugames.LocalizationSystem.Models.Utility.Editor {
    public static class LocaleUtility {
        
        public static LocaleTerm AddLocaleTerm(Locale locale, string termName, Type termType) {
            
            if (Application.isPlaying) {
                Debug.LogError("Static Locale can not be changed in runtime!");
                return null;
            }
            
            bool validType = termType.IsInheritor(LocaleTermUtility.BaseType) && !termType.IsAbstract;
            
            if (!validType) {
                Debug.LogError($"Term of type{termType} can not be added. Specified type is not valid. " +
                                 $"TermType must be inheritor of {nameof(LocaleTerm)}");
                return null;
            }

            LocaleTerm termObject = (LocaleTerm)ScriptableObject.CreateInstance(termType);
            termObject.name = termName;

            SerializedObject serializedTermObject = new SerializedObject(termObject);
            serializedTermObject.FindProperty("term").stringValue = termName;
            serializedTermObject.ApplyModifiedPropertiesWithoutUndo();

#pragma warning disable CS0618
            bool wasAdded = locale.AddTerm(termObject);
#pragma warning restore CS0618
            if (!wasAdded) Object.DestroyImmediate(termObject);
            
            return (wasAdded) ? termObject : null;
        }

        public static bool RemoveLocaleTerm(Locale locale, LocaleTerm term) {
            
            if (Application.isPlaying) {
                Debug.LogError("Static Locale can not be changed in runtime!");
                return false;
            }
#pragma warning disable CS0618
            return locale.RemoveTerm(term);
#pragma warning restore CS0618
        }

        public static void SetLayout(Locale locale, TermStructureInfo[] layout) {
#pragma warning disable CS0618
            locale.SetLayout(layout);
#pragma warning restore CS0618
        }
        
        public static bool SetLanguage(Locale locale, SystemLanguage newLanguage) {
#pragma warning disable CS0618
            return locale.SetLanguage(newLanguage);
#pragma warning restore CS0618
        }

        public static bool ClearTerms(Locale locale) {
#pragma warning disable CS0618
            return locale.ClearTerms();
#pragma warning restore CS0618
        }
        
        public static void RenameTermSubAsset(Locale locale, LocaleTerm term) {
            if (!AssetHelper.IsSubAssetOf(term, locale)) return;
            string termPath = AssetDatabase.GetAssetPath(term);
            if (term.name != term.Term) {
                term.name = term.Term;
                EditorUtility.SetDirty(locale);
                EditorUtility.SetDirty(term);
                AssetDatabase.ImportAsset(termPath);
            }
        }
    }
}