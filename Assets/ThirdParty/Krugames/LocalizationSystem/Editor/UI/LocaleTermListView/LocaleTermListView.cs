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


        public delegate void ElementSelectDelegate(LocaleTermListView self, LocaleTerm localeTerm);
        public event ElementSelectDelegate OnTermSelect;
        
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
            
            
            /*_searchToolbar.SearchField.RegisterCallback<ChangeEvent<string>>(SearchChangeEvent);
            _pagerToolbar.OnPageChange += PageChangeEvent;

            _termView.AddToClassList(TermViewClassName);*/

            _root.Add(_searchToolbar);
            _root.Add(_tableHeader);
            _root.Add(_pagerToolbar);
            
            _viewMode = ViewMode.Default;
            UpdateViewMode();

            _defaultContent.SetTerms(_terms);
            //SetTerms(terms);
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