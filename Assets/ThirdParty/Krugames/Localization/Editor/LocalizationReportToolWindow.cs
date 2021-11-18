using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;

namespace Game {
    public class LocalizationReportToolWindow : EditorWindow {

        private static int width = 300;
        private static int height = 400;
        private static int padding = 10;

        public string mainReport = "";
        public string[] messages;
        private Vector2 scrollView = Vector2.zero;

        private static Color shadowColor = new Color(0f, 0f, 0f, 0.4f);

        //styles
        private GUIStyle reportStyle;
        private GUIStyle messagesStyle;
        private GUIStyle borderStyle;

        private bool specialStylesSet = false;

        [System.Obsolete]
        public static LocalizationReportToolWindow Init(string mainReport, string[] messages) {
            LocalizationReportToolWindow window = (LocalizationReportToolWindow)EditorWindow.GetWindow(typeof(LocalizationReportToolWindow), true);
            window.minSize = new Vector2(width, height);
            window.maxSize = new Vector2(width, height);
            window.position = new Rect(Screen.currentResolution.width / 2 - width / 2, Screen.currentResolution.height / 2 - height / 2, width, height);
            window.SetupLayoutStyles();
            window.mainReport = mainReport;
            window.messages = messages;
            window.titleContent.text = "Report";
            window.title = "Report";
            window.Show();
            return window;
        }

        private void OnGUI() {

            if (!specialStylesSet) {
                reportStyle = new GUIStyle(GUI.skin.label);
                reportStyle.wordWrap = true;

                messagesStyle = new GUIStyle(GUI.skin.label);
                messagesStyle.wordWrap = true;

                specialStylesSet = true;
            }

            Rect reportRect = new Rect(padding, padding, position.width - padding * 2, 90);
            Rect messagesRect = new Rect(0, 100, position.width, position.height - 100);

            GUILayout.BeginArea(messagesRect);
            scrollView = GUILayout.BeginScrollView(scrollView);
            for (int i = 0; i < messages.Length; i++) {
                GUILayout.BeginHorizontal();
                GUILayout.Space(padding);
                GUILayout.Label(messages[i], messagesStyle);
                GUILayout.Space(padding);
                GUILayout.EndHorizontal();
                HorizontalBorder(1);
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();

            GUILayout.BeginArea(reportRect);
            GUILayout.Label(mainReport, reportStyle);
            GUILayout.EndArea();

            DrawLineShadow(new Vector2(0, 100), new Vector2(position.width, 100), new Vector2(0, 1));
        }

        private void SetupLayoutStyles() {
            borderStyle = new GUIStyle();
            borderStyle.normal.background = MakeColorTexture(1, 1, new Color(0.5f, 0.5f, 0.5f));
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
        private void DrawLineShadow(Vector3 start, Vector3 end, Vector3 offset, int samples = 6) {
            Color lastColor = Handles.color;
            for (int i = 0; i <= samples; i++) {
                Handles.color = shadowColor * Mathf.Tan((float)(samples - i) / (float)samples);
                Handles.DrawLine(start + offset * (float)i, end + offset * (float)i);
            }
            Handles.color = lastColor;
        }
        private void HorizontalBorder(int size, bool inner = false) {
            GUILayout.BeginVertical(borderStyle);
            GUILayout.Space(size);
            GUILayout.EndVertical();
        }
    }
}
