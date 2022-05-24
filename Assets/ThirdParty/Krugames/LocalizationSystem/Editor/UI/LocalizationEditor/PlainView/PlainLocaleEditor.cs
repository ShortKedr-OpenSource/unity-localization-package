using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Models;
using Krugames.LocalizationSystem.Models.Terms;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor.PlainView {
    public class PlainLocaleEditor : VisualElement {

        private Locale _locale;

        private TittleSearchToolbar _toolbar;
        private TermElementTableHeader _tableHeader;
        private ScrollView _elementList;
        private PagerTooltipToolbar _toolbarPager;

        public PlainLocaleEditor(Locale locale) {
            _locale = locale;
            
            //TODO review, style imports many times, cuz of hierarchy
            //styleSheets.Add(LocalizationEditorStyles.LocalizationEditorStyle);

            style.flexGrow = 1f;
            style.borderTopWidth = 0;
            style.borderBottomWidth = 0;
            style.borderLeftWidth = 0;
            style.borderRightWidth = 0;

            _toolbar = new TittleSearchToolbar("Term Editor (Plain)");

            var tittle = new Label("English");
            var search = new ToolbarSearchField();

            _tableHeader = new TermElementTableHeader();

            _elementList = new ScrollView(ScrollViewMode.Vertical) {
                showVertical = true, //TODO remove
                style = {
                    flexGrow = 1f,
                }
            };

            _toolbarPager = new PagerTooltipToolbar() {
                style = {
                    borderBottomWidth = 0f,
                    borderTopWidth = 1f,
                }
            };
            
            Add(_toolbar);
            Add(_tableHeader);
            Add(_elementList);
            Add(_toolbarPager);


            StringTerm term = ScriptableObject.CreateInstance<StringTerm>();
            term.SetSmartValue("oqwjecjwqoiecjioqw");

            SerializedObject obj = new SerializedObject(term);
            obj.FindProperty("term").stringValue = "string_term";
            obj.ApplyModifiedProperties();

            for (int i = 0; i < 9; i++) {
                _elementList.Add(new TermElement(term, (i % 2 == 0) ? FillRule.Even : FillRule.Odd));
            }

        }

        //TODO private methods
        
        public void SetLocale(Locale locale) {
            
        }
    }
}