using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Common.Editor.UnityInternal {

    /// <summary>
    /// UnityInternal API is not safe, use it on your own risk
    /// Provides access to UIElements internal style sheets
    /// </summary>
    public static class UIElementsStyles {

        private static StyleSheet _commonDarkStyleSheet;
        private static StyleSheet _commonLightStyleSheet;

        public static StyleSheet CommonDarkStyleSheet {
            get {
                if (_commonDarkStyleSheet == null) GetDefaultStyles();
                return _commonDarkStyleSheet;
            }
        }

        public static StyleSheet CommonLightStyleSheet {
            get {
                if (_commonLightStyleSheet == null) GetDefaultStyles();
                return _commonLightStyleSheet;
            }
        }

        public static StyleSheet CurrentCommonStyleSheet {
            get {
                if (EditorGUIUtility.isProSkin) return CommonDarkStyleSheet;
                else return CommonLightStyleSheet;
            }
        }
        
        static UIElementsStyles() {
            GetDefaultStyles();
        }

        private static void GetDefaultStyles() {
            try {
                var assembly = typeof(UnityEditor.UIElements.Toolbar).Assembly;

                var utilityType = assembly.GetType("UnityEditor.UIElements.UIElementsEditorUtility");

                var darkStyleSheet = utilityType.GetField("s_DefaultCommonDarkStyleSheet", 
                    BindingFlags.Static | BindingFlags.NonPublic);
                var lightStyleSheet = utilityType.GetField("s_DefaultCommonLightStyleSheet", 
                    BindingFlags.Static | BindingFlags.NonPublic);

                _commonDarkStyleSheet = (StyleSheet) darkStyleSheet.GetValue(null);
                _commonLightStyleSheet = (StyleSheet) lightStyleSheet.GetValue(null);
            } catch (Exception e) {
                _commonDarkStyleSheet = null;
                _commonLightStyleSheet = null;
                
                Debug.LogError("Default UIElements styles can not be found in this Unity version!\n" + e);
                return;
            }
        }

    }
}