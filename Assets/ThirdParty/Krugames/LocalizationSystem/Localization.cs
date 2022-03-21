using System;
using System.Runtime.CompilerServices;
using Krugames.LocalizationSystem.Models;
using Krugames.LocalizationSystem.Models.Dynamic;
using Krugames.LocalizationSystem.Models.Interfaces;
using UnityEngine;

//TODO Remake callbacks to auto null remove

namespace Krugames.LocalizationSystem {
    public static class Localization {

        private static readonly LocaleLibrary LocaleLibrary = LocaleLibrary.Instance;

        private static event Action OnLanguageChangedSimple;  
        private static bool _wasLanguageCallbackInitialized = false;

        
        public static SystemLanguage CurrentLanguage => LocaleLibrary.CurrentLanguage;
        public static ILocale CurrentLocale => LocaleLibrary.CurrentLocale;
        public static SystemLanguage[] SupportedLanguages => LocaleLibrary.SupportedLanguages;
        public static ILocale BaseValidLocale => LocaleLibrary.BaseValidLocale;
        public static ILocale[] ValidLocales => LocaleLibrary.ValidLocales;
        public static DynamicLocale[] DynamicLocales => LocaleLibrary.DynamicLocales;

        /// <inheritdoc cref="Models.LocaleLibrary.Initialize"/>>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Initialize() {
            LocaleLibrary.Initialize();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void InitializeLanguageChangeCallback() {
            LocaleLibrary.OnLanguageChanged += OnLanguageChangeEvent;
            _wasLanguageCallbackInitialized = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void OnLanguageChangeEvent(LocaleLibrary library, SystemLanguage oldLanguage,
            SystemLanguage newLanguage) {
            OnLanguageChangedSimple?.Invoke();
        }
        
        /// <summary>
        /// Add callback to localization system' language change event
        /// </summary>
        /// <param name="callback">callback</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLanguageUpdateCallback(Action callback) {
            if (!_wasLanguageCallbackInitialized) InitializeLanguageChangeCallback();
            OnLanguageChangedSimple += callback;
        }

        /// <inheritdoc cref="AddLanguageUpdateCallback(System.Action)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLanguageUpdateCallback(LocaleLibrary.LanguageChangeDelegate callback) {
            LocaleLibrary.OnLanguageChanged += callback;
        }

        /// <summary>
        /// Remove existing callback from localization system' language change event
        /// </summary>
        /// <param name="callback">callback</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveLanguageUpdateCallback(Action callback) {
            if (!_wasLanguageCallbackInitialized) InitializeLanguageChangeCallback();
            OnLanguageChangedSimple -= callback;
        }
        
        /// <inheritdoc cref="RemoveLanguageUpdateCallback(System.Action)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveLanguageUpdateCallback(LocaleLibrary.LanguageChangeDelegate callback) {
            LocaleLibrary.OnLanguageChanged -= callback;
        }

        ///<inheritdoc cref="Models.LocaleLibrary.AddDynamicLocale"/>>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AddDynamicLocale(DynamicLocale dynamicLocale) {
            return LocaleLibrary.AddDynamicLocale(dynamicLocale);
        }

        /// <inheritdoc cref="Models.LocaleLibrary.RemoveDynamicLocale"/>>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool RemoveDynamicLocale(DynamicLocale dynamicLocale) {
            return LocaleLibrary.RemoveDynamicLocale(dynamicLocale);
        }
        
        /// <inheritdoc cref="Models.LocaleLibrary.SetLanguage"/>>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SetLanguage(SystemLanguage language) {
            return LocaleLibrary.SetLanguage(language);
        }
        
        /// <inheritdoc cref="Models.LocaleLibrary.SupportsLanguage"/>>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SupportsLanguage(SystemLanguage language) {
            return LocaleLibrary.SupportsLanguage(language);
        }

        /// <inheritdoc cref="Models.LocaleLibrary.GetLocale"/>>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILocale GetLocale(SystemLanguage language) {
            return LocaleLibrary.GetLocale(language);
        }

        /// <inheritdoc cref="Models.LocaleLibrary.GetTermValue(string)"/>>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetTermValue(string term) {
            return LocaleLibrary.GetTermValue(term);
        }
        
        /// <inheritdoc cref="Models.LocaleLibrary.GetTermValue(string, Type)"/>>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetTermValue(string term, Type type) {
            return LocaleLibrary.GetTermValue(term, type);
        }
        
        /// <inheritdoc cref="Models.LocaleLibrary.GetTermValue{TTermValueType}(string)"/>>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TTermValueType GetTermValue<TTermValueType>(string term) {
            return LocaleLibrary.GetTermValue<TTermValueType>(term);
        }
        
        /// <inheritdoc cref="Models.LocaleLibrary.GetTermValue(string, SystemLanguage)"/>>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetTermValue(string term, SystemLanguage language) {
            return LocaleLibrary.GetTermValue(term, language);
        }
        
        /// <inheritdoc cref="Models.LocaleLibrary.GetTermValue(string, Type, SystemLanguage)"/>>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetTermValue(string term, Type type, SystemLanguage language) {
            return LocaleLibrary.GetTermValue(term, type, language);
        }
        
        /// <inheritdoc cref="Models.LocaleLibrary.GetTermValue{TTermValueType}(string, SystemLanguage)"/>>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TTermValueType GetTermValue<TTermValueType>(string term, SystemLanguage language) {
            return LocaleLibrary.GetTermValue<TTermValueType>(term, language);
        }

        /// <summary>
        /// Unload unreferenced Dynamic API objects.
        /// This method is alias for Resources.UnloadUnusedAssets() Unity' method
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnloadUnusedResources() {
            Resources.UnloadUnusedAssets();
        }
    }
}