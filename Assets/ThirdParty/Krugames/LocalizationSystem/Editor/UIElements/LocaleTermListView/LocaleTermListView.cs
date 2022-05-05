using Krugames.LocalizationSystem.Models;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

//TODO move to new LocaleTermListView folder
//TODO separate elements to different files
//TODO move LocaleTermTableElement to new folder for this class
namespace Krugames.LocalizationSystem.Editor.UIElements {
    public class LocaleTermListView : Box {
        
        private LocaleTerm[] _terms;

        private VisualElement _content;
        private TittleSearchToolbar _toolbar;
        private LocaleTermListViewTableHeader _tableHeader;
        private VisualElement _termView;
        private Toolbar _pagerToolbar;

        private int _maxCount = 12;
        
        //TODO element getters,

        public LocaleTermListView() {
            style.flexGrow = 0f;
            style.width = new StyleLength(StyleKeyword.Auto);

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

            _toolbar = new TittleSearchToolbar("Terms");

            _content.Add(_toolbar);


            _tableHeader = new LocaleTermListViewTableHeader();
 
            
            _content.Add(_tableHeader);
            _content.Add(_termView = new VisualElement() {
                style = {
                    flexGrow = 1,
                    width = new StyleLength(StyleKeyword.Auto),
                    marginBottom = 0f,
                    paddingBottom = 0f,
                }
            });
            
            _pagerToolbar = new Toolbar() {
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
            for (int i = 0; i < _terms.Length*2; i++) {
                _termView.Add(new LocaleTermListViewElement(terms[i%_terms.Length], 
                    (i%2 == 0) ? FillRule.Even : FillRule.Odd));
                count++;
                if (count >= _maxCount) break;
            }
        }

        //TODO add pages
    }
}