using System;
using System.IO;
using System.Reflection;
using Krugames.LocalizationSystem.Common.Extensions;
using Krugames.LocalizationSystem.Editor.Serialization.Locators;
using Krugames.LocalizationSystem.Editor.Serialization.Serializers;
using Krugames.LocalizationSystem.Models.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Krugames.LocalizationSystem.Editor.Serialization.Utility {
    public static class LocaleSerializerUtility {

        public static void SerializeToFile(string path, ILocale locale, LocaleSerializer<string> serializer) {
            string serializedData = serializer.SerializeSmart(locale);
            File.WriteAllText(path, serializedData);
        }

        public static void DeserializeFromFile(string path, IModifiableLocale targetLocale, LocaleSerializer<string> serializer) {
            string data = File.ReadAllText(path);
            serializer.DeserializeSmart(targetLocale, data);
        }

        public static void SerializeToFile(string path, ILocale locale, LocaleSerializer<byte[]> serializer) {
            byte[] serializedData = serializer.SerializeSmart(locale);
            File.WriteAllBytes(path, serializedData);
        }

        public static void DeserializeFromFile(string path, IModifiableLocale targetLocale, LocaleSerializer<byte[]> serializer) {
            byte[] data = File.ReadAllBytes(path);
            serializer.DeserializeSmart(targetLocale, data);
        }

        public static object CreateSerializer(Type serializerType) {
            if (!serializerType.IsInheritor(typeof(LocaleSerializer))) return null;
            ConstructorInfo ci = serializerType.GetConstructor(Type.EmptyTypes);
            if (ci == null) Debug.LogError($"Not able to create {nameof(LocaleSerializer)} of type " +
                                           $"{serializerType}, since there is no empty constructor for this type");
            return ci.Invoke(null);
        }
        
        public static TSerializerType CreateSerializer<TSerializerType>() where TSerializerType : LocaleSerializer {
            return (TSerializerType) CreateSerializer(typeof(TSerializerType));
        }

        public static void Export(ILocale locale, Type serializerType) {
            var buildData = LocaleSerializerLocator.GetBuildData(serializerType);
            object serializer = CreateSerializer(serializerType);

            string title = "Export locale";
            string defaultFileName = "locale";

            if (serializer is LocaleSerializer<string> stringSerializer) {
                
                var path = EditorUtility.SaveFilePanel(title, null, defaultFileName, buildData.Extension);
                if (!string.IsNullOrEmpty(path)) SerializeToFile(path, locale, stringSerializer);
                
            } else if (serializer is LocaleSerializer<byte[]> byteSerializer) {
               
                var path = EditorUtility.SaveFilePanel(title, null, defaultFileName, buildData.Extension);
                if (!string.IsNullOrEmpty(path)) SerializeToFile(path, locale, byteSerializer);
                
            } else {
                Debug.LogError($"Exprot {nameof(LocaleSerializer)} must serialize to bytes[] or string formats. Other formats are not supported for export. " +
                               "Consider to remove registration attribute for this serializer to prevent users watching this error");

            }
        }

        public static void Import(IModifiableLocale targetLocale, Type serializerType) {
            var buildData = LocaleSerializerLocator.GetBuildData(serializerType);
            object serializer = CreateSerializer(serializerType);

            string title = "Import locale";
            
            if (serializer is LocaleSerializer<string> stringSerializer) {

                var path = EditorUtility.OpenFilePanel(title, null, buildData.Extension);
                if (!string.IsNullOrEmpty(path)) DeserializeFromFile(path, targetLocale, stringSerializer);

            } else if (serializer is LocaleSerializer<byte[]> byteSerializer) {
              
                var path = EditorUtility.OpenFilePanel(title, null, buildData.Extension);
                if (!string.IsNullOrEmpty(path)) DeserializeFromFile(path, targetLocale, byteSerializer);

            } else {
                Debug.LogError($"Import {nameof(LocaleSerializer)} must serialize to bytes[] or string formats. Other formats are not supported for export. " +
                               "Consider to remove registration attribute for this serializer to prevent users watching this error");
            }
        }
    }
}