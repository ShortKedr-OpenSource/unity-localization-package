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

//TODO can be templated to generic ListView<TItemType>, ListViewElement<TItemType>
//TODO fix issue with PagerToolbar when it shows 1/0 with PageCount=0
//TODO test callback overcall, fix if necessary
namespace Krugames.LocalizationSystem.Editor.UI {
    public class LocaleTermListView : Box {
        
        private enum ViewMode {
            Default = 0,
            Search = 1
        }
        
        private const string RootClassName = nameof(LocaleTermListView) + "_Root";
        private const int DefaultItemPerPage = 12;

        
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
        private string _lastSearchString = string.Empty;
        private int _lastDefaultPage;

        private ViewMode _viewMode;
        private LocaleTermListViewContent.SelectionInfo _selection = LocaleTermListViewContent.SelectionInfo.Nothing;
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
            _itemsPerPage = itemsPerPage;
            
            styleSheets.Add(LocalizationEditorStyles.GlobalStyle);
            
            _root = new VisualElement();
            _root.styleSheets.Add(LocalizationEditorStyles.GlobalStyle);
            _root.AddToClassList(RootClassName);
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
            
            _defaultContent.OnPageChange += ContentPageChange;
            _searchContent.OnPageChange += ContentPageChange;
            
            _defaultContent.OnSelect += ElementSelectEvent;
            _searchContent.OnSelect += ElementSelectEvent;

            _root.Add(_searchToolbar);
            _root.Add(_tableHeader);
            _root.Add(_pagerToolbar);

            SetTerms(terms);
            SetViewMode(ViewMode.Default);
        }

        private void ElementSelectEvent(LocaleTermListViewContent self, LocaleTermListViewContent.SelectionInfo selectionInfo) {
            _selection = selectionInfo;
            OnTermSelect?.Invoke(this, _selection.Term);
        }

        private void ContentPageChange(LocaleTermListViewContent self, int newPageNumber) {
            if (_viewMode == ViewMode.Default && self == _defaultContent) {
                _pagerToolbar.ChangePageWithoutNotify(_defaultContent.CurrentPage);
            }
            
            if (_viewMode == ViewMode.Search && self == _searchContent) {
                _pagerToolbar.ChangePageWithoutNotify(_searchContent.CurrentPage);
            }
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
            switch (_viewMode) {
                case ViewMode.Default:
                    _defaultContent.SetPageWithoutNotify(newPage);
                    break;
                
                case ViewMode.Search:
                    _searchContent.SetPageWithoutNotify(newPage);
                    break;
            }
        }

        private void SearchChangeEvent(ChangeEvent<string> evt) {
            Search(evt.newValue);
        }

        public void SetTerms(LocaleTerm[] terms) {
            CancelSearch();
            
            if (terms == null) _terms = Array.Empty<LocaleTerm>();
            else _terms = terms;
            
            RebuildSearchIndex();
            
            _defaultContent.SetTerms(_terms);
            SetViewMode(ViewMode.Default, true);
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
            
            _defaultContent.RemoveSelection();
            _searchContent.RemoveSelection();
            switch (_viewMode) {
                case ViewMode.Default:
                    _root.Add(_defaultContent);
                    _defaultContent.PlaceInFront(_tableHeader);
                    _defaultContent.Select(_selection.Term);
                    break;
                
                case ViewMode.Search:
                    _root.Add(_searchContent);
                    _searchContent.PlaceInFront(_tableHeader);
                    _searchContent.Select(_selection.Term);
                    break;
            }
        }

        private void SetViewMode(ViewMode viewMode, bool force = false) {
            if (!force && _viewMode == viewMode) return;
            _viewMode = viewMode;

            int pageCount = (viewMode == ViewMode.Default) ? _defaultContent.PageCount : _searchContent.PageCount;
            int currentPage = (viewMode == ViewMode.Default) ? _defaultContent.CurrentPage : _searchContent.CurrentPage;

            _pagerToolbar.SetPageCount(pageCount);
            _pagerToolbar.ChangePageWithoutNotify(currentPage);
            
            UpdateViewMode();
        }
        
        public void Search(string searchString) {
            if (searchString == _lastSearchString) return;
            _lastSearchString = searchString;
            
            if (searchString.Length < SearchIndex.SearchLength) {
                CancelSearch();
            } else {
                var searchResult = _termSearchIndex.GetSearchResult(searchString);
                LocaleTerm[] foundTerms = new LocaleTerm[searchResult.Count];
                for (int i = 0; i < searchResult.Count; i++) {
                    foundTerms[i] = _resourceTermDict[searchResult[i]];
                }
                _lastDefaultPage = _defaultContent.CurrentPage;
                _searchContent.SetTerms(foundTerms);
                _searchContent.SetPageWithoutNotify(1);
                SetViewMode(ViewMode.Search);
            }
        }
        
        public void CancelSearch() {
            if (_viewMode == ViewMode.Search) {
                //_searchToolbar.SearchField.SetValueWithoutNotify(string.Empty);
                SetViewMode(ViewMode.Default);
                _defaultContent.SetPageWithoutNotify(_lastDefaultPage);
                _lastSearchString = string.Empty;
            }
        }

    }
}