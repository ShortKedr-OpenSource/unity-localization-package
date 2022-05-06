using Krugames.LocalizationSystem.Models.Attributes;
using Krugames.LocalizationSystem.Models.Terms;
using UnityEngine;

[assembly: RegisterLocaleTerm(typeof(Vector3Term))]

namespace Krugames.LocalizationSystem.Models.Terms {
    [CreateAssetMenu(fileName = "Vector3Term", menuName = "Localization/Terms/Vector3", order = 0)]
    public class Vector3Term : LocaleTerm<Vector3> {
        //TODO remove this bullshit
        [SerializeField] private Vector3 someField1;
        [SerializeField] private Vector3 someField2;
        [SerializeField] private Vector3 someField3;
        [SerializeField] private Vector3 someField4;
        [SerializeField] private Vector3 someField5;
        [SerializeField] private Vector3 someField6;
        [SerializeField] private Vector3 someField7;
        [SerializeField] private Vector3 someField8;
        [SerializeField] private Vector3 someField9;
        [SerializeField] private Vector3 someField10;
        [SerializeField] private Vector3 someField11;
        [SerializeField] private Vector3 someField12;
        [SerializeField] private Vector3 someField13;
    }
}