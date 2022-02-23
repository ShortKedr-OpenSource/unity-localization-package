using System;
using Krugames.LocalizationSystem.Models.Attributes;
using Krugames.LocalizationSystem.Models.Terms;
using UnityEngine;

[assembly: RegisterLocaleTerm(typeof(StringTerm), "String")]

namespace Krugames.LocalizationSystem.Models.Terms {
    [CreateAssetMenu(fileName = "StringTerm", menuName = "Localization/Terms/String", order = 0)]
    public class StringTerm : LocaleTerm<string> {
    }
}