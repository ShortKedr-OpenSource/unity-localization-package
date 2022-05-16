using System;
using System.Collections.Generic;
using Krugames.LocalizationSystem.Editor.UI;
using Krugames.LocalizationSystem.Editor.Styles;
using RenwordDigital.StringSearchEngine;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Implementation {
    public class SelectorListPopup : PopupWindowContent {
        
        private string _title;
        private ListSelectableElement[] _listElements;

        private EditorWindow _editorWindow;
        
        private PopupWindowRoot _popupRoot;
        private SearchToolbar _searchToolbar;
        private TittleToolbar _tittleToolbar;
        private ListGroup _listGroup;

        private SearchIndex _searchIndex = new SearchIndex();
        private Dictionary<Resource, ListSelectableElement> _resourceToListElement;
        private List<Resource> _lastSearchResult = null;

        private bool _inited = false;
        
        public string Title => _title;

        public SelectorListPopup(string title, Element[] elements) {
            _title = title;
            if (elements == null) _listElements = Array.Empty<ListSelectableElement>();
            else {
                _listElements = new ListSelectableElement[elements.Length];
                for (int i = 0; i < _listElements.Length; i++) {
                    _listElements[i] = new ListSelectableElement(elements[i].ClickAction) {
                        text = elements[i].Name,
                    };
                }
            }
        }
        
        public override void OnOpen() {

            if (!_inited) {
                _popupRoot = new PopupWindowRoot();
                
                _popupRoot.Add(_searchToolbar = new SearchToolbar());
                _popupRoot.Add(_tittleToolbar = new TittleToolbar(_title));
                _popupRoot.Add(_listGroup = new ListGroup());

                for (int i = 0; i < _listElements.Length; i++) {
                    _listElements[i].clicked += Close;
                }

                _resourceToListElement = new Dictionary<Resource, ListSelectableElement>(_listElements.Length);
                List<Resource> elementResources = new List<Resource>(_listElements.Length);
                for (int i = 0; i < _listElements.Length; i++) {
                    Resource resource = new Resource(_listElements[i].text);
                    elementResources.Add(resource);
                    _resourceToListElement.Add(resource, _listElements[i]);
                }

                _searchIndex.SetResources(elementResources.ToArray());
                _searchToolbar.OnSearchChanged += SearchListElements;

                _inited = true;
            }

            _editorWindow = editorWindow;
            VisualElement root = editorWindow.rootVisualElement;
            root.styleSheets.Add(LocalizationEditorStyles.GlobalStyle);
            root.style.marginTop = 0;
            root.style.marginBottom = 0;
            root.style.marginLeft = 0;
            root.style.marginRight = 0;
            root.Add(_popupRoot);

            CancelSearchResult();
        }

        public override Vector2 GetWindowSize() {
            return new Vector2(225, 300);
        }

        public override void OnGUI(Rect rect) {
            //do nothing since we implement it with UIElements
        }

        public void SearchListElements(string searchString) {
            if (_searchToolbar.SearchText != searchString) {
                _searchToolbar.SearchText = searchString;
                return;
            }
            
            searchString = searchString.Trim();

            if (string.IsNullOrEmpty(searchString)){
                CancelSearchResult();
            } else if (searchString.Length < 3) {
                _lastSearchResult = new List<Resource>(0);
                UpdateList();
            } else {
                _lastSearchResult = _searchIndex.GetSearchResult(searchString);
                UpdateList();
            }
        }

        public void CancelSearchResult() {
            _searchToolbar.SearchText = "";
            _lastSearchResult = null;
            UpdateList();
        }
        
        public void UpdateList() {
            if (_lastSearchResult == null) {
                _listGroup.Clear();
                for (int i = 0; i < _listElements.Length; i++) {
                    _listGroup.Add(_listElements[i]);
                }
            } else {
                _listGroup.Clear();
                for (int i = 0; i < _lastSearchResult.Count; i++) {
                    if (!_resourceToListElement.ContainsKey(_lastSearchResult[i])) continue;
                    _listGroup.Add(_resourceToListElement[_lastSearchResult[i]]);
                }
            }
        }

        private void Close() {
            _editorWindow.Close();
        }

        public struct Element {
            public string Name;
            public Action ClickAction;

            public Element(string name, Action clickAction) {
                Name = name;
                ClickAction = clickAction;
            }
        }  
    }
}