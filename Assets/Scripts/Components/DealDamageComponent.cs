using Unity.Entities;

namespace DapperDino.DamageSystems.Components
{
    [GenerateAuthoringComponent]
    public struct DealDamage : IComponentData
    {
        public int Value;
    }
}
