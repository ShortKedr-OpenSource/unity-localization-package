using System;
using System.ComponentModel;

namespace Krugames.LocalizationSystem.Editor.Serialization.Utility {
    /// <summary>
    /// Json Primitive Checker for Newtonsoft Json
    /// Based on primitive types, described on this page:
    /// https://www.newtonsoft.com/json/help/html/serializationguide.htm
    /// </summary>
    public static class JsonPrimitiveChecker {
        public static bool IsPrimitive(object value) {
            Type type = value.GetType();

            if (type == typeof(string)) return true;
            if (type == typeof(byte)) return true;
            if (type == typeof(sbyte)) return true;
            if (type == typeof(ushort)) return true;
            if (type == typeof(uint)) return true;
            if (type == typeof(ulong)) return true;
            if (type == typeof(short)) return true;
            if (type == typeof(int)) return true;
            if (type == typeof(long)) return true;
            if (type == typeof(float)) return true;
            if (type == typeof(double)) return true;
            if (type == typeof(decimal)) return true;
            if (type.IsEnum) return true;
            if (type == typeof(DateTime)) return true;
            if (type == typeof(byte[])) return true;
            if (type == typeof(Type)) return true;
            if (type == typeof(Guid)) return true;
            if (type == typeof(TypeConverter)) return true;

            return false;
        }
    }
}