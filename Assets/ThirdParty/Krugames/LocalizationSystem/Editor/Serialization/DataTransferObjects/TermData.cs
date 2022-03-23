using System;
using System.IO;
using Krugames.LocalizationSystem.Common.Extensions;
using Krugames.LocalizationSystem.Editor.Serialization.Utility;
using Krugames.LocalizationSystem.Models;
using Krugames.LocalizationSystem.Models.Utility.Editor;
using UnityEditor;
using UnityEngine;

namespace Krugames.LocalizationSystem.Editor.Serialization.DataTransferObjects {
    [Serializable]
    public struct TermData {
        public string term;
        public object value;
        public string valueType;
        public bool isAsset;

        public TermData(string term, object value, bool isAsset, Type valueType) {
            this.term = term;
            this.value = (JsonPrimitiveChecker.IsPrimitive(value)) ? value : JsonUtility.ToJson(value);
            this.isAsset = isAsset;
            this.valueType = valueType.FullName;
        }

        public TermData(LocaleTerm localeTerm) {
            Type termType = localeTerm.GetType();
            Type valueType = null;
            if (termType.IsInheritor(LocaleTermUtility.BaseGenericType)) {
                valueType = LocaleTermUtility.GetValueTypeOfGenericTermType(termType);
            } else {
                valueType = localeTerm.Value.GetType();
            }

            object value = localeTerm.Value;
            bool isAsset = false;
            if (value is UnityEngine.Object unityObject) {
                string path = AssetDatabase.GetAssetPath(unityObject);
                value = Path.GetFileName(path);
                isAsset = true;
            }
            
            this.term = localeTerm.Term;
            this.value = (JsonPrimitiveChecker.IsPrimitive(value)) ? value : JsonUtility.ToJson(value);
            this.isAsset = isAsset;
            this.valueType = valueType.FullName;
        }
    }
}