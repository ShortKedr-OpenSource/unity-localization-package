using System;
using System.Collections.Generic;
using Krugames.LocalizationSystem.Common.Editor.UnityInternal;
using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Implementation;
using Krugames.LocalizationSystem.Models;
using RenwordDigital.StringSearchEngine;
using ThirdParty.Krugames.LocalizationSystem.Editor.UI;
using UnityEngine;
using UnityEngine.UIElements;

//TODO can be generalised (universal type)
//TODO stay rebuildpage same way, but add searchModeLogic. Add MultiPage search
//TODO remake select to term_index;
namespace Krugames.LocalizationSystem.Editor.UI {
    public class LocaleTermListView : Box {

        //TODO 2 list_views (normal | search)
        //TODO selected_item (item from another list_view) can_not be selected

        private enum ViewMode {
            Default = 0,
            Search = 1
        }

        private const int DefaultItemPerPage = 12;
        
        //private const string TermViewClassName = nameof(LocaleTermListView) + "_List"; //TODO remove from uss
        private const string RootClassName = nameof(LocaleTermListView) + "_Root"; 
        
        private LocaleTerm[] _terms;

        private VisualElement _root;
        
        private TittleSearchToolbar _searchToolbar;
        private LocaleTermListViewTableHeader _tableHeader;
        private PagerToolbar _pagerToolbar;

        private VisualElement _listContainer;
        private LocaleTermListViewContent _defaultContent;
        private LocaleTermListViewContent _searchContent;

        private SearchIndex _termSearchIndex;
        private Dictionary<Resource, LocaleTerm> _resourceTermDict;

        private ViewMode _viewMode;
        private int _itemsPerPage;

        private OptionPopup _managedElementOptions; 
        private LocaleTermListViewElement _managedElement = null;

        
        public delegate void ElementDelegate(LocaleTermListView self, LocaleTerm localeTerm);
        public event ElementDelegate OnTermSelect;
        public event ElementDelegate OnTermDeleteSelect;
        
        //TODO add props
        
        public LocaleTermListView(int itemPerPage = DefaultItemPerPage) : this(Array.Empty<LocaleTerm>(), itemPerPage) {
        }
        
        public LocaleTermListView(LocaleTerm[] terms, int itemsPerPage = DefaultItemPerPage) {
            _terms = terms;
            _itemsPerPage = itemsPerPage;
            
            styleSheets.Add(LocalizationEditorStyles.GlobalStyle);
            
            _root = new VisualElement();
            _root.styleSheets.Add(LocalizationEditorStyles.GlobalStyle);
            Add(_root);
            
            _searchToolbar = new TittleSearchToolbar("Terms");
            _tableHeader = new LocaleTermListViewTableHeader();
            _pagerToolbar = new PagerToolbar(0);

            _defaultContent = new LocaleTermListViewContent(_itemsPerPage);
            _searchContent = new LocaleTermListViewContent(_itemsPerPage);

            focusable = true;
            pickingMode = PickingMode.Position;

            var defaultPageElements = _defaultContent.PageElements;
            for (int i = 0; i < defaultPageElements.Length; i++) {
                defaultPageElements[i].OnPropertiesClick += ElementPropertiesClickEvent;
            }

            var searchPageElements = _searchContent.PageElements;
            for (int i = 0; i < searchPageElements.Length; i++) {
                searchPageElements[i].OnPropertiesClick += ElementPropertiesClickEvent;
            }

            _managedElementOptions = new OptionPopup(new SelectorListPopup.Element[] {
                new SelectorListPopup.Element("Properties", ManagedElement_OpenProperties),
                new SelectorListPopup.Element("Delete", ManagedElement_Delete),
            });
            
            _searchToolbar.SearchField.RegisterCallback<ChangeEvent<string>>(SearchChangeEvent);
            _pagerToolbar.OnPageChange += ToolbarPageChangeEvent;

            _root.Add(_searchToolbar);
            _root.Add(_tableHeader);
            _root.Add(_pagerToolbar);
            
            _viewMode = ViewMode.Default;
            UpdateViewMode();

            _defaultContent.SetTerms(_terms);
            //SetTerms(terms);
            
            /*var popup = new OptionPopup(null, new SelectorListPopup.Element[] {
                new SelectorListPopup.Element("Properties", () => Debug.Log("Props")),
                new SelectorListPopup.Element("Delete", () => Debug.Log("Delete")),
            });
            Rect rect = new Rect(new Vector2(Event.current.mousePosition.x, Event.current.mousePosition.y), Vector2.one);
            PopupWindow.Show(rect, popup);*/
        }

        private void ManagedElement_OpenProperties() {
            EditorInternalUtility.OpenPropertyEditor(_managedElement.LocaleTerm);
        }
        
        private void ManagedElement_Delete() {
            OnTermDeleteSelect?.Invoke(this, _managedElement.LocaleTerm);
        }

        private void ElementPropertiesClickEvent(LocaleTermListViewElement element) {
            _managedElement = element;
            if (_managedElement == null || _managedElement.LocaleTerm == null) return;
            Rect mouseRect = new Rect(Event.current.mousePosition, Vector2.one); 
            UnityEditor.PopupWindow.Show(mouseRect, _managedElementOptions);
        }

        private void ToolbarPageChangeEvent(PagerToolbar self, int newPage) {
            throw new NotImplementedException();
        }

        private void SearchChangeEvent(ChangeEvent<string> evt) {
            throw new NotImplementedException();
        }

        public void SetTerms(LocaleTerm[] terms) {
            throw new NotImplementedException();
            /*
            _terms = terms;
            RebuildSearchIndex();
            _pagerToolbar.SetPageCount(GetPageCount());
            RebuildPage();
            */
        }
        
        /// <summary>
        /// Update values in views
        /// </summary>
        public void UpdateView() {
            switch (_viewMode) {
                case ViewMode.Default:
                    _defaultContent.UpdateView();
                    break;
                
                case ViewMode.Search:
                    _searchContent.UpdateView();
                    break;
            }
        }

        public void Search(string searchString) {
            throw new NotImplementedException();
            /*if (searchString == _searchToolbar.SearchField.value) return;

            if (searchString.Length < SearchIndex.SearchLength) {
                CancelSearch();
            } else {
                var searchResult = _termSearchIndex.GetSearchResult(searchString);
                LocaleTerm[] foundTerms = new LocaleTerm[searchResult.Count];
                for (int i = 0; i < searchResult.Count; i++) {
                    foundTerms[i] = _resourceTermDict[searchResult[i]];
                }
                RebuildPage(foundTerms);
                _viewMode = ViewMode.Search;
            }*/
        }
        
        public void CancelSearch() {
            throw new NotImplementedException();
            /*if (_viewMode == ViewMode.Search) {
                _searchToolbar.SearchField.SetValueWithoutNotify(string.Empty);
                _viewMode = ViewMode.Default;
            }*/
        }

        private void RebuildSearchIndex() {
            _resourceTermDict = new Dictionary<Resource, LocaleTerm>(_terms.Length);
            Resource[] resources = new Resource[_terms.Length];
            for (int i = 0; i < _terms.Length; i++) {
                resources[i] = new Resource(_terms[i].Term + _terms[i].Value);
                _resourceTermDict.Add(resources[i], _terms[i]);
            }
            _termSearchIndex = new SearchIndex(resources, _terms.Length);
        }

        
        /// <summary>
        /// Updates visible content view based on view mode
        /// </summary>
        private void UpdateViewMode() {
            _defaultContent.RemoveFromHierarchy();
            _searchContent.RemoveFromHierarchy();
            switch (_viewMode) {
                case ViewMode.Default:
                    _root.Add(_defaultContent);
                    _defaultContent.PlaceInFront(_tableHeader);
                    break;
                
                case ViewMode.Search:
                    _root.Add(_searchContent);
                    _searchContent.PlaceInFront(_tableHeader);
                    break;
            }
        }
        
    }
}