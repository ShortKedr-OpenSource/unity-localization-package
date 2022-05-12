using System;
using Krugames.LocalizationSystem.Editor.UIElements;
using Krugames.LocalizationSystem.Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEditor.PopupWindow;


//TODO Make fixed height and sizes in pecentage + fixed + flex-grow
namespace Krugames.LocalizationSystem.Editor {
    [CustomEditor(typeof(Locale))]
    public class LocaleInspector : UnityEditor.Editor {

        private VisualElement _rootElement;
        
        private Rect _addTermBtnRect;
        private Rect _exportBtnRect;
        private Rect _importBtnRect;
        
        private LocaleTermSelector _termSelector;
        private LocaleSerializerSelector _serializerSelector;

        private Locale _locale;
        private SerializedProperty _languageProp;
        private SerializedProperty _termsProp;
        
        private LocaleTermEditor _localeTermEditor;
        private LocaleTermListView _localeTermList;
        private IMGUIContainer _headerContainer;
        private IMGUIContainer _footerContainer;
        
        public override VisualElement CreateInspectorGUI() {
            _locale = (Locale) target;
            _languageProp = serializedObject.FindProperty("language");
            _termsProp = serializedObject.FindProperty("terms");

            _rootElement = new VisualElement();
            
            _rootElement.Add(_headerContainer = new IMGUIContainer(OnIMGUIHeaderGUI));
            _rootElement.Add(_localeTermEditor = new LocaleTermEditor());
            _rootElement.Add(_localeTermList = new LocaleTermListView(_locale.GetTerms()));
            _rootElement.Add(_footerContainer = new IMGUIContainer(OnIMGUIFooterGUI));
            
            _localeTermList.OnTermSelect += TermSelectEvent;
            _localeTermEditor.OnChange += TermChangeEvent;

            return _rootElement;
        }

        private void TermChangeEvent(LocaleTermEditor self) {
            _localeTermList.UpdateListValues();
        }

        private void TermSelectEvent(LocaleTermListView termList, LocaleTerm localeTerm) {
            _localeTermEditor.SetTerm(termList.SelectedTerm);
        }

        private void OnIMGUIHeaderGUI() {
            GUILayout.Space(4f);
            EditorGUILayout.PropertyField(_languageProp);
            GUILayout.Space(4f);
            serializedObject.ApplyModifiedProperties();
        }

        private void OnIMGUIFooterGUI() {
            GUILayout.Space(4f);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add term", GUILayout.MinWidth(225), GUILayout.MinHeight(26))) {
                if (_termSelector == null) _termSelector = LocaleTermSelector.Create(AddTerm);
                PopupWindow.Show(_addTermBtnRect, _termSelector);
            }
            if (Event.current.type == EventType.Repaint) _addTermBtnRect = GUILayoutUtility.GetLastRect();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.Space(2);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Export", GUILayout.MinWidth(110.5f), GUILayout.MinHeight(26))) {
                if (_serializerSelector == null) _serializerSelector = LocaleSerializerSelector.Create(Export);
                PopupWindow.Show(_exportBtnRect, _serializerSelector);
            }
            if (Event.current.type == EventType.Repaint) _exportBtnRect = GUILayoutUtility.GetLastRect();
            if (GUILayout.Button("Import", GUILayout.MinWidth(110.5f), GUILayout.MinHeight(26))) {
                if (_serializerSelector == null) _serializerSelector = LocaleSerializerSelector.Create(Export);
                PopupWindow.Show(_importBtnRect, _serializerSelector);
            }
            if (Event.current.type == EventType.Repaint) _importBtnRect = GUILayoutUtility.GetLastRect();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        private void AddTerm(Type termType, Type valueType) {
            Debug.Log($"TermType: {termType}; ValueType: {valueType}");
        }
        
        private void Export(Type serializerType) {
            Debug.Log($"SerializerType: {serializerType}");
        }
        
        private void OpenLocalizationEditor() {
            Debug.Log("Open Localization Editor");
        }

    }
}