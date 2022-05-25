using System.Net;
using System.Runtime.CompilerServices;
using Codice.Client.BaseCommands.Import;
using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Models;
using Krugames.LocalizationSystem.RapidStorage.Servers;
using TMPro;
using UnityEditor;
using UnityEditor.UIElements;
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
        public const string InspectorClassName = nameof(PlainTermElement) + "_Editor";
        
        private LocaleTerm _localeTerm;
        private FillRule _fillRule;
        private UnityEditor.Editor _termEditor;
        private bool _allowEditorDrawing = false;

        private readonly Box _mainRoot;
        private readonly Box _editorRoot;
        
        private readonly Label _termLabel;
        private readonly Label _valueLabel;
        private readonly Label _noteLabel;
        private readonly Button _propsButton;
        
        private readonly IMGUIContainer _editorElement;
        private readonly TextField _noteField;

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

        
        public PlainTermElement(LocaleTerm localeTerm, FillRule fillRule) {
            _fillRule = fillRule;
            AddFillRuleClass();

            _mainRoot = new Box();
            _editorRoot = new Box();
            
            _termLabel = new Label() {
                pickingMode = PickingMode.Position,
            };

            _valueLabel = new Label() {
                pickingMode = PickingMode.Position,
            };
            
            _noteLabel = new Label() {
                pickingMode = PickingMode.Position,
            };

            _propsButton = new Button(Event_PropertiesButtonClick);

            _editorElement = new IMGUIContainer(OnEditorGUI);
            _noteField = new TextField("Note");
            
            _mainRoot.AddToClassList(MainRootClassName);
            _editorRoot.AddToClassList(EditorRootClassName);
            _termLabel.AddToClassList(TermLabelClassName);
            _valueLabel.AddToClassList(ValueLabelClassName);
            _noteLabel.AddToClassList(NoteLabelClassName);
            _propsButton.AddToClassList(PropsButtonClassName);
            _editorElement.AddToClassList(InspectorClassName);
            
            _mainRoot.Add(_termLabel);
            _mainRoot.Add(_valueLabel);
            _mainRoot.Add(_noteLabel);
            _mainRoot.Add(_propsButton);

            _editorRoot.Add(_editorElement);
            _editorRoot.Add(_noteField);

            Add(_mainRoot);

            _clickable = new Clickable(Event_ElementClick);
            _mainRoot.AddManipulator(_clickable);

            _mainRoot.RegisterCallback<MouseUpEvent>(Event_ElementMouseUp);
            _noteLabel.RegisterCallback<ChangeEvent<string>>(Event_NoteLabelChange);
            _noteField.RegisterCallback<ChangeEvent<string>>(Event_NoteFieldChange);
            
            SetTerm(localeTerm);
            
            ObjectChangeEvents.changesPublished += Event_OnObjectChangesPublished;
        }

        private void OnEditorGUI() {
            if (!_allowEditorDrawing) return;
            _termEditor.OnInspectorGUI();
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

        private void Event_ElementClick() {
            SwitchEditorOpenState(); //TODO replace with mode
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
                _noteField.SetValueWithoutNotify(TermNoteServer.GetNote(_localeTerm.Term));
                _noteLabel.text = _noteField.value;
                Update();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetTerm(LocaleTerm localeTerm) {
            if (_localeTerm != null) TermNoteServer.SetNote(_localeTerm.Term, _noteField.text);
            _localeTerm = localeTerm;
            if (_localeTerm == null) _termEditor = null;
            else {
                _termEditor = UnityEditor.Editor.CreateEditor(localeTerm);
                _noteField.SetValueWithoutNotify(TermNoteServer.GetNote(_localeTerm.Term));
                _noteLabel.text = _noteField.value;
                _previousTermName = _localeTerm.Term;
            }
            _allowEditorDrawing = _termEditor != null;
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