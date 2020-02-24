using UnityEngine;

namespace DapperDino.DamageSystems
{
    [CreateAssetMenu(fileName = "New Damage Type", menuName = "Damage Type")]
    public class DamageType : ScriptableObject
    {
        [SerializeField] private int id = -1;
        [SerializeField] private new string name = "New Damage Type Name";
        [SerializeField] private Color colour = new Color(0f, 0f, 0f, 1f);

        public int Id => id;
        public string Name => name;
        public Color Colour => colour;
    }
}
