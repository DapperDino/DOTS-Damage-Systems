using System;
using UnityEngine;

namespace DapperDino.DamageSystems
{
    [CreateAssetMenu(fileName = "New Damage Resistance Stats", menuName = "Damage Resistance Stats")]
    public class DamageResistanceStats : ScriptableObject
    {
        [SerializeField] private Resistance[] resistances = new Resistance[0];

        public Resistance[] Resistances => resistances;

        [Serializable]
        public class Resistance
        {
            [SerializeField] private DamageType damageType = null;
            [SerializeField] private float value = 1f;

            public DamageType DamageType => damageType;
            public float Value => value;
        }
    }
}
