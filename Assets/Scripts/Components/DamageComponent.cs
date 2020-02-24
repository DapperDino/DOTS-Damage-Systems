using Unity.Entities;

namespace DapperDino.DamageSystems.Components
{
    public struct Damage : IBufferElementData
    {
        public int DamageTypeId;
        public float Value;
    }
}
