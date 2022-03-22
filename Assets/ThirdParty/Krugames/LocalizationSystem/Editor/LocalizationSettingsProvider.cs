using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Krugames.LocalizationSystem.Editor {
    public class LocalizationSettingsProvider {

        [SettingsProvider]
        private static SettingsProvider CreateLocalizationSettingsProvider() {
            var provider = new SettingsProvider("Project/Localization", SettingsScope.Project) {
                label = "Localization",
                guiHandler = delegate(string searchContext) {
                    var settings = new SerializedObject(LocalizationSettings.Instance);
                    EditorGUILayout.PropertyField(settings.FindProperty("autoInitialize"), GUILayout.ExpandWidth(true));
                    EditorGUILayout.PropertyField(settings.FindProperty("useSystemLanguageAsDefault"));
                    EditorGUILayout.PropertyField(settings.FindProperty("loadLastUsedLanguageAsCurrent"));
                    settings.ApplyModifiedProperties();
                },
                keywords = new HashSet<string>() {
                    "Auto Initialize",
                    "Use System Language As Default",
                    "Load Last Used Language As Current"
                }
            };
            return provider;
        }
    }
}