﻿using System;
using System.Text;
using Krugames.LocalizationSystem;
using Krugames.LocalizationSystem.Models;
using TMPro;
using UnityEngine;

namespace Example.SimpleExample {
    public class ComplexText : MonoBehaviour {
        
        [SerializeField] private string descriptionTerm = "none";
        [SerializeField] private string silenceWordTerm;
        [SerializeField] private TMP_Text text;

        [SerializeField] private float damage = 90f;
        [SerializeField] private float tickInterval = 1f;
        [SerializeField] private float skillTime = 1f;
        
        private void Awake() {
            Callback_LanguageUpdate();
            Localization.AddLanguageUpdateCallback(Callback_LanguageUpdate);
        }

        private void Callback_LanguageUpdate() {
            string desc = Localization.GetTermValue<string>(descriptionTerm);
            string silenceWord = Localization.GetTermValue<string>(silenceWordTerm);
            text.text = desc
                .Replace("@silence_word@", silenceWord)
                .Replace("@damage@", damage.ToString("0"))
                .Replace("@tick_interval@", tickInterval.ToString("0"))
                .Replace("@skill_time@", skillTime.ToString("0"));
        }
    }
}