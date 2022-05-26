using System;
using Krugames.LocalizationSystem.Models;
using UnityEngine;
using UnityEngine.UIElements;

//TODO add array navigation
namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    public class LocaleList : TittledContentBox {

        public struct SelectionInfo {
            
            public static readonly SelectionInfo Nothing = new SelectionInfo(-1, null); 
            
            public readonly int Index;
            public readonly Locale Locale;

            public SelectionInfo(int index, Locale locale) {
                this.Index = index;
                this.Locale = locale;
            }
            
            public static bool operator ==(SelectionInfo left, SelectionInfo right) {
                if (left.Index == right.Index && left.Locale == right.Locale) return true;
                return false;
            }

            public static bool operator !=(SelectionInfo left, SelectionInfo right) {
                if (left.Index != right.Index || left.Locale != right.Locale) return true;
                return false;
            }
            
            public bool Equals(SelectionInfo other) {
                return Index == other.Index && Equals(Locale, other.Locale);
            }

            public override bool Equals(object obj) {
                return obj is SelectionInfo other && Equals(other);
            }

            public override int GetHashCode() {
                unchecked {
                    return (Index * 397) ^ (Locale != null ? Locale.GetHashCode() : 0);
                }
            }
        }

        private const string SelectedClassName = "Selected";
        
        private Locale[] _locales;
        
        private ScrollView _scrollView;
        private LocaleListElement[] _listElements;

        private SelectionInfo _selection = SelectionInfo.Nothing;
        
        public delegate void ElementDelegate(LocaleList self, LocaleListElement element);
        public delegate void SelectionDelegate(LocaleList self, SelectionInfo selectionInfo);
        public event ElementDelegate OnClick;
        public event ElementDelegate OnPropertiesClick;
        public event SelectionDelegate OnSelect;
        
        public Locale[] Locales => _locales;
        public LocaleListElement[] ListElements => _listElements;
        public SelectionInfo Selection => _selection;

        public LocaleList(string tittle, Locale[] locales) : base(tittle) {

            _scrollView = new ScrollView(ScrollViewMode.Vertical) {
                style = {
                    flexGrow = 1.0f
                }
            };
            
            Content.Add(_scrollView);

            _listElements = Array.Empty<LocaleListElement>();
            
            SetLocales(locales);
        }

        private FillRule GetFillRule(int index) {
            return (index % 2 == 0) ? FillRule.Even : FillRule.Odd;
        }
        
        private void Update() {
            LocaleListElement[] oldListElements = _listElements;
            int incrementAmount = _locales.Length - oldListElements.Length;

            _listElements = new LocaleListElement[oldListElements.Length + incrementAmount];
            int optimalLength = Mathf.Max(_listElements.Length, oldListElements.Length);
            for (int i = 0; i < optimalLength; i++) {
                if (i >= oldListElements.Length) {
                    _listElements[i] = new LocaleListElement(null, GetFillRule(i));
                    _listElements[i].OnClick += ElementClickEvent;
                    _listElements[i].OnPropertiesClick += ElementPropertiesClickEvent;
                    _scrollView.Add(_listElements[i]);
                } else if (i >= _listElements.Length) {
                    oldListElements[i].RemoveFromHierarchy();
                    oldListElements[i].OnClick -= ElementClickEvent;
                } else {
                    _listElements[i] = oldListElements[i];
                }
            }

            for (int i = 0; i < _listElements.Length; i++) {
                _listElements[i].SetLocale(_locales[i]);
                _listElements[i].RemoveCustomLabel();
            }
        }

        private void ElementClickEvent(LocaleListElement self) {
            OnClick?.Invoke(this, self);
            Select(GetIndexByElement(self));
        }

        private void ElementPropertiesClickEvent(LocaleListElement self) {
            OnPropertiesClick?.Invoke(this, self);
        }

        private void Select(int index) {
            if (index < 0 || index >= _listElements.Length) _selection = SelectionInfo.Nothing;
            else _selection = new SelectionInfo(index, _listElements[index].Locale);
            UpdateSelection();
            OnSelect?.Invoke(this, _selection);
        }

        private int GetIndexByElement(LocaleListElement element) {
            for (int i = 0; i < _listElements.Length; i++) {
                if (_listElements[i] == element) return i;
            }
            return -1;
        }

        private void UpdateSelection() {
            for (int i = 0; i < _listElements.Length; i++) _listElements[i].RemoveFromClassList(SelectedClassName);
            if (_selection != SelectionInfo.Nothing) _listElements[_selection.Index].AddToClassList(SelectedClassName);
        }
        
        public void SetLocales(Locale[] locales) {
            RemoveSelection();
            if (locales == null) _locales = Array.Empty<Locale>();
            else _locales = locales;
            Update();
        }
        
        public void UpdateValues() {
            for (int i = 0; i < _listElements.Length; i++) {
                _listElements[i].Update();
            }
        }

        public void RemoveSelection() {
            _selection = SelectionInfo.Nothing;
            UpdateSelection();
        }
    }
}