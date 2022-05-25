using System.Net;
using System.Runtime.CompilerServices;
using Codice.Client.BaseCommands.Import;
using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Models;
using Krugames.LocalizationSystem.RapidStorage.Servers;
using TMPro;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor.PlainView {
    public class PlainTermElement : Box {

        public const string ActiveClassName = "Active";
        public const string EvenClassName = "Even";
        public const string OddClassName = "Odd";
        public const string MainRootClassName = nameof(PlainTermElement) + "_MainRoot";
        public const string EditorRootClassName = nameof(PlainTermElement) + "_EditorRoot";
        public const string TermLabelClassName = nameof(PlainTermElement)+"_TermLabel";
        public const string ValueLabelClassName = nameof(PlainTermElement)+"_ValueLabel";
        public const string NoteLabelClassName = nameof(PlainTermElement) + "_NoteLabel";
        public const string PropsButtonClassName = nameof(PlainTermElement)+"_PropsButton";
        public const string TermPropertyClassName = nameof(PlainTermElement) + "_TermProperty";
        public const string ValuePropertyClassName = nameof(PlainTermElement) + "_ValueProperty";
        public const string NotePropertyClassName = nameof(PlainTermElement) + "_NoteProperty";
        
        private LocaleTerm _localeTerm;
        private FillRule _fillRule;
        private bool _hideTermPropertyField = false;

        private SerializedObject _serializedObject;
        private SerializedProperty _termProperty;
        private SerializedProperty _smartValueProperty;

        private readonly Box _mainRoot;
        private readonly Box _editorRoot;
        
        private readonly Label _termLabel;
        private readonly Label _valueLabel;
        private readonly Label _noteLabel;
        private readonly Button _propsButton;
        
        private readonly PropertyField _termPropertyField;
        private readonly PropertyField _smartValuePropertyField;
        private readonly TextField _notePropertyField;

        private readonly Clickable _clickable;

        private string _previousTermName;

        public delegate void ActiveStateChangeDelegate(PlainTermElement element, bool newState);
        public delegate void ClickDelegate(PlainTermElement element);
        public event ClickDelegate OnClick;
        public event ClickDelegate OnPropertiesClick;
        public event ActiveStateChangeDelegate OnActiveStateChanged;

        
        public LocaleTerm LocaleTerm => _localeTerm;

        public Label TermLabel => _termLabel;

        public Label ValueLabel => _valueLabel;

        public bool Active {
            get => GetEditorOpenState();
            set => SetEditorOpenState(value);
        }

        public FillRule FillRule {
            get => _fillRule;
            set {
                RemoveFillRuleClass();
                _fillRule = value;
                AddFillRuleClass();
            }
        }

        public bool HideTermPropertyField {
            get => _hideTermPropertyField;
            set {
                _hideTermPropertyField = value;
                RebuildEditor();
            }
        }


        public PlainTermElement(LocaleTerm localeTerm, FillRule fillRule) {
            _fillRule = fillRule;
            AddFillRuleClass();

            _mainRoot = new Box();
            _editorRoot = new Box();
            
            _termLabel = new Label() { pickingMode = PickingMode.Position};
            _valueLabel = new Label() {pickingMode = PickingMode.Position,};
            _noteLabel = new Label() {pickingMode = PickingMode.Position,};
            _propsButton = new Button(Event_PropertiesButtonClick);

            _termPropertyField = new PropertyField();
            _smartValuePropertyField = new PropertyField();
            _notePropertyField = new TextField("Note");
            
            _mainRoot.AddToClassList(MainRootClassName);
            _editorRoot.AddToClassList(EditorRootClassName);
            _termLabel.AddToClassList(TermLabelClassName);
            _valueLabel.AddToClassList(ValueLabelClassName);
            _noteLabel.AddToClassList(NoteLabelClassName);
            _propsButton.AddToClassList(PropsButtonClassName);
            _termPropertyField.AddToClassList(TermPropertyClassName);
            _smartValuePropertyField.AddToClassList(ValuePropertyClassName);
            _notePropertyField.AddToClassList(NotePropertyClassName);
            
            _mainRoot.Add(_termLabel);
            _mainRoot.Add(_valueLabel);
            _mainRoot.Add(_noteLabel);
            _mainRoot.Add(_propsButton);

            Add(_mainRoot);

            _clickable = new Clickable(Event_ElementClick);
            _mainRoot.AddManipulator(_clickable);

            _mainRoot.RegisterCallback<MouseUpEvent>(Event_ElementMouseUp);
            _noteLabel.RegisterCallback<ChangeEvent<string>>(Event_NoteLabelChange);
            _notePropertyField.RegisterCallback<ChangeEvent<string>>(Event_NoteFieldChange);
            
            ObjectChangeEvents.changesPublished += Event_OnObjectChangesPublished;
            
            SetTerm(localeTerm);
        }

        private void AddFillRuleClass() {
            if (_fillRule == FillRule.Even) AddToClassList(EvenClassName);
            else if (_fillRule == FillRule.Odd) AddToClassList(OddClassName);
        }

        private void RemoveFillRuleClass() {
            if (_fillRule == FillRule.Even) RemoveFromClassList(EvenClassName);
            else if (_fillRule == FillRule.Odd) RemoveFromClassList(OddClassName);
        }

        private void SetEditorOpenState(bool state) {
            _mainRoot.RemoveFromClassList(ActiveClassName);
            if (state) {
                Add(_editorRoot);
                _editorRoot.PlaceInFront(_mainRoot);
                _mainRoot.AddToClassList(ActiveClassName);
            } else {
                _editorRoot.RemoveFromHierarchy();
            }
            OnActiveStateChanged?.Invoke(this, state);
        }

        private bool GetEditorOpenState() {
            return Contains(_editorRoot);
        }
        
        private void SwitchEditorOpenState() {
            SetEditorOpenState(!GetEditorOpenState());
        }

        private void RebuildEditor() {
            _editorRoot.Clear();
            if (!_hideTermPropertyField && _termProperty != null) _editorRoot.Add(_termPropertyField);
            if (_smartValueProperty != null) _editorRoot.Add(_smartValuePropertyField);
            _editorRoot.Add(_notePropertyField);
        }

        private void Event_ElementClick() {
            SwitchEditorOpenState();
            OnClick?.Invoke(this);
        }

        private void Event_PropertiesButtonClick() {
            OnPropertiesClick?.Invoke(this);
        }
        
        private void Event_ElementMouseUp(MouseUpEvent evt) {
            if (evt.button == 1) OnPropertiesClick?.Invoke(this);
        }
        
        private void Event_NoteLabelChange(ChangeEvent<string> evt) {
            _noteLabel.tooltip = evt.newValue;
        }
        
        private void Event_NoteFieldChange(ChangeEvent<string> evt) {
            if (_localeTerm == null) return;
            TermNoteServer.SetNote(_localeTerm.Term, evt.newValue);
            _noteLabel.text = evt.newValue;
        }
        
        private void Event_OnObjectChangesPublished(ref ObjectChangeEventStream stream) {
            if (GetEditorOpenState()) {
                Update();
            }

            if (_localeTerm != null && _previousTermName != _localeTerm.Term) {
                _previousTermName = _localeTerm.Term;
                _notePropertyField.SetValueWithoutNotify(TermNoteServer.GetNote(_localeTerm.Term));
                _noteLabel.text = _notePropertyField.value;
                Update();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetTerm(LocaleTerm localeTerm) {
            
            if (_localeTerm != null) TermNoteServer.SetNote(_localeTerm.Term, _notePropertyField.text);
            _termPropertyField.Unbind();
            _smartValuePropertyField.Unbind();

            _localeTerm = localeTerm;
            
            if (_localeTerm == null) {
                _serializedObject = null;
                _termProperty = null;
                _smartValueProperty = null;
            } else {
                _serializedObject = new SerializedObject(localeTerm);

                _termProperty = _serializedObject.FindProperty("term");
                _smartValueProperty = _serializedObject.FindProperty("smartValue");

                if (_termProperty != null) _termPropertyField.BindProperty(_termProperty);
                if (_smartValueProperty != null) _smartValuePropertyField.BindProperty(_smartValueProperty);

                _notePropertyField.SetValueWithoutNotify(TermNoteServer.GetNote(_localeTerm.Term));
                _noteLabel.text = _notePropertyField.value;
                _noteLabel.tooltip = _noteLabel.text;
                
                _previousTermName = _localeTerm.Term;
            }
            RebuildEditor();
            SetEditorOpenState(false);
            Update();
        }

        public void Update() {
            if (_localeTerm == null) {
                visible = false;
                return;
            }
            visible = true;
            _termLabel.text = _localeTerm.Term;
            _valueLabel.text = _localeTerm.Value.ToString();
        }

    }
}