using System;
using System.Collections.Generic;
using System.IO;
using Krugames.LocalizationSystem.Editor.Package;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Krugames.LocalizationSystem.RapidStorage.Servers {
    public class TermNoteServer : IDisposable {

        private const int DefaultBucket = 128;
        
        private static TermNoteServer _instance;
        public static TermNoteServer Instance {
            get {
                if (_instance == null) _instance = new TermNoteServer();
                return _instance;
            }
        }

        private List<TermNote> _notes;
        private Dictionary<string, TermNote> _noteByTermName;

        private TermNoteServer() {
            AssemblyReloadEvents.beforeAssemblyReload += Dispose;
            EditorApplication.quitting += Dispose; 

            string dataFilePath = GetDataFilePath();
            if (File.Exists(dataFilePath)) {
                string json = File.ReadAllText(dataFilePath);
                _notes = JsonConvert.DeserializeObject<List<TermNote>>(json);
                _noteByTermName = new Dictionary<string, TermNote>(_notes.Count);
                for (int i = 0; i < _notes.Count; i++) {
                    if (_noteByTermName.ContainsKey(_notes[i].TermName)) continue;
                    _noteByTermName.Add(_notes[i].TermName, _notes[i]);
                }
            } else {
                _notes = new List<TermNote>(DefaultBucket);
                _noteByTermName = new Dictionary<string, TermNote>(DefaultBucket);
            }
        }

        private string GetDataFilePath() {
            string projectDirectory = Directory.GetParent(Application.dataPath).FullName;
            string dataDirectory = (projectDirectory + "/" + PackageVariables.PackagePath + "RapidStorage/Editor/Data/")
                .Replace('\\', '/');
            return dataDirectory + "TermNotes.json";
        }

        public static string GetNote(string termName) {
            TermNoteServer instance = Instance;
            if (instance._noteByTermName.ContainsKey(termName)) return instance._noteByTermName[termName].Note;
            return string.Empty;
        }

        public static void SetNote(string termName, string note) {
            TermNoteServer instance = Instance;
            if (instance._noteByTermName.ContainsKey(termName)) instance._noteByTermName[termName].Note = note;
            else {
                TermNote newNote = new TermNote(termName, note);
                instance._notes.Add(newNote);
                instance._noteByTermName.Add(newNote.TermName, newNote);
            }
        }
        
        public void Dispose() {
            string json = JsonConvert.SerializeObject(_notes);
            string dataFilePath = GetDataFilePath();
            File.WriteAllText(dataFilePath, json);
        }
    }
}