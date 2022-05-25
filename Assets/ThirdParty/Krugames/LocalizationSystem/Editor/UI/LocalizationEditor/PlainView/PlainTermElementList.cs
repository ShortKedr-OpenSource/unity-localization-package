using System;
using Krugames.LocalizationSystem.Models;
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

        private const int DefaultPageLength = 25;
        
        private LocaleTerm[] _terms;
        private int _pageLength;

        private PlainTermElement[] _elements;
        //TODO add pages
        //TODO add stuff

        public PlainTermElementList() : this(null) {
        }

        public PlainTermElementList(LocaleTerm[] terms, int pageLength = DefaultPageLength) : base(ScrollViewMode.Vertical) {
            _pageLength = pageLength;

            _elements = new PlainTermElement[pageLength];
            for (int i = 0; i < _elements.Length; i++) {
                _elements[i] = new PlainTermElement(null, (i % 2 == 0) ? FillRule.Even : FillRule.Odd);
                _elements[i].OnActiveStateChanged += Event_ElementActiveStateChanged;
                Add(_elements[i]);
            }

            SetTerms(terms);
            
            showVertical = true;
        }

        private void Event_ElementActiveStateChanged(PlainTermElement element, bool newState) {
            if (newState) {
                for (int i = 0; i < _elements.Length; i++) {
                    if (_elements[i] == element) continue;
                    if (_elements[i].Active) _elements[i].Active = false;
                }
            }
        }

        public void SetTerms(LocaleTerm[] terms) {
            if (terms == null) _terms = Array.Empty<LocaleTerm>();
            else _terms = terms;
            
            for (int i = 0; i < _elements.Length; i++) {
                if (i < _terms.Length) _elements[i].SetTerm(_terms[i]);
            }
        }
    }
}