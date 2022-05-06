using System;
using System.Collections.Generic;
using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Models;
using RenwordDigital.StringSearchEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

//TODO move to new LocaleTermListView folder
//TODO separate elements to different files
//TODO move LocaleTermTableElement to new folder for this class
namespace Krugames.LocalizationSystem.Editor.UIElements {
    public class LocaleTermListView : Box {

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
        
        private LocaleTerm _selectedLocaleTerm = null;

        
        public delegate void TermSelectDelegate(LocaleTerm selectedTerm);
        public event TermSelectDelegate OnTermSelect;
        
        
        //TODO element getters,

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
            
            _termView.AddToClassList(TermViewClassName);

            _contentContent.Add(_toolbar);
            _contentContent.Add(_tableHeader);
            _contentContent.Add(_termView);
            _contentContent.Add(_pagerToolbar);

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
            if (_searchMode) CancelTermsSearchResult();
            else RebuildPage();
        }

        private void TermElementClickEvent(LocaleTermListViewElement element) {
            SelectLocaleTerm(element.LocaleTerm);
        }
        
        private void SearchChangeEvent(ChangeEvent<string> evt) {
            Debug.Log("Omaewa");
            string searchString = evt.newValue;
            if (searchString.Length < SearchIndex.SearchLength) {
                if (_searchMode) {
                    RebuildPage();
                    _searchMode = false;
                }
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

        private void SearchTerms(string searchString) {

            if (searchString == _toolbar.SearchField.value) {
                return;
            }
            
            if (searchString.Length < SearchIndex.SearchLength) {
                CancelTermsSearchResult();
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
        
        private void CancelTermsSearchResult() {
            if (_searchMode) {
                RebuildPage();
                _toolbar.SearchField.SetValueWithoutNotify(string.Empty);
                _searchMode = false;
            }
        }

        private void SelectLocaleTerm(LocaleTerm localeTerm) {
            _selectedLocaleTerm = localeTerm;
            
            for (int i = 0; i < _pageElements.Length; i++) _pageElements[i].RemoveFromClassList(SelectedClassName);
            
            if (_selectedLocaleTerm != null) {
                for (int i = 0; i < _pageElements.Length; i++) {
                    if (_pageElements[i].LocaleTerm == _selectedLocaleTerm) _pageElements[i].AddToClassList(SelectedClassName);
                }
            }
            
            OnTermSelect?.Invoke(localeTerm);
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
            SelectLocaleTerm(_selectedLocaleTerm);
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

        private LocaleTerm[] GetPageTerms(int pageNumber) {
            int pageCount = GetPageCount();
            if (pageNumber < 1 || pageNumber > pageCount) return Array.Empty<LocaleTerm>();

            int startIndex = ((pageNumber-1) * _itemsPerPage);
            int endIndex = (pageNumber == pageCount)
                ? (startIndex - 1 + _terms.Length % _itemsPerPage)
                : (pageNumber) * _itemsPerPage - 1;

            int count = endIndex - startIndex + 1;
            LocaleTerm[] pageTerms = new LocaleTerm[count];

            for (int i = 0; i < pageTerms.Length; i++) {
                pageTerms[i] = _terms[startIndex + i];
            }

            return pageTerms;
        }
    }
}