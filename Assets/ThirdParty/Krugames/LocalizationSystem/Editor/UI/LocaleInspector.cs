using System;
using Krugames.LocalizationSystem.Editor.Serialization.Utility;
using Krugames.LocalizationSystem.Models;
using Krugames.LocalizationSystem.Models.Utility.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEditor.PopupWindow;

//TODO Add Undo ability
//TODO Make fixed height and sizes in pecentage + fixed + flex-grow
namespace Krugames.LocalizationSystem.Editor.UI {
    [CustomEditor(typeof(Locale))]
    public class LocaleInspector : UnityEditor.Editor {

        private VisualElement _rootElement;
        
        private Rect _addTermBtnRect;
        private Rect _exportBtnRect;
        private Rect _importBtnRect;
        
        private LocaleTermSelector _termSelector;
        private LocaleSerializerSelector _serializerExportSelector;
        private LocaleSerializerSelector _serializerImportSelector;

        private Locale _locale;
        private SerializedProperty _languageProp;
        private SerializedProperty _termsProp;
        
        private LocaleTermEditor _localeTermEditor;
        private LocaleTermListView _localeTermList;
        private IMGUIContainer _headerContainer;
        private IMGUIContainer _footerContainer;

        public override VisualElement CreateInspectorGUI() {
            _rootElement.Add(_headerContainer);
            _rootElement.Add(_localeTermEditor);
            _rootElement.Add(_localeTermList);
            _rootElement.Add(_footerContainer);
            return _rootElement;
        }

        private void OnEnable() {
            _locale = (Locale) target;
            _languageProp = serializedObject.FindProperty("language");
            _termsProp = serializedObject.FindProperty("terms");

            _rootElement = new VisualElement();
            
            _headerContainer = new IMGUIContainer(OnIMGUIHeaderGUI);
            _localeTermEditor = new LocaleTermEditor();
            _localeTermList = new LocaleTermListView(_locale.GetTerms());
            _footerContainer = new IMGUIContainer(OnIMGUIFooterGUI);
            
            _localeTermList.OnTermSelect += TermSelectEvent;
            _localeTermList.OnTermDeleteSelect += TermDeleteSelectEvent;
            ObjectChangeEvents.changesPublished += ObjectChangeEvent;
        }
        
        private void ObjectChangeEvent(ref ObjectChangeEventStream stream) {
            _localeTermList.UpdateView();
        }

        private void TermSelectEvent(LocaleTermListView termList, LocaleTerm localeTerm) {
            if (_localeTermEditor.Term != null) LocaleUtility.RenameTermSubAsset(_locale, _localeTermEditor.Term);
            _localeTermEditor.SetTerm(localeTerm);
        }
        
        private void TermDeleteSelectEvent(LocaleTermListView self, LocaleTerm localeTerm) {
            if (localeTerm == null) return;
            if (_localeTermEditor.Term == localeTerm) _localeTermEditor.SetTerm(null);
            bool result = LocaleUtility.RemoveLocaleTerm(_locale, localeTerm);
            if (result) {
                _localeTermList.SetTerms(_locale.GetTerms());
            }
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
                if (_serializerExportSelector == null) _serializerExportSelector = LocaleSerializerSelector.Create(Export);
                PopupWindow.Show(_exportBtnRect, _serializerExportSelector);
            }
            if (Event.current.type == EventType.Repaint) _exportBtnRect = GUILayoutUtility.GetLastRect();
            if (GUILayout.Button("Import", GUILayout.MinWidth(110.5f), GUILayout.MinHeight(26))) {
                if (_serializerImportSelector == null) _serializerImportSelector = LocaleSerializerSelector.Create(Import);
                PopupWindow.Show(_importBtnRect, _serializerImportSelector);
            }
            if (Event.current.type == EventType.Repaint) _importBtnRect = GUILayoutUtility.GetLastRect();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        private void AddTerm(Type termType, Type valueType) {
            string defaulTermName = "new_term";
            LocaleTerm term = LocaleUtility.AddLocaleTerm(_locale, defaulTermName, termType);
            _localeTermList.SetTerms(_locale.GetTerms());
            _localeTermEditor.SetTerm(term);
        }
        
        private void Export(Type serializerType) {
            LocaleSerializerUtility.Export(_locale, serializerType);
        }
        
        private void Import(Type serializerType) {
             LocaleSerializerUtility.Import(_locale, serializerType);
            _localeTermEditor.SetTerm(null);
            _localeTermList.SetTerms(_locale.GetTerms());
        }

        private void OnDestroy() {
            if (_localeTermEditor.Term != null) LocaleUtility.RenameTermSubAsset(_locale, _localeTermEditor.Term);
            ObjectChangeEvents.changesPublished -= ObjectChangeEvent;
        }
    }
}