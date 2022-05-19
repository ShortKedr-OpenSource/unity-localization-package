using UnityEngine;

namespace Krugames.LocalizationSystem.Models.Interfaces {
    //TODO edit this thing
    public interface IModifiableLocale : ILocale{
        /// <summary>
        /// Sets Locale language
        /// </summary>
        /// <param name="newLanguage">new language to set</param>
        /// <returns>true if language was set</returns>
        public bool SetLanguage(SystemLanguage newLanguage);

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

        /// <summary>
        /// Replace current term base with new one
        /// </summary>
        /// <param name="terms">terms array for new term base</param>
        /// <returns>true if no errors was occured</returns>
        public bool SetTerms(LocaleTerm[] terms);
        
        /// <summary>
        /// Clear all terms from Locale term base
        /// </summary>
        /// <returns>true if term base was cleared</returns>
        public bool ClearTerms();
    }
}