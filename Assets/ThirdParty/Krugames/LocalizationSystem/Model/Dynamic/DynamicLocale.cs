using System;
using Krugames.LocalizationSystem.Models.Interfaces;
using Krugames.LocalizationSystem.Models.Structs;
using UnityEngine;

//TODO implement

namespace Krugames.LocalizationSystem.Models.Dynamic {
    /// <summary>
    /// Dynamic locale is a part of Dynamic Localization API that allows dynamic modifications of localization at runtime.
    /// Dynamic locale allows the creation and adding locales to the Localization System at runtime.
    /// It's very useful, if you use such technologies as Cloud Content Delivery, for example.
    /// Also, it's very handy, if you prefer a dynamic approach or want to add locales directly through the code.
    /// Working with dynamic locales requires being careful - these locales can have layouts, that are different
    /// from the base Locale layout (for example, some terms can have different types in contrast to base Locale).
    /// Also, dynamic locales can stay in memory after being unreferenced,
    /// so to manage unreferenced Dynamic Localization API stuff, check ReferenceManager class
    /// </summary>
    public class DynamicLocale : ILocale, IModifiableLocale, ILocaleGettableLayout, ILocaleSettableLayout {

        public DynamicLocale() {
            throw new NotImplementedException();
        }

        public DynamicLocale(TermStructureInfo[] layout) {
            throw new NotImplementedException();
        }
        
        public bool AppendTerm(string term, object value) {
            throw new NotImplementedException();
        }

        public bool AppendTerm<TTermValueType>(string term, TTermValueType value) {
            throw new NotImplementedException();
        }

        public bool RemoveTerm(string term) {
            throw new NotImplementedException();
        }

        public bool RemoveTerm<TTermValueType>(string term) {
            throw new NotImplementedException();
        }

        public bool ModifyTerm(string term, object newValue) {
            throw new NotImplementedException();
        }

        public bool ModifyTerm<TTermValueType>(string term, TTermValueType newValue) {
            throw new NotImplementedException();
        }

        public SystemLanguage Language => throw new NotImplementedException();
        
        public LocaleTerm GetTerm(string term) {
            throw new NotImplementedException();
        }

        public LocaleTerm GetTerm(string term, Type type) {
            throw new NotImplementedException();
        }

        public TTermType GetTerm<TTermType>(string term) where TTermType : LocaleTerm {
            throw new NotImplementedException();
        }

        public object GetTermValue(string term) {
            throw new NotImplementedException();
        }

        public object GetTermValue(string term, Type type) {
            throw new NotImplementedException();
        }

        public TTermValueType GetTermValue<TTermValueType>(string term) {
            throw new NotImplementedException();
        }

        public LocaleTerm[] GetTerms() {
            throw new NotImplementedException();
        }

        public TTermType[] GetTerms<TTermType>() where TTermType : LocaleTerm {
            throw new NotImplementedException();
        }

        public bool SupportsTermType(Type termType) {
            throw new NotImplementedException();
        }

        public bool SupportsValueType(Type valueType) {
            throw new NotImplementedException();
        }

        public bool ContainsTermType(Type termType) {
            throw new NotImplementedException();
        }

        public bool ContainsValueType(Type valueType) {
            throw new NotImplementedException();
        }

        public TermStructureInfo[] GetLayout() {
            throw new NotImplementedException();
        }

        public void SetLayout(TermStructureInfo[] layout) {
            throw new NotImplementedException();
        }

        public bool SetLanguage(SystemLanguage newLanguage) {
            throw new NotImplementedException();
        }

        public bool ClearTerms() {
            throw new NotImplementedException();
        }

        public bool AddTerm(LocaleTerm term) {
            throw new NotImplementedException();
        }

        public bool RemoveTerm(LocaleTerm term) {
            throw new NotImplementedException();
        }
    }
}