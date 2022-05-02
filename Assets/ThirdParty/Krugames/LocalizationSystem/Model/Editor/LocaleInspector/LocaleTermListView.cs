using Krugames.LocalizationSystem.Editor.UIElements;
using Krugames.LocalizationSystem.Models;
using ThirdParty.Krugames.LocalizationSystem.Model.Editor.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ThirdParty.Krugames.LocalizationSystem.Model.Editor {
    public class LocaleTermListView : Box {

        private LocaleTerm[] _terms;

        private Toolbar _toolbar;
        private Toolbar _toolbarHeader;
        private ScrollView _termView;

        public LocaleTermListView() {
            style.flexGrow = 0f;
            style.width = new StyleLength(StyleKeyword.Auto);
            style.height = new StyleLength(StyleKeyword.Auto);
            style.minHeight = 350f;
            style.maxHeight = 350f;
            
            style.borderTopLeftRadius = 5f;
            style.borderTopRightRadius = 5f;
            style.borderBottomLeftRadius = 5f;
            style.borderBottomRightRadius = 5f;

            style.marginTop = 4f;
            style.marginBottom = 4f;
            style.marginRight = 4f;
            
            style.borderTopColor = new Color(0.0f,0.0f,0.0f, 0.25f);
            style.borderBottomColor = new Color(0.0f,0.0f,0.0f, 0.25f);
            style.borderLeftColor = new Color(0.0f,0.0f,0.0f, 0.25f);
            style.borderRightColor = new Color(0.0f,0.0f,0.0f, 0.25f);
                
            style.borderTopWidth = 1f;
            style.borderBottomWidth = 1f;
            style.borderLeftWidth = 1f;
            style.borderRightWidth = 1f;
            
            _toolbar = new Toolbar() {
                style = {
                    borderTopLeftRadius = 5f,
                    borderTopRightRadius = 5f,
                    justifyContent = new StyleEnum<Justify>(Justify.Center),
                }
            };

            var termsLabel = new Label("Terms") {
                style = {
                    flexGrow = 1f,
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter),
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row),
                    justifyContent = new StyleEnum<Justify>(Justify.FlexEnd),
                    unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold),
                }
            };

            var search = new ToolbarSearchField() {
                style = {
                    maxWidth = 100f,
                    width = 100f,
                    minWidth = 10f,
                }
            };

            termsLabel.Add(search);
            _toolbar.Add(termsLabel);
            
            Add(_toolbar);


            _toolbarHeader = new Toolbar() {
                style = {
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter),
                    unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold),
                    maxHeight = 22,
                    minHeight = 22,
                }
            };
            _toolbarHeader.Add(new Label("term") {
                style = {
                    minWidth = EditorGUIUtility.labelWidth,
                    maxWidth = EditorGUIUtility.labelWidth,
                    borderRightWidth = 1f,
                    borderRightColor = new Color(0.0f, 0.0f, 0.0f, 0.3f)
                }
            });
            _toolbarHeader.Add(new Label("value") {
                style = {
                    flexGrow = 1f,
                }
            });
            Add(_toolbarHeader);
            
            Add(_termView = new ScrollView(ScrollViewMode.Vertical) {
                style = {
                    flexGrow = 1,
                    width = new StyleLength(StyleKeyword.Auto),
                    marginBottom = -8f,
                    paddingBottom = 0f,
                }
            });
        }

        public void SetTerms(LocaleTerm[] terms) {
            _terms = terms;
            _termView.Clear();
            for (int i = 0; i < _terms.Length*5; i++) {
                _termView.Add(new LocaleTermTableElement(terms[i%_terms.Length], 
                    (i%2 == 0) ? LocaleTermTableElement.FillRule.Even : LocaleTermTableElement.FillRule.Odd));
            }
        }

        //TODO add pages
    }
}