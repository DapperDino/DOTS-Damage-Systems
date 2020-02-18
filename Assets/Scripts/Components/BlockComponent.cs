using Unity.Entities;

namespace DapperDino.DamageSystems.Components
{
    [GenerateAuthoringComponent]
    public struct Block : IComponentData
    {
        public int Value;
    }
}
