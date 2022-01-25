using System;
using UnityEngine;

namespace Krugames.LocalizationSystem.Models {
    /// <summary>
    /// Base Class for all Terms
    /// </summary>
    public abstract class LocaleTerm : ScriptableObject {
        [SerializeField] protected string term;
        protected object value;

        public string Term => term;
        public object Value => value;

        protected LocaleTerm(string term, object value) {
            this.term = term;
            this.value = value;
        }

        public virtual void SetValue(object value) {
            this.value = value;
        }
    }

    /// <summary>
    /// Generic Term class. All new term types must be inherited from this class
    /// </summary>
    /// <typeparam name="TValueType">Type of value</typeparam>
    public abstract class LocaleTerm<TValueType> : LocaleTerm {
        
        protected static readonly Type VALUE_TYPE = typeof(TValueType);

        [SerializeField] protected TValueType smartValue;

        protected LocaleTerm(string term, TValueType value) : base(term, value) {
            smartValue = value;
        }

        public override void SetValue(object value) {
            if (value is TValueType revealedValue) {
                this.value = value;
                this.smartValue = revealedValue;
            } else {
                throw new ArgumentException("Wrong type of argument. Argument is not type of " + nameof(TValueType));
            }
        }

        public virtual void SetSmartValue(TValueType value) {
            this.value = value;
            this.smartValue = value;
        }
        
        public TValueType SmartValue => smartValue;
        
        public Type ValueType => VALUE_TYPE;
    }
}