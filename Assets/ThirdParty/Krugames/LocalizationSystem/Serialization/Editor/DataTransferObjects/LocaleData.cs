using System;
using System.Collections.Generic;
using Krugames.LocalizationSystem.Models;
using Krugames.LocalizationSystem.Models.Interfaces;
using UnityEngine;

namespace Krugames.LocalizationSystem.Editor.Serialization.DataTransferObjects {
    [Serializable]
    public struct LocaleData {
        public SystemLanguage language;
        public TermData[] termData;

        public LocaleData(SystemLanguage language, TermData[] termData) {
            this.language = language;
            this.termData = termData;
        }

        public LocaleData(ILocale locale) {
            LocaleTerm[] terms = locale.GetTerms();
            List<TermData> resultTermData = new List<TermData>(terms.Length);
            for (int i = 0; i < terms.Length; i++) {
                if (terms[i] == null) continue;
                resultTermData.Add(new TermData(terms[i]));
            }
            
            this.language = locale.Language;
            this.termData = resultTermData.ToArray();
        }
    }
}