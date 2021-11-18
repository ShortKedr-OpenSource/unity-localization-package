using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Krugames.LocalizationSystem {
    public class Locale {

        private SystemLanguage language;
        private Dictionary<string, string> terms = new Dictionary<string, string>();

        public SystemLanguage Language => language;
        public Dictionary<string, string> Terms => terms;

        [Serializable]
        public struct LocalizationUnit {
            public string key;
            public string value;

            public LocalizationUnit(string key, string value) {
                this.key = key;
                this.value = value;
            }
        }

        public Locale(SystemLanguage language) {
            this.language = language;
        }

        public static Locale FromJson(string json, SystemLanguage language) {
            Locale locale = new Locale(language);

            string[] jsonStrings = json.Split(';');

            if (jsonStrings.Length == 1 && jsonStrings[0].Trim() == "") return locale;

            LocalizationUnit unit;
            for (int i = 0; i < jsonStrings.Length; i++) {
                unit = JsonUtility.FromJson<LocalizationUnit>(jsonStrings[i]);
                if (!locale.Terms.ContainsKey(unit.key)) locale.Terms.Add(unit.key, unit.value);
            }
            return locale;   
        }

        public static List<LocalizationUnit> FromJsonToUnitList(string json) {

            string[] jsonStrings = json.Split(';');

            List<LocalizationUnit> list = new List<LocalizationUnit>();

            if (jsonStrings.Length == 1 && jsonStrings[0].Trim() == "") return list;

            LocalizationUnit unit;
            for (int i = 0; i < jsonStrings.Length; i++) {
                unit = JsonUtility.FromJson<LocalizationUnit>(jsonStrings[i]);
                list.Add(unit);
            }
            return list;
        }

        public static Dictionary<string, string> FromJsonToUnitDictionary(string json) {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            string[] jsonStrings = json.Split(';');

            if (jsonStrings.Length == 1 && jsonStrings[0].Trim() == "") return dictionary;

            LocalizationUnit unit;
            for (int i = 0; i < jsonStrings.Length; i++) {
                unit = JsonUtility.FromJson<LocalizationUnit>(jsonStrings[i]);
                if (!dictionary.ContainsKey(unit.key)) dictionary.Add(unit.key, unit.value);
            }
            return dictionary;
        }

        public static string[] UnitListToJson(List<LocalizationUnit> units, bool prettyPrint = false) {
            string[] jsonStrings = new string[units.Count];

            for (int i = 0; i < units.Count; i++) {
               jsonStrings[i] = JsonUtility.ToJson(units[i], prettyPrint) + ((i != units.Count - 1) ? ";\n" : "");
            }

            return jsonStrings;
        }

        public string[] ToJson(bool prettyPrint = false) {
            string[] jsonStrings = new string[terms.Count];
            LocalizationUnit unit;
            int i = 0;
            foreach (KeyValuePair<string, string> kvp in terms) {
                unit = new LocalizationUnit(kvp.Key, kvp.Value);
                jsonStrings[i] = JsonUtility.ToJson(unit, prettyPrint) + ((i != terms.Count) ? ";\n" : "");
                i++;
            };

            return jsonStrings;          
        }
    }
}
