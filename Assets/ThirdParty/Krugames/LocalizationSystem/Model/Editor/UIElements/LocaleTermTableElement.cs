using System.Runtime.CompilerServices;
using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace ThirdParty.Krugames.LocalizationSystem.Model.Editor.UIElements {
    public class LocaleTermTableElement : Box {
        
        private LocaleTerm _localeTerm;

        private Label _termLabel;
        private Label _valueLabel;

        private Color oddBaseBackgroundColor = new Color(0f, 0f, 0f, 0f);
        private Color evenBaseBackgroundColor = new Color(0f, 0f, 0f, 0.05f);

        public enum FillRule {
            Odd,
            Even
        };
        private FillRule _fillRule;
        
        public LocaleTermTableElement(LocaleTerm localeTerm, FillRule fillRule) {

            _fillRule = fillRule;
            
            styleSheets.Add(LocalizationEditorStyles.GlobalStyle);

            style.borderTopWidth = 0;
            style.borderBottomWidth = 0;
            style.borderRightWidth = 0;
            style.borderLeftWidth = 0;
            style.backgroundColor = GetBaseBackground();

            style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleLeft);
            
            _localeTerm = localeTerm; 
            
            Add(_termLabel = new Label(_localeTerm.Term) {
                style = {
                    flexGrow = 0f,
                    borderRightWidth = 1f,
                    //borderTopWidth = 1f,
                    borderRightColor = new Color(0.0f, 0.0f, 0.0f, 0.3f),
                    borderTopColor = new Color(0.0f, 0.0f, 0.0f, 0.3f),
                    maxWidth = EditorGUIUtility.labelWidth,
                    minWidth = EditorGUIUtility.labelWidth,
                    paddingRight = 0,
                    maxHeight = 25,
                    minHeight = 25,
                    marginTop = 0,
                    marginBottom = 0,
                    paddingLeft = 5,
                    overflow = new StyleEnum<Overflow>(Overflow.Hidden),
                }
            });

            Add(_valueLabel = new Label(_localeTerm.Value.ToString()) {
                style = {
                    paddingLeft = 5,
                    flexGrow = 1f,
                    minHeight = 25,
                    maxHeight = 25,
                    overflow = new StyleEnum<Overflow>(),
                    //borderRightWidth = 1f,
                    //borderTopWidth = 1f,
                    borderRightColor = new Color(0.0f, 0.0f, 0.0f, 0.3f),
                    borderTopColor = new Color(0.0f, 0.0f, 0.0f, 0.3f),
                    marginTop = 0,
                    marginBottom = 0,
                }
            });

            RegisterCallback<MouseEnterEvent>(evt => style.backgroundColor = new Color(0.1f, 0.45f, 1.0f, 0.3f));
            RegisterCallback<MouseLeaveEvent>(evt => style.backgroundColor = GetBaseBackground());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Color GetBaseBackground() {
            if (_fillRule == FillRule.Odd) return oddBaseBackgroundColor;
            return evenBaseBackgroundColor;
        }
        
        
    }
}