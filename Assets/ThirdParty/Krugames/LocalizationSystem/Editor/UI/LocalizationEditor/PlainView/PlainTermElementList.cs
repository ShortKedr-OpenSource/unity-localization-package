using System;
using System.Collections.Generic;
using Krugames.LocalizationSystem.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor.PlainView {
    public class PlainTermElementList : ScrollView {

        private struct PageInfo {
            public readonly int StartIndex;
            public readonly int EndIndex;
            public readonly int ElementCount;

            public PageInfo(int startIndex, int endIndex, int elementCount) {
                StartIndex = startIndex;
                EndIndex = endIndex;
                ElementCount = elementCount;
            }
        }
        
        public struct SelectionInfo {

            public static readonly SelectionInfo Nothing = new SelectionInfo(-1, null); 
            
            public readonly int Index;
            public readonly LocaleTerm Term;

            public SelectionInfo(int index, LocaleTerm term) {
                Index = index;
                Term = term;
            }

            public static bool operator ==(SelectionInfo left, SelectionInfo right) {
                if (left.Index == right.Index && left.Term == right.Term) return true;
                return false;
            }

            public static bool operator !=(SelectionInfo left, SelectionInfo right) {
                if (left.Index != right.Index || left.Term != right.Term) return true;
                return false;
            }
            
            public bool Equals(SelectionInfo other) {
                return Index == other.Index && Equals(Term, other.Term);
            }

            public override bool Equals(object obj) {
                return obj is SelectionInfo other && Equals(other);
            }

            public override int GetHashCode() {
                unchecked {
                    return (Index * 397) ^ (Term != null ? Term.GetHashCode() : 0);
                }
            }
        }

        private LocaleTerm[] _terms;
        private int _pageLength;

        private PlainTermElement[] _pageElements;

        private int _pageCount;
        private int _currentPage;
        private SelectionInfo _selection = SelectionInfo.Nothing;

        private Dictionary<LocaleTerm, int> _indexByTerm = new Dictionary<LocaleTerm, int>(16);

        public delegate void SelectDelegate(PlainTermElementList self, SelectionInfo selectionInfo);
        public delegate void PageChangeDelegate(PlainTermElementList self, int newPageNumber);
        public delegate void LayoutChangeDelegate(PlainTermElementList self, LocaleTerm[] newLayout);

        public event SelectDelegate OnSelect;
        public event PageChangeDelegate OnPageChange;
        public event LayoutChangeDelegate OnLayoutChange;


        public LocaleTerm[] Terms => _terms;
        
        public int PageLength => _pageLength;
        
        public PlainTermElement[] PageElements => _pageElements;

        public int PageCount => _pageCount;

        public SelectionInfo Selection => _selection;
        
        //TODO add CurrentPage get set;

        public PlainTermElementList(int pageLength) : this(null, pageLength){
        }


        public PlainTermElementList(LocaleTerm[] terms, int pageLength) : base(ScrollViewMode.Vertical) {
            _pageLength = pageLength;

            _pageElements = new PlainTermElement[pageLength];
            for (int i = 0; i < _pageElements.Length; i++) {
                FillRule fillRule = (i % 2 == 0) ? FillRule.Even : FillRule.Odd;
                _pageElements[i] = new PlainTermElement(null, fillRule);
                _pageElements[i].OnActiveStateChanged += Event_ElementActiveStateChanged;
                Add(_pageElements[i]);
            }

            showVertical = true;
            focusable = true;
            pickingMode = PickingMode.Position;
            
            RegisterCallback<KeyUpEvent>(Event_KeyUp);

            _currentPage = 1;
            SetTerms(terms);
        }

        private void Rebuild(bool updateSelection = true) {
            var pageTerms = GetPageTerms(_currentPage);
            for (int i = 0; i < _pageElements.Length; i++) {
                if (i < pageTerms.Length) _pageElements[i].SetTerm(pageTerms[i]);
                else _pageElements[i].SetTerm(null);
            }
            if (updateSelection) UpdateSelection();
        }
        
        private LocaleTerm[] GetPageTerms(int pageNumber) {
            if (pageNumber < 1 || pageNumber > _pageCount) return Array.Empty<LocaleTerm>();
            PageInfo pageInfo = GetPageInfo(pageNumber);
            LocaleTerm[] pageTerms = new LocaleTerm[pageInfo.ElementCount];
            for (int i = 0; i < pageTerms.Length; i++) {
                pageTerms[i] = _terms[pageInfo.StartIndex + i];
            }
            return pageTerms;
        }
        
        private PageInfo GetPageInfo(int pageNumber) {
            int startIndex = ((pageNumber-1) * _pageLength);
            int endIndex = (pageNumber == _pageCount)
                ? (startIndex + (_terms.Length-1) % _pageLength)
                : (pageNumber * _pageLength) - 1;
            int count = endIndex - startIndex + 1;
            return new PageInfo(startIndex, endIndex, count);
        }

        private int GetTermIndexByListElement(PlainTermElement element) {
            if (_pageCount == 0) return -1;
            int index = 0;
            for (int i = 0; i < _pageElements.Length; i++) {
                if (element == _pageElements[i]) {
                    index = i;
                    break;
                }
            }
            return index + (_currentPage-1) * _pageLength;
        }
        
        private PlainTermElement GetListElementByTermIndex(int index) {
            PageInfo pageInfo = GetPageInfo(_currentPage);
            if (pageInfo.StartIndex > index || pageInfo.EndIndex < index) return null;
            return _pageElements[index - pageInfo.StartIndex];
        }
        
        private void UpdateSelection() {
            for (int i = 0; i < _pageElements.Length; i++) _pageElements[i].Active = false;
            if (_selection == SelectionInfo.Nothing) return;
            var activeElement = GetListElementByTermIndex(_selection.Index);
            if (activeElement != null) activeElement.Active = true;
        }
        
        private void BuildIndexCache() {
            _indexByTerm.Clear();
            for (int i = 0; i < _terms.Length; i++) {
                if (_indexByTerm.ContainsKey(_terms[i])) continue;
                _indexByTerm.Add(_terms[i], i);
            }
        }
        
        private void SetPage(int pageNumber, bool updateSelection = true, bool notify = true) {
            _currentPage = pageNumber;
            if (_pageCount == 0) _currentPage = 0; 
            else if (_currentPage > _pageCount) _currentPage = _pageCount;
            else if (_currentPage < 1) _currentPage = 1;
            if (notify) OnPageChange?.Invoke(this, _currentPage);
            Rebuild(updateSelection);
        }

        private void Event_KeyUp(KeyUpEvent evt) {
            if (_selection == SelectionInfo.Nothing) return;
            
            PageInfo pageInfo = GetPageInfo(_currentPage);
            
            int nextTermIndex = _selection.Index + 1;
            int previousTermIndex = _selection.Index - 1;
            
            if (evt.keyCode == KeyCode.DownArrow && nextTermIndex < _terms.Length) {
                if (nextTermIndex > pageInfo.EndIndex) SetPage(_currentPage + 1, false);
                Select(nextTermIndex);
            }

            if (evt.keyCode == KeyCode.UpArrow && previousTermIndex >= 0) {
                if (previousTermIndex < pageInfo.StartIndex) SetPage(_currentPage - 1, false);
                Select(previousTermIndex);
            }
        }

        private void Event_ElementActiveStateChanged(PlainTermElement element, bool newState) {
            if (newState) {
                for (int i = 0; i < _pageElements.Length; i++) {
                    if (_pageElements[i] == element) continue;
                    if (_pageElements[i].Active) _pageElements[i].Active = false;
                }
            }
        }

        public void SetTerms(LocaleTerm[] terms) {
            if (terms == null) {
                _terms = Array.Empty<LocaleTerm>();
                _pageCount = 0;
            } else {
                _terms = terms;
                _pageCount = Mathf.CeilToInt((float) _terms.Length / _pageLength);
            }
            _selection = SelectionInfo.Nothing;
            BuildIndexCache();
            OnLayoutChange?.Invoke(this, _terms);
            Rebuild();
        }
        
        public void SetPageLength(int pageLength) {
            _pageLength = pageLength;
            _pageCount = Mathf.CeilToInt((float) _terms.Length / _pageLength);
            Rebuild();
        }
        
        public void UpdateView() {
            for (int i = 0; i < _pageElements.Length; i++) {
                _pageElements[i].Update();
            }
        }
        
        public void RemoveSelection() {
            _selection = SelectionInfo.Nothing;
            UpdateSelection();
        }
        
        public void Select(int index) {
            if (index < 0 || index >= _terms.Length) return;
            _selection = new SelectionInfo(index, _terms[index]);
            OnSelect?.Invoke(this, _selection);
            UpdateSelection();
        }
        
        public void Select(LocaleTerm term) {
            if (term == null) return;
            if (_indexByTerm.ContainsKey(term)) Select(_indexByTerm[term]);
        }
        
        public void SetPageWithoutNotify(int pageNumber) {
            SetPage(pageNumber, true, false);
        }
    }
}