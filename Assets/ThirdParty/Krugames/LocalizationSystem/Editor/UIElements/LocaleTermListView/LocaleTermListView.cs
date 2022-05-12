using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Models;
using RenwordDigital.StringSearchEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

//TODO can be generalised (universal type)
//TODO stay rebuildpage same way, but add searchModeLogic. Add MultiPage search
//TODO remake select to term_index;
namespace Krugames.LocalizationSystem.Editor.UIElements {
    public class LocaleTermListView : Box {

        private struct PageInfo {
            public readonly int startIndex;
            public readonly int endIndex;
            public readonly int count;

            public PageInfo(int startIndex, int endIndex, int count) {
                this.startIndex = startIndex;
                this.endIndex = endIndex;
                this.count = count;
            }
        }

        private class ViewData {
            public LocaleTerm[] pageTerms;
            public int currentPage;
            public int selectedTermIndex;
            
            public ViewData(LocaleTerm[] pageTerms, int currentPage, int selectedTermIndex = -1) {
                this.pageTerms = pageTerms;
                this.currentPage = currentPage;
                this.selectedTermIndex = selectedTermIndex;
            }
        }

        private enum ViewMode {
            Default = 0,
            Search = 1
        }
        
        
        private const int DefaultItemPerPage = 12;
        
        private const string TermViewClassName = nameof(LocaleTermListView) + "_List";
        private const string SelectedClassName = "Selected";
        
        
        private LocaleTerm[] _terms;

        private LocaleTermListViewContent _contentContent;
        private TittleSearchToolbar _toolbar;
        private LocaleTermListViewTableHeader _tableHeader;
        private VisualElement _termView;
        private PagerToolbar _pagerToolbar;

        private LocaleTermListViewElement[] _pageElements;
        private int _itemsPerPage;
        
        private SearchIndex _termSearchIndex;
        private Dictionary<Resource, LocaleTerm> _resourceTermDict;
        private bool _searchMode = false;

        private ViewMode _viewMode;
        private ViewData _defaultViewData;
        private ViewData _searchViewData;

        
        public delegate void ElementSelectDelegate(LocaleTermListView self, LocaleTerm localeTerm);
        public event ElementSelectDelegate OnTermSelect;
        
        
        public LocaleTerm[] Terms => _terms;
        
        public LocaleTerm SelectedTerm => (GetCurrentViewData().selectedTermIndex < 0 || GetCurrentViewData().selectedTermIndex >= _terms.Length)
            ? null
            : _terms[GetCurrentViewData().selectedTermIndex];
        
        
        public LocaleTermListView(int itemPerPage = DefaultItemPerPage) : this(Array.Empty<LocaleTerm>(), itemPerPage) {
        }
        
        public LocaleTermListView(LocaleTerm[] terms, int itemsPerPage = DefaultItemPerPage) {
            styleSheets.Add(LocalizationEditorStyles.GlobalStyle);
            
            _terms = terms;
            _itemsPerPage = itemsPerPage;

            _contentContent = new LocaleTermListViewContent();
            Add(_contentContent);
            
            _toolbar = new TittleSearchToolbar("Terms");
            _tableHeader = new LocaleTermListViewTableHeader();
            _termView = new VisualElement();
            _pagerToolbar = new PagerToolbar(GetPageCount());
            
            _pageElements = new LocaleTermListViewElement[_itemsPerPage];
            for (int i = 0; i < _pageElements.Length; i++) {
                _pageElements[i] = new LocaleTermListViewElement(null, (i % 2 == 0) ? FillRule.Even : FillRule.Odd);
                _pageElements[i].OnClick += TermElementClickEvent;
                _pageElements[i].AddManipulator(new ClickSelector());
                _termView.Add(_pageElements[i]); 
            }
            
            _toolbar.SearchField.RegisterCallback<ChangeEvent<string>>(SearchChangeEvent);
            _pagerToolbar.OnPageChange += PageChangeEvent;

            focusable = true;
            pickingMode = PickingMode.Position;
            
            //TODO move to method
            RegisterCallback<KeyUpEvent>(evt => {

                LocaleTerm selectedTerm = SelectedTerm;
                if (selectedTerm == null) return;
                
                int currentPage = _pagerToolbar.CurrentPage;
                PageInfo pageInfo = GetPageInfo(currentPage);
                
                if (evt.keyCode == KeyCode.DownArrow && GetCurrentViewData().selectedTermIndex + 1 < _terms.Length) {
                    int indexToSelect = GetCurrentViewData().selectedTermIndex + 1;
                    if (indexToSelect > pageInfo.endIndex) _pagerToolbar.CurrentPage += 1;
                    SelectElement(indexToSelect);
                }

                if (evt.keyCode == KeyCode.UpArrow && GetCurrentViewData().selectedTermIndex - 1 >= 0) {
                    int indexToSelect = GetCurrentViewData().selectedTermIndex - 1;
                    if (indexToSelect < pageInfo.startIndex) _pagerToolbar.CurrentPage -= 1;
                    SelectElement(indexToSelect);
                }
            });

            _termView.AddToClassList(TermViewClassName);

            _contentContent.Add(_toolbar);
            _contentContent.Add(_tableHeader);
            _contentContent.Add(_termView);
            _contentContent.Add(_pagerToolbar);

            _viewMode = ViewMode.Default;
            _defaultViewData = new ViewData(terms, 1);
            _searchViewData = new ViewData(Array.Empty<LocaleTerm>(), 1);
            
            SetTerms(terms);
        }

        public void SetTerms(LocaleTerm[] terms) {
            _terms = terms;
            RebuildSearchIndex();
            _pagerToolbar.SetPageCount(GetPageCount());
            RebuildPage();
        }
        
        public void UpdateListValues() {
            for (int i = 0; i < _pageElements.Length; i++) {
                _pageElements[i].Update();
            }
        }

        private void PageChangeEvent(PagerToolbar self, int newPage) {
            if (_searchMode) CancelSearch();
            else RebuildPage();
        }

        private void TermElementClickEvent(LocaleTermListViewElement element) {
            SelectElement(GetTermIndexByListElement(element));
        }
        
        private void SearchChangeEvent(ChangeEvent<string> evt) {
            SearchTerms(evt.newValue);
        }

        private void SearchTerms(string searchString) {
            if (searchString == _toolbar.SearchField.value) return;

            if (searchString.Length < SearchIndex.SearchLength) {
                CancelSearch();
            } else {
                var searchResult = _termSearchIndex.GetSearchResult(searchString);
                LocaleTerm[] foundTerms = new LocaleTerm[searchResult.Count];
                for (int i = 0; i < searchResult.Count; i++) {
                    foundTerms[i] = _resourceTermDict[searchResult[i]];
                }
                RebuildPage(foundTerms);
                _searchMode = true;
            }
        }
        
        private void CancelSearch() {
            if (_searchMode) {
                RebuildPage();
                _toolbar.SearchField.SetValueWithoutNotify(string.Empty);
                _searchMode = false;
            }
        }

        private void SelectElement(int index) {
            if (index < 0 || index >= _terms.Length) GetCurrentViewData().selectedTermIndex = -1;
            else GetCurrentViewData().selectedTermIndex = index;
            RebuildSelection();
            Focus();
            OnTermSelect?.Invoke(this, _terms[index]);
        }

        private int GetTermIndexByListElement(LocaleTermListViewElement element) {
            if (element.LocaleTerm == null) return -1;
            if (GetPageCount() == 0) return -1;
            return (_pagerToolbar.CurrentPage-1) * _itemsPerPage + _termView.IndexOf(element);
        }

        private LocaleTermListViewElement GetListElementByTermIndex(int index) {
            if (index < 0 || index >= _terms.Length) return null;
            
            //TODO fix
            int currentPage = _pagerToolbar.CurrentPage;//CurrentPage;
            int indexedPage = (index / _itemsPerPage) + 1;

            if (currentPage != indexedPage) return null;
            
            int elementIndex = index % _itemsPerPage;
            return _pageElements[elementIndex];
        }

        private void RebuildSelection() {
            for (int i = 0; i < _pageElements.Length; i++) _pageElements[i].RemoveFromClassList(SelectedClassName);
            GetListElementByTermIndex(GetCurrentViewData().selectedTermIndex)?.AddToClassList(SelectedClassName);
        }
        
        private void RebuildPage() {
            var pageTerms = GetPageTerms(_pagerToolbar.CurrentPage);
            RebuildPage(pageTerms);
        }

        private void RebuildPage(LocaleTerm[] pageTerms) {
            for (int i = 0; i < _pageElements.Length; i++) {
                if (i < pageTerms.Length) _pageElements[i].SetTerm(pageTerms[i]);
                else _pageElements[i].SetTerm(null);
            }
            SelectElement(GetCurrentViewData().selectedTermIndex);
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

        private int GetPageCount() {
            return Mathf.CeilToInt((float)_terms.Length / _itemsPerPage);
        }

        private PageInfo GetPageInfo(int pageNumber) {
            int pageCount = GetPageCount();
            int startIndex = ((pageNumber-1) * _itemsPerPage);
            int endIndex = (pageNumber == pageCount)
                ? (startIndex - 1 + _terms.Length % _itemsPerPage)
                : (pageNumber) * _itemsPerPage - 1;
            int count = endIndex - startIndex + 1;
            return new PageInfo(startIndex, endIndex, count);
        }

        private LocaleTerm[] GetPageTerms(int pageNumber) {
            int pageCount = GetPageCount();
            if (pageNumber < 1 || pageNumber > pageCount) return Array.Empty<LocaleTerm>();

            PageInfo pageInfo = GetPageInfo(pageNumber);
            
            LocaleTerm[] pageTerms = new LocaleTerm[pageInfo.count];

            for (int i = 0; i < pageTerms.Length; i++) {
                pageTerms[i] = _terms[pageInfo.startIndex + i];
            }

            return pageTerms;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ViewData GetCurrentViewData() {
            switch (_viewMode) {
                case ViewMode.Default: default:
                    return _defaultViewData;
                case ViewMode.Search:
                    return _searchViewData;
            }
        }
    }
}