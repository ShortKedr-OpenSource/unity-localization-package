using System;
using Krugames.LocalizationSystem.Common.Extensions;
using UnityEditor;
using UnityEngine;

namespace Krugames.LocalizationSystem.Models.Utility.Editor {
    public static class LocaleUtility {
        public static LocaleTerm AddLocaleTerm(Locale targetLocale, string termName, Type termType) {
            bool validType = termType.IsInheritor(LocaleTermUtility.BaseType) && !termType.IsAbstract;
            bool canBeAdded = targetLocale.GetTerm(termName) == null;
            bool isAsset = AssetDatabase.IsMainAsset(targetLocale);
            
            if (!validType) {
                Debug.LogWarning($"Term of type{termType} can not be added. Specified type is not valid. " +
                                 $"TermType must be inheritor of {nameof(LocaleTerm)}");
                return null;
            }

            if (!isAsset) {
                Debug.LogWarning($"Term can not be added to non asset locale. Locale must be an asset!");
                return null;
            }
            
            if (!canBeAdded) {
                Debug.LogWarning($"Term with identifier \"{termName}\" is already exists and can not be added");
                return null;
            }

            LocaleTerm termObject = (LocaleTerm)ScriptableObject.CreateInstance(termType);
            termObject.name = termName;
            AssetDatabase.AddObjectToAsset(termObject, targetLocale);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(termObject));

            return termObject;
        }

        public static bool RemoveLocaleTerm(Locale targetLocale, string termName) {
            bool isAsset = AssetDatabase.IsMainAsset(targetLocale);
            
            if (!isAsset) {
                Debug.LogWarning($"Term can not be removed from non asset locale. Locale must be an asset!");
                return false;
            }

            LocaleTerm localeTerm = targetLocale.GetTerm(termName);
            
            if (localeTerm == null) return false;

            Debug.Log(AssetDatabase.GetAssetPath(localeTerm));
            return true;
        }
    }
}