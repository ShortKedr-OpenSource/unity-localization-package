using System.Collections.Generic;
using Krugames.LocalizationSystem.Models;
using Krugames.LocalizationSystem.Models.Terms;
using RenwordDigital.StringSearchEngine;
using ThirdParty.Krugames.LocalizationSystem.Editor.UI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

//TODO allow remove terms
namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor.PlainView {
    public class PlainLocaleEditor : VisualElement {

        private enum ViewMode {
            Default = 0,
            Search = 1
        }
        
        private Locale _locale;

        private TittleSearchToolbar _toolbar;
        private PlaintTermElementTableHeader _tableHeader;
        private VisualElement _listContainer;
        private PagerTooltipToolbar _toolbarPager;
        
        private PlainTermElementList _defaultList;
        private PlainTermElementList _searchList;
        
        private SearchIndex _termSearchIndex;
        private Dictionary<Resource, LocaleTerm> _resourceTermDict;
        private string _lastSearchString = string.Empty;
        private int _lastDefaultPage;
        
        private ViewMode _viewMode;
        private LocaleTermListViewContent.SelectionInfo _selection = LocaleTermListViewContent.SelectionInfo.Nothing;
        private int _itemsPerPage;
        
        private OptionPopup _managedElementOptions; 
        private PlainTermElement _managedElement = null;

        public PlainLocaleEditor(Locale locale) {
            _locale = locale;

            style.flexGrow = 1f;
            style.borderTopWidth = 0;
            style.borderBottomWidth = 0;
            style.borderLeftWidth = 0;
            style.borderRightWidth = 0;

            _toolbar = new TittleSearchToolbar("Term Editor (Plain)");

            var tittle = new Label("English");
            var search = new ToolbarSearchField();

            _tableHeader = new PlaintTermElementTableHeader();

            _defaultList = new PlainTermElementList();
            _searchList = new PlainTermElementList();

            _toolbarPager = new PagerTooltipToolbar() {
                style = {
                    borderBottomWidth = 0f,
                    borderTopWidth = 1f,
                }
            };
            
            Add(_toolbar);
            Add(_tableHeader);
            Add(_defaultList);
            Add(_toolbarPager);


            StringTerm term = ScriptableObject.CreateInstance<StringTerm>();
            term.SetSmartValue("value");

            SerializedObject obj = new SerializedObject(term);
            obj.FindProperty("term").stringValue = "string_term";
            obj.ApplyModifiedProperties();

            List<LocaleTerm> terms = new List<LocaleTerm>();
            for (int i = 0; i < 10; i++) {
                terms.Add(term);
            }
            _defaultList.SetTerms(terms.ToArray());

        }

        //TODO private methods
        
        public void SetLocale(Locale locale) {
            
        }
    }
}