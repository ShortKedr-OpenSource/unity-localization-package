using System;
using System.Collections.Generic;

namespace Krugames.LocalizationSystem.Common.Extensions {
    public static class TypeExtensions {

        /// <summary>
        /// Return whole hierarchy of class parents
        /// </summary>
        /// <param name="type">target type</param>
        /// <returns>Array of parent class types</returns>
        public static Type[] GetClassParentTypes(this Type type) {
            List<Type> parentTypes = new List<Type>(4);
            Type baseType = type.BaseType;
            while (baseType != null) {
                parentTypes.Add(baseType);
                baseType = baseType.BaseType;
            }
            return parentTypes.ToArray();
        }

        /// <summary>
        /// Checks if type has parent in its inheritance.
        /// Generic type definitions like typeof(LocaleTerm<>) is also included 
        /// </summary>
        /// <param name="type">target type</param>
        /// <param name="baseType">base type to check</param>
        /// <returns>return true if target type has baseType in inheritance hierarchy</returns>
        public static bool IsInheritor(this Type type, Type baseType) {
            if (!baseType.IsGenericTypeDefinition) {
                if (type == baseType) return false;
                return baseType.IsAssignableFrom(type);
            }
            
            Type nextBaseType = type.BaseType;
            while (nextBaseType != null) {
                if (nextBaseType.IsGenericType && baseType == nextBaseType.GetGenericTypeDefinition()) {
                    return true;
                }
                nextBaseType = nextBaseType.BaseType;
            }
            
            return false;
        }
    }
}