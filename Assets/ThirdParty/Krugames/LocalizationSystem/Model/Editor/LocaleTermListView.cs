using Krugames.LocalizationSystem.Editor.UIElements;
using Krugames.LocalizationSystem.Models;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ThirdParty.Krugames.LocalizationSystem.Model.Editor {
    public class LocaleTermListView : Box {

        private LocaleTerm[] _terms;

        public LocaleTermListView() {
            style.flexGrow = 1f;
            style.width = new StyleLength(StyleKeyword.Auto);
            style.height = new StyleLength(StyleKeyword.Auto);
            style.minHeight = 300f;
            
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
            var toolbar = new Toolbar() {
                style = {
                    borderTopLeftRadius = 5f,
                    borderTopRightRadius = 5f,
                    alignContent = new StyleEnum<Align>(Align.Center),
                    justifyContent = new StyleEnum<Justify>(Justify.Center),
                }
            };
            toolbar.Add(new Label("Terms"));
            toolbar.Add(new ToolbarSearchField() {
                style= {
                    maxWidth = 100f,
                }
            });
            Add(toolbar);
        }

        public void SetTerms(LocaleTerm[] terms) {
            _terms = terms;
            for (int i = 0; i < _terms.Length; i++) {
                Add(new Label(_terms[i].Term + " | " + terms[i].Value.ToString()));
            }
        }
        
        public class ListElement : Box{
            private LocaleTerm _target;

            private Label _termName;
            private Label _termType;
            private PropertyField _termValue;

            public ListElement(LocaleTerm target) {
                _target = target;
            }
        }
        
        //TODO add pages
    }
}