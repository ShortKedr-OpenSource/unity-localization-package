using System;
using UnityEngine;

namespace Krugames.LocalizationSystem.Models.Interfaces {
    public interface ILocale {

        public SystemLanguage Language { get; }

        public LocaleTerm GetTerm(string term);
        public LocaleTerm GetTerm(string term, Type type);
        public TTermType GetTerm<TTermType>(string term) where TTermType : LocaleTerm;

        public object GetTermValue(string term);
        public object GetTermValue(string term, Type type);
        public TTermValueType GetTermValue<TTermValueType>(string term);

        public bool SupportsTermType(Type type);
    }
}