using System;
using System.Reflection;
using Krugames.LocalizationSystem.Models.Locators;

namespace Krugames.LocalizationSystem.Models.Utility {
    public static class LocaleTermUtility {
        
        public readonly static Type BaseType = typeof(LocaleTerm);
        public readonly static Type BaseGenericType = typeof(LocaleTerm<>);

        public static Type GetValueTypeOfGenericTermType(Type type) {
            BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
            FieldInfo fieldInfo = type.GetField("VALUE_TYPE", flags);
            Type valueType = (Type) fieldInfo.GetValue(null);
            return valueType;
        }

        public static Type GetValueTypeOfGenericTermType<TTermType>() where TTermType : LocaleTerm {
            return GetValueTypeOfGenericTermType(typeof(TTermType));
        }

        public static Type GetTermTypeByValueType(Type valueType) {
            return LocaleTermLocator.GetTermTypeByValueType(valueType);
        }

        public static Type GetTermTypeByValueType<TValueType>() {
            return LocaleTermLocator.GetTermTypeByValueType(typeof(TValueType));
        }
    }
}