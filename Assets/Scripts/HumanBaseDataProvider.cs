using Krugames.LocalizationSystem;
using UnityEngine;

namespace DefaultNamespace {
    public class HumanBaseDataProvider : MonoBehaviour {
        [SerializeField] private string firstName = "fname";
        [SerializeField] private string lastName = "lname";
        
        [Header("Localization")]
        [SerializeField] private string firstNameTerm = "first_name"; 
        [SerializeField] private string lastNameTerm = "last_name";

        public void AssignLocalizationData() {
            firstName = Localization.GetTerm(firstNameTerm);
            lastName = Localization.GetTerm(lastNameTerm);
        }
        
        private void Awake() {
            AssignLocalizationData();
            Localization.AddLanguageUpdateCallback(AssignLocalizationData);
        }

        private void OnDestroy() {
            Localization.RemoveLanguageUpdateCallback(AssignLocalizationData);
        }
    }
}