using System;
using Krugames.LocalizationSystem.Common.Extensions;
using Krugames.LocalizationSystem.Editor.Serialization.Attributes;
using UnityEngine;

namespace Krugames.LocalizationSystem.Editor.Serialization.Attributes {
    /// <summary>
    /// Register LocaleSerializer type in the localization system. 
    /// These locale serializers works only on editor side.
    /// Any locale serializer must be inherited from LocaleSerializer class
    /// in order to be successfully register in the system
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class RegisterLocaleSerializerAttribute : Attribute {

        public static readonly Type BaseSerializerType = typeof(LocaleSerializer);
        
        public readonly Type SerializerType;
        public readonly string Name;
        public readonly string Extension;
        
        public RegisterLocaleSerializerAttribute(Type serializerType, string name = "", string extension = "") {
            SerializerType = serializerType;
            Name = (name == "") ? SerializerType.Name : name;
            Extension = extension;
        }

        public bool IsValid {
            get {
                bool result = SerializerType != null && SerializerType.IsInheritor(BaseSerializerType);
#if UNITY_EDITOR
                if (!result) {
                    Debug.Log("Register data is not Valid. Serializer type can not be null and must be " +
                              $"inherited from {BaseSerializerType.Name}");
                }
#endif
                return result;
            }
        }
    }
}