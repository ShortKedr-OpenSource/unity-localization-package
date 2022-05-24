using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Models;
using UnityEditor;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    public class LocaleListElement : Box {

        private const string EvenClassName = "Even";
        private const string OddClassName = "Odd";
        private const string LocaleLabelClassName = nameof(LocaleListElement)+"_LocaleLabel";
        private const string LanguageLabelClassName = nameof(LocaleListElement)+"_LangLabel";
        private const string PropsButtonClassName = nameof(LocaleListElement)+"_PropsButton";
        
        private Locale _locale;
        private FillRule _fillRule;
        private bool _allowProperties;
        
        private Label _localeLabel;
        private Label _languageLabel;
        private Button _propertiesButton;

        private Clickable _clickable;

        private string customLabel = string.Empty;


        public delegate void ClickDelegate(LocaleListElement self);
        public event ClickDelegate OnClick;
        public event ClickDelegate OnPropertiesClick;


        public Locale Locale => _locale;

        public Label LocaleLabel => _localeLabel;

        public Label LanguageLabel => _languageLabel;

        public Button PropertiesButton => _propertiesButton;


        public FillRule FillRule {
            get => _fillRule;
            set {
                RemoveFillRuleClass();
                _fillRule = value;
                AddFillRuleClass();
            }
        }

        public bool AllowProperties {
            get => _allowProperties;
            set {
                _allowProperties = value;
                _propertiesButton.RemoveFromHierarchy();
                if (_allowProperties) {
                    Add(_propertiesButton);
                    _propertiesButton.PlaceInFront(_languageLabel);
                }
            }
        }

        public LocaleListElement(Locale locale, FillRule fillRule, bool allowProperties = true) {
            _locale = locale;
            _fillRule = fillRule;
            _allowProperties = allowProperties;
            
            styleSheets.Add(LocalizationEditorStyles.LocalizationEditorStyle);
            AddFillRuleClass();

            _localeLabel = new Label() {
                pickingMode = PickingMode.Ignore,
                style = {
                    minWidth = EditorGUIUtility.labelWidth,
                }
            };

            _languageLabel = new Label() {
                pickingMode = PickingMode.Ignore,
            };

            _propertiesButton = new Button(PropertiesButtonClickEvent);

            _localeLabel.AddToClassList(LocaleLabelClassName);
            _languageLabel.AddToClassList(LanguageLabelClassName);
            _propertiesButton.AddToClassList(PropsButtonClassName);
            
            _clickable = new Clickable(ElementClickEvent);
            this.AddManipulator(_clickable);
            
            Add(_localeLabel);
            Add(_languageLabel);
            if (_allowProperties) Add(_propertiesButton);
            
            RegisterCallback<MouseUpEvent>(ElementMouseUpEvent);
            
            SetLocale(locale);
        }

        private void AddFillRuleClass() {
            if (_fillRule == FillRule.Even) AddToClassList(EvenClassName);
            else if (_fillRule == FillRule.Odd) AddToClassList(OddClassName);
        }

        private void RemoveFillRuleClass() {
            if (_fillRule == FillRule.Even) RemoveFromClassList(EvenClassName);
            else if (_fillRule == FillRule.Odd) RemoveFromClassList(OddClassName);
        }
        
        private void ElementClickEvent() {
            OnClick?.Invoke(this);
        }
        
        private void PropertiesButtonClickEvent() {
            OnPropertiesClick?.Invoke(this);
        }
        
        private void ElementMouseUpEvent(MouseUpEvent evt) {
            if (evt.button == 1) OnPropertiesClick?.Invoke(this);
        }

        public void SetLocale(Locale locale) {
            _locale = locale;
            Update();
        }
        
        public void Update() {
            if (_locale == null) {
                visible = false;
                return;
            }
            visible = true;
            if (!string.IsNullOrEmpty(customLabel)) _localeLabel.text = customLabel;
            else _localeLabel.text = _locale.name;
            _languageLabel.text = _locale.Language.ToString();
        }

        public void SetCustomLabel(string text) {
            customLabel = text;
            Update();
        }

        public void RemoveCustomLabel() {
            SetCustomLabel(string.Empty);
        }
    }
}