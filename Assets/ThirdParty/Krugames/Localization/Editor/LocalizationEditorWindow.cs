using System;
using System.Collections.Generic;
using System.IO;
using Game;
using UnityEditor;
using UnityEngine;

namespace Krugames.LocalizationSystem.Editor {
    public class LocalizationEditorWindow : EditorWindow {

        private Localization library;
        private int currenLocaleIndex = -1;
        private List<Localization.LocaleLoadData> workingLoadData;

        private TextAsset loadedTextAsset;
        private List<Locale.LocalizationUnit> loadedLocaleUnits;

        //values
        private static float minWidth = 800f;
        private static float minHeight = 500f;

        private static float leftPanelWidth = 300f + 16f;
        private static float maxTermWidth = 300f;

        private static Color shadowColor = new Color(0f, 0f, 0f, 0.4f);
        private static Color leftPanelBackgroundColor = new Color(0.7f, 0.7f, 0.7f);

        private Vector2 leftPanelScroll = Vector2.zero;
        private Vector2 rightPanelScroll = Vector2.zero;
        private static int unitsPerPage = 50;
        private int currentPageIndex = 0;

        private string topPanelMessage = "";
        private bool updateTimerWorking = false;
        private int updateTimer = 0;
        private static int updateTime = 60;

        //textures
        private Texture2D selectedButtonTexture;

        //styles
        private GUIStyle deleteMarkButtonStyle;
        private GUIStyle leftPanelBackgroundStyle;
        private GUIStyle leftPanelHeaderStyle;
        private GUIStyle leftPanelHeaderTextStyle;
        private GUIStyle leftPanelElementStyle;
        private GUIStyle borderStyle;
        private GUIStyle innerBorderStyle;
        private GUIStyle selectedButtonStyle;
        private GUIStyle boxStyle;
        private GUIStyle topPanelMessageStyle;
        private GUIStyle rightAnchorMessageStyle;

        private static class PrefsKeys {
            public static string firstExecute = "Krugames/Localization_Editor/firstExecute";
        }

        [MenuItem("Tools/Localization")]
        private static void Init() {
            LocalizationEditorWindow window =
                (LocalizationEditorWindow) EditorWindow.GetWindow(typeof(LocalizationEditorWindow));
            window.minSize = new Vector2(minWidth, minHeight);
            if (!EditorPrefs.HasKey(PrefsKeys.firstExecute)) {
                window.position = new Rect(Screen.currentResolution.width / 2 - minWidth / 2,
                    Screen.currentResolution.height / 2 - minHeight / 2, minWidth, minHeight);
                EditorPrefs.SetBool(PrefsKeys.firstExecute, true);
            }

            window.titleContent.text = "Localization";
            window.SetupLayoutStyles();
            window.Show();

        }

        private void Awake() {
            library = Localization.Instance;
            LoadTextures();
        }

        private void Update() {
            if (updateTimerWorking) {
                if (updateTimer >= updateTime) {
                    updateTimer = 0;
                    updateTimerWorking = false;
                    topPanelMessage = "";
                    Repaint();
                }
                else updateTimer++;
            }
        }

        [System.Obsolete]
        private void OnGUI() {

            GUIStyle style;
            if (library == null) {
                style = new GUIStyle();
                style.normal.textColor = Color.red;
                style.fontSize = 15;
                GUILayout.Space(10);
                GUILayout.Label("    Error! Localization library object not set!", style);
                GUILayout.Space(10);
                GUILayout.Label(
                    "Create new prefab with Localization component in\n \"Resources/Localization\" path to fix open problem");
                return;
            }

            deleteMarkButtonStyle = new GUIStyle(GUI.skin.button);
            deleteMarkButtonStyle.fontSize = 9;

            if (selectedButtonStyle == null) {
                selectedButtonStyle = new GUIStyle(GUI.skin.button);
                selectedButtonStyle.normal.background = selectedButtonTexture;
                selectedButtonStyle.border = new RectOffset(6, 6, 6, 6);

                boxStyle = new GUIStyle(GUI.skin.box);
                boxStyle.border = new RectOffset(6, 6, 6, 6);
                
                leftPanelHeaderStyle = new GUIStyle(GUI.skin.textField);
                leftPanelHeaderStyle.hover.background = null;

                leftPanelHeaderTextStyle = new GUIStyle(GUI.skin.label);
                leftPanelHeaderTextStyle.alignment = TextAnchor.MiddleCenter;
                leftPanelHeaderTextStyle.fontSize = 12;
                leftPanelHeaderTextStyle.fontStyle = FontStyle.Bold;
                
                leftPanelBackgroundStyle = new GUIStyle(GUI.skin.box);
                //leftPanelBackgroundStyle.normal.background = MakeColorTexture(1, 1, leftPanelBackgroundColor);

                leftPanelElementStyle = new GUIStyle(GUI.skin.textArea);
                //leftPanelElementStyle.normal.background = MakeColorTexture(1, 1, new Color(0.68f, 0.69f, 0.68f));
                leftPanelElementStyle.alignment = TextAnchor.MiddleCenter;
                leftPanelElementStyle.fontSize = 12;
            }

            DrawRightPanel();
            DrawLeftPanel();
        }

        [System.Obsolete]
        private void DrawRightPanel() {

            bool isLocaleLoaded = loadedLocaleUnits != null && loadedTextAsset != null;

            int pageCount = 0;
            int firstUnitIndex = 0;
            int lastUnitIndex = 0;

            if (isLocaleLoaded) {
                pageCount = Mathf.CeilToInt((float) loadedLocaleUnits.Count / (float) unitsPerPage);

                if (currentPageIndex >= pageCount) currentPageIndex = pageCount - 1;
                if (currentPageIndex < 0) currentPageIndex = 0;

                firstUnitIndex = currentPageIndex * unitsPerPage;
                lastUnitIndex = (firstUnitIndex + unitsPerPage - 1 < loadedLocaleUnits.Count)
                    ? firstUnitIndex + unitsPerPage - 1
                    : loadedLocaleUnits.Count - 1;
            }

            if (topPanelMessageStyle == null) {
                topPanelMessageStyle = new GUIStyle(GUI.skin.label);
                topPanelMessageStyle.normal.textColor = new Color(0f, 0.8f, 0f);
                topPanelMessageStyle.alignment = TextAnchor.MiddleRight;

                rightAnchorMessageStyle = new GUIStyle(GUI.skin.label);
                rightAnchorMessageStyle.normal.textColor = Color.black;
                rightAnchorMessageStyle.alignment = TextAnchor.MiddleRight;
            }

            Rect rightPanelRect = new Rect(leftPanelWidth, 0, position.width - leftPanelWidth, position.height);

            GUILayout.BeginArea(rightPanelRect);

            GUILayout.BeginVertical(leftPanelHeaderStyle);

            GUILayout.Space(2);

            GUILayout.BeginHorizontal(GUILayout.MinHeight(20));
            GUILayout.Space(15);
            if (isLocaleLoaded) {
                if (GUILayout.Button("Save Locale", GUILayout.MaxWidth(100))) {
                    SaveLocaleData();
                }

                GUILayout.Label(topPanelMessage, topPanelMessageStyle);
                GUILayout.Space(15);

                GUILayout.Label((firstUnitIndex + 1).ToString() + " to " + (lastUnitIndex + 1).ToString(),
                    rightAnchorMessageStyle);

                if (GUILayout.Button("<", deleteMarkButtonStyle, GUILayout.MaxWidth(16))) {
                    currentPageIndex -= 1;
                    return;
                }

                currentPageIndex = EditorGUILayout.IntField(currentPageIndex + 1, GUILayout.MaxWidth(32)) - 1;
                GUILayout.Label("of " + pageCount.ToString(), GUILayout.MaxWidth(48));
                if (GUILayout.Button(">", deleteMarkButtonStyle, GUILayout.MaxWidth(16))) {
                    currentPageIndex += 1;
                    return;
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.MaxHeight(24));
            GUILayout.Space(15);
            if (isLocaleLoaded) {
                if (GUILayout.Button("Check keys", GUILayout.MaxWidth(100))) {
                    CheckKeys();
                }

                GUILayout.FlexibleSpace();
                GUILayout.Label("total: " + loadedLocaleUnits.Count.ToString(), rightAnchorMessageStyle);
            }

            GUILayout.Space(15);
            GUILayout.EndHorizontal();

            GUILayout.Space(2);
            HorizontalBorder(1);
            GUILayout.Space(2);



            GUILayout.BeginHorizontal();
            GUILayout.Label("№", leftPanelHeaderStyle, GUILayout.MaxWidth(45));
            GUILayout.Label("", borderStyle, GUILayout.MaxWidth(1));
            GUILayout.Label("KEY", leftPanelHeaderStyle, GUILayout.MaxWidth(maxTermWidth));
            GUILayout.Label("", borderStyle, GUILayout.MaxWidth(1));
            GUILayout.Label("TERM", leftPanelHeaderStyle);
            GUILayout.EndHorizontal();

            GUILayout.Space(2);
            HorizontalBorder(1);

            GUILayout.EndVertical();

            GUILayout.Space(5);

            if (isLocaleLoaded) {

                rightPanelScroll = GUILayout.BeginScrollView(rightPanelScroll);

                Locale.LocalizationUnit unit;
                if (currenLocaleIndex == 0) {
                    for (int i = firstUnitIndex; i <= lastUnitIndex; i++) {

                        string key = loadedLocaleUnits[i].key;
                        string term = loadedLocaleUnits[i].value;

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(5);
                        GUILayout.Label((i + 1).ToString(), GUILayout.MaxWidth(40));
                        key = EditorGUILayout.TextField(key, GUILayout.MaxWidth(maxTermWidth - 5),
                            GUILayout.MinWidth(maxTermWidth - 5));
                        GUILayout.Space(2);
                        term = EditorGUILayout.TextArea(term,
                            GUILayout.MaxWidth(rightPanelRect.width - 100 - maxTermWidth));
                        if (GUILayout.Button("X", deleteMarkButtonStyle, GUILayout.MaxWidth(16),
                            GUILayout.MaxHeight(16))) {
                            RemoveLocaleUnit(i);
                            return;
                        }

                        GUILayout.EndHorizontal();
                        GUILayout.Space(3);

                        unit = new Locale.LocalizationUnit();
                        unit.key = key;
                        unit.value = term;
                        loadedLocaleUnits[i] = unit;
                    }

                    GUILayout.Space(10);

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Add unit", GUILayout.MinWidth(150), GUILayout.MinHeight(25))) {
                        AddLocaleUnit();
                        pageCount = Mathf.CeilToInt((float) loadedLocaleUnits.Count / (float) unitsPerPage);
                        currentPageIndex = pageCount - 1;
                        return;
                    }

                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(60);

                }
                else {
                    for (int i = firstUnitIndex; i <= lastUnitIndex; i++) {

                        string key = loadedLocaleUnits[i].key;
                        string term = loadedLocaleUnits[i].value;

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(5);
                        GUILayout.Label((i + 1).ToString(), GUILayout.MaxWidth(40));
                        GUILayout.Label(key, GUILayout.MaxWidth(maxTermWidth - 5),
                            GUILayout.MinWidth(maxTermWidth - 5));
                        GUILayout.Space(2);
                        term = EditorGUILayout.TextArea(term,
                            GUILayout.MaxWidth(rightPanelRect.width - 100 - maxTermWidth));
                        GUILayout.EndHorizontal();
                        GUILayout.Space(3);

                        unit = new Locale.LocalizationUnit();
                        unit.key = key;
                        unit.value = term;
                        loadedLocaleUnits[i] = unit;
                    }

                    GUILayout.Space(60);
                }

                if (Event.current.isKey && Event.current.keyCode == KeyCode.Return) {
                    SaveLocaleData();
                }

                GUILayout.EndScrollView();
            }

            GUILayout.EndArea();
        }

        [System.Obsolete]
        private void DrawLeftPanel() {

            Rect leftPanelRect = new Rect(0, 0, leftPanelWidth, position.height);
            GUILayout.BeginArea(leftPanelRect, leftPanelBackgroundStyle);

            GUILayout.BeginVertical(leftPanelHeaderStyle);
            GUILayout.Space(2);
            GUILayout.Label("LOCALES", leftPanelHeaderTextStyle);
            GUILayout.Space(2);
            GUILayout.EndVertical();

            //DrawLineShadow(new Vector2(0, 19), new Vector2(leftPanelWidth, 19), new Vector2(0, 1));

            leftPanelScroll = GUILayout.BeginScrollView(leftPanelScroll);

            GUILayout.Space(10);

            for (int i = 0; i < library.localeAssets.Length; i++) {
                GUILayout.BeginHorizontal(boxStyle);
                GUILayout.Space(4);
                GUILayout.BeginVertical(leftPanelElementStyle);

                GUILayout.Space(2);
                GUILayout.BeginHorizontal();
                if (i == 0) {
                    GUILayout.Label("Base locale", leftPanelElementStyle);
                }
                else {
                    GUILayout.Space(24);
                    GUILayout.Label("Locale " + i.ToString(), leftPanelElementStyle);
                    if (GUILayout.Button("X", deleteMarkButtonStyle, GUILayout.MaxWidth(16), GUILayout.MaxHeight(16))) {
                        RemoveLocale(i);
                        if (i >= library.localeAssets.Length) return;
                    }
                }

                GUILayout.EndHorizontal();
                GUILayout.Space(2);
                HorizontalBorder(1, true);

                GUILayout.Space(2);

                GUILayout.BeginHorizontal();
                library.localeAssets[i].referencedLanguage =
                    (SystemLanguage) EditorGUILayout.EnumPopup(library.localeAssets[i].referencedLanguage);
                TextAsset newTextAsset =
                    (TextAsset) EditorGUILayout.ObjectField(library.localeAssets[i].localeAsset, typeof(TextAsset));
                if (library.localeAssets[i].localeAsset != newTextAsset) {
                    library.localeAssets[i].localeAsset = newTextAsset;
                    if (currenLocaleIndex == i) {
                        LoadChoosenLocaleData();
                    }
                }

                GUILayout.EndHorizontal();

                GUILayout.Space(2);

                GUILayout.BeginHorizontal();

                GUILayout.Space(30);
                if (currenLocaleIndex == i) {
                    GUILayout.Button("Selected", selectedButtonStyle);
                }
                else {
                    if (GUILayout.Button("Select")) {
                        currenLocaleIndex = i;
                        LoadChoosenLocaleData();
                    }
                }

                GUILayout.Space(30);
                GUILayout.EndHorizontal();

                GUILayout.Space(2);

                GUILayout.EndVertical();

                GUILayout.Space(4);
                GUILayout.EndHorizontal();
                GUILayout.Space(2);
            }

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add locale", GUILayout.MinWidth(150), GUILayout.MinHeight(25))) {
                AddNewLocale();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(60);
            GUILayout.EndScrollView();

            GUILayout.EndArea();
            DrawLineShadow(new Vector2(leftPanelWidth, 0), new Vector2(leftPanelWidth, position.height),
                new Vector2(1, 0));
        }

        private void HorizontalBorder(int size, bool inner = false) {
            GUILayout.BeginVertical((inner) ? innerBorderStyle : borderStyle);
            GUILayout.Space(size);
            GUILayout.EndVertical();
        }

        private void DrawLineShadow(Vector3 start, Vector3 end, Vector3 offset, int samples = 6) {
            Color lastColor = Handles.color;
            for (int i = 0; i <= samples; i++) {
                Handles.color = shadowColor * Mathf.Tan((float) (samples - i) / (float) samples);
                Handles.DrawLine(start + offset * (float) i, end + offset * (float) i);
            }

            Handles.color = lastColor;
        }

        private void ShowTopMessage(string message, Color color) {
            topPanelMessageStyle.normal.textColor = color;
            topPanelMessage = message;
            updateTimerWorking = true;
            Repaint();
        }

        private Texture2D MakeColorTexture(int width, int height, Color color) {
            Color[] pixels = new Color[width * height];

            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = color;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pixels);
            result.Apply();

            return result;
        }

        private void LoadTextures() {
            selectedButtonTexture =
                AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editors/Localization/editor_button_green.png");
        }

        private void SetupLayoutStyles() {

            

            innerBorderStyle = new GUIStyle();
            innerBorderStyle.normal.background = MakeColorTexture(1, 1, new Color(0.6f, 0.6f, 0.6f));

            borderStyle = new GUIStyle();
            borderStyle.normal.background = MakeColorTexture(1, 1, new Color(0.5f, 0.5f, 0.5f));

        }

        private void AddNewLocale() {
            workingLoadData = new List<Localization.LocaleLoadData>(library.localeAssets);
            workingLoadData.Add(new Localization.LocaleLoadData());
            library.localeAssets = workingLoadData.ToArray();
        }

        private void RemoveLocale(int index) {
            if (index < 1 || index >= library.localeAssets.Length) return;

            workingLoadData = new List<Localization.LocaleLoadData>(library.localeAssets);

            workingLoadData.RemoveAt(index);
            if (currenLocaleIndex == index) {
                currenLocaleIndex = -1;
                ClearLoadedLocaleData();
            }
            else if (currenLocaleIndex > index) {
                currenLocaleIndex -= 1;
                ClearLoadedLocaleData();
            }

            if (currenLocaleIndex >= library.localeAssets.Length) ClearLoadedLocaleData();

            library.localeAssets = workingLoadData.ToArray();
        }

        private void LoadChoosenLocaleData() {

            SaveLocaleData(false);

            if (currenLocaleIndex < 0 || currenLocaleIndex >= library.localeAssets.Length) return;

            loadedTextAsset = library.localeAssets[currenLocaleIndex].localeAsset;
            if (loadedTextAsset == null) {
                ClearLoadedLocaleData();
                ShowTopMessage("Locale file not assign!", new Color(1f, 1f, 0f));
                return;
            }

            if (currenLocaleIndex == 0) {
                loadedLocaleUnits = Locale.FromJsonToUnitList(loadedTextAsset.text);
            }
            else {
                loadedLocaleUnits = Locale.FromJsonToUnitList(library.localeAssets[0].localeAsset.text);
                Dictionary<string, string> dict = Locale.FromJsonToUnitDictionary(loadedTextAsset.text);
                Locale.LocalizationUnit unit;
                for (int i = 0; i < loadedLocaleUnits.Count; i++) {
                    string term = "";
                    bool result = dict.TryGetValue(loadedLocaleUnits[i].key, out term);

                    unit = new Locale.LocalizationUnit();
                    unit.key = loadedLocaleUnits[i].key;
                    if (result) {
                        unit.value = term;
                    }
                    else {
                        unit.value = "";
                    }

                    loadedLocaleUnits[i] = unit;
                }

            }
        }

        private void SaveLocaleData(bool showMessages = true) {
            if (loadedTextAsset != null && loadedLocaleUnits != null) {
                string[] json = Locale.UnitListToJson(loadedLocaleUnits);

                FileStream fs = File.Open(AssetDatabase.GetAssetPath(loadedTextAsset), FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                for (int i = 0; i < json.Length; i++) {
                    sw.Write(json[i]);
                }

                sw.Close();
                fs.Close();
                
                AssetDatabase.Refresh(); //TODO fix
                if (showMessages) ShowTopMessage("Locale Saved!", new Color(0f, 1f, 0f));
            }
            else {
                if (showMessages) ShowTopMessage("Locale save error!", new Color(1f, 0f, 0f));
            }
        }

        private void ClearLoadedLocaleData() {
            loadedTextAsset = null;
            loadedLocaleUnits = null;
        }

        private void RemoveLocaleUnit(int index) {
            if (loadedLocaleUnits == null || index < 0 || index >= loadedLocaleUnits.Count) return;
            loadedLocaleUnits.RemoveAt(index);
        }

        private void AddLocaleUnit() {
            if (loadedLocaleUnits == null || currenLocaleIndex != 0) return;
            loadedLocaleUnits.Add(new Locale.LocalizationUnit("", ""));
        }

        [System.Obsolete]
        private void CheckKeys() {
            if (loadedLocaleUnits == null || loadedTextAsset == null) return;

            List<string> messages = new List<string>(100);

            Dictionary<string, bool> keys = new Dictionary<string, bool>();
            for (int i = 0; i < loadedLocaleUnits.Count; i++) {
                if (loadedLocaleUnits[i].key.Trim() == "") {
                    messages.Add("Term number " + (i + 1).ToString() + " has NULL key");
                }
                else if (keys.ContainsKey(loadedLocaleUnits[i].key)) {
                    messages.Add("Term number " + (i + 1).ToString() + " is already used");
                }
                else {
                    keys.Add(loadedLocaleUnits[i].key, true);
                }
            }

            string report = "Localization keys was verified for doubles. Verification has following results:";

            if (messages.Count == 0) messages.Add("GOOD RESULT!\nAll term KEYS are UNIQUE");

            LocalizationReportToolWindow.Init(report, messages.ToArray());
        }

        private void OnDestroy() {
            SaveLocaleData();
        }
    }
}