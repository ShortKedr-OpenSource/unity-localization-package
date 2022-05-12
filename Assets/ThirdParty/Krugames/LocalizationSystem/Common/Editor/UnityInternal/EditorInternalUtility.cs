using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Krugames.LocalizationSystem.Common.Editor.UnityInternal {
    /// <summary>
    /// UnityInternal API is not safe, use it on your own risk
    /// This class contains ways to call internal unity utilities for unity 2020.3 version or lower
    /// Note that this class can work different ways, based on your,
    /// since internal functionality is hidden and can be changed overtime by unity developer team
    /// </summary>
    public static class EditorInternalUtility {

        public static bool OpenPropertyEditor(UnityEngine.Object unityObject, bool debug = false) {
            try {
                Assembly editorAssembly = typeof(EditorWindow).Assembly;
                Type propertyEditorType = editorAssembly.GetType("UnityEditor.PropertyEditor");
                BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Static;
                var method = propertyEditorType?.GetMethod("OpenPropertyEditor", flags);
                method?.Invoke(null, new object[] {unityObject, true});
            } catch (Exception e) {
                if (debug) {
                    Debug.LogError("PropertyEditor can not be launched on current version of Unity engine," +
                                   $"Error: {e}");
                }
                return false;
            }
            return true;
        }
    }
}