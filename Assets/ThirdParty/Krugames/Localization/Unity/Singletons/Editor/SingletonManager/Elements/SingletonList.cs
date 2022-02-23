using System;
using System.Collections.Generic;
using Core.Unity.Singletons.Editor;
using RenwordDigital.StringSearchEngine;
using UnityEngine;
using UnityEngine.UIElements;

namespace Krugames.Core.Unity.Singletons.Editor.SingletonManager.Elements {
    public class SingletonList : Box {

        private Type[] _singletonTypes;
        private Resource[] _singletonResources;
        private SearchIndex _searchIndex = new SearchIndex();

        private ScrollView _scrollView;
        private SingletonElement[] _singletonElements;

        private Dictionary<Resource, SingletonElement> _resourceToElementDict = new Dictionary<Resource, SingletonElement>();

        private List<Resource> _lastSearchResult = null;
        
        public Type[] SingletonTypes => _singletonTypes;
        public Resource[] SingletonResources => _singletonResources;
        public SearchIndex SearchIndex => _searchIndex;
        
        public SingletonElement[] SingletonElements => _singletonElements;

        public SingletonList() : base() {
            style.flexGrow = 1f;
            style.justifyContent = new StyleEnum<Justify>(Justify.FlexStart);
            style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column);
            style.alignItems = new StyleEnum<Align>(Align.Stretch);
            
            style.backgroundColor = new StyleColor(new Color(0f, 0f, 0f, 0f));

            _scrollView = new ScrollView(ScrollViewMode.Vertical) {
                style = {
                    flexGrow = 1f,
                    
                    width = new StyleLength(StyleKeyword.Auto),
                    height = new StyleLength(StyleKeyword.Auto),
                    
                    justifyContent = new StyleEnum<Justify>(Justify.FlexStart),
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column),
                    alignItems = new StyleEnum<Align>(Align.Stretch),
                    
                    backgroundColor = new StyleColor(new Color(0f, 0f, 0f, 0f)),

                    paddingLeft = 4f,
                    paddingRight = 4f,
                },
                
            };
            
            this.Add(_scrollView);
        }

        public void SetSingletonTypes(Type[] singletonTypes) {

            List<Type> validTypes = new List<Type>(singletonTypes.Length);
            List<Resource> singletonResources = new List<Resource>(singletonTypes.Length);
            
            for (int i = 0; i < singletonTypes.Length; i++) {
                if (singletonTypes[i] == null) continue;
                if (!ScriptableSingletonUtility.IsTypeScriptableSingleton(singletonTypes[i])) continue;
                validTypes.Add(singletonTypes[i]);
                singletonResources.Add(new Resource(ScriptableSingletonUtility.GetSingletonName(singletonTypes[i])));
            }

            _singletonTypes = validTypes.ToArray();
            _singletonResources = singletonResources.ToArray();
            
            _searchIndex.SetResources(_singletonResources);

            RebuildSingletonElementList();
        }

        public void RebuildSingletonElementList() {
            _resourceToElementDict = new Dictionary<Resource, SingletonElement>(_singletonTypes.Length);
            _singletonElements = new SingletonElement[_singletonTypes.Length];
            
            _scrollView.Clear();
            
            for (int i = 0; i < _singletonTypes.Length; i++) {
                _singletonElements[i] = new SingletonElement(_singletonTypes[i]);

                _resourceToElementDict.Add(_singletonResources[i], _singletonElements[i]);
                
                _scrollView.Add(_singletonElements[i]);
            }
        }

        public void UpdateSelf() {
            if (_lastSearchResult == null) {
                _scrollView.Clear();
                for (int i = 0; i < _singletonElements.Length; i++) {
                    _scrollView.Add(_singletonElements[i]);
                    _singletonElements[i].UpdateSelf();
                }
            } else {
                _scrollView.Clear();
                for (int i = 0; i < _lastSearchResult.Count; i++) {
                    if (_resourceToElementDict.ContainsKey(_lastSearchResult[i])) {
                        _scrollView.Add(_resourceToElementDict[_lastSearchResult[i]]);
                        _resourceToElementDict[_lastSearchResult[i]].UpdateSelf();
                    }    
                }
            }
        }
        
        public void SearchForSingleton(string searchString) {
            searchString = searchString.Trim();

            if (string.IsNullOrEmpty(searchString) || searchString.Length < 3) {
                CancelSearchResult();
            } else {
                _lastSearchResult = _searchIndex.GetSearchResult(searchString);
                UpdateSelf();
            }
        }

        public void CancelSearchResult() {
            _lastSearchResult = null;
            UpdateSelf();
        }

        public bool HasActiveSearchResult() {
            return _lastSearchResult != null;
        }
    }
}