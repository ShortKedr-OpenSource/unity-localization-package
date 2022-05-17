using System;
using Krugames.LocalizationSystem.Common.Extensions;
using Krugames.LocalizationSystem.Models.Utility;
using UnityEngine;

namespace Krugames.LocalizationSystem.Models.Attributes {
    
    /// <summary>
    /// Register LocaleTerm type in the localization system.
    /// LocaleTerm must have LocaleTerm<TValueType> in inheritance hierarchy
    /// in order to be successfully registered in the system;
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class RegisterLocaleTermAttribute : Attribute {

        public readonly Type TermType;
        public readonly string Name;

        public RegisterLocaleTermAttribute(Type termType, string name = "") {
            TermType = termType;
            Name = (name == "") 
                ? ((IsValid) ? LocaleTermUtility.GetValueTypeOfGenericTermType(TermType).Name : TermType.Name) 
                : name;
        }

        public bool IsValid {
            get {
                bool result = TermType != null && TermType.IsInheritor(LocaleTermUtility.BaseGenericType);
#if UNITY_EDITOR
                if (!result) {
                    Debug.Log("Register data is not Valid. Term type can not be null and must be " +
                              $"inherited from {LocaleTermUtility.BaseGenericType.Name}");
                }
#endif
                return result;
            }
        }
    }
    
}