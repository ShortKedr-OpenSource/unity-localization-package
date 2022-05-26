using System;
using System.Collections.Generic;
using Krugames.LocalizationSystem.Common.Editor.UnityInternal;
using Krugames.LocalizationSystem.Implementation;
using Krugames.LocalizationSystem.Models;
using Krugames.LocalizationSystem.Models.Utility.Editor;
using Krugames.LocalizationSystem.RapidStorage.Servers;
using RenwordDigital.StringSearchEngine;
using ThirdParty.Krugames.LocalizationSystem.Editor.UI;
using UnityEngine;
using UnityEngine.UIElements;

//TODO allow remove terms
namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor.PlainView {
    public class PlainLocaleEditor : VisualElement {
        
        private enum ViewMode {
            Default = 0,
            Search = 1
        }

        private const int DefaultPageLength = 25;
        
        private Locale _locale;
        private bool _canRemoveTerm = true;
        private bool _canAddTerm = true;
        
        private LocaleTerm[] _terms;

        private TittleSearchToolbar _topToolbar;
        private PlainTermElementTableHeader _tableHeader;
        private VisualElement _listContainer;
        private PagerTooltipToolbar _bottomToolbar;
        
        private PlainTermElementList _defaultList;
        private PlainTermElementList _searchList;

        private Button _addTermButton;
        
        private SearchIndex _termSearchIndex;
        private Dictionary<Resource, LocaleTerm> _resourceTermDict;
        private string _lastSearchString = string.Empty;
        private int _lastDefaultPage;
        
        private ViewMode _viewMode;
        private PlainTermElementList.SelectionInfo _selection = PlainTermElementList.SelectionInfo.Nothing;
        private int _itemsPerPage;

        private LocaleTermSelector _localeTermSelector;
        private OptionPopup _managedElementOptions;
        private OptionPopup _managedElementOptionsRemoveRestricted;
        private PlainTermElement _managedElement = null;
        
        public delegate void ElementDelegate(PlainLocaleEditor self, LocaleTerm localeTerm);
        public event ElementDelegate OnTermSelect;
        
        public Locale Locale => _locale;

        public bool CanRemoveTerm {
            get => _canRemoveTerm;
            set => _canRemoveTerm = value;
        }

        public bool CanAddTerm {
            get => _canAddTerm;
            set {
                _canAddTerm = value;
                UpdateBottomMenuButtons();
            }
        }

        public PlainLocaleEditor(int itemsPerPage = DefaultPageLength) : this(null, itemsPerPage) {
        }

        public PlainLocaleEditor(Locale locale, int itemsPerPage = DefaultPageLength) {
            _itemsPerPage = itemsPerPage;

            _topToolbar = new TittleSearchToolbar("Term Editor (Plain)");
            _tableHeader = new PlainTermElementTableHeader();
            _bottomToolbar = new PagerTooltipToolbar(0);
            
            _defaultList = new PlainTermElementList(null, DefaultPageLength);
            _searchList = new PlainTermElementList(null, DefaultPageLength);

            _addTermButton = new Button(Event_AddTermClick){text="Add Term"};

            focusable = true;
            pickingMode = PickingMode.Position;

            var defaultPageElements = _defaultList.PageElements;
            for (int i = 0; i < defaultPageElements.Length; i++) {
                defaultPageElements[i].OnPropertiesClick += Event_ElementPropertiesClick;
                defaultPageElements[i].OnNoteChange += Event_ElementNoteChange;
            }

            var searchPageElements = _searchList.PageElements;
            for (int i = 0; i < searchPageElements.Length; i++) {
                searchPageElements[i].OnPropertiesClick += Event_ElementPropertiesClick;
                searchPageElements[i].OnNoteChange += Event_ElementNoteChange;
            }
            
            InitializeOptionPopups();

            _topToolbar.SearchField.RegisterCallback<ChangeEvent<string>>(Event_SearchChange);
            _bottomToolbar.OnPageChange += Event_ToolbarPageChange;

            _defaultList.OnPageChange += Event_ContentPageChange;
            _searchList.OnPageChange += Event_ContentPageChange;

            _defaultList.OnSelect += Event_ElementSelectEvent;
            _searchList.OnSelect += Event_ElementSelectEvent;
            
            Add(_topToolbar);
            Add(_tableHeader);
            Add(_bottomToolbar);
            
            SetLocale(locale);
            SetViewMode(ViewMode.Default);
            UpdateBottomMenuButtons();
        }

        private void InitializeOptionPopups() {
            _managedElementOptions = new OptionPopup(new SelectorListPopup.Element[] {
                new SelectorListPopup.Element("Properties", ManagedElement_OpenProperties),
                new SelectorListPopup.Element("Remove", ManagedElement_Delete),
            });

            _managedElementOptionsRemoveRestricted = new OptionPopup(new SelectorListPopup.Element[] {
                new SelectorListPopup.Element("Properties", ManagedElement_OpenProperties),
            });

            _localeTermSelector = LocaleTermSelector.Create(Event_OnAddTermSelect);
        }

        private void SetTerms(LocaleTerm[] terms) {
            CancelSearch();
            _topToolbar.SearchField.SetValueWithoutNotify(string.Empty);
            _selection = PlainTermElementList.SelectionInfo.Nothing;

            if (terms == null) _terms = Array.Empty<LocaleTerm>();
            else _terms = terms;
            
            RebuildSearchIndex();
            
            _defaultList.SetTerms(_terms);
            SetViewMode(ViewMode.Default, true);
        }
        
        private void RebuildSearchIndex() {
            _resourceTermDict = new Dictionary<Resource, LocaleTerm>(_terms.Length);
            Resource[] resources = new Resource[_terms.Length];
            for (int i = 0; i < _terms.Length; i++) {
                resources[i] = new Resource(_terms[i].Term + _terms[i].Value + TermNoteServer.GetNote(_terms[i].Term));
                _resourceTermDict.Add(resources[i], _terms[i]);
            }
            _termSearchIndex = new SearchIndex(resources, _terms.Length);
        }
        
        private void UpdateViewMode() {
            _defaultList.RemoveFromHierarchy();
            _searchList.RemoveFromHierarchy();
            
            _defaultList.RemoveSelection();
            _searchList.RemoveSelection();
            switch (_viewMode) {
                case ViewMode.Default:
                    Add(_defaultList);
                    _defaultList.PlaceInFront(_tableHeader);
                    _defaultList.Select(_selection.Term);
                    break;
                
                case ViewMode.Search:
                    Add(_searchList);
                    _searchList.PlaceInFront(_tableHeader);
                    _searchList.Select(_selection.Term);
                    break;
            }
        }
        
        private void SetViewMode(ViewMode viewMode, bool force = false) {
            if (!force && _viewMode == viewMode) return;
            _viewMode = viewMode;

            int pageCount = (viewMode == ViewMode.Default) ? _defaultList.PageCount : _searchList.PageCount;
            int currentPage = (viewMode == ViewMode.Default) ? _defaultList.CurrentPage : _searchList.CurrentPage;

            _bottomToolbar.SetPageCount(pageCount);
            _bottomToolbar.ChangePageWithoutNotify(currentPage);
            
            UpdateViewMode();
        }

        private void UpdateBottomMenuButtons() {
            _addTermButton.RemoveFromHierarchy();
            if (_canAddTerm && _locale != null) _bottomToolbar.LeftAnchor.Add(_addTermButton);
        }

        private void Event_ElementNoteChange(PlainTermElement element, string newNote) {
            //TODO optimize search with dynamic search index changes
            RebuildSearchIndex();
        }
        
        private void Event_OnAddTermSelect(Type selectedTermType, Type valueType) {
            if (_locale == null) return;
            LocaleTerm term = LocaleUtility.AddLocaleTerm(_locale, "none_" + valueType.Name.ToLower(), selectedTermType);
            SetTerms(_locale.GetTerms());
            Select(term);
        }
        
        private void Event_AddTermClick() {
            if (_locale == null) return;
            Rect buttonRect = _addTermButton.worldBound;
            buttonRect.width = 0;
            buttonRect.height = 0;
            UnityEditor.PopupWindow.Show(buttonRect, _localeTermSelector);
        }
        
        private void Event_ElementSelectEvent(PlainTermElementList self, PlainTermElementList.SelectionInfo selectionInfo) {
            _selection = selectionInfo;
            OnTermSelect?.Invoke(this, _selection.Term);
        }

        private void Event_ContentPageChange(PlainTermElementList self, int newPageNumber) {
            if (_viewMode == ViewMode.Default && self == _defaultList) {
                _bottomToolbar.ChangePageWithoutNotify(_defaultList.CurrentPage);
            }
            
            if (_viewMode == ViewMode.Search && self == _searchList) {
                _bottomToolbar.ChangePageWithoutNotify(_searchList.CurrentPage);
            }
        }

        private void Event_ToolbarPageChange(PagerTooltipToolbar self, int newPage) {
            switch (_viewMode) {
                case ViewMode.Default:
                    _defaultList.SetPageWithoutNotify(newPage);
                    break;
                
                case ViewMode.Search:
                    _searchList.SetPageWithoutNotify(newPage);
                    break;
            }
        }

        private void Event_SearchChange(ChangeEvent<string> evt) {
            Search(evt.newValue);
        }
        
        private void Event_ElementPropertiesClick(PlainTermElement element) {
            _managedElement = element;
            if (_managedElement == null || _managedElement.LocaleTerm == null) return;
            OptionPopup popup = (CanRemoveTerm) ? _managedElementOptions : _managedElementOptionsRemoveRestricted;
            Rect mouseRect = new Rect(Event.current.mousePosition, Vector2.one); 
            UnityEditor.PopupWindow.Show(mouseRect, popup);
        }
        
        private void ManagedElement_OpenProperties() {
            if (_managedElement == null || _managedElement.LocaleTerm == null) return;
            EditorInternalUtility.OpenPropertyEditor(_managedElement.LocaleTerm);
        }
        
        private void ManagedElement_Delete() {
            if (_locale == null) return;
            if (_managedElement == null || _managedElement.LocaleTerm == null) return;
            bool result = LocaleUtility.RemoveLocaleTerm(_locale, _managedElement.LocaleTerm);
            if (result) {
                SetTerms(_locale.GetTerms());
            }
        }

        public void SetLocale(Locale locale) {
            _locale = locale;
            if (_locale == null) SetTerms(null);
            else SetTerms(_locale.GetTerms());
            UpdateBottomMenuButtons();
        }

        public void UpdateView() {
            switch (_viewMode) {
                case ViewMode.Default:
                    _defaultList.UpdateView();
                    break;
                
                case ViewMode.Search:
                    _searchList.UpdateView();
                    break;
            }
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
                _lastDefaultPage = _defaultList.CurrentPage;
                _searchList.SetTerms(foundTerms);
                _searchList.SetPageWithoutNotify(1);
                SetViewMode(ViewMode.Search);
            }
        }
        
        public void CancelSearch() {
            if (_viewMode == ViewMode.Search) {
                SetViewMode(ViewMode.Default);
                _defaultList.SetPageWithoutNotify(_lastDefaultPage);
                _lastSearchString = string.Empty;
            }
        }
        
        public void Select(LocaleTerm term) {
            switch (_viewMode) {
                case ViewMode.Default:
                    _defaultList.Select(term);
                    break;
                
                case ViewMode.Search:
                    _searchList.Select(term);
                    break;
            }
        }
    }
}