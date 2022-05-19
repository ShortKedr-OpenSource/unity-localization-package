using System;
using System.Collections.Generic;
using System.Text;
using Krugames.LocalizationSystem.Editor.Serialization.Attributes;
using Krugames.LocalizationSystem.Editor.Serialization.DataTransferObjects;
using Krugames.LocalizationSystem.Editor.Serialization.Serializers;
using Krugames.LocalizationSystem.Models.Interfaces;
using UnityEngine;

[assembly: RegisterLocaleSerializer(typeof(LocaleCsvSerializer), "CSV", "csv")]

namespace Krugames.LocalizationSystem.Editor.Serialization.Serializers {

    //TODO improve csv serialization
    //TODO fix: after excel editing this thing add additional " to json statements

    /// <summary>
    /// ILocale to CSV Serializer
    /// </summary>
    public class LocaleCsvSerializer : LocaleSerializer<string>{
        public override string SerializeSmart(ILocale locale) {
            LocaleData localeData = new LocaleData(locale);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Language;{locale.Language}");
            builder.AppendLine($"Term;Value;ValueType;IsAsset");
            for (int i = 0; i < localeData.termData.Length; i++) {
                builder.AppendLine($"{localeData.termData[i].term};" +
                                   $"{localeData.termData[i].value};" +
                                   $"{localeData.termData[i].valueType};" +
                                   $"{localeData.termData[i].isAsset}");
            }
            return builder.ToString();
        }

        public override void DeserializeSmart(IModifiableLocale targetLocale, string data) {
            string[] lines = data.Split('\n');

            SystemLanguage language = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), lines[0].Split(';')[1]);
            List<TermData> termData = new List<TermData>(lines.Length - 2);
            
            for (int i = 2; i < lines.Length; i++) {
                string[] values = lines[i].Split(';');
                if (values.Length < 4) continue;

                Debug.Log($"{values[0]}, {values[1]}, {values[2]}, {values[3]}");
                
                Type type = Type.GetType(values[2]);
                string term = values[0];
                bool isAsset = bool.Parse(values[3]);
                object value = (isAsset) ? values[1] : ParseValue(values[1], type);
                TermData newTermData = new TermData(term, value, isAsset, type);
                termData.Add(newTermData);
            }

            LocaleData localeData = new LocaleData(language, termData.ToArray());
            localeData.SetDataToLocale(targetLocale);
        }

        private object ParseValue(string value, Type valueType) {
            if (valueType == typeof(string)) return value;
            if (valueType == typeof(byte)) return byte.Parse(value);
            if (valueType == typeof(sbyte)) return sbyte.Parse(value);
            if (valueType == typeof(ushort)) return ushort.Parse(value);
            if (valueType == typeof(uint)) return uint.Parse(value);
            if (valueType == typeof(ulong)) return ulong.Parse(value);
            if (valueType == typeof(short)) return short.Parse(value);
            if (valueType == typeof(int)) return int.Parse(value);
            if (valueType == typeof(long)) return long.Parse(value);
            if (valueType == typeof(float)) return float.Parse(value);
            if (valueType == typeof(double)) return double.Parse(value);
            if (valueType == typeof(decimal)) return decimal.Parse(value);
            if (valueType == typeof(bool)) return bool.Parse(value);
            if (valueType.IsEnum) return Enum.Parse(valueType, value);
            if (valueType == typeof(DateTime)) return DateTime.Parse(value);
            if (valueType == typeof(Type)) return Type.GetType(value);
            if (valueType == typeof(Guid)) return Guid.Parse(value);
            return value;
        }
    }
}