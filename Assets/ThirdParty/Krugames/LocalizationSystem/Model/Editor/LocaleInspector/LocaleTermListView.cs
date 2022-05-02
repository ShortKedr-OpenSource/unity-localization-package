using Krugames.LocalizationSystem.Models;
using ThirdParty.Krugames.LocalizationSystem.Model.Editor.UIElements;
using UnityEditor;
using UnityEditor.Lumin.Packaging.Manifest;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

//TODO move to new LocaleTermListView folder
//TODO separate elements to different files
//TODO move LocaleTermTableElement to new folder for this class
namespace ThirdParty.Krugames.LocalizationSystem.Model.Editor {
    public class LocaleTermListView : Box {
        
        private LocaleTerm[] _terms;

        private VisualElement _content;
        private Toolbar _toolbar; //TODO description: header border round by default,
        private Toolbar _tableHeader; //TODO description: normal border round by default,
        private VisualElement _termView;
        private Toolbar _pagerToolbar;  //TODO description: footer border round by default,

        private int _maxCount = 12;
        
        public LocaleTermListView() {
            style.flexGrow = 0f;
            style.width = new StyleLength(StyleKeyword.Auto);
            /*style.height = 350f;
            style.minHeight = 350f;
            style.maxHeight = 350f;*/

            style.marginTop = 4f;
            style.marginBottom = 4f;
            style.marginRight = 4f;
            style.backgroundColor = new Color(0f, 0f, 0f, 0f);

            Add(_content = new VisualElement() {
                style = {
                    flexGrow = 1f,
                    
                    borderTopWidth = 1f,
                    borderBottomWidth = 1f,
                    borderLeftWidth = 1f,
                    borderRightWidth = 1f,
                    
                    borderTopColor = new Color(0.0f,0.0f,0.0f, 0.25f),
                    borderBottomColor = new Color(0.0f,0.0f,0.0f, 0.25f),
                    borderLeftColor = new Color(0.0f,0.0f,0.0f, 0.25f),
                    borderRightColor = new Color(0.0f,0.0f,0.0f, 0.25f),
                    
                    borderTopLeftRadius = 5f,
                    borderTopRightRadius = 5f,
                    borderBottomLeftRadius = 5f,
                    borderBottomRightRadius = 5f,
                    overflow = new StyleEnum<Overflow>(Overflow.Hidden),
                }
            });
            
            _toolbar = new Toolbar() {
                style = {
                    flexGrow = 0f,
                    justifyContent = new StyleEnum<Justify>(Justify.Center),
                    minHeight = 24f,
                    maxHeight = 24f,
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
            
            _content.Add(_toolbar);


            _tableHeader = new Toolbar() {
                style = {
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter),
                    unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold),
                    maxHeight = 24f,
                    minHeight = 24f,
                }
            };
            _tableHeader.Add(new Label("term") {
                style = {
                    minWidth = EditorGUIUtility.labelWidth,
                    maxWidth = EditorGUIUtility.labelWidth,
                    borderRightWidth = 1f,
                    borderRightColor = new Color(0.0f, 0.0f, 0.0f, 0.3f)
                }
            });
            _tableHeader.Add(new Label("value") {
                style = {
                    flexGrow = 1f,
                }
            });
            _content.Add(_tableHeader);
            
            _content.Add(_termView = new VisualElement() {
                style = {
                    flexGrow = 1,
                    width = new StyleLength(StyleKeyword.Auto),
                    marginBottom = 0f,
                    paddingBottom = 0f,
                }
            });
            
            _pagerToolbar = _toolbar = new Toolbar() {
                style = {
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row),
                    justifyContent = new StyleEnum<Justify>(Justify.FlexEnd),
                    minHeight = 24f,
                    maxHeight = 24f,
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter),
                    borderBottomWidth = 0f,
                    borderTopWidth = 1f,
                    overflow = new StyleEnum<Overflow>(Overflow.Hidden),
                }
            };
            
            _pagerToolbar.Add(new ToolbarButton() {
                text = "<",
                style = {
                    minWidth = 20,
                    maxWidth = 20,
                    paddingRight = 5f,
                    backgroundImage = new StyleBackground(AssetDatabase.LoadAssetAtPath<Texture2D>("Builtin Skins/DarkSkin/Images/ArrowNavigationLeft.png"))
                }
            });
            _pagerToolbar.Add(new IntegerField(){style={width = new StyleLength(StyleKeyword.Auto), minWidth = 56}});
            _pagerToolbar.Add(new Label(" / 99999 "));
            _pagerToolbar.Add(new ToolbarButton() {
                text = ">",
                style = {
                    minWidth = 20,
                    maxWidth = 20,
                    paddingLeft = 5f,
                    backgroundImage = new StyleBackground(AssetDatabase.LoadAssetAtPath<Texture2D>("Builtin Skins/DarkSkin/Images/ArrowNavigationRight.png"))
                }
            });
            _content.Add(_pagerToolbar);
        }

        public void SetTerms(LocaleTerm[] terms) {
            _terms = terms;
            _termView.Clear();
            int count = 0;
            for (int i = 0; i < _terms.Length*5; i++) {
                _termView.Add(new LocaleTermTableElement(terms[i%_terms.Length], 
                    (i%2 == 0) ? LocaleTermTableElement.FillRule.Even : LocaleTermTableElement.FillRule.Odd));
                count++;
                if (count >= _maxCount) break;
            }
        }

        //TODO add pages
    }
}