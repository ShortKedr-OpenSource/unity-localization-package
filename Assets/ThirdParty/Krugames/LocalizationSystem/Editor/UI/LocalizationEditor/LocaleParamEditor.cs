﻿using Krugames.LocalizationSystem.Models;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    public class LocaleParamEditor : TittledContentBox {

        private Locale _locale;
        private SerializedObject _localeSerializedObject;
        private SerializedProperty _languageProperty;

        private ScrollView _scrollView;
        private Label _localeLabel;
        private PropertyField _languageField;

        private string _customLabel = string.Empty;

        public delegate void ValueChangeDelegate(LocaleParamEditor self);
        public event ValueChangeDelegate OnChange;

        public LocaleParamEditor(string tittle, Locale locale) : base(tittle) {
            _scrollView = new ScrollView(ScrollViewMode.Vertical);
            _localeLabel = new Label();
            _languageField = new PropertyField();
            
            _languageField.RegisterValueChangeCallback(SerializedPropertyChanged);
            
            _scrollView.Add(_localeLabel);
            _scrollView.Add(_languageField);

            Content.Add(_scrollView);
            SetLocale(locale);
        }

        private void SerializedPropertyChanged(SerializedPropertyChangeEvent evt) {
            OnChange?.Invoke(this);
        }

        public void SetLocale(Locale locale) {
            _locale = locale;
            _languageField.Unbind();
            if (locale != null) {
                _localeSerializedObject = new SerializedObject(_locale);
                _languageProperty = _localeSerializedObject.FindProperty("language");
                _localeLabel.text = (string.IsNullOrEmpty(_customLabel)) ? locale.name : _customLabel;
                _languageField.BindProperty(_languageProperty);
                _scrollView.visible = true;
            } else {
                _scrollView.visible = false;
            }
        }

        public void SetCustomLabel(string text) {
            _customLabel = text;
        }

        public void RemoveCustomLabel() {
            SetCustomLabel(string.Empty);
        }
    }
}