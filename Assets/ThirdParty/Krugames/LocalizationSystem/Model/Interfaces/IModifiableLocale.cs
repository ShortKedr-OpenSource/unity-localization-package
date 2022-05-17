using UnityEngine;

namespace Krugames.LocalizationSystem.Models.Interfaces {
    public interface IModifiableLocale {
        /// <summary>
        /// Sets Locale language
        /// </summary>
        /// <param name="newLanguage">new language to set</param>
        /// <returns>true if language was set</returns>
        public bool SetLanguage(SystemLanguage newLanguage);
        
        /// <summary>
        /// Replace Locale terms with new terms array
        /// </summary>
        /// <param name="terms">new terms</param>
        /// <returns>true if terms was set</returns>
        public bool SetTerms(LocaleTerm[] terms);

        /// <summary>
        /// Add new term to Locale term base
        /// </summary>
        /// <returns>true if term was added</returns>
        public bool AddTerm(LocaleTerm term);

        /// <summary>
        /// Remove term from Locale term base if exists
        /// </summary>
        /// <param name="term">term to remove</param>
        /// <returns>true if term was removed</returns>
        public bool RemoveTerm(LocaleTerm term);
    }
}