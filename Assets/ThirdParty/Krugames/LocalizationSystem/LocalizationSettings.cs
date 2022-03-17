using Krugames.LocalizationSystem.Unity.Singletons;
using UnityEngine;

namespace Krugames.LocalizationSystem {
    public class LocalizationSettings : ScriptableSingleton<LocalizationSettings> {
        
        [SerializeField] private bool autoInitialize = true;
        [SerializeField] private bool useSystemLanguageAsDefault = true;
        [SerializeField] private bool loadLastUsedLanguageAsCurrent = true;
        
        //TODO DebugMode

        /// <summary>
        /// Automatically initialize whole localization static library on startup.
        /// If false, static library will be initialized on first use
        /// </summary>
        public static bool AutoInitialize => Instance.autoInitialize;
        
        /// <summary>
        /// Set current language to system language at first application start.
        /// If false, base locale language will be used instead.
        /// Also you can change language manually to set custom language.
        /// </summary>
        public static bool UseSystemLanguageAsDefault => Instance.useSystemLanguageAsDefault;
        
        /// <summary>
        /// Load language from last session on application start.
        /// Will work only after first application start.
        /// if false, language will be chosen, based on UseSystemLanguageAsDefault property rule
        /// </summary>
        public static bool LoadLastUsedLanguageAsCurrent => Instance.loadLastUsedLanguageAsCurrent;
    }
}