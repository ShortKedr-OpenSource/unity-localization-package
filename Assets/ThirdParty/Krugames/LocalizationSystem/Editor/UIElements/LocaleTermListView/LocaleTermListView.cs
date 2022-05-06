using System;
using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Models;
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
        private int _itemPerPage;


        public delegate void TermSelectDelegate(LocaleTerm selectedTerm);
        public event TermSelectDelegate OnTermSelect;
        
        
        //TODO element getters,

        public LocaleTermListView(int itemPerPage = DefaultItemPerPage) : this(Array.Empty<LocaleTerm>(), itemPerPage) {
        }
        
        public LocaleTermListView(LocaleTerm[] terms, int itemPerPage = DefaultItemPerPage) {
            styleSheets.Add(LocalizationEditorStyles.GlobalStyle);
            
            _terms = terms;
            _itemPerPage = itemPerPage;

            _contentContent = new LocaleTermListViewContent();
            Add(_contentContent);
            
            _toolbar = new TittleSearchToolbar("Terms");
            _tableHeader = new LocaleTermListViewTableHeader();
            _termView = new VisualElement();
            _pagerToolbar = new PagerToolbar(0);
            
            _pageElements = new LocaleTermListViewElement[_itemPerPage];
            for (int i = 0; i < _pageElements.Length; i++) {
                _pageElements[i] = new LocaleTermListViewElement(null, (i % 2 == 0) ? FillRule.Even : FillRule.Odd);
                _pageElements[i].OnClick += TermElementClickEvent;
                _pageElements[i].AddManipulator(new ClickSelector());
                _termView.Add(_pageElements[i]);
            }
            
            _termView.AddToClassList(TermViewClassName);

            _contentContent.Add(_toolbar);
            _contentContent.Add(_tableHeader);
            _contentContent.Add(_termView);
            _contentContent.Add(_pagerToolbar);

            SetTerms(terms);
        }

        private void TermElementClickEvent(LocaleTermListViewElement element) {
            if (element.LocaleTerm == null) return;
            for (int i = 0; i < _pageElements.Length; i++) {
                _pageElements[i].RemoveFromClassList(SelectedClassName);
            }
            element.AddToClassList(SelectedClassName);
            //TODO cacheSelectedTermElement;
            OnTermSelect?.Invoke(element.LocaleTerm);
        }

        public void SetTerms(LocaleTerm[] terms) {
            _terms = terms;
            UpdateListView();
        }

        private void UpdateListView() {
            int length = Math.Min(_pageElements.Length, _terms.Length);
            for (int i = 0; i < length; i++) {
                _pageElements[i].SetTerm(_terms[i]);
            }
        }

        //TODO add pages
    }
}